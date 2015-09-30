using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.OrderModule.Web.Security
{
    public class OrderResponsibleScope : PermissionScope
    {
        public OrderResponsibleScope()
        {
            Scope = "order:employee:{{userId}}";
        }

        public OrderResponsibleScope(CustomerOrder order)
            :this()
        {
            Scope = "order:employee:" + order.EmployeeId;
        }

        public static OrderResponsibleScope TryParse(string scope)
        {
            if (scope.StartsWith("order:employee:"))
            {
                return new OrderResponsibleScope();
            }
            return null;
        }

        public override string ToString()
        {
            return Scope;
        }

    }
}
