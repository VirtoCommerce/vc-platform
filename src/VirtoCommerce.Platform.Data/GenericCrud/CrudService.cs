using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.GenericCrud
{
    /// <summary>
    /// Generic service to simplify CRUD implementation.
    /// To implement the service for applied purpose, inherit your search service from this.
    /// </summary>
    /// <typeparam name="TModel">The type of service layer model</typeparam>
    /// <typeparam name="TEntity">The type of data access layer entity (EF) </typeparam>
    /// <typeparam name="TChangeEvent">The type of *change event</typeparam>
    /// <typeparam name="TChangedEvent">The type of *changed event</typeparam>
    public abstract class CrudService<TModel, TEntity, TChangeEvent, TChangedEvent> : ICrudService<TModel>
        where TModel : Entity, ICloneable
        where TEntity : Entity, IDataEntity<TEntity, TModel>
        where TChangeEvent : GenericChangedEntryEvent<TModel>
        where TChangedEvent : GenericChangedEntryEvent<TModel>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly Func<IRepository> _repositoryFactory;

        /// <summary>
        /// Construct new CrudService
        /// </summary>
        /// <param name="repositoryFactory">Repository factory to get access to the data source</param>
        /// <param name="platformMemoryCache">The cache used to temporary store returned values</param>
        /// <param name="eventPublisher">The publisher to propagate platform-wide events (TChangeEvent, TChangedEvent)</param>
        protected CrudService(Func<IRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Returns a list of model instances for specified IDs.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="responseGroup"></param>
        /// <param name="clone">If false, returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.</param>
        /// <returns></returns>
        public virtual async Task<IList<TModel>> GetAsync(IList<string> ids, string responseGroup = null, bool clone = true)
        {
            var cacheKeyPrefix = CacheKey.With(GetType(), nameof(GetAsync), responseGroup);

            var models = await _platformMemoryCache.GetOrLoadByIdsAsync(cacheKeyPrefix, ids,
                missingIds => GetByIdsNoCache(missingIds, responseGroup),
                ConfigureCache);

            if (!clone)
            {
                return models;
            }

            return models
                .Select(x => x.CloneTyped())
                .ToList();
        }

        protected virtual async Task<IList<TModel>> GetByIdsNoCache(IList<string> ids, string responseGroup)
        {
            using var repository = _repositoryFactory();

            // Disable DBContext change tracking for better performance 
            repository.DisableChangesTracking();

            var entities = await LoadEntities(repository, ids, responseGroup);

            return ProcessModels(entities, responseGroup);
        }

        protected virtual void ConfigureCache(MemoryCacheEntryOptions cacheOptions, string id, TModel model)
        {
            cacheOptions.AddExpirationToken(CreateCacheToken(id));
        }

        protected virtual IList<TModel> ProcessModels(IList<TEntity> entities, string responseGroup)
        {
            return entities
                ?.Select(x => ProcessModel(responseGroup, x, ToModel(x)))
                .ToList();
        }

        /// <summary>
        /// Post-read processing of the model instance.
        /// A good place to make some additional actions, tune model data.
        /// Override to add some model data changes, calculations, etc...
        /// </summary>
        /// <param name="responseGroup"></param>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual TModel ProcessModel(string responseGroup, TEntity entity, TModel model)
        {
            return model;
        }

        /// <summary>
        /// Custom CRUD service must override this method to implement a call to repository for data read
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="ids"></param>
        /// <param name="responseGroup"></param>
        /// <returns></returns>
        protected abstract Task<IList<TEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup);

        /// <summary>
        /// Just calls LoadEntities with empty response group
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected virtual Task<IList<TEntity>> LoadEntities(IRepository repository, IList<string> ids)
        {
            return LoadEntities(repository, ids, responseGroup: null);
        }

        /// <summary>
        /// Custom CRUD service can override to implement some actions before save
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        protected virtual Task BeforeSaveChanges(IList<TModel> models)
        {
            // Basic implementation left empty
            return Task.CompletedTask;
        }

        /// <summary>
        /// Custom CRUD service can override to implement some actions after save
        /// </summary>
        /// <param name="models"></param>
        /// <param name="changedEntries"></param>
        /// <returns></returns>
        protected virtual Task AfterSaveChangesAsync(IList<TModel> models, IList<GenericChangedEntry<TModel>> changedEntries)
        {
            // Basic implementation left empty
            return Task.CompletedTask;
        }

        /// <summary>
        /// Custom CRUD service can override to implement a call to the repository for soft delete.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected virtual Task SoftDelete(IRepository repository, IList<string> ids)
        {
            // Basic implementation of soft delete intentionally left empty.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Persists a list of model instances to the data source.
        /// Can be overridden to implement full custom save.
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual async Task SaveChangesAsync(IList<TModel> models)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<TModel>>();
            var changedEntities = new List<TEntity>();

            await BeforeSaveChanges(models);

            var originalModels = new List<TModel>();

            using (var repository = _repositoryFactory())
            {
                var existingEntities = await LoadExistingEntities(repository, models);

                foreach (var model in models)
                {
                    var originalEntity = FindExistingEntity(existingEntities, model);
                    var modifiedEntity = FromModel(model, pkMap);

                    if (originalEntity != null)
                    {
                        // This extension is allow to get around breaking changes is introduced in EF Core 3.0 that leads to throw
                        // Database operation expected to affect 1 row(s) but actually affected 0 row(s) exception when trying to add the new children entities with manually set keys
                        // https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#detectchanges-honors-store-generated-key-values
                        repository.TrackModifiedAsAddedForNewChildEntities(originalEntity);

                        var originalModel = ToModel(originalEntity);
                        originalModels.Add(originalModel);
                        changedEntries.Add(new GenericChangedEntry<TModel>(model, originalModel, EntryState.Modified));
                        modifiedEntity.Patch(originalEntity);
                        if (originalEntity is IAuditable auditableOriginalEntity)
                        {
                            auditableOriginalEntity.ModifiedDate = DateTime.UtcNow;
                        }
                        changedEntities.Add(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                        changedEntries.Add(new GenericChangedEntry<TModel>(model, EntryState.Added));
                        changedEntities.Add(modifiedEntity);
                    }
                }

                //Raise domain events
                await _eventPublisher.Publish(EventFactory<TChangeEvent>(changedEntries));
                await CommitAsync(repository);
            }

            pkMap.ResolvePrimaryKeys();

            ClearCache(originalModels);
            ClearCache(models);

            foreach (var (changedEntry, i) in changedEntries.Select((x, i) => (x, i)))
            {
                changedEntry.NewEntry = ToModel(changedEntities[i]);
            }

            await AfterSaveChangesAsync(models, changedEntries);

            await _eventPublisher.Publish(EventFactory<TChangedEvent>(changedEntries));
        }

        protected virtual Task<IList<TEntity>> LoadExistingEntities(IRepository repository, IList<TModel> models)
        {
            var ids = models.Where(x => !x.IsTransient()).Select(x => x.Id).ToList();

            return ids.Any()
                ? LoadEntities(repository, ids)
                : Task.FromResult<IList<TEntity>>(Array.Empty<TEntity>());
        }

        protected virtual TEntity FindExistingEntity(IList<TEntity> existingEntities, TModel model)
        {
            return existingEntities?.FirstOrDefault(x => x.Id == model.Id);
        }

        protected virtual async Task CommitAsync(IRepository repository)
        {
            await repository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Delete models, related to specific set of their ids, from the data source.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="softDelete"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(IList<string> ids, bool softDelete = false)
        {
            var models = await GetAsync(ids);

            using (var repository = _repositoryFactory())
            {
                //Raise domain events before deletion
                var changedEntries = models.Select(x => new GenericChangedEntry<TModel>(x, EntryState.Deleted)).ToList();
                await _eventPublisher.Publish(EventFactory<TChangeEvent>(changedEntries));

                if (softDelete)
                {
                    await SoftDelete(repository, ids);
                    await repository.UnitOfWork.CommitAsync();
                }
                else
                {
                    var keyMap = new PrimaryKeyResolvingMap();
                    foreach (var model in models)
                    {
                        var entity = FromModel(model, keyMap);
                        repository.Remove(entity);
                    }
                    await repository.UnitOfWork.CommitAsync();
                    await AfterDeleteAsync(models, changedEntries);
                }
                ClearCache(models);

                //Raise domain events after deletion
                await _eventPublisher.Publish(EventFactory<TChangedEvent>(changedEntries));
            }
        }

        /// <summary>
        /// Custom CRUD service can override to implement some actions after delete
        /// </summary>
        /// <param name="models"></param>
        /// <param name="changedEntries"></param>
        /// <returns></returns>
        protected virtual Task AfterDeleteAsync(IList<TModel> models, IList<GenericChangedEntry<TModel>> changedEntries)
        {
            // Basic implementation left empty
            return Task.CompletedTask;
        }

        protected virtual IChangeToken CreateCacheToken(string id)
        {
            return GenericCachingRegion<TModel>.CreateChangeTokenForKey(id);
        }

        /// <summary>
        /// Clear the cache.
        /// Default implementation expires <see cref="GenericSearchCachingRegion{TModel}"/> region and <see cref="GenericCachingRegion{TModel}"/> regions for every entity
        /// Can be overridden to expire different regions/tokens.
        /// </summary>
        /// <param name="models"></param>
        protected virtual void ClearCache(IList<TModel> models)
        {
            ClearSearchCache(models);

            foreach (var model in models)
            {
                GenericCachingRegion<TModel>.ExpireTokenForKey(model.Id);
            }
        }

        protected virtual void ClearSearchCache(IList<TModel> models)
        {
            GenericSearchCachingRegion<TModel>.ExpireRegion();
        }

        protected virtual TModel ToModel(TEntity entity)
        {
            return entity.ToModel(AbstractTypeFactory<TModel>.TryCreateInstance());
        }

        protected virtual TEntity FromModel(TModel model, PrimaryKeyResolvingMap keyMap)
        {
            return AbstractTypeFactory<TEntity>.TryCreateInstance().FromModel(model, keyMap);
        }

        protected virtual GenericChangedEntryEvent<TModel> EventFactory<TEvent>(IList<GenericChangedEntry<TModel>> changedEntries)
        {
            return (GenericChangedEntryEvent<TModel>)typeof(TEvent).GetConstructor(new[] { typeof(IEnumerable<GenericChangedEntry<TModel>>) }).Invoke(new object[] { changedEntries });
        }
    }
}
