using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Caching;

public class RequestScopedCache : IRequestScopedCache
{
    // Lazy<Task> -> the factory runs at most once per key even under a concurrent same-key miss.
    // The Task<T> itself is stored (not its unwrapped result), so value-type results are never boxed.
    private readonly ConcurrentDictionary<string, Lazy<Task>> _cache = new();

    // OrdinalIgnoreCase per the platform id-cache convention (MemoryCacheExtensions): a case-insensitive DB
    // collation can return an entity Id cased differently from the requested id, which would otherwise miss
    // its reservation and be negatively cached. No standard ignore-case comparer exists for a string tuple;
    // capture-free lambdas keep this static singleton alloc-free per call (vs. an allocating per-id ToLower key).
    private static readonly IEqualityComparer<(string Prefix, string Id)> _byIdKeyComparer =
        AnonymousComparer.Create<(string Prefix, string Id)>(
            (x, y) => string.Equals(x.Prefix, y.Prefix, StringComparison.OrdinalIgnoreCase)
                      && string.Equals(x.Id, y.Id, StringComparison.OrdinalIgnoreCase),
            obj => HashCode.Combine(
                obj.Prefix is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Prefix),
                obj.Id is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Id)));

    // By-id entries store the Task<T> directly - single-flight comes from promise reservation, so there
    // is no per-id factory whose start a Lazy could defer. The tuple key avoids the aliasing of string
    // concatenation ("P:A"+"B" vs "P"+"A:B") and can't collide with by-key entries (separate dictionary).
    private readonly ConcurrentDictionary<(string Prefix, string Id), Task> _cacheById = new(_byIdKeyComparer);

    public virtual Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        var lazy = _cache.GetOrAdd(key, static (_, arg) => new Lazy<Task>(arg), factory);

        return (Task<T>)lazy.Value;
    }

    public virtual Task<IDictionary<string, T>> GetOrLoadMapByIdsAsync<T>(
        string keyPrefix,
        ICollection<string> ids,
        Func<T, string> idSelector,
        Func<ICollection<string>, Task<IList<T>>> loadMissing)
        where T : class
    {
        ArgumentException.ThrowIfNullOrEmpty(keyPrefix);
        ArgumentNullException.ThrowIfNull(ids);
        ArgumentNullException.ThrowIfNull(idSelector);
        ArgumentNullException.ThrowIfNull(loadMissing);

        return GetOrLoadMapByIdsCoreAsync(keyPrefix, ids, idSelector, loadMissing);
    }

    private async Task<IDictionary<string, T>> GetOrLoadMapByIdsCoreAsync<T>(
        string keyPrefix,
        ICollection<string> ids,
        Func<T, string> idSelector,
        Func<ICollection<string>, Task<IList<T>>> loadMissing)
        where T : class
    {
        var result = new Dictionary<string, T>(ids.Count, StringComparer.OrdinalIgnoreCase);
        List<KeyValuePair<string, Task<T>>> pending = null;
        Dictionary<string, TaskCompletionSource<T>> owned = null;

        try
        {
            // Single pass: only the reservation winner loads an id, so concurrent overlapping callers
            // load each id at most once. No explicit dedup: a duplicate input id hits the added entry.
            foreach (var id in ids)
            {
                if (string.IsNullOrEmpty(id))
                {
                    continue;
                }

                var task = GetOrReserve<T>((keyPrefix, id), out var reservation);
                if (reservation is not null)
                {
                    (owned ??= new Dictionary<string, TaskCompletionSource<T>>(StringComparer.OrdinalIgnoreCase)).Add(id, reservation);
                }

                // A just-won reservation task is never completed here, so it lands in pending.
                if (task.IsCompletedSuccessfully)
                {
                    await CollectHitAsync(result, id, task);
                }
                else
                {
                    (pending ??= []).Add(new KeyValuePair<string, Task<T>>(id, task));
                }
            }
        }
        catch (Exception ex)
        {
            // A failure while still reserving (e.g. a wrong-typed cached id throws InvalidCastException)
            // leaves the ids this call already owned reserved-but-unloaded. Release them - drop each from
            // the cache so it stays retryable, and fault its promise to unblock any awaiter - so one id's
            // type mismatch does not negatively cache the innocent ids this call happened to own.
            ReleaseReservations(keyPrefix, owned, ex);
            throw;
        }

        if (owned is not null)
        {
            try
            {
                await LoadAndPublishAsync(owned, idSelector, loadMissing);
            }
            catch (Exception ex)
            {
                // The load itself failed: owned reservations must never leak incomplete (awaiters would
                // hang). The cached fault applies the documented by-id failure semantics - same-id calls
                // for the rest of the request rethrow, they are not retried.
                FaultReservations(owned, ex);
                throw;
            }
        }

        await CollectPendingAsync(pending, result);

        return result;
    }

    private static async Task CollectHitAsync<T>(Dictionary<string, T> result, string id, Task<T> task)
        where T : class
    {
        // Called for completed tasks only - the await continues synchronously, no allocation.
        var value = await task;
        if (value is not null)
        {
            result[id] = value;
        }
    }

    private Task<T> GetOrReserve<T>((string Prefix, string Id) key, out TaskCompletionSource<T> reservation)
        where T : class
    {
        if (_cacheById.TryGetValue(key, out var cached))
        {
            reservation = null;

            return (Task<T>)cached;
        }

        // RunContinuationsAsynchronously: completing the reservation must not run other callers'
        // continuations inline on the publishing thread.
        var candidate = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        cached = _cacheById.GetOrAdd(key, static (_, arg) => arg.Task, candidate);
        reservation = ReferenceEquals(cached, candidate.Task) ? candidate : null;

        return (Task<T>)cached;
    }

    private static async Task LoadAndPublishAsync<T>(
        Dictionary<string, TaskCompletionSource<T>> owned,
        Func<T, string> idSelector,
        Func<ICollection<string>, Task<IList<T>>> loadMissing)
        where T : class
    {
        // Null result counts as empty, mirroring the platform's GetOrLoadByIdsAsync.
        var loaded = await loadMissing(owned.Keys) ?? [];

        foreach (var item in loaded)
        {
            if (item is null)
            {
                continue;
            }

            var loadedId = idSelector(item);
            if (!string.IsNullOrEmpty(loadedId) && owned.TryGetValue(loadedId, out var reservation))
            {
                // TrySetResult: duplicate ids from the load - first wins.
                reservation.TrySetResult(item);
            }
        }

        // Not-returned owned ids: negatively cache as null for the request.
        foreach (var reservation in owned.Values)
        {
            reservation.TrySetResult(null);
        }
    }

    private void ReleaseReservations<T>(string keyPrefix, Dictionary<string, TaskCompletionSource<T>> owned, Exception exception)
        where T : class
    {
        if (owned is null)
        {
            return;
        }

        foreach (var (id, reservation) in owned)
        {
            // Remove only our own reservation (match the stored task), then fault it: a later call for
            // this id re-reserves and loads it fresh instead of hitting a negatively-cached entry.
            _cacheById.TryRemove(new KeyValuePair<(string Prefix, string Id), Task>((keyPrefix, id), reservation.Task));
            reservation.TrySetException(exception);
        }
    }

    private static void FaultReservations<T>(Dictionary<string, TaskCompletionSource<T>> owned, Exception exception)
        where T : class
    {
        if (owned is null)
        {
            return;
        }

        foreach (var reservation in owned.Values)
        {
            reservation.TrySetException(exception);
        }
    }

    private static async Task CollectPendingAsync<T>(List<KeyValuePair<string, Task<T>>> pending, Dictionary<string, T> result)
        where T : class
    {
        if (pending is null)
        {
            return;
        }

        foreach (var (id, task) in pending)
        {
            var value = await task;
            if (value is not null)
            {
                result.TryAdd(id, value);
            }
        }
    }
}
