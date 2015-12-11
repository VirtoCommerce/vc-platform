using System;
using System.Collections.Generic;
using System.Linq;
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

            var store = obj as Model.Store;
            if (store != null)
                return new[] { Type + ":" + store.Id };

            return Enumerable.Empty<string>();
        }
    }
}
