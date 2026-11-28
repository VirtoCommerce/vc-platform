using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public class DynamicPropertyDictionaryItemsService : IDynamicPropertyDictionaryItemsService
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;

        public DynamicPropertyDictionaryItemsService(Func<IPlatformRepository> repositoryFactory, IPlatformMemoryCache memoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
        }

        public virtual Task<DynamicPropertyDictionaryItem[]> GetDynamicPropertyDictionaryItemsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), "GetDynamicPropertyDictionaryItemsAsync", string.Join("-", ids));
            return _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(DynamicPropertiesCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    //Optimize performance and CPU usage
                    repository.DisableChangesTracking();

                    var result = await repository.GetDynamicPropertyDictionaryItemByIdsAsync(ids);
                    return result.Select(x => x.ToModel(AbstractTypeFactory<DynamicPropertyDictionaryItem>.TryCreateInstance())).ToArray();
                }
            });
        }

        public virtual async Task SaveDictionaryItemsAsync(DynamicPropertyDictionaryItem[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Any(x => string.IsNullOrEmpty(x.Name)))
            {
                throw new InvalidCollectionItemException("One or more items in the collection have a null or empty Name.");
            }

            var savedEntities = new List<DynamicPropertyDictionaryItemEntity>();
            using (var repository = _repositoryFactory())
            {
                var dbExistItems = await repository.GetDynamicPropertyDictionaryItemByIdsAsync(items.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var item in items)
                {
                    var originalEntity = dbExistItems.FirstOrDefault(x => x.Id == item.Id);
                    var modifiedEntity = AbstractTypeFactory<DynamicPropertyDictionaryItemEntity>.TryCreateInstance().FromModel(item);
                    if (originalEntity != null)
                    {
                        modifiedEntity.Patch(originalEntity);
                        savedEntities.Add(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                        savedEntities.Add(modifiedEntity);
                    }
                }
                await repository.UnitOfWork.CommitAsync();

                for (var i = 0; i < items.Length; i++)
                {
                    SyncGeneratedValues(savedEntities[i], items[i]);
                }

                DynamicPropertiesCacheRegion.ExpireRegion();
            }

        }

        protected virtual void SyncGeneratedValues(DynamicPropertyDictionaryItemEntity entity, DynamicPropertyDictionaryItem model)
        {
            model.CreatedDate = entity.CreatedDate;
            model.CreatedBy = entity.CreatedBy;
            model.ModifiedDate = entity.ModifiedDate;
            model.ModifiedBy = entity.ModifiedBy;
        }

        public virtual async Task DeleteDictionaryItemsAsync(string[] itemIds)
        {
            if (itemIds == null)
            {
                throw new ArgumentNullException(nameof(itemIds));
            }

            using (var repository = _repositoryFactory())
            {
                var items = repository.DynamicPropertyDictionaryItems
                    .Where(v => itemIds.Contains(v.Id))
                    .ToList();

                foreach (var item in items)
                {
                    repository.Remove(item);
                }

                await repository.UnitOfWork.CommitAsync();

                DynamicPropertiesCacheRegion.ExpireRegion();
            }
        }
    }
}
