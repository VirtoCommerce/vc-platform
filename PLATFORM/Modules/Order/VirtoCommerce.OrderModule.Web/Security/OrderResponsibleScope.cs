using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.OrderModule.Web.Security
{
    /// <summary>
    /// Scope bounded by order responsible
    /// </summary>
    public class OrderResponsibleScope : PermissionScope
    {
        public OrderResponsibleScope()
        {
            Scope = "{{userId}}";
        }

        public override bool IsScopeAvailableForPermission(string permission)
        {
            return (permission == OrderPredefinedPermissions.Read ||
                permission == OrderPredefinedPermissions.Update);
        }

        public override IEnumerable<string> GetEntityScopeStrings(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            var customerOrder = obj as CustomerOrder;
            if (customerOrder != null)
            {
                return new[] { base.Type + ":" + customerOrder.EmployeeId };
            }
            return Enumerable.Empty<string>();
        }
    }
}
