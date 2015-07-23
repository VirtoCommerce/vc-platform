using System;
using System.Linq;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Logging;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using domainModel = VirtoCommerce.Domain.Commerce.Model;
using Microsoft.Practices.ObjectBuilder2;

namespace AvaTax.TaxModule.Web.Observers
{
    public class CalculateCartTaxesObserver : IObserver<CartChangeEvent>
	{
        private readonly ITaxSettings _taxSettings;
        private readonly IContactService _customerSearchService;

        public CalculateCartTaxesObserver(ITaxSettings taxSettings, IContactService customerSearchService)
        {
            _taxSettings = taxSettings;
            _customerSearchService = customerSearchService;
        }

		#region IObserver<ShoppingCart> Members

		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(CartChangeEvent value)
		{
            if (value.ChangeState == EntryState.Modified)
			    CalculateCustomerOrderTaxes(value);
		}

		#endregion

		private void CalculateCustomerOrderTaxes(CartChangeEvent context)
		{
            SlabInvoker<VirtoCommerceEventSource.TaxRequestContext>.Execute(slab =>
                {
		            if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                        && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                        && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
		            {
                        var cart = context.ModifiedCart;
		                Contact contact = null;
                        if (cart.CustomerId != null)
		                    contact = _customerSearchService.GetById(cart.CustomerId);

                        var request = cart.ToAvaTaxRequest(_taxSettings.CompanyCode, contact);
                        if (request != null)
                        {
                            slab.docCode = request.DocCode;
                            slab.customerCode = request.CustomerCode;
                            slab.docType = request.DocType.ToString();
                            slab.amount = (double) cart.Total;
                    
                            var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                            var getTaxResult = taxSvc.GetTax(request);

                            if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                            {
                                //if tax calculation failed create exception with provided error info
                                var error = string.Join(Environment.NewLine,
                                    getTaxResult.Messages.Select(m => m.Summary));
                                throw new Exception(error);
                            }

                                //reset all cart items taxes
                                if (cart.Items.Any())
                                {
                                    cart.Items.ForEach(x =>
                                    {
                                        x.TaxTotal = 0;
                                        x.TaxDetails = null;
                                    });
                                }

                                //reset all cart shipments taxes
                                if (cart.Shipments.Any())
                                {
                                    cart.Shipments.ForEach(x =>
                                    {
                                        x.TaxTotal = 0;
                                        x.TaxDetails = null;
                                    });
                                }

                                foreach (var taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                                {
                                    var lineItem = cart.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
                                    if (lineItem != null)
                                    {
                                        lineItem.TaxTotal = taxLine.Tax;
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
                                        var shipment = cart.Shipments.FirstOrDefault(s => s.Id != null ? s.Id.Equals(taxLine.LineNo) : s.ShipmentMethodCode.Equals(taxLine.LineNo));
                                        if (shipment != null)
                                        {
                                            shipment.TaxTotal = taxLine.Tax;
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
                                }
                        }
                        else
                        {
                            throw new Exception("Failed to create get tax request");
                        }
                    }
                    else
                    {
                        throw new Exception("Tax calculation disabled or credentials not provided");
                    }
                })
                .OnError(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.TaxCalculationError)
                .OnSuccess(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.GetTaxRequestTime);
		}
    }
}