using System.Collections.Generic;
using VirtoCommerce.Domain.Order.Model;

namespace Zapier.IntegrationModule.Web.Providers.Interfaces
{
    public interface IOrdersProvider
    {
        IEnumerable<CustomerOrder> GetNewOrders();
        IEnumerable<CustomerOrder> GetNewOrdersWithLineItems();
    }
}