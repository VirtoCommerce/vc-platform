#nullable enable

using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock;

/// <summary>
/// Validation, default timeout, timeout exception, tracing and metrics shared by <see cref="IDistributedLock"/> implementations.
/// </summary>
public abstract class DistributedLockBase : IDistributedLock
{
    /// <summary>Name of the <see cref="ActivitySource"/> and the <see cref="Meter"/>.</summary>
    public const string ActivitySourceName = "VirtoCommerce.Platform.DistributedLock";

    public const string OutcomeAcquired = "acquired";
    public const string OutcomeTimeout = "timeout";
    public const string OutcomeCancelled = "cancelled";
    public const string OutcomeUnavailable = "unavailable";
    public const string OutcomeError = "error";

    /// <summary>Resource family recorded for names without a <c>:</c> separator.</summary>
    public const string OtherResourceFamily = "other";

    // Hex characters of the SHA-256 resource hash recorded on spans: 64 bits, enough to correlate one resource across instances.
    private const int ResourceHashLength = 16;

    private static readonly ActivitySource _activitySource = new(ActivitySourceName);
    private static readonly Meter _meter = new(ActivitySourceName);

    // Seconds-based buckets from 5 ms to 5 min. Without advice, OpenTelemetry applies its default bounds, which are sized for milliseconds.
    private static readonly InstrumentAdvice<double> _durationAdvice = new()
    {
        HistogramBucketBoundaries = [0.005, 0.01, 0.025, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5, 10, 30, 60, 300],
    };

    private static readonly Histogram<double> _waitDuration = _meter.CreateHistogram(
        "vc.lock.wait.duration", "s", "Time spent acquiring a distributed lock, by outcome.", tags: null, advice: _durationAdvice);

    private static readonly Histogram<double> _holdDuration = _meter.CreateHistogram(
        "vc.lock.hold.duration", "s", "Time a distributed lock was held, from acquisition to release.", tags: null, advice: _durationAdvice);

    private static readonly Counter<long> _acquisitions = _meter.CreateCounter<long>(
        "vc.lock.acquisitions", "{acquisition}", "Distributed lock acquisition calls (each may make several attempts), by outcome.");

    private static readonly Counter<long> _lost = _meter.CreateCounter<long>(
        "vc.lock.lost", "{lock}", "Distributed locks lost while still held.");

    protected DistributedLockBase(IOptions<DistributedLockOptions> options)
    {
        LockOptions = options.Value;
    }

    protected DistributedLockOptions LockOptions { get; }

    public virtual async Task<IDistributedLockHandle> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        var effectiveTimeout = timeout ?? LockOptions.DefaultTimeout;

        return await TryAcquireAsync(resource, effectiveTimeout, cancellationToken).ConfigureAwait(false)
            ?? throw new DistributedLockTimeoutException(resource, effectiveTimeout);
    }

    public virtual async Task<IDistributedLockHandle?> TryAcquireAsync(string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resource);
        if (timeout < TimeSpan.Zero && timeout != Timeout.InfiniteTimeSpan)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), timeout, "The timeout must be non-negative or Timeout.InfiniteTimeSpan.");
        }

        cancellationToken.ThrowIfCancellationRequested();

        var family = GetResourceFamily(resource);
        using var activity = _activitySource.StartActivity("DistributedLock acquire");
        // Resource names often contain user or entity ids; traces get a stable hash and the id-free family instead of the name.
        activity?.SetTag("vc.lock.resource_hash", HashResource(resource));
        activity?.SetTag("vc.lock.resource_family", family);
        var startTimestamp = Stopwatch.GetTimestamp();
        var outcome = OutcomeError;

        try
        {
            var handle = await TryAcquireCoreAsync(resource, timeout, cancellationToken).ConfigureAwait(false);
            outcome = handle is null ? OutcomeTimeout : OutcomeAcquired;
            return handle is null ? null : new InstrumentedHandle(handle, family);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            outcome = OutcomeCancelled;
            throw;
        }
        catch (DistributedLockUnavailableException)
        {
            outcome = OutcomeUnavailable;
            throw;
        }
        finally
        {
            var waitSeconds = Stopwatch.GetElapsedTime(startTimestamp).TotalSeconds;
            activity?.SetTag("vc.lock.outcome", outcome);
            activity?.SetTag("vc.lock.wait.duration", waitSeconds);
            if (outcome is not OutcomeAcquired and not OutcomeTimeout && activity is not null)
            {
                activity.SetStatus(ActivityStatusCode.Error, outcome);
            }

            var tags = new TagList { { "vc.lock.outcome", outcome }, { "vc.lock.resource_family", family } };
            _waitDuration.Record(waitSeconds, tags);
            _acquisitions.Add(1, tags);
        }
    }

    /// <summary>
    /// Acquires the lock within <paramref name="timeout"/>, or returns <c>null</c> when another holder kept it.
    /// The timeout is validated: non-negative, or <see cref="Timeout.InfiniteTimeSpan"/> to wait until acquired or cancelled.
    /// Throw <see cref="DistributedLockUnavailableException"/> when the lock store cannot be reached.
    /// </summary>
    protected abstract Task<IDistributedLockHandle?> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken);

    /// <summary>
    /// Low-cardinality name used for metrics and spans: the resource without its last <c>:</c> segment, and at most
    /// its first two segments, for example <c>cart:recalc</c> for <c>cart:recalc:{cartId}</c>. A name without <c>:</c>
    /// becomes <see cref="OtherResourceFamily"/>, since it may be an id itself. Names that follow <c>{module}:{entity}:{id}</c>
    /// keep ids out of telemetry; an id in the first two segments would still reach it.
    /// </summary>
    protected internal static string GetResourceFamily(string resource)
    {
        var last = resource.LastIndexOf(':');
        if (last <= 0)
        {
            return OtherResourceFamily;
        }

        var family = resource.AsSpan(0, last);
        var first = family.IndexOf(':');
        if (first >= 0)
        {
            var second = family[(first + 1)..].IndexOf(':');
            if (second >= 0)
            {
                family = family[..(first + 1 + second)];
            }
        }

        return family.ToString();
    }

    private static string HashResource(string resource)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(resource)))[..ResourceHashLength];
    }

    /// <summary>
    /// Records hold time and lock loss around the implementation's handle.
    /// </summary>
    private sealed class InstrumentedHandle : IDistributedLockHandle
    {
        private readonly IDistributedLockHandle _inner;
        private readonly string _family;
        private readonly long _acquiredTimestamp = Stopwatch.GetTimestamp();
        private readonly CancellationTokenRegistration _lostRegistration;
        private int _released;

        public InstrumentedHandle(IDistributedLockHandle inner, string family)
        {
            _inner = inner;
            _family = family;
            _lostRegistration = inner.HandleLostToken.Register(static state =>
            {
                _lost.Add(1, new TagList { { "vc.lock.resource_family", (string)state! } });
            }, family);
        }

        public string Resource => _inner.Resource;

        public CancellationToken HandleLostToken => _inner.HandleLostToken;

        public void Dispose()
        {
            if (Release())
            {
                _inner.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (Release())
            {
                await _inner.DisposeAsync().ConfigureAwait(false);
            }
        }

        private bool Release()
        {
            if (Interlocked.Exchange(ref _released, 1) != 0)
            {
                return false;
            }

            _lostRegistration.Dispose();
            _holdDuration.Record(Stopwatch.GetElapsedTime(_acquiredTimestamp).TotalSeconds, new TagList { { "vc.lock.resource_family", _family } });
            return true;
        }
    }
}
