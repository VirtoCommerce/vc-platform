#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DistributedLock;

/// <summary>
/// Cluster-wide mutual exclusion for module and solution code.
/// Redis-backed when <c>ConnectionStrings:RedisConnectionString</c> is configured; otherwise an in-process lock
/// that serializes callers within one platform instance. Locks are not reentrant.
/// </summary>
/// <example>
/// <code>
/// await using var handle = await distributedLock.AcquireAsync($"cart:recalc:{cartId}", cancellationToken: cancellationToken);
/// </code>
/// </example>
public interface IDistributedLock
{
    /// <summary>
    /// Acquires the lock, waiting up to <paramref name="timeout"/> (default: <c>DistributedLock:DefaultTimeout</c>).
    /// </summary>
    /// <param name="resource">Lock name, for example <c>cart:recalc:{cartId}</c>. Must not contain secrets.</param>
    /// <param name="timeout">Maximum wait; <see cref="TimeSpan.Zero"/> tries once, <see cref="Timeout.InfiniteTimeSpan"/> waits until acquired or cancelled.</param>
    /// <param name="cancellationToken">Cancels the wait.</param>
    /// <returns>A handle that releases the lock when disposed.</returns>
    /// <exception cref="DistributedLockTimeoutException">Another holder kept the lock for the whole <paramref name="timeout"/>.</exception>
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled while waiting.</exception>
    Task<IDistributedLockHandle> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tries to acquire the lock within <paramref name="timeout"/>. The default tries once without waiting;
    /// <see cref="Timeout.InfiniteTimeSpan"/> waits until acquired or cancelled.
    /// </summary>
    /// <returns>A handle that releases the lock when disposed, or <c>null</c> when another holder kept the lock for the whole timeout.</returns>
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached; the resource may be free.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled while waiting.</exception>
    Task<IDistributedLockHandle?> TryAcquireAsync(string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default);
}
