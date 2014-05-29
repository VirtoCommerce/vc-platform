using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Repositories;

namespace VirtoCommerce.Scheduling.Jobs
{
    /// <summary>
    /// Processes (update) newly created order statuses.
    /// </summary>
    public class ProcessOrderStatusWork : IJobActivity
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IOrderRepository _orderRepository;
        public ProcessOrderStatusWork(IStoreRepository storeRepository, IOrderRepository orderRepository)
        {
            _storeRepository = storeRepository;
            _orderRepository = orderRepository;
        }

        public void Execute(IJobContext context)
        {
                var currentTime = DateTime.UtcNow;
                var statusFilter = OrderStatus.Pending.ToString();

                using (_storeRepository)
                using (_orderRepository)
                {
                    var stores = _storeRepository.Stores.Expand(x => x.FulfillmentCenter).ToList();

                    var items = _orderRepository.Orders
                        .Where(x => x.Status == statusFilter)
                        .Expand("OrderForms/Shipments")
                        .ToArray();

                    foreach (var item in items)
                    {
                        var store = stores.FirstOrDefault(x => x.StoreId == item.StoreId);
                        if (store != null
                            && store.FulfillmentCenter != null
                            && item.Created.HasValue
                            && item.Created.Value.AddMinutes(store.FulfillmentCenter.PickDelay) < currentTime)
                        {
                            item.Status = OrderStatus.InProgress.ToString();
                            if (item.OrderForms.Any())
                            {
                                item.OrderForms[0].Shipments.ToList()
                                    .ForEach(x => x.Status = ShipmentStatus.InventoryAssigned.ToString());
                            }
                            _orderRepository.UnitOfWork.Commit();
                        }
                    }
                }
            }
        }
}
