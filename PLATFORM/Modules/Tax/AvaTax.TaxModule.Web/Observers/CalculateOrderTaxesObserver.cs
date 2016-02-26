using System;
using System.Linq;
using Common.Logging;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Store.Services;

namespace AvaTax.TaxModule.Web.Observers
{
    /// <summary>
    /// TODO: deprecated because tax calculation occur by implicit request
    /// Need remove this in future
    /// </summary>
    [Obsolete]
    public class CalculateOrderTaxesObserver : IObserver<OrderChangeEvent>
	{
        private readonly IStoreService _storeService;
        

        public CalculateOrderTaxesObserver(IStoreService storeService)
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
			    CalculateCustomerOrderTaxes(value);
		}

		#endregion
		private void CalculateCustomerOrderTaxes(OrderChangeEvent context)
		{
            //if (context.ModifiedOrder.IsCancelled)
            //{
            //    return;
            //}

            //var order = context.ModifiedOrder;
            //var originalOrder = context.OrigOrder;

            ////do nothing if order Items quantities did not changed
            //if (
            //    originalOrder.Items.Any(
            //        li => !order.Items.Any(oli => oli.Id.Equals(li.Id)) ||
            //            order.Items.Single(oli => li.Id.Equals(oli.Id)).Quantity < li.Quantity))
            //    return;

            //var store = _storeService.GetById(order.StoreId);
            //var taxProvider = store.TaxProviders.FirstOrDefault(x => x.Code == typeof(AvaTaxRateProvider).Name);
            //if (taxProvider != null && taxProvider.IsActive)
            //{                
            //    (taxProvider as AvaTaxRateProvider).CalculateOrderTax(order);
            //}
		}
	}
}