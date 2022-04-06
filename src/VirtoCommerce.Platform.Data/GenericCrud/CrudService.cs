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
        protected readonly IEventPublisher _eventPublisher;
        protected readonly IPlatformMemoryCache _platformMemoryCache;
        protected readonly Func<IRepository> _repositoryFactory;

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
        /// Return a model instance for specified id and response group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="responseGroup"></param>
        /// <returns></returns>
        public virtual async Task<TModel> GetByIdAsync(string id, string responseGroup = null)
        {
            if (id == null)
            {
                return null;
            }

            var entities = await GetByIdsAsync(new[] { id }, responseGroup);
            return entities.FirstOrDefault();
        }

        /// <summary>
        /// Return an enumerable set of model instances for specified ids and response group.
        /// Custom CRUD service can override this to implement fully specific read.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="responseGroup"></param>
        /// <returns></returns>
        public virtual async Task<IReadOnlyCollection<TModel>> GetAsync(List<string> ids, string responseGroup = null)
        {
            var cacheKeyPrefix = CacheKey.With(GetType(), nameof(GetAsync), responseGroup);

            var models = await _platformMemoryCache.GetOrLoadByIdsAsync(cacheKeyPrefix, ids,
                async missingIds =>
                {
                    using var repository = _repositoryFactory();

                    // Disable DBContext change tracking for better performance 
                    repository.DisableChangesTracking();

                    var entities = await LoadEntities(repository, missingIds, responseGroup);

                    return entities
                        .Select(x => ProcessModel(responseGroup, x, x.ToModel(AbstractTypeFactory<TModel>.TryCreateInstance())));
                },
                (cacheOptions, id) =>
                {
                    cacheOptions.AddExpirationToken(CreateCacheToken(id));
                });

            return models
                .Select(x => x.CloneTyped())
                .OrderBy(x => ids.IndexOf(x.Id))
                .ToList();
        }

        [Obsolete("Use method GetAsync instead")]
        public virtual async Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<string> ids, string responseGroup = null)
        {
            return await GetAsync(new List<string>(ids), responseGroup);
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
        protected abstract Task<IEnumerable<TEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids, string responseGroup);

        /// <summary>
        /// Just calls LoadEntities with "Full" response group
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected virtual Task<IEnumerable<TEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids)
        {
            return LoadEntities(repository, ids, null);
        }

        /// <summary>
        /// Custom CRUD service can override to implement some actions before save
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        protected virtual Task BeforeSaveChanges(IEnumerable<TModel> models)
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
        protected virtual Task AfterSaveChangesAsync(IEnumerable<TModel> models, IEnumerable<GenericChangedEntry<TModel>> changedEntries)
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
        protected virtual Task SoftDelete(IRepository repository, IEnumerable<string> ids)
        {
            // Basic implementation of soft delete intentionally left empty.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Persists specific set of enumerable model instances to the data source.
        /// Can be overridden to implement full custom save.
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual async Task SaveChangesAsync(IEnumerable<TModel> models)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<TModel>>();

            await BeforeSaveChanges(models);

            using (var repository = _repositoryFactory())
            {
                var dataExistEntities = await LoadExistingEntities(repository, models);

                foreach (var model in models)
                {

                    var originalEntity = FindExistingEntity(dataExistEntities, model);
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

            await AfterSaveChangesAsync(models, changedEntries);

            await _eventPublisher.Publish(EventFactory<TChangedEvent>(changedEntries));
        }

        protected virtual Task<IEnumerable<TEntity>> LoadExistingEntities(IRepository repository, IEnumerable<TModel> models)
        {
            return LoadEntities(repository, models.Where(x => !x.IsTransient()).Select(x => x.Id));
        }

        protected virtual TEntity FindExistingEntity(IEnumerable<TEntity> existingEntities, TModel model)
        {
            return existingEntities.FirstOrDefault(x => x.Id == model.Id);
        }

        /// <summary>
        /// Delete models, related to specific set of their ids, from the data source.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="softDelete"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(IEnumerable<string> ids, bool softDelete = false)
        {
            var models = (await GetAsync(new List<string>(ids)));

            using (var repository = _repositoryFactory())
            {
                //Raise domain events before deletion
                var changedEntries = models.Select(x => new GenericChangedEntry<TModel>(x, EntryState.Deleted));
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
                        var entity = AbstractTypeFactory<TEntity>.TryCreateInstance().FromModel(model, keyMap);
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
        protected virtual Task AfterDeleteAsync(IEnumerable<TModel> models, IEnumerable<GenericChangedEntry<TModel>> changedEntries)
        {
            // Basic implementation left empty
            return Task.CompletedTask;
        }

        /// <summary>
        /// Create cache region.
        /// Default implementation creates <see cref="GenericCrudCachingRegion<TModel>"/>.
        /// Can be overridden to create some different region.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Obsolete("Use CreateCacheToken(string id)")]
        protected virtual IChangeToken CreateCacheToken(IEnumerable<string> ids)
        {
            return GenericCachingRegion<TModel>.CreateChangeToken(ids);
        }

        protected virtual IChangeToken CreateCacheToken(string id)
        {
            return GenericCachingRegion<TModel>.CreateChangeTokenForKey(id);
        }

        /// <summary>
        /// Clear the cache.
        /// Default implementation expires <see cref="GenericSearchCacheRegion<TModel>"/> region and <see cref="GenericCrudCachingRegion<TModel>"/> regions for every entity
        /// Can be overridden to expire different regions/tokens.
        /// </summary>
        /// <param name="models"></param>
        protected virtual void ClearCache(IEnumerable<TModel> models)
        {
            GenericSearchCachingRegion<TModel>.ExpireRegion();

            foreach (var model in models)
            {
                GenericCachingRegion<TModel>.ExpireTokenForKey(model.Id);
            }
        }

        protected virtual GenericChangedEntryEvent<TModel> EventFactory<TEvent>(IEnumerable<GenericChangedEntry<TModel>> changedEntries)
        {
            return (GenericChangedEntryEvent<TModel>)typeof(TEvent).GetConstructor(new Type[] { typeof(IEnumerable<GenericChangedEntry<TModel>>) }).Invoke(new object[] { changedEntries });
        }
    }
}
