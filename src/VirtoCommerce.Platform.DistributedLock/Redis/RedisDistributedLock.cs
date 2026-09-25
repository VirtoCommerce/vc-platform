#nullable enable

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock.Redis;

/// <summary>
/// <see cref="IDistributedLock"/> on Redis (RedLock.net). Held locks are extended automatically until the handle is disposed;
/// a failed extension cancels <see cref="IDistributedLockHandle.HandleLostToken"/>.
/// </summary>
/// <remarks>
/// Each attempt is a single RedLock acquisition, so pass a factory created with <c>RedLockRetryConfiguration(retryCount: 1)</c>.
/// Waiting between attempts, backoff and cancellation are handled here rather than by RedLock's own wait loop.
/// </remarks>
public sealed class RedisDistributedLock : DistributedLockBase
{
    private const double BackoffFactor = 1.8;
    private const double JitterRatio = 0.2;

    // The handle checks the RedLock status this often (a fraction of the expiry, within these bounds).
    private static readonly TimeSpan _minLostCheckInterval = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan _maxLostCheckInterval = TimeSpan.FromSeconds(5);

    private readonly IDistributedLockFactory _lockFactory;
    private readonly ILogger<RedisDistributedLock> _logger;

    public RedisDistributedLock(IDistributedLockFactory lockFactory, IOptions<DistributedLockOptions> options, ILogger<RedisDistributedLock> logger)
        : base(options)
    {
        _lockFactory = lockFactory;
        _logger = logger;
        logger.LogInformation("Distributed lock: using Redis.");
    }

    protected override async Task<IDistributedLockHandle?> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var key = string.IsNullOrEmpty(LockOptions.KeyPrefix) ? resource : $"{LockOptions.KeyPrefix}:{resource}";
        var startTimestamp = Stopwatch.GetTimestamp();
        var delay = LockOptions.RetryInterval;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var redLock = await _lockFactory.CreateLockAsync(key, LockOptions.Expiry).ConfigureAwait(false);
            if (redLock.IsAcquired)
            {
                return new Handle(resource, redLock, LockOptions.Expiry, GetLostCheckInterval(LockOptions.Expiry), _logger);
            }

            // A lock that was not acquired is not disposed: RedLock already released its keys after the failed attempt,
            // holds no renewal timer, and DisposeAsync would send one more unlock (a full timeout during an outage).
            var status = redLock.Status;

            // Not contention, so report it instead of returning null, which callers read as "held elsewhere":
            // NoQuorum - Redis could not be reached (errors are recorded per instance, not thrown);
            // Expired - the key was set, but acquisition took longer than the lease.
            if (status is RedLockStatus.NoQuorum or RedLockStatus.Expired)
            {
                throw new DistributedLockUnavailableException(resource, $"{status} ({redLock.InstanceSummary})");
            }

            var remaining = timeout == Timeout.InfiniteTimeSpan
                ? Timeout.InfiniteTimeSpan
                : timeout - Stopwatch.GetElapsedTime(startTimestamp);

            if (remaining != Timeout.InfiniteTimeSpan && remaining <= TimeSpan.Zero)
            {
                return null;
            }

            var wait = GetRetryWait(delay, LockOptions.MaxRetryInterval, Random.Shared.NextDouble());
            if (remaining != Timeout.InfiniteTimeSpan && wait > remaining)
            {
                wait = remaining;
            }

            await Task.Delay(wait, cancellationToken).ConfigureAwait(false);

