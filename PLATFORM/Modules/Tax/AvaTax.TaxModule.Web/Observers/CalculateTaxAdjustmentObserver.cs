using System;
using System.Linq;
using AvaTax.TaxModule.Web.Converters;
//using AvaTax.TaxModule.Web.Logging;
//using Common.Logging;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;
using domainModel = VirtoCommerce.Domain.Commerce.Model;
using Microsoft.Practices.ObjectBuilder2;

namespace AvaTax.TaxModule.Web.Observers
{
    public class CalculateTaxAdjustmentObserver : IObserver<OrderChangeEvent>
    {
        private readonly ITaxSettings _taxSettings;
        private readonly IContactService _customerSearchService;

        public CalculateTaxAdjustmentObserver(ITaxSettings taxSettings, IContactService customerSearchService)
        {
            _taxSettings = taxSettings;
            _customerSearchService = customerSearchService;
        }

        #region IObserver<CustomerOrder> Members

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(OrderChangeEvent value)
        {
            if (value.ChangeState == EntryState.Modified)
                CalculatePartialReturnTaxes(value);
        }

        #endregion
        private void CalculatePartialReturnTaxes(OrderChangeEvent context)
        {
            //do nothing if order cancelled (should work another observer)
            if (context.ModifiedOrder.Status == "Cancelled")
            {
                return;
            }
            
            var originalOrder = context.OrigOrder;
            var modifiedOrder = context.ModifiedOrder;

            //do nothing if order Items quantities did not changed
            if (
                modifiedOrder.Items.All(
                    li => li.Quantity.Equals(originalOrder.Items.Single(oli => oli.Id.Equals(li.Id)).Quantity)))
                return;

            //otherwise make partial return/add request
            
            if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username)
                && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
            {
                var taxSvc = new JsonTaxSvc(_taxSettings.Username,
                    _taxSettings.Password,
                    _taxSettings.ServiceUrl);

                //if all payments completed commit tax document in avalara
                var isCommit = modifiedOrder.InPayments != null && modifiedOrder.InPayments.Any()
                    && modifiedOrder.InPayments.All(pi => pi.IsApproved);
                
                Contact contact = null;
                if (modifiedOrder.CustomerId != null)
                    contact = _customerSearchService.GetById(modifiedOrder.CustomerId);

                var request = modifiedOrder.ToAvaTaxAdjustmentRequest(_taxSettings.CompanyCode, contact, originalOrder, isCommit);
                if (request != null)
                {
                    var getTaxResult = taxSvc.GetTax(request);

                    if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                    {
                        var error = string.Join(Environment.NewLine,
                            getTaxResult.Messages.Select(m => m.Summary));
                        OnError(new Exception(error));
                    }
                    else
                    {
                        foreach (var taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                        {
                            var lineItem = modifiedOrder.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
                            if (lineItem != null)
                            {
                                lineItem.Tax += taxLine.Tax;
                                if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any())
                                {
                                    lineItem.TaxDetails =
                                        taxLine.TaxDetails.Select(taxDetail => new domainModel.TaxDetail
                                        {
                                            Amount = taxDetail.Tax,
                                            Name = taxDetail.TaxName,
                                            Rate = taxDetail.Rate
                                        }).ToList();
                                }
                            }
                        }

                        modifiedOrder.Tax = 0;
                    }
                }
            }
            else
            {
                OnError(new Exception("AvaTax credentials not provided"));
            }
        }
    }
}