using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Client
{
    public class SeoKeywordClient
    {
        #region Cache Constants
        public const string SeoKeywordCacheKey = "SK:S:{0}:{1}:{2}:{3}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository;
        private readonly IAppConfigRepository _appConfigRepository;
        #endregion

        public SeoKeywordClient(IAppConfigRepository appConfigRepository, ICacheRepository cacheRepository)
        {
            _appConfigRepository = appConfigRepository;
            _cacheRepository = cacheRepository;
            _isEnabled = AppConfigConfiguration.Instance.Cache.IsEnabled;
        }

        public SeoUrlKeyword GetSeoKeyword(SeoUrlKeywordTypes type, string language, string keyword = null, string keywordvalue = null)
        {
            if (keyword == null && keywordvalue == null)
            {
                throw new ArgumentNullException("keyword","Keyword or KeywordValue must be provided");
            }
            return CacheHelper.Get(string.Format(SeoKeywordCacheKey, type, language, keyword ?? "", keywordvalue ?? ""),
                () => _appConfigRepository.SeoUrlKeywords
                    .FirstOrDefault(s => language.Equals(s.Language, StringComparison.OrdinalIgnoreCase) && (int)type == s.KeywordType && s.IsActive &&
                    (keyword == null || keyword.Equals(s.Keyword, StringComparison.OrdinalIgnoreCase)) &&
                     (keywordvalue == null || keywordvalue.Equals(s.KeywordValue, StringComparison.OrdinalIgnoreCase))), 
                     AppConfigConfiguration.Instance.Cache.SettingsTimeout, _isEnabled);
        }

        CacheHelper _cacheHelper;
        public CacheHelper CacheHelper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
