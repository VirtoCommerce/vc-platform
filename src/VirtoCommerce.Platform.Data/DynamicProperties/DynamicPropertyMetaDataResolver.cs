using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public class DynamicPropertyMetaDataResolver : IDynamicPropertyMetaDataResolver
    {
        private readonly IDynamicPropertySearchService _searchService;
        private readonly IPlatformMemoryCache _memoryCache;
        const int _pageSize = 100;
        public DynamicPropertyMetaDataResolver(
            IDynamicPropertySearchService searchService
            , IPlatformMemoryCache memoryCache)
        {
            _searchService = searchService;
            _memoryCache = memoryCache;
        }

        public virtual async Task<DynamicProperty> GetByNameAsync(string objectType, string propertyName)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var cacheKey = CacheKey.With(GetType(), nameof(GetByNameAsync));
            var dict = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(DynamicPropertiesCacheRegion.CreateChangeToken());

                var criteria = new DynamicPropertySearchCriteria
                {
                    Skip = 0,
                    Take = _pageSize
                };

                var result = await _searchService.SearchAllNoCloneAsync(criteria);

                return result.Distinct().ToDictionary(x => $"{x.ObjectType}__{x.Name}", StringComparer.OrdinalIgnoreCase).WithDefaultValue(null);

            });
            return dict[$"{objectType}__{propertyName}"];
        }
    }
}
