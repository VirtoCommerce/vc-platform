using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Client
{
    public class SeoKeywordClient
    {
        #region Cache Constants
        public const string AllSeoKeywordCacheKey = "SK:S:ALL";
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

        public SeoUrlKeyword[] GetSeoKeywords(SeoUrlKeywordTypes type, string language = null, string keyword = null, string keywordvalue = null)
        {
            if (keyword == null && keywordvalue == null)
            {
                throw new ArgumentNullException("keyword","Keyword or KeywordValue must be provided");
            }

            var allKeywords = GetAllSeoKeywords();

            return
                allKeywords.Where(
                        s =>
                        (language == null || language.Equals(s.Language, StringComparison.OrdinalIgnoreCase))
                        && (keyword == null || keyword.Equals(s.Keyword, StringComparison.OrdinalIgnoreCase))
                        && (keywordvalue == null
                            || keywordvalue.Equals(s.KeywordValue, StringComparison.OrdinalIgnoreCase))
                        && (int)type == s.KeywordType && s.IsActive).ToArray();
        }

        private IEnumerable<SeoUrlKeyword> GetAllSeoKeywords()
        {
            return CacheHelper.Get(
                CacheHelper.CreateCacheKey(Constants.SeoCachePrefix,AllSeoKeywordCacheKey),
                () => _appConfigRepository.SeoUrlKeywords
                    .Where(s => s.IsActive).ToArray(), AppConfigConfiguration.Instance.Cache.SeoKeywordsTimeout, _isEnabled);
        }

        CacheHelper _cacheHelper;
        public CacheHelper CacheHelper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
