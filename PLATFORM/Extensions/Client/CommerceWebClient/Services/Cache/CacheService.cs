using MvcSiteMapProvider;
using System;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.AppConfig.Services;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Web.Client.Caching.Interfaces;

namespace VirtoCommerce.Web.Client.Services.Cache
{
    [UnityInstanceProviderServiceBehavior]
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly IOutputCacheManager _outputCacheManager;
        private readonly StoreClient _storeClient;
        private readonly ICustomerSessionService _sessionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        public CacheService() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <param name="outputCacheManager">The output cache manager.</param>
        /// <param name="storeClient"></param>
        /// <param name="sessionService"></param>
        public CacheService(ICacheRepository cacheRepository, 
            IReadWriteOutputCacheManager outputCacheManager, 
            ICustomerSessionService sessionService,
            StoreClient storeClient):this()
        {
            _cacheRepository = cacheRepository;
            _outputCacheManager = outputCacheManager;
            _storeClient = storeClient;
            _sessionService = sessionService;
        }

        public int ClearOuputCache(string controller, string action)
        {
            if (string.IsNullOrEmpty(controller))
            {
                if (!string.IsNullOrEmpty(action))
                {
                    throw new ArgumentNullException(controller, "Controller must be provided when action not empty");
                }

                //Relase site map cache on clear all output cache for all stores
                foreach (var store in _storeClient.GetStores())
                {
                    //Need to set storeId for session because SiteMapStoreCacheKeyGenerator uses it to generate cache key
                    _sessionService.CustomerSession.StoreId = store.StoreId;
                    SiteMaps.ReleaseSiteMap();
                }
            }


           return _outputCacheManager.RemoveItems(controller, action); 
        }

        public int ClearDatabaseCache(string cachePrefix)
        {
            var cacheKeyPrefix = CacheHelper.CreateCacheKey(cachePrefix ?? "");
            var enumerableCache = _cacheRepository.GetEnumerator();
            var count = 0;

            while (enumerableCache.MoveNext())
            {
                if (enumerableCache.Key != null)
                {
                    var key = enumerableCache.Key.ToString();
                    if (key.StartsWith(cacheKeyPrefix))
                    {
                        count++;
                        _cacheRepository.Remove(enumerableCache.Key.ToString());
                    }
                }
            }
            return count;
        }
    }
}
