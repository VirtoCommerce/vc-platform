using System;
using System.Linq;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Store.Services;

namespace AvaTax.TaxModule.Web.Observers
{
    public class CancelOrderTaxesObserver : IObserver<OrderChangeEvent>
	{
        private readonly IStoreService _storeService;

        public CancelOrderTaxesObserver(IStoreService storeService)
        {
            _storeService = storeService;
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
			    CancelCustomerOrderTaxes(value);
		}

		#endregion
		private void CancelCustomerOrderTaxes(OrderChangeEvent context)
		{
            if (!context.ModifiedOrder.IsCancelled)
            {
                return;
            }

            var order = context.ModifiedOrder;
            var store = _storeService.GetById(order.StoreId);
            var taxProvider = store.TaxProviders.FirstOrDefault(x => x.Code == typeof(AvaTaxRateProvider).Name);
            if (taxProvider != null && taxProvider is AvaTaxRateProvider && taxProvider.IsActive)
            {
                (taxProvider as AvaTaxRateProvider).CancelTaxDocument(order);
            }
        }
	}
}