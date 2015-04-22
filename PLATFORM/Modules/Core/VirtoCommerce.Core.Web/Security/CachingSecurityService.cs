using System;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class CachingSecurityService
    {
        public const string CacheTimeout = "VirtoCommerce.Core.Security.CacheTimeout";

        protected CacheHelper Cache { get; private set; }
        protected ISettingsManager SettingsManager { get; private set; }

        public CachingSecurityService(ICacheRepository cacheRepository, ISettingsManager settingsManager)
        {
            SettingsManager = settingsManager;
            Cache = new CacheHelper(cacheRepository, Constants.SecurityCachePrefix);
        }

        protected TimeSpan GetCacheTimeout()
        {
            var value = SettingsManager.GetValue(CacheTimeout, 60);
            return TimeSpan.FromSeconds(value);
        }
    }
}
