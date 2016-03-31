using System;
using System.Linq;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Store.Services;

namespace AvaTax.TaxModule.Web.Observers
{
    /// <summary>
    /// TODO: deprecated because tax calculation occur by implicit request
    /// Need remove this in future
    /// </summary>
    [Obsolete]
    public class CalculateCartTaxesObserver : IObserver<CartChangeEvent>
	{
        private readonly IStoreService _storeService;

        public CalculateCartTaxesObserver(IStoreService storeService)
        {
            _storeService = storeService;
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
            var store = _storeService.GetById(cart.StoreId);
            var taxProvider = store.TaxProviders.FirstOrDefault(x => x.Code == typeof(AvaTaxRateProvider).Name);
            if (taxProvider != null && taxProvider is AvaTaxRateProvider && taxProvider.IsActive)
            {
                (taxProvider as AvaTaxRateProvider).CalculateCartTax(cart);
            }
        }
    }
}