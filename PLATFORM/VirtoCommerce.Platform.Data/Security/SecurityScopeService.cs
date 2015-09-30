using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class SecurityScopeService : IPermissionScopeService
    {
        private List<Func<IPermissionScopeProvider>> _scopeProvidersFactory = new List<Func<IPermissionScopeProvider>>();
      
        #region ISecurityScopeService Members
        public IEnumerable<IPermissionScopeProvider> GetAllScopeProviders()
        {
            return _scopeProvidersFactory.Select(x => x());
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
