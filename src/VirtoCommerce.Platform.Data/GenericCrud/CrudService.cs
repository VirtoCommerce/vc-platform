using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Caching.GenericCrud;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.GenericCrud
{
    public abstract class CrudService<TModel, TEntity, TChangeEvent, TChangedEvent> : ICrudService<TModel>
        where TModel : Entity, ICloneable
        where TEntity : Entity, IDataEntity<TEntity, TModel>
        where TChangeEvent : GenericChangedEntryEvent<TModel>
        where TChangedEvent : GenericChangedEntryEvent<TModel>
    {
        protected readonly IEventPublisher _eventPublisher;
        protected readonly IPlatformMemoryCache _platformMemoryCache;
        protected readonly Func<IRepository> _repositoryFactory;


        protected CrudService(IEventPublisher eventPublisher, IPlatformMemoryCache platformMemoryCache, Func<IRepository> repositoryFactory)
        {
            _eventPublisher = eventPublisher;
            _platformMemoryCache = platformMemoryCache;
            _repositoryFactory = repositoryFactory;
        }

        public virtual async Task<TModel> GetByIdAsync(string id, string responseGroup = null)
        {
            var entities = await GetByIdsAsync(new[] { id }, responseGroup);
            return entities.FirstOrDefault();
        }

        public virtual async Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<string> ids, string responseGroup = null)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", ids), responseGroup);
            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var models = new List<TModel>();

                using (var repository = _repositoryFactory())
                {
                    //Disable DBContext change tracking for better performance 
                    repository.DisableChangesTracking();

                    //It is so important to generate change tokens for all ids even for not existing objects to prevent an issue
                    //with caching of empty results for non - existing objects that have the infinitive lifetime in the cache
                    //and future unavailability to create objects with these ids.
                    cacheEntry.AddExpirationToken(CreateCacheToken(ids));

                    var entities = await LoadEntities(repository, ids, responseGroup);

                    foreach (var entity in entities)
                    {
                        var model = entity.ToModel(AbstractTypeFactory<TModel>.TryCreateInstance());
                        model = AfterLoadEntities(responseGroup, entity, model);
                        if (model != null) models.Add(model);
                    }

                }

                return models;
            });

            return result.Select(x => (TModel)x.Clone());
        }

        protected virtual TModel AfterLoadEntities(string responseGroup, TEntity entity, TModel model)
        {
            return model;
        }

        protected abstract Task<IEnumerable<TEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids, string responseGroup);

        protected Task<IEnumerable<TEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids)
        {
            return LoadEntities(repository, ids, "Full");
        }

        protected virtual void BeforeSaveChanges(IEnumerable<TModel> models)
        {
            // Basic implementation left empty
        }

        protected virtual void AfterSaveChanges(IEnumerable<TModel> models, IEnumerable<GenericChangedEntry<TModel>> changedEntries)
        {
            // Basic implementation left empty
        }

        protected virtual Task SoftDelete(IRepository repository, IEnumerable<string> ids)
        {
            // Basic implementation of soft delete intentionally left empty.
            return Task.CompletedTask;
        }

        public virtual async Task SaveChangesAsync(IEnumerable<TModel> models)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<TModel>>();

            BeforeSaveChanges(models);

            using (var repository = _repositoryFactory())
            {
                var dataExistEntities = await LoadEntities(repository, models.Where(x => !x.IsTransient()).Select(x => x.Id));

                foreach (var model in models)
                {

                    var originalEntity = dataExistEntities.FirstOrDefault(x => x.Id == model.Id);
                    var modifiedEntity = AbstractTypeFactory<TEntity>.TryCreateInstance().FromModel(model, pkMap);

                    if (originalEntity != null)
                    {
                        // This extension is allow to get around breaking changes is introduced in EF Core 3.0 that leads to throw
                        // Database operation expected to affect 1 row(s) but actually affected 0 row(s) exception when trying to add the new children entities with manually set keys
                        // https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#detectchanges-honors-store-generated-key-values
                        repository.TrackModifiedAsAddedForNewChildEntities(originalEntity);

                        changedEntries.Add(new GenericChangedEntry<TModel>(model, originalEntity.ToModel(AbstractTypeFactory<TModel>.TryCreateInstance()), EntryState.Modified));
                        modifiedEntity.Patch(originalEntity);
                        if (originalEntity is IAuditable auditableOriginalEntity)
                        {
                            auditableOriginalEntity.ModifiedDate = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                        changedEntries.Add(new GenericChangedEntry<TModel>(model, EntryState.Added));
                    }
                }

                //Raise domain events
                await _eventPublisher.Publish(EventFactory<TChangeEvent>(changedEntries));
                await repository.UnitOfWork.CommitAsync();
            }
            pkMap.ResolvePrimaryKeys();

            ClearCache(models);

            AfterSaveChanges(models, changedEntries);

            await _eventPublisher.Publish(EventFactory<TChangedEvent>(changedEntries));
        }

        public virtual async Task DeleteAsync(IEnumerable<string> ids, bool softDelete = false)
        {
            var models = (await GetByIdsAsync(ids));

            using (var repository = _repositoryFactory())
            {
                //Raise domain events before deletion
                var changedEntries = models.Select(x => new GenericChangedEntry<TModel>(x, EntryState.Deleted));
                await _eventPublisher.Publish(EventFactory<TChangeEvent>(changedEntries));

                if (softDelete)
                {
                    await SoftDelete(repository, ids);
                }
                else
                {
                    var keyMap = new PrimaryKeyResolvingMap();
                    foreach (var model in models)
                    {
                        var entity = AbstractTypeFactory<TEntity>.TryCreateInstance().FromModel(model, keyMap);
                        repository.Remove(entity);
                    }
                    await repository.UnitOfWork.CommitAsync();
                }

                ClearCache(models);

                //Raise domain events after deletion
                await _eventPublisher.Publish(EventFactory<TChangedEvent>(changedEntries));
            }
        }

        protected virtual IChangeToken CreateCacheToken(IEnumerable<string> ids)
        {
            return GenericCrudCachingRegion<TModel>.CreateChangeToken(ids);
        }

        protected virtual void ClearCache(IEnumerable<TModel> entities)
        {
            GenericSearchCacheRegion<TModel>.ExpireRegion();

            foreach (var entity in entities)
            {
                GenericCrudCachingRegion<TModel>.ExpireTokenForKey(entity.Id);
            }
        }

        protected virtual GenericChangedEntryEvent<TModel> EventFactory<TEvent>(IEnumerable<GenericChangedEntry<TModel>> changedEntries)
        {
            return (GenericChangedEntryEvent<TModel>)typeof(TEvent).GetConstructor(new Type[] { typeof(IEnumerable<GenericChangedEntry<TModel>>) }).Invoke(new object[] { changedEntries });
        }
    }
}
