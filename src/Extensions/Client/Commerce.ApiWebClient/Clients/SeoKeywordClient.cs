using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;
using VirtoCommerce.Web.Core.Configuration.Application;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiWebClient.Clients
{
    public class SeoKeywordClient
    {
        #region Cache Constants
        public const string AllSeoKeywordCacheKey = "SK:S:ALL";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository;
        #endregion

        public SeoKeywordClient(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
            _isEnabled = AppConfigConfiguration.Instance.Cache.IsEnabled;
        }

        public SeoClient SeoApiClient
        {
            get
            {
                return ClientContext.Clients.CreateDefaultSeoClient("en-us");
            }
        }

        public SeoKeyword[] GetSeoKeywords(SeoUrlKeywordTypes type, string language = null, string keyword = null, string keywordvalue = null)
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
                        && (keywordvalue == null || keywordvalue.Equals(s.KeywordValue, StringComparison.OrdinalIgnoreCase))
                        && type == s.KeywordType).ToArray();
        }

        private IEnumerable<SeoKeyword> GetAllSeoKeywords()
        {
            return CacheHelper.Get(
                CacheHelper.CreateCacheKey(Constants.SeoCachePrefix,AllSeoKeywordCacheKey),
                () => Task.Run(() => SeoApiClient.GetKeywordsAsync()).Result, 
                    AppConfigConfiguration.Instance.Cache.SeoKeywordsTimeout, _isEnabled);
        }

        CacheHelper _cacheHelper;
        public CacheHelper CacheHelper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
