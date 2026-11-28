using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ChangeLog;

public class LastChangesService : ILastChangesService, IDisposable
{
    /// <summary>
    /// The maximum number of reset names kept between region resets. A reset of one more name moves the date
    /// of every name forward instead, so clients see every name as changed and the memory stays bounded.
    /// </summary>
    public const int MaxResetNames = 10_000;

    private readonly Lock _lock = new();
    private volatile State _state = new();
    private bool _disposed;

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

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            LastChangesCacheRegion.EntityNameReset -= SetResetDate;
        }

        _disposed = true;
    }

    private void SetResetDate(string entityName)
    {
        if (entityName is null)
        {
            return;
        }

        var state = GetState();

        if (state.ResetDates.ContainsKey(entityName) || state.TryReserveName())
        {
            state.ResetDates[entityName] = DateTimeOffset.UtcNow;
            return;
        }

        // Too many names: start a new state, whose date is later than this reset
        lock (_lock)
        {
            if (ReferenceEquals(_state, state))
            {
                _state = new State();
            }
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
        private int _nameCount;

        public IChangeToken RegionChangeToken { get; } = LastChangesCacheRegion.CreateChangeToken();

        public DateTimeOffset RegionResetDate { get; } = DateTimeOffset.UtcNow;

        public ConcurrentDictionary<string, DateTimeOffset> ResetDates { get; } = new(StringComparer.OrdinalIgnoreCase);

        // ConcurrentDictionary.Count takes every lock, so the names are counted here
        public bool TryReserveName()
        {
            return Interlocked.Increment(ref _nameCount) <= MaxResetNames;
        }
    }
}
