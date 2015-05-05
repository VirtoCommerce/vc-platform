using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using Zapier.IntegrationModule.Web.Providers.Interfaces;

namespace Zapier.IntegrationModule.Web.Providers.Implementations
{
    public class OrdersProvider: IOrdersProvider
    {
        private readonly ICustomerOrderSearchService _orderService;

        public OrdersProvider(ICustomerOrderSearchService orderService)
        {
            _orderService = orderService;
        }
        
        public IEnumerable<CustomerOrder> GetNewOrders()
        {
            return _orderService.Search(new SearchCriteria()).CustomerOrders.OrderByDescending(o => o.CreatedDate);
        }

        public IEnumerable<CustomerOrder> GetNewOrdersWithLineItems()
        {
            return _orderService.Search(new SearchCriteria()).CustomerOrders.OrderByDescending(o => o.CreatedDate);
        }
    }
}