using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock;

/// <summary>
/// Runs existing <see cref="IDistributedLockService"/> callers on <see cref="IDistributedLock"/>.
/// <c>tryLockTimeout</c> is the wait (<c>null</c>, zero or negative tries once, as before). <c>lockTimeout</c> and <c>retryInterval</c> are ignored:
/// <c>DistributedLock:Expiry</c> and <c>DistributedLock:RetryInterval</c> apply, and held Redis locks are extended automatically.
/// </summary>
public class DistributedLockServiceAdapter : IDistributedLockService
{
    private readonly IDistributedLock _distributedLock;

    public DistributedLockServiceAdapter(IDistributedLock distributedLock)
    {
        _distributedLock = distributedLock;
    }

    public virtual T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
    {
        using var handle = _distributedLock.Acquire(resourceKey, GetWait(tryLockTimeout), cancellationToken ?? CancellationToken.None);

        return resolver();
    }

    public virtual async Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
    {
        var handle = await _distributedLock.AcquireAsync(resourceKey, GetWait(tryLockTimeout), cancellationToken ?? CancellationToken.None).ConfigureAwait(false);
        await using (handle.ConfigureAwait(false))
        {
            return await resolver().ConfigureAwait(false);
        }
    }

    // The previous implementation made a single attempt for a missing, zero or negative tryLockTimeout.
    private static TimeSpan GetWait(TimeSpan? tryLockTimeout)
    {
        return tryLockTimeout is { } wait && wait > TimeSpan.Zero ? wait : TimeSpan.Zero;
    }
}
