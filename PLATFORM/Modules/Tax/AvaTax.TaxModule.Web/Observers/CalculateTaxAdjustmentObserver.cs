using System;
using System.Linq;
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
    public class CalculateTaxAdjustmentObserver : IObserver<OrderChangeEvent>
    {
        private readonly IStoreService _storeService;

        public CalculateTaxAdjustmentObserver(IStoreService storeService)
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
            var store = _storeService.GetById(modifiedOrder.StoreId);
            var taxProvider = store.TaxProviders.FirstOrDefault(x => x.Code == typeof(AvaTaxRateProvider).Name);
            if (taxProvider != null && taxProvider is AvaTaxRateProvider && taxProvider.IsActive)
            {
                (taxProvider as AvaTaxRateProvider).AdjustOrderTax(originalOrder, modifiedOrder);
            }
        }
    }
}