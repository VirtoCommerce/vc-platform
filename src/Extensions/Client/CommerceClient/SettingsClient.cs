using System;
using System.Linq;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Client
{
    public class SettingsClient
    {
        #region Cache Constants
        public const string SettingsCacheKey = "SS:S:{0}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository;
        private readonly IAppConfigRepository _appConfigRepository;
        #endregion

        public SettingsClient(IAppConfigRepository appConfigRepository, ICacheRepository cacheRepository)
        {
            _appConfigRepository = appConfigRepository;
            _cacheRepository = cacheRepository;
            _isEnabled = AppConfigConfiguration.Instance.Cache.IsEnabled;
        }

        public SettingValue[] GetSettings(string settingName)
        {
            return CacheHelper.Get(
                CacheHelper.CreateCacheKey(Constants.SettingsCachePrefix,string.Format(SettingsCacheKey, settingName)),
                () => _appConfigRepository.Settings
                    .Where(s => settingName.Equals(s.Name, StringComparison.OrdinalIgnoreCase))
                    .SelectMany(s => s.SettingValues).ToArray(), AppConfigConfiguration.Instance.Cache.SettingsTimeout, _isEnabled);
        }

        CacheHelper _cacheHelper;
        public CacheHelper CacheHelper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