            delay = GetNextRetryDelay(delay, LockOptions.MaxRetryInterval);
        }
    }

    /// <summary>
    /// The wait before the next attempt: <paramref name="delay"/> with ±20% jitter, never above <paramref name="maxRetryInterval"/>.
    /// </summary>
    /// <param name="delay">The current backoff step.</param>
    /// <param name="maxRetryInterval">The upper bound for the wait.</param>
    /// <param name="random">A value in [0, 1) that picks the jitter.</param>
    internal static TimeSpan GetRetryWait(TimeSpan delay, TimeSpan maxRetryInterval, double random)
    {
        var factor = 1 + ((random * 2) - 1) * JitterRatio;
        var wait = TimeSpan.FromTicks((long)(delay.Ticks * factor));
        return wait > maxRetryInterval ? maxRetryInterval : wait;
    }

    /// <summary>
    /// The next backoff step: 1.8 times <paramref name="delay"/>, capped at <paramref name="maxRetryInterval"/>.
    /// </summary>
    internal static TimeSpan GetNextRetryDelay(TimeSpan delay, TimeSpan maxRetryInterval)
    {
        return TimeSpan.FromTicks(Math.Min((long)(delay.Ticks * BackoffFactor), maxRetryInterval.Ticks));
    }

    private static TimeSpan GetLostCheckInterval(TimeSpan expiry)
    {
        var interval = expiry / 4;
        if (interval < _minLostCheckInterval)
        {
            return _minLostCheckInterval;
        }

        return interval > _maxLostCheckInterval ? _maxLostCheckInterval : interval;
    }

    private sealed class Handle : IDistributedLockHandle
    {
        private readonly IRedLock _redLock;
        private readonly TimeSpan _expiry;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _lost = new();
        private readonly Timer _lostCheck;
        private readonly Lock _sync = new();
        private bool _released;
        private int _lastExtendCount;
        private long _lastRenewalTimestamp = Stopwatch.GetTimestamp();

        public Handle(string resource, IRedLock redLock, TimeSpan expiry, TimeSpan lostCheckInterval, ILogger logger)
        {
            Resource = resource;
            _redLock = redLock;
            _expiry = expiry;
            _logger = logger;
            _lastExtendCount = redLock.ExtendCount;
            HandleLostToken = _lost.Token;
            _lostCheck = new Timer(static state => ((Handle)state!).CheckLost(), this, lostCheckInterval, lostCheckInterval);
        }

        public string Resource { get; }

        public CancellationToken HandleLostToken { get; }

        public void Dispose()
        {
            if (StopWatching())
            {
                _redLock.Dispose();
                _lost.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (StopWatching())
            {
                await _redLock.DisposeAsync().ConfigureAwait(false);
                _lost.Dispose();
            }
        }

        private void CheckLost()
        {
            // Timer.Dispose does not wait for a running callback, so the check runs under the same lock as the release:
            // after release, RedLock reports Unlocked and the check would otherwise report a false loss.
            RedLockStatus status;
            RedLockInstanceSummary instanceSummary;
            lock (_sync)
            {
                if (_released || _lost.IsCancellationRequested || !IsLost())
                {
                    return;
                }

                status = _redLock.Status;
                instanceSummary = _redLock.InstanceSummary;
            }

            _logger.LogWarning("Distributed lock {Resource} lost while held: {Status} ({InstanceSummary}).", Resource, status, instanceSummary);
            try
            {
                // Outside the lock: cancellation runs caller callbacks, which may release the handle.
                _lost.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // Released concurrently.
            }
            catch (Exception ex)
            {
                // A throwing HandleLostToken callback must not escape: this runs on a timer thread, where an unhandled
                // exception terminates the process. The token is cancelled even when a callback throws.
                _logger.LogError(ex, "A HandleLostToken callback for distributed lock {Resource} failed.", Resource);
            }
        }

        private bool IsLost()
        {
            // RedLock renews every Expiry / 2. A failed renewal, including an unreachable Redis, changes Status. Some cases
            // leave Status as Acquired: a renewal that throws outside RedLock's per-instance handling (for example on a closed
            // multiplexer), a skipped tick while the previous renewal still runs, or a renewal timer that does not run at all
            // (thread-pool starvation). Without an observed renewal for a whole Expiry the key has probably expired in Redis,
            // so both signals mean the lock may now belong to someone else.
            var extendCount = _redLock.ExtendCount;
            if (extendCount != _lastExtendCount)
            {
                _lastExtendCount = extendCount;
                _lastRenewalTimestamp = Stopwatch.GetTimestamp();
            }

            return !_redLock.IsAcquired || Stopwatch.GetElapsedTime(_lastRenewalTimestamp) >= _expiry;
        }

        private bool StopWatching()
        {
            lock (_sync)
            {
                if (_released)
                {
                    return false;
                }

                _released = true;
            }

            _lostCheck.Dispose();
            return true;
        }
    }
}
