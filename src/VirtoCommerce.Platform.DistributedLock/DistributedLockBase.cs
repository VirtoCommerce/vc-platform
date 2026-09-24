#nullable enable

using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock;

/// <summary>
/// Validation, default timeout, timeout exception and tracing shared by <see cref="IDistributedLock"/> implementations.
/// </summary>
public abstract class DistributedLockBase : IDistributedLock
{
    public const string ActivitySourceName = "VirtoCommerce.Platform.DistributedLock";

    // Hex characters of the SHA-256 resource hash recorded on spans: 64 bits, enough to correlate one resource across instances.
    private const int ResourceHashLength = 16;

    private static readonly ActivitySource _activitySource = new(ActivitySourceName);

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

        using var activity = _activitySource.StartActivity("DistributedLock acquire");
        // Resource names often contain user or entity ids; traces get a stable hash instead of the name.
        activity?.SetTag("vc.lock.resource_hash", HashResource(resource));
        var startTimestamp = Stopwatch.GetTimestamp();

        try
        {
            var handle = await TryAcquireCoreAsync(resource, timeout, cancellationToken).ConfigureAwait(false);
            activity?.SetTag("vc.lock.outcome", handle is null ? "timeout" : "acquired");
            return handle;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            activity?.SetTag("vc.lock.outcome", "cancelled");
            throw;
        }
        finally
        {
            activity?.SetTag("vc.lock.wait_ms", Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds);
        }
    }

    /// <summary>
    /// Acquires the lock within <paramref name="timeout"/>, or returns <c>null</c>. The timeout is validated:
    /// non-negative, or <see cref="Timeout.InfiniteTimeSpan"/> to wait until acquired or cancelled.
    /// </summary>
    protected abstract Task<IDistributedLockHandle?> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken);

    private static string HashResource(string resource)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(resource)))[..ResourceHashLength];
    }
}
