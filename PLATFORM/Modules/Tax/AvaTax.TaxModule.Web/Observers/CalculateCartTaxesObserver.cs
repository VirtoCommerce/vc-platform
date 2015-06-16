using System;
using System.Linq;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Common.Events;

namespace AvaTax.TaxModule.Web.Observers
{
    public class CalculateCartTaxesObserver : IObserver<CartChangeEvent>, IPriorityObserver
	{
        private readonly ITax _taxSettings;

        public CalculateCartTaxesObserver(ITax taxSettings)
        {
            _taxSettings = taxSettings;
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
			CalculateCustomerOrderTaxes(value);
		}

		#endregion
		private void CalculateCustomerOrderTaxes(CartChangeEvent context)
		{
			var cart = context.ModifiedCart;

            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
            {
                var taxSvc = new TaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var request = cart.ToAvaTaxRequest(_taxSettings.CompanyCode);
                var getTaxResult = taxSvc.GetTax(request);
                if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                {
                    var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Details));
                    OnError(new Exception(error));
                }
                else
                {
                    foreach (TaxLine taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                    {
                        cart.Items.ToArray()[Int32.Parse(taxLine.LineNo)].TaxTotal = taxLine.Tax;
                        //foreach (TaxDetail taxDetail in taxLine.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                        //{
                        //}
                    }
                    cart.TaxTotal = getTaxResult.TotalTax;
                }
            }
            else
            {
                OnError(new Exception("AvaTax credentials not provided"));
            }
		}



        public int Priority
        {
            get { return 100; }
        }
    }
}