#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DistributedLock;

public static class DistributedLockExtensions
{
    /// <summary>
    /// Runs <paramref name="action"/> under the lock and returns its result.
    /// </summary>
    /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
    public static async Task<T> ExecuteAsync<T>(this IDistributedLock distributedLock, string resource,
        Func<CancellationToken, Task<T>> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(distributedLock);
        ArgumentNullException.ThrowIfNull(action);

        var handle = await distributedLock.AcquireAsync(resource, timeout, cancellationToken).ConfigureAwait(false);
        await using (handle.ConfigureAwait(false))
        {
            return await action(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Runs <paramref name="action"/> under the lock.
    /// </summary>
    /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
    public static async Task ExecuteAsync(this IDistributedLock distributedLock, string resource,
        Func<CancellationToken, Task> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(distributedLock);
        ArgumentNullException.ThrowIfNull(action);

        var handle = await distributedLock.AcquireAsync(resource, timeout, cancellationToken).ConfigureAwait(false);
        await using (handle.ConfigureAwait(false))
        {
            await action(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Runs <paramref name="action"/> only if the lock is acquired within <paramref name="timeout"/> (default: no wait).
    /// </summary>
    /// <returns><c>true</c> if the action ran; <c>false</c> if the lock was held elsewhere.</returns>
    public static async Task<bool> TryExecuteAsync(this IDistributedLock distributedLock, string resource,
        Func<CancellationToken, Task> action, TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(distributedLock);
        ArgumentNullException.ThrowIfNull(action);

        var handle = await distributedLock.TryAcquireAsync(resource, timeout, cancellationToken).ConfigureAwait(false);
        if (handle is null)
        {
            return false;
        }

        await using (handle.ConfigureAwait(false))
        {
            await action(cancellationToken).ConfigureAwait(false);
        }

        return true;
    }

    // Synchronous counterparts. They block the calling thread while waiting for the lock, so use them only where
    // no async path exists: application startup, console tools, or synchronous legacy APIs. Prefer the async methods
    // in request handlers and background jobs.

    /// <summary>
    /// Synchronous <see cref="IDistributedLock.AcquireAsync"/>: blocks until the lock is acquired or <paramref name="timeout"/> elapses.
    /// </summary>
    /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
    public static IDistributedLockHandle Acquire(this IDistributedLock distributedLock, string resource,
        TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(distributedLock);

        return distributedLock.AcquireAsync(resource, timeout, cancellationToken).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Synchronous <see cref="IDistributedLock.TryAcquireAsync"/>: returns <c>null</c> when the lock is held elsewhere.
    /// </summary>
    public static IDistributedLockHandle? TryAcquire(this IDistributedLock distributedLock, string resource,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(distributedLock);

        return distributedLock.TryAcquireAsync(resource, timeout, cancellationToken).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Runs <paramref name="action"/> under the lock, blocking the calling thread while waiting.
    /// </summary>
    /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
    public static void Execute(this IDistributedLock distributedLock, string resource,
        Action action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);

        using var handle = distributedLock.Acquire(resource, timeout, cancellationToken);
        action();
    }

    /// <summary>
    /// Runs <paramref name="action"/> under the lock and returns its result, blocking the calling thread while waiting.
    /// </summary>
    /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
    public static T Execute<T>(this IDistributedLock distributedLock, string resource,
        Func<T> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);

        using var handle = distributedLock.Acquire(resource, timeout, cancellationToken);
        return action();
    }

    /// <summary>
    /// Runs <paramref name="action"/> only if the lock is acquired within <paramref name="timeout"/> (default: no wait).
    /// </summary>
    /// <returns><c>true</c> if the action ran; <c>false</c> if the lock was held elsewhere.</returns>
    public static bool TryExecute(this IDistributedLock distributedLock, string resource,
        Action action, TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);

        using var handle = distributedLock.TryAcquire(resource, timeout, cancellationToken);
        if (handle is null)
        {
            return false;
        }

        action();
        return true;
    }
}
