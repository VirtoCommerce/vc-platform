using System;
using System.Linq;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Logging;
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
            if (context.ModifiedOrder.IsCancelled)
            {
                return;
            }
            
            var originalOrder = context.OrigOrder;
            var modifiedOrder = context.ModifiedOrder;

            //do nothing if order Items quantities did not changed
            if (
                originalOrder.Items.All(
                    li => modifiedOrder.Items.Any(oli => oli.Id.Equals(li.Id)) && 
                        modifiedOrder.Items.Single(oli => li.Id.Equals(oli.Id)).Quantity.Equals(li.Quantity)))
                return;

            //otherwise make partial return/add request
            SlabInvoker<VirtoCommerceEventSource.TaxRequestContext>.Execute(slab =>
                {
                    if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username)
                        && !string.IsNullOrEmpty(_taxSettings.Password)
                        && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                        && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
                    {
                        //if all payments completed commit tax document in avalara
                        var isCommit = modifiedOrder.InPayments != null && modifiedOrder.InPayments.Any()
                            && modifiedOrder.InPayments.All(pi => pi.IsApproved);
                
                        Contact contact = null;
                        if (modifiedOrder.CustomerId != null)
                            contact = _customerSearchService.GetById(modifiedOrder.CustomerId);

                        var request = modifiedOrder.ToAvaTaxAdjustmentRequest(_taxSettings.CompanyCode, contact, originalOrder, isCommit);
                        if (request != null)
                        {
                            slab.docCode = request.ReferenceCode;
                            slab.docType = request.DocType.ToString();
                            slab.customerCode = request.CustomerCode;
                            slab.amount = (double)originalOrder.Sum;

                            var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                            var getTaxResult = taxSvc.GetTax(request);

                            if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                            {
                                var error = string.Join(Environment.NewLine,
                                    getTaxResult.Messages.Select(m => m.Summary));
                                throw new Exception(error);
                            }
                    
                                foreach (var taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                                {
                                    var lineItem = modifiedOrder.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
                                    if (lineItem != null)
                                    {
                                        lineItem.Tax += taxLine.Tax;
                                        if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any(td => !string.IsNullOrEmpty(td.TaxName)))
                                        {
                                    
                                            var taxLines =
                                                taxLine.TaxDetails.Where(td => !string.IsNullOrEmpty(td.TaxName)).Select(taxDetail => new domainModel.TaxDetail
                                                {
                                                    Amount = taxDetail.Tax,
                                                    Name = taxDetail.TaxName,
                                                    Rate = taxDetail.Rate
                                                }).ToList();

                                            lineItem.TaxDetails = lineItem.TaxDetails == null ? taxLines : lineItem.TaxDetails.AddRange(taxLines);
                                        }
                                    }
                                }

                                modifiedOrder.Tax = 0;
                        }
                    }
                    else
                    {
                       throw new Exception("AvaTax credentials not provided or tax calculation disabled");
                    }
                })
                .OnError(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.TaxCalculationError)
                .OnSuccess(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.GetTaxRequestTime);
        }
    }
}