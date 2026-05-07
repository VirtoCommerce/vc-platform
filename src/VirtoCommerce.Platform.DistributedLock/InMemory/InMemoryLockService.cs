using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.DistributedLock.InMemory;

/// <summary>
/// In-process implementation of <see cref="IDistributedLockService"/> for deployments
/// where Redis is not configured. Serializes work for the same resource key within a
/// single platform process. Not safe across multiple replicas — when running with
/// more than one platform instance, configure Redis so the Redis-backed
/// <c>DistributedLockService</c> is used instead.
/// </summary>
/// <remarks>
/// Behavior choices that intentionally differ from the Redis implementation:
/// <list type="bullet">
///   <item><description>
///     When <c>tryLockTimeout</c> is <c>null</c>, this implementation waits indefinitely
///     (until <c>cancellationToken</c> fires) instead of throwing on first contention.
///     This is what cart command builders rely on — they queue concurrent per-user
///     requests rather than failing them with HTTP 500s.
///   </description></item>
///   <item><description>
///     <c>lockTimeout</c> is irrelevant for an in-process semaphore (the lock is
///     released when the calling task completes or the process exits) and is ignored.
///   </description></item>
///   <item><description>
///     <c>retryInterval</c> is ignored — <see cref="SemaphoreSlim.WaitAsync(TimeSpan, CancellationToken)"/>
///     handles waiting natively without polling.
///   </description></item>
/// </list>
/// One <see cref="SemaphoreSlim"/> is created per distinct resource key and kept for the
/// lifetime of the service. For the cart use case keys are bounded by active user count,
/// so this is acceptable. <see cref="Dispose()"/> disposes all retained semaphores.
/// </remarks>
public class InMemoryLockService : IDistributedLockService, IDisposable
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();
    private bool _disposed;

    public T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
    {
        ArgumentNullException.ThrowIfNull(resourceKey);
        ArgumentNullException.ThrowIfNull(resolver);
        ThrowIfDisposed();

        var sem = GetSemaphore(resourceKey);
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
        ThrowIfDisposed();

        var sem = GetSemaphore(resourceKey);
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
            foreach (var kvp in _locks)
            {
                kvp.Value.Dispose();
            }
            _locks.Clear();
        }

        _disposed = true;
    }

    private SemaphoreSlim GetSemaphore(string resourceKey)
    {
        // ConcurrentDictionary.GetOrAdd may invoke the factory more than once under
        // contention, but only one value is ever stored. The discarded SemaphoreSlim
        // instances become unreachable and are GC-collected; no leaks.
        return _locks.GetOrAdd(resourceKey, _ => new SemaphoreSlim(1, 1));
    }

    private static bool TryWait(SemaphoreSlim sem, TimeSpan? tryLockTimeout, CancellationToken? cancellationToken)
    {
        var ct = cancellationToken ?? CancellationToken.None;

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

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(InMemoryLockService));
        }
    }
}
