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
            //SlabInvoker<TaxEventSource.TaxRequestContext>.Execute(slab =>
            //    {
		            var order = context.ModifiedOrder;

		            if (order.Items.Any())
		                order.Items.ForEach(x =>
		                {
		                    x.Tax = 0;
		                    x.TaxDetails = null;
		                });

		            order.Tax = 0;

		            if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username)
		                && !string.IsNullOrEmpty(_taxSettings.Password)
		                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
		                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
		            {
		                var taxSvc = new JsonTaxSvc(_taxSettings.Username,
		                    _taxSettings.Password,
		                    _taxSettings.ServiceUrl);
		                var isCommit = order.InPayments != null && order.InPayments.Any()
		                    && order.InPayments.All(pi => pi.IsApproved);

                        Contact contact = null;
                        if (order.CustomerId != null)
                            contact = _customerSearchService.GetById(order.CustomerId);

		                var request = order.ToAvaTaxRequest(_taxSettings.CompanyCode, contact, isCommit);
		                if (request != null)
		                {
                            //slab.DocCode = request.DocCode;
                            //slab.CustomerCode = request.CustomerCode;
                            //slab.Amount = order.Sum;

                            //TaxEventSource.Log.Write(TaxEventSource.EventCodes.LogRequestData, slab);

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
		                            var lineItem = order.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
		                            if (lineItem != null)
		                            {
		                                lineItem.Tax = taxLine.Tax;
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
		                            else
		                            {
		                                var shipment = order.Shipments.FirstOrDefault(s => s.Id.Equals(taxLine.LineNo));
		                                if (shipment != null)
		                                {
		                                    shipment.Tax = taxLine.Tax;
		                                    if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any())
		                                    {
		                                        shipment.TaxDetails =
		                                            taxLine.TaxDetails.Select(taxDetail => new domainModel.TaxDetail
		                                            {
		                                                Amount = taxDetail.Tax,
		                                                Name = taxDetail.TaxName,
		                                                Rate = taxDetail.Rate
		                                            }).ToList();
		                                    }
		                                }
		                            }
		                        }
		                        order.Tax = getTaxResult.TotalTax;
		                    }
		                }
		            }
		            else
		            {
		                OnError(new Exception("AvaTax credentials not provided"));
		            }
                //})
                //.OnError(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.ApplicationError)
                //.OnSuccess(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.Startup);
		}
	}
}