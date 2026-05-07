using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.DistributedLock.InMemory;

/// <summary>
/// In-process implementation of <see cref="IDistributedLockService"/> for deployments
/// where Redis is not configured. Serializes work for the same resource key within
/// a single platform process. Not safe across multiple replicas — when running with
/// more than one platform instance, configure Redis so the Redis-backed
/// <c>DistributedLockService</c> is used instead.
/// </summary>
public class InMemoryLockService : IDistributedLockService
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
    {
        ArgumentNullException.ThrowIfNull(resourceKey);
        ArgumentNullException.ThrowIfNull(resolver);

        var sem = _locks.GetOrAdd(resourceKey, _ => new SemaphoreSlim(1, 1));
        if (!TryWait(sem, tryLockTimeout, cancellationToken))
        {
            throw new PlatformException($"Resource `{resourceKey}' is currently unavailable due to high demand.");
        }

        try
        {
            return resolver();
        }
        finally
        {
            sem.Release();
        }
    }

    public async Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
    {
        ArgumentNullException.ThrowIfNull(resourceKey);
        ArgumentNullException.ThrowIfNull(resolver);

        var sem = _locks.GetOrAdd(resourceKey, _ => new SemaphoreSlim(1, 1));
        if (!await TryWaitAsync(sem, tryLockTimeout, cancellationToken))
        {
            throw new PlatformException($"Resource `{resourceKey}' is currently unavailable due to high demand.");
        }

        try
        {
            return await resolver();
        }
        finally
        {
            sem.Release();
        }
    }

    private static bool TryWait(SemaphoreSlim sem, TimeSpan? tryLockTimeout, CancellationToken? cancellationToken)
    {
        var ct = cancellationToken ?? CancellationToken.None;

        // Mirror DistributedLockService (Redis) semantics: when tryLockTimeout is supplied,
        // it bounds how long we wait to acquire the lock; otherwise wait indefinitely.
        if (tryLockTimeout.HasValue)
        {
            return sem.Wait(tryLockTimeout.Value, ct);
        }

        sem.Wait(ct);
        return true;
    }

    private static async Task<bool> TryWaitAsync(SemaphoreSlim sem, TimeSpan? tryLockTimeout, CancellationToken? cancellationToken)
    {
        var ct = cancellationToken ?? CancellationToken.None;

        if (tryLockTimeout.HasValue)
        {
            return await sem.WaitAsync(tryLockTimeout.Value, ct);
        }

        await sem.WaitAsync(ct);
        return true;
    }
}
