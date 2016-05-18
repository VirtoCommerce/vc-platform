using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.StoreModule.Web.Security
{
    /// <summary>
    /// Restricted to permission within selected stores
    /// </summary>
    public class StoreSelectedScope : PermissionScope
    {
        public override bool IsScopeAvailableForPermission(string permission)
        {
            return permission == StorePredefinedPermissions.Read
                      || permission == StorePredefinedPermissions.Update;
        }

        public override IEnumerable<string> GetEntityScopeStrings(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            string storeId = null;
            var coreStore = obj as Store;
            var webModelStore = obj as Model.Store;
            if (coreStore != null)
            {
                storeId = coreStore.Id;
            }
            if (webModelStore != null)
            {
                storeId = webModelStore.Id;
            }

            if (storeId != null)
                return new[] { Type + ":" + storeId };

            return Enumerable.Empty<string>();
        }
    }
}
