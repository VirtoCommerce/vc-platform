using System;
using System.Linq;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Platform.Core.Common;

namespace AvaTax.TaxModule.Web.Observers
{
    public class CalculateCartTaxesObserver : IObserver<CartChangeEvent>
	{
        private readonly ITaxSettings _taxSettings;
        //private readonly ICatalogSearchService _catalogSearchService;

        public CalculateCartTaxesObserver(ITaxSettings taxSettings)
        {
            _taxSettings = taxSettings;
            //_catalogSearchService = catalogService;
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
                var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var request = cart.ToAvaTaxRequest(_taxSettings.CompanyCode);
                if (request != null)
                {
                    //var store = _storeService.GetById(cart.StoreId);
                    
                    //if (store != null)
                    //{
                    //    request.Addresses.AddDistinct(new Address { AddressCode = "origin", Country = store.Country, Region = store.Region });
                    //    request.Lines.ForEach(l => l.OriginCode = "origin");
                    //}
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
                            cart.Items.ToArray()[Int32.Parse(taxLine.LineNo)].TaxTotal = taxLine.Tax;
                            //foreach (TaxDetail taxDetail in taxLine.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                            //{
                            //    cart.Items.ToArray()[Int32.Parse(taxLine.LineNo)].TaxDetails = new[]
                            //    {
                            //        new VirtoCommerce.Domain.Commerce.Model.TaxDetail
                            //        {
                            //            Amount = taxDetail.Tax,
                            //            Name = taxDetail.TaxName,
                            //            Rate = taxDetail.Rate
                            //        }
                            //    };
                            //}
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