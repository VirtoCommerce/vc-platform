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

namespace VirtoCommerce.Platform.Data.GenericCrud;

/// <summary>
/// Generic service to simplify CRUD implementation with outer entities.
/// To implement the service for applied purpose, inherit your search service from this.
/// </summary>
/// <typeparam name="TModel">The type of service layer model</typeparam>
/// <typeparam name="TEntity">The type of data access layer entity (EF) </typeparam>
/// <typeparam name="TChangingEvent">The type of *changing event</typeparam>
/// <typeparam name="TChangedEvent">The type of *changed event</typeparam>
public abstract class OuterEntityService<TModel, TEntity, TChangingEvent, TChangedEvent>
    : CrudService<TModel, TEntity, TChangingEvent, TChangedEvent>, IOuterEntityService<TModel>
    where TModel : Entity, IHasOuterId, ICloneable
    where TEntity : Entity, IHasOuterId, IDataEntity<TEntity, TModel>
    where TChangingEvent : GenericChangedEntryEvent<TModel>
    where TChangedEvent : GenericChangedEntryEvent<TModel>
{
    private readonly Func<IRepository> _repositoryFactory;

    /// <summary>
    /// Construct new OuterEntityService
    /// </summary>
    /// <param name="repositoryFactory">Repository factory to get access to the data source</param>
    /// <param name="platformMemoryCache">The cache used to temporarily store returned values</param>
    /// <param name="eventPublisher">The publisher to propagate platform-wide events (TChangingEvent, TChangedEvent)</param>
    protected OuterEntityService(Func<IRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
        : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {
        _repositoryFactory = repositoryFactory;
    }

    public virtual async Task<IList<TModel>> GetByOuterIdsAsync(IList<string> outerIds, string responseGroup = null, bool clone = true)
    {
        using var repository = _repositoryFactory();

        var query = GetEntitiesQuery(repository);

        query = outerIds.Count == 1
            ? query.Where(x => x.OuterId.Equals(outerIds[0]))
            : query.Where(x => outerIds.Contains(x.OuterId));

        var ids = await query
            .Select(x => x.Id)
            .ToArrayAsync();

        if (ids.Length == 0)
        {
            return [];
        }

        return await GetAsync(ids, responseGroup, clone);
    }

    protected abstract IQueryable<TEntity> GetEntitiesQuery(IRepository repository);
}
