using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class PermissionScopeService : IPermissionScopeService
    {
        private List<Func<IPermissionScopeProvider>> _scopeProvidersFactory = new List<Func<IPermissionScopeProvider>>();
      
        #region ISecurityScopeService Members
        public IEnumerable<PermissionScope> GetPermissionScopes(string permission)
        {
            return _scopeProvidersFactory.SelectMany(x => x().GetPermissionScopes(permission)).OfType<PermissionScope>().ToArray();
        }

        public void RegisterSopeProvider(Func<IPermissionScopeProvider> providerFactory)
        {
            if (providerFactory == null)
            {
                throw new ArgumentNullException("providerFactory");
            }
            _scopeProvidersFactory.Add(providerFactory);
        }
        #endregion
    }
}
