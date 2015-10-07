using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class PermissionScopeService : IPermissionScopeService
    {
        private List<Func<PermissionScope>> _scopeFactories = new List<Func<PermissionScope>>();
      
        #region ISecurityScopeService Members
        public IEnumerable<PermissionScope> GetAvailablePermissionScopes(string permission)
        {
            return _scopeFactories.Select(x => x()).Where(x=>x.IsScopeAvailableForPermission(permission)).ToArray();
        }

        public PermissionScope GetScopeByTypeName(string typeName)
        {
            return _scopeFactories.Select(x => x()).FirstOrDefault(x => String.Equals(x.Type, typeName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<string> GetObjectPermissionScopeStrings(object obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            return _scopeFactories.SelectMany(x => x().GetEntityScopeStrings(obj)).Where(x => !String.IsNullOrEmpty(x));
        }

        public void RegisterSope(Func<PermissionScope> scopeFactory)
        {
            if (scopeFactory == null)
            {
                throw new ArgumentNullException("scopeFactory");
            }
            _scopeFactories.Add(scopeFactory);
        }
        #endregion
    }
}
