using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ChangeLog;

public class LastChangesService : ILastChangesService
{
    private readonly Lock _lock = new();
    private volatile State _state = new();

    // The memory cache is not used anymore; the parameter is kept for compatibility.
    public LastChangesService(IPlatformMemoryCache memoryCache)
    {
        LastChangesCacheRegion.EntityNameReset += SetResetDate;
    }

    public DateTimeOffset GetLastModifiedDate(string entityName)
    {
        var state = GetState();

        if (entityName is null)
        {
            return state.RegionResetDate;
        }

        return state.ResetDates.TryGetValue(entityName, out var resetDate)
            ? resetDate
            : state.RegionResetDate;
    }

    public void Reset(IEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        foreach (var entityTypeName in EntityTypeNames.Get(entity.GetType()))
        {
            Reset(entityTypeName);
        }
    }

    public void Reset(string entityName)
    {
        SetResetDate(entityName);

        // Notifies other platform instances
        LastChangesCacheRegion.ExpireTokenForKey(entityName);
    }

    private void SetResetDate(string entityName)
    {
        if (entityName is not null)
        {
            GetState().ResetDates[entityName] = DateTimeOffset.UtcNow;
        }
    }

    // A region or global cache reset changes the date of every name
    private State GetState()
    {
        var state = _state;

        if (state.RegionChangeToken.HasChanged)
        {
            lock (_lock)
            {
                if (_state.RegionChangeToken.HasChanged)
                {
                    _state = new State();
                }

                state = _state;
            }
        }

        return state;
    }

    private sealed class State
    {
        public IChangeToken RegionChangeToken { get; } = LastChangesCacheRegion.CreateChangeToken();

        public DateTimeOffset RegionResetDate { get; } = DateTimeOffset.UtcNow;

        public ConcurrentDictionary<string, DateTimeOffset> ResetDates { get; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
