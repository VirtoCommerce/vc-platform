using System;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class CachingSecurityService
    {
        protected CacheHelper Cache { get; private set; }
        protected ISettingsManager SettingsManager { get; private set; }

        public CachingSecurityService(ICacheRepository cacheRepository, ISettingsManager settingsManager)
        {
            SettingsManager = settingsManager;
            Cache = new CacheHelper(cacheRepository, Constants.SecurityCachePrefix);
        }

        protected TimeSpan GetCacheTimeout()
        {
            var value = SettingsManager.GetValue("VirtoCommerce.Core.Security.CacheTimeout", 60);
            return TimeSpan.FromSeconds(value);
        }
    }
}
