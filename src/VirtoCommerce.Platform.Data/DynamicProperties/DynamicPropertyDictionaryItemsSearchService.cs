using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public class DynamicPropertyDictionaryItemsSearchService : IDynamicPropertyDictionaryItemsSearchService
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;
        private readonly IDynamicPropertyDictionaryItemsService _dynamicPropertyDictionaryItemsService;

        public DynamicPropertyDictionaryItemsSearchService(Func<IPlatformRepository> repositoryFactory, IPlatformMemoryCache memoryCache, IDynamicPropertyDictionaryItemsService dynamicPropertyDictionaryItemsService)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
            _dynamicPropertyDictionaryItemsService = dynamicPropertyDictionaryItemsService;
        }

        public virtual async Task<DynamicPropertyDictionaryItemSearchResult> SearchDictionaryItemsAsync(DynamicPropertyDictionaryItemSearchCriteria criteria)
        {
            var cacheKey = CacheKey.With(GetType(), "SearchDictionaryItemsAsync", criteria.GetHashCode().ToString());
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(DynamicPropertiesCacheRegion.CreateChangeToken());
                var result = AbstractTypeFactory<DynamicPropertyDictionaryItemSearchResult>.TryCreateInstance();
                using (var repository = _repositoryFactory())
                {
                    //Optimize performance and CPU usage
                    repository.DisableChangesTracking();

                    var query = repository.DynamicPropertyDictionaryItems;

                    if (!string.IsNullOrEmpty(criteria.PropertyId))
                    {
                        query = query.Where(x => x.PropertyId == criteria.PropertyId);
                    }
                    if (!string.IsNullOrEmpty(criteria.Keyword))
                    {
                        query = query.Where(x => x.Name.Contains(criteria.Keyword));
                    }

                    var sortInfos = criteria.SortInfos;
                    if (sortInfos.IsNullOrEmpty())
                    {
                        sortInfos = new[] { new SortInfo { SortColumn = "Name" } };
                    }
                    query = query.OrderBySortInfos(sortInfos);
                    result.TotalCount = await query.CountAsync();
                    var ids = await query.Skip(criteria.Skip)
                                         .Take(criteria.Take)
                                         .Select(x => x.Id)
                                         .ToListAsync();

                    var properties = await _dynamicPropertyDictionaryItemsService.GetDynamicPropertyDictionaryItemsAsync(ids.ToArray());
                    result.Results = properties.OrderBy(x => ids.IndexOf(x.Id))
                                               .ToList();
                }
                return result;
            });
        }
    }
}
