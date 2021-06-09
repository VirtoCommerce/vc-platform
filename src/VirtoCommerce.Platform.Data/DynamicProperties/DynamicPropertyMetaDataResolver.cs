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
        const int _pageSize = 100;
        public DynamicPropertyMetaDataResolver(
            IDynamicPropertySearchService searchService
            , IPlatformMemoryCache memoryCache)
        {
            _searchService = searchService;
            _memoryCache = memoryCache;
        }

        public async virtual Task<DynamicProperty> GetByNameAsync(string objectType, string propertyName)
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

               var result = new List<DynamicProperty>();

               var criteria = new DynamicPropertySearchCriteria
               {
                   Skip = 0,
                   Take = _pageSize
               };

               var searchResult = await _searchService.SearchDynamicPropertiesAsync(criteria);
               result.AddRange(searchResult.Results);
               var totalCount = searchResult.TotalCount;
               for (var skip = _pageSize; skip < totalCount; skip += _pageSize)
               {
                   criteria.Skip = skip;
                   searchResult = await _searchService.SearchDynamicPropertiesAsync(criteria);
                   result.AddRange(searchResult.Results);
               }
               return result.Distinct().ToDictionary(x => $"{x.ObjectType}__{x.Name}", StringComparer.InvariantCultureIgnoreCase).WithDefaultValue(null);

           });
            return dict[$"{objectType}__{propertyName}"];
        }
    }
}
