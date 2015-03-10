using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class ApiAccountProvider : CachingSecurityService, IApiAccountProvider
    {
        private readonly Func<ISecurityRepository> _securityRepository;

        public ApiAccountProvider(Func<ISecurityRepository> securityRepository, ICacheRepository cacheRepository, ISettingsManager settingsManager)
            : base(cacheRepository, settingsManager)
        {
            _securityRepository = securityRepository;
        }

        #region IApiAccountProvider Members

        public ApiAccount GetAccountByAppId(string appId)
        {
            return Cache.Get(
                Cache.CreateKey("ApiAccount", appId),
                () => LoadApiAccount(appId),
                GetCacheTimeout());
        }

        public ApiAccount LoadApiAccount(string appId)
        {
            using (var repository = _securityRepository())
            {
                var apiAccount = repository.ApiAccounts.FirstOrDefault(c => c.IsActive && c.AppId == appId);
                return apiAccount;
            }
        }

        #endregion
    }
}
