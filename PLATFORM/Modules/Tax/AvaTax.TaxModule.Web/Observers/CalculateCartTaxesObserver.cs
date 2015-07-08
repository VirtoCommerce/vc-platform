using System;
using System.Linq;
using AvaTax.TaxModule.Web.Converters;
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
			var cart = context.ModifiedCart;

		    if (cart.Items.Any())
		    {
		        cart.Items.ForEach(x => {
		            x.TaxTotal = 0;
                    x.TaxDetails = null;
		        });
		    }
            
		    if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
		    {
		        Contact contact = null;
                if (cart.CustomerId != null)
		            contact = _customerSearchService.GetById(cart.CustomerId);

                var request = cart.ToAvaTaxRequest(_taxSettings.CompanyCode, contact);
                if (request != null)
                {
                    //var store = _storeService.GetById(cart.StoreId);
                    
                    //if (store != null)
                    //{
                    //    request.Addresses.AddDistinct(new Address { AddressCode = "origin", Country = store.Country, Region = store.Region });
                    //    request.Lines.ForEach(l => l.OriginCode = "origin");
                    //}

                    var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);

                    var getTaxResult = taxSvc.GetTax(request);
                    if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                    {
                        var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Summary));
                        OnError(new Exception(error));
                    }
                    else
                    {
                        foreach (TaxLine taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                        {
                            var lineItem = cart.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
                            if (lineItem != null)
                            {
                                lineItem.TaxTotal = taxLine.Tax;
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
                                var shipment = cart.Shipments.FirstOrDefault(s => s.Id.Equals(taxLine.LineNo));
                                if (shipment != null)
                                {
                                    shipment.TaxTotal = taxLine.Tax;
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
                    }
                }
            }
            else
            {
                OnError(new Exception("Tax calculation disabled or credentials not provided"));
            }
		}
    }
}