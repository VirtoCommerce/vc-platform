using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Content.Web.Security
{
    /// <summary>
    /// Restricted to permission within selected stores
    /// </summary>
    public class ContentSelectedStoreScope : PermissionScope
    {
        public override bool IsScopeAvailableForPermission(string permission)
        {
            return permission == ContentPredefinedPermissions.Read
                      || permission == ContentPredefinedPermissions.Update
                      || permission == ContentPredefinedPermissions.Create
                      || permission == ContentPredefinedPermissions.Delete;
        }

        public override IEnumerable<string> GetEntityScopeStrings(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var contentScopeObj = obj as ContentScopeObject;
            if (contentScopeObj != null)
                return new [] { Type + ":" + contentScopeObj.StoreId };

            return Enumerable.Empty<string>(); ;
        }
    }
}