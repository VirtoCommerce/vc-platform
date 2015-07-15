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
    public class CalculateOrderTaxesObserver : IObserver<OrderChangeEvent>
	{
        private readonly ITaxSettings _taxSettings;
        private readonly IContactService _customerSearchService;

        public CalculateOrderTaxesObserver(ITaxSettings taxSettings, IContactService customerSearchService)
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
			    CalculateCustomerOrderTaxes(value);
		}

		#endregion
		private void CalculateCustomerOrderTaxes(OrderChangeEvent context)
		{
            if (context.ModifiedOrder.IsCancelled)
            {
                return;
            }

            var order = context.ModifiedOrder;
            var originalOrder = context.OrigOrder;

            //do nothing if order Items quantities did not changed
            if (
                originalOrder.Items.Any(
                    li => !order.Items.Any(oli => oli.Id.Equals(li.Id)) ||
                        order.Items.Single(oli => li.Id.Equals(oli.Id)).Quantity < li.Quantity))
                return;

            SlabInvoker<VirtoCommerceEventSource.TaxRequestContext>.Execute(slab =>
                {
		            if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username)
		                && !string.IsNullOrEmpty(_taxSettings.Password)
		                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
		                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
		            {
                        //if all payments completed commit tax document in avalara
                        var isCommit = order.InPayments != null && order.InPayments.Any()
		                    && order.InPayments.All(pi => pi.IsApproved);

                        Contact contact = null;
                        if (order.CustomerId != null)
                            contact = _customerSearchService.GetById(order.CustomerId);

		                var request = order.ToAvaTaxRequest(_taxSettings.CompanyCode, contact, isCommit);
		                if (request != null)
		                {
                            slab.docCode = request.DocCode;
                            slab.docType = request.DocType.ToString();
                            slab.customerCode = request.CustomerCode;
                            slab.amount = (double) order.Sum;
		                    slab.isCommit = isCommit;

                            var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
		                    var getTaxResult = taxSvc.GetTax(request);
                            
		                    if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
		                    {
                                //if tax calculation failed create exception with provided error info
		                        var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Summary));
		                        throw new Exception(error);
		                    }
		                    
                                //reset items taxes
                                if (order.Items.Any())
                                    order.Items.ForEach(x =>
                                    {
                                        x.Tax = 0;
                                        x.TaxDetails = null;
                                    });

                                //reset order shipments taxes
                                if (order.Shipments.Any())
                                    order.Shipments.ForEach(x =>
                                    {
                                        x.Tax = 0;
                                        x.TaxDetails = null;
                                    });

                                order.Tax = 0;

		                        foreach (var taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
		                        {
		                            var lineItem = order.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
		                            if (lineItem != null)
		                            {
		                                lineItem.Tax = taxLine.Tax;
                                        if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any(td => !string.IsNullOrEmpty(td.TaxName)))
		                                {
		                                    lineItem.TaxDetails =
		                                        taxLine.TaxDetails.Where(td => !string.IsNullOrEmpty(td.TaxName)).Select(taxDetail => new domainModel.TaxDetail
		                                        {
		                                            Amount = taxDetail.Tax,
		                                            Name = taxDetail.TaxName,
		                                            Rate = taxDetail.Rate
		                                        }).ToList();
		                                }
		                            }
		                            else
		                            {
                                        var shipment = order.Shipments.FirstOrDefault(s => s.Id != null ? s.Id.Equals(taxLine.LineNo) : s.ShipmentMethodCode.Equals(taxLine.LineNo));
		                                if (shipment != null)
		                                {
		                                    shipment.Tax = taxLine.Tax;
                                            if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any(td => !string.IsNullOrEmpty(td.TaxName)))
		                                    {
		                                        shipment.TaxDetails =
		                                            taxLine.TaxDetails.Where(td => !string.IsNullOrEmpty(td.TaxName)).Select(taxDetail => new domainModel.TaxDetail
		                                            {
		                                                Amount = taxDetail.Tax,
		                                                Name = taxDetail.TaxName,
		                                                Rate = taxDetail.Rate
		                                            }).ToList();
		                                    }
		                                }
		                            }

		                        order.Tax = getTaxResult.TotalTax;
		                    }
		                }
		                else
		                {
                            throw new Exception("Failed to create get tax request");
		                }
		            }
		            else
		            {
                        throw new Exception("Failed to create get tax request");
		            }
                })
                .OnError(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.TaxCalculationError)
                .OnSuccess(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.GetSalesInvoiceRequestTime);
		}
	}
}