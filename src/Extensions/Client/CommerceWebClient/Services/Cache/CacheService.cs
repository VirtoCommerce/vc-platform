using System;
using VirtoCommerce.Foundation.AppConfig.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Web.Client.Caching.Interfaces;

namespace VirtoCommerce.Web.Client.Services.Cache
{
    [UnityInstanceProviderServiceBehavior]
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly IOutputCacheManager _outputCacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        public CacheService() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <param name="outputCacheManager">The output cache manager.</param>
        public CacheService(ICacheRepository cacheRepository, IReadWriteOutputCacheManager outputCacheManager)
            :this()
        {
            _cacheRepository = cacheRepository;
            _outputCacheManager = outputCacheManager;
        }

        public void ClearOuputCache(string controller, string action)
        {
            if (string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
            {
                throw new ArgumentNullException(controller, "Controller must be provided when action not empty");
            }

           _outputCacheManager.RemoveItems(controller, action); 
        }

        public void ClearDatabaseCache(string cachePrefix)
        {
            var cacheKeyPrefix = CacheHelper.CreateCacheKey(cachePrefix ?? "");
            var enumerableCache = _cacheRepository.GetEnumerator();

            while (enumerableCache.MoveNext())
            {
                if (enumerableCache.Key != null)
                {
                    var key = enumerableCache.Key.ToString();
                    if (key.StartsWith(cacheKeyPrefix))
                    {
                        _cacheRepository.Remove(enumerableCache.Key.ToString());
                    }
                }
            }
        }
    }
}
