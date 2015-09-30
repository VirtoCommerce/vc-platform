using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.OrderModule.Web.Security
{
    public class OrderStoreScope : PermissionScope
    {

        public OrderStoreScope()
        {
            Scope = "order:store";
        }

        public OrderStoreScope(CustomerOrder order)
            :this()
        {
            Scope += ":" + order.StoreId;
        }
        private OrderStoreScope(string storeId)
        {
            StoreId = storeId;
        }

        public string StoreId { get; set; }

        public static OrderStoreScope TryParse(string scope)
        {
            if (scope.StartsWith("order:store:"))
            {
                return new OrderStoreScope(scope.Substring("order:store:".Length));
            }
            return null;
        }

        public override string ToString()
        {
            return Scope;
        }
    }
}
