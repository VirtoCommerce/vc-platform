#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DistributedLock;

public static class DistributedLockExtensions
{
    /// <summary>
    /// Runs <paramref name="action"/> under the lock and returns its result. The token passed to <paramref name="action"/>
    /// is cancelled when <paramref name="cancellationToken"/> is cancelled or the lock is lost (<see cref="IDistributedLockHandle.HandleLostToken"/>).
    /// </summary>
    /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached.</exception>
    /// <exception cref="DistributedLockLostException">The lock was lost while <paramref name="action"/> ran and <paramref name="cancellationToken"/> was not cancelled; the work may be done.</exception>
    public static async Task<T> ExecuteAsync<T>(this IDistributedLock distributedLock, string resource,
        Func<CancellationToken, Task<T>> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(distributedLock);
        ArgumentNullException.ThrowIfNull(action);

        var handle = await distributedLock.AcquireAsync(resource, timeout, cancellationToken).ConfigureAwait(false);
        await using (handle.ConfigureAwait(false))
        {
            return await RunAsync(handle, action, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Runs <paramref name="action"/> under the lock. The token passed to <paramref name="action"/> is cancelled when
    /// <paramref name="cancellationToken"/> is cancelled or the lock is lost (<see cref="IDistributedLockHandle.HandleLostToken"/>).
    /// </summary>
    /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached.</exception>
    /// <exception cref="DistributedLockLostException">The lock was lost while <paramref name="action"/> ran and <paramref name="cancellationToken"/> was not cancelled; the work may be done.</exception>
    public static async Task ExecuteAsync(this IDistributedLock distributedLock, string resource,
        Func<CancellationToken, Task> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(distributedLock);
        ArgumentNullException.ThrowIfNull(action);

        var handle = await distributedLock.AcquireAsync(resource, timeout, cancellationToken).ConfigureAwait(false);
        await using (handle.ConfigureAwait(false))
        {
            await RunAsync(handle, action, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Runs <paramref name="action"/> only if the lock is acquired within <paramref name="timeout"/> (default: no wait).
    /// The token passed to <paramref name="action"/> is also cancelled when the lock is lost.
    /// </summary>
    /// <returns><c>true</c> if the action ran; <c>false</c> if the lock was held elsewhere.</returns>
    /// <exception cref="DistributedLockUnavailableException">The lock store could not be reached; nothing is skipped silently.</exception>
    /// <exception cref="DistributedLockLostException">The lock was lost while <paramref name="action"/> ran and <paramref name="cancellationToken"/> was not cancelled; the work may be done.</exception>
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
            await RunAsync(handle, action, cancellationToken).ConfigureAwait(false);
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
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached.</exception>
    public static IDistributedLockHandle Acquire(this IDistributedLock distributedLock, string resource,
        TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(distributedLock);

        return distributedLock.AcquireAsync(resource, timeout, cancellationToken).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Synchronous <see cref="IDistributedLock.TryAcquireAsync"/>: returns <c>null</c> when the lock is held elsewhere.
    /// </summary>
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached.</exception>
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
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached.</exception>
    /// <exception cref="DistributedLockLostException">The lock was lost while <paramref name="action"/> ran; the work may be done.</exception>
    public static void Execute(this IDistributedLock distributedLock, string resource,
        Action action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);

        using var handle = distributedLock.Acquire(resource, timeout, cancellationToken);
        action();
        ThrowIfLost(handle, cancellationToken);
    }

    /// <summary>
    /// Runs <paramref name="action"/> under the lock and returns its result, blocking the calling thread while waiting.
    /// </summary>
    /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached.</exception>
    /// <exception cref="DistributedLockLostException">The lock was lost while <paramref name="action"/> ran; the work may be done.</exception>
    public static T Execute<T>(this IDistributedLock distributedLock, string resource,
        Func<T> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);

        using var handle = distributedLock.Acquire(resource, timeout, cancellationToken);
        var result = action();
        ThrowIfLost(handle, cancellationToken);
        return result;
    }

    /// <summary>
    /// Runs <paramref name="action"/> only if the lock is acquired within <paramref name="timeout"/> (default: no wait).
    /// </summary>
    /// <returns><c>true</c> if the action ran; <c>false</c> if the lock was held elsewhere.</returns>
    /// <exception cref="DistributedLockUnavailableException">The lock store (Redis) could not be reached.</exception>
    /// <exception cref="DistributedLockLostException">The lock was lost while <paramref name="action"/> ran; the work may be done.</exception>
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
        ThrowIfLost(handle, cancellationToken);
        return true;
    }

    // Runs the action with a token that is also cancelled on lock loss. A loss the caller did not cause is reported as
    // DistributedLockLostException, whether the action completed or stopped on the token, so it is never mistaken for
    // a user cancellation or a clean run.
    private static Task RunAsync(IDistributedLockHandle handle, Func<CancellationToken, Task> action, CancellationToken cancellationToken)
    {
        return RunAsync(handle, async token =>
        {
            await action(token).ConfigureAwait(false);
            return true;
        }, cancellationToken);
    }

    private static async Task<T> RunAsync<T>(IDistributedLockHandle handle, Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken)
    {
        using var lockScope = CreateLockScope(handle, cancellationToken);
        T result;
        try
        {
            result = await action(lockScope?.Token ?? cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException ex) when (IsLost(handle, cancellationToken))
        {
            throw new DistributedLockLostException(handle.Resource, "the action was cancelled", ex);
        }

        ThrowIfLost(handle, cancellationToken);
        return result;
    }

    private static void ThrowIfLost(IDistributedLockHandle handle, CancellationToken cancellationToken)
    {
        if (IsLost(handle, cancellationToken))
        {
            throw new DistributedLockLostException(handle.Resource, "the action completed after the loss and may have run concurrently with another holder");
        }
    }

    // Lost, and not because the caller cancelled: a caller cancellation keeps its usual OperationCanceledException.
    private static bool IsLost(IDistributedLockHandle handle, CancellationToken cancellationToken)
    {
        return handle.HandleLostToken.IsCancellationRequested && !cancellationToken.IsCancellationRequested;
    }

    // Links the caller's token with the handle's lost signal; null when the lock cannot be lost, so the caller's token is used as is.
    private static CancellationTokenSource? CreateLockScope(IDistributedLockHandle handle, CancellationToken cancellationToken)
    {
        return handle.HandleLostToken.CanBeCanceled
            ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, handle.HandleLostToken)
            : null;
    }
}
