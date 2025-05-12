using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.GenericCrud;

/// <summary>
/// Generic service to simplify CRUD implementation with outer entities.
/// To implement the service for applied purpose, inherit your search service from this.
/// </summary>
/// <typeparam name="TModel">The type of service layer model</typeparam>
/// <typeparam name="TEntity">The type of data access layer entity (EF) </typeparam>
/// <typeparam name="TChangeEvent">The type of *change event</typeparam>
/// <typeparam name="TChangedEvent">The type of *changed event</typeparam>
public abstract class OuterEntityService<TModel, TEntity, TChangeEvent, TChangedEvent>
    : CrudService<TModel, TEntity, TChangeEvent, TChangedEvent>, IOuterEntityService<TModel>
    where TModel : Entity, ICloneable, IHasOuterId
    where TEntity : Entity, IHasOuterId, IDataEntity<TEntity, TModel>
    where TChangeEvent : GenericChangedEntryEvent<TModel>
    where TChangedEvent : GenericChangedEntryEvent<TModel>
{
    private readonly IPlatformMemoryCache _platformMemoryCache;
    private readonly Func<IRepository> _repositoryFactory;

    /// <summary>
    /// Construct new OuterEntityService
    /// </summary>
    /// <param name="repositoryFactory">Repository factory to get access to the data source</param>
    /// <param name="platformMemoryCache">The cache used to temporarily store returned values</param>
    /// <param name="eventPublisher">The publisher to propagate platform-wide events (TChangeEvent, TChangedEvent)</param>
    protected OuterEntityService(Func<IRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
        : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {
        _repositoryFactory = repositoryFactory;
        _platformMemoryCache = platformMemoryCache;
    }

    public virtual async Task<IList<TModel>> GetByOuterIdsAsync(IList<string> outerIds, string responseGroup = null, bool clone = true)
    {
        using var repository = _repositoryFactory();

        if (repository.UnitOfWork is DbContextUnitOfWork dbContextUoW)
        {
            var entityIds = await dbContextUoW.DbContext
            .Set<TEntity>()
            .Cast<IHasOuterId>()
            .Where(x => outerIds.Contains(x.OuterId))
            .Cast<IEntity>()
            .Select(x => x.Id)
            .ToArrayAsync();

            return await GetAsync(entityIds, responseGroup, clone);
        }

        return Array.Empty<TModel>();
    }
}
