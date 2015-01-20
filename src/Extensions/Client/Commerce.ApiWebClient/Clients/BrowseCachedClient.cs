using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiWebClient.Clients
{
    using System.Net.Http;

    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Caching;
    using VirtoCommerce.ApiWebClient.Caching.Interfaces;
    using VirtoCommerce.Web.Core.Configuration.Catalog;

    using CacheHelper = VirtoCommerce.ApiWebClient.Caching.CacheHelper;

    public class BrowseCachedClient : BrowseClient
    {
        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository = new HttpCacheRepository();
        #endregion

        public BrowseCachedClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, token)
        {
        }

        public BrowseCachedClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        protected override Task<T> GetAsync<T>(Uri requestUri, string userId = null)
        {
            // TODO: vary cache timeout based on url requested, since we know the exact URI for each resource
            return Helper.GetAsync(requestUri.ToString(),
                () => base.GetAsync<T>(requestUri, userId),
                CatalogConfiguration.Instance.Cache.ItemTimeout,
                true);
        }

        CacheHelper _cacheHelper;
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
