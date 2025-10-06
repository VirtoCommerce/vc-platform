using System;
using System.Collections.Generic;
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

        private const int _pageSize = 100;

        public DynamicPropertyMetaDataResolver(
            IDynamicPropertySearchService searchService,
            IPlatformMemoryCache memoryCache)
        {
            _searchService = searchService;
            _memoryCache = memoryCache;
        }

        public virtual async Task<DynamicProperty> GetByNameAsync(string objectType, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(objectType);
            ArgumentNullException.ThrowIfNull(propertyName);

            var dict = await GetAllPropertiesByTypeAndName();

            if (dict.TryGetValue(objectType, out var innerDict))
            {
                if (innerDict.TryGetValue(propertyName, out var dynamicProperty))
                {
                    return dynamicProperty;
                }
            }

            return null;
        }

        public virtual async Task<IList<DynamicProperty>> SearchAllAsync(string objectType)
        {
            ArgumentNullException.ThrowIfNull(objectType);

            var dict = await GetAllPropertiesByTypeAndName();

            if (dict.TryGetValue(objectType, out var innerDict))
            {
                return innerDict.Values.ToList();
            }

            return Array.Empty<DynamicProperty>();
        }

        private Task<Dictionary<string, Dictionary<string, DynamicProperty>>> GetAllPropertiesByTypeAndName()
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetAllPropertiesByTypeAndName));

            return _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.AddExpirationToken(DynamicPropertiesCacheRegion.CreateChangeToken());

                return await GetAllPropertiesByTypeAndNameNoCache();
            });
        }

        private async Task<Dictionary<string, Dictionary<string, DynamicProperty>>> GetAllPropertiesByTypeAndNameNoCache()
        {
            var criteria = new DynamicPropertySearchCriteria
            {
                Skip = 0,
                Take = _pageSize
            };

            var result = await _searchService.SearchAllNoCloneAsync(criteria);

            var outer = result
                .Distinct()
                .GroupBy(x => x.ObjectType, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase));

            return outer;
        }
    }
}
