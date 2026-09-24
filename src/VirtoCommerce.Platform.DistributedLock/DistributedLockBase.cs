using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Validation, default timeout, timeout exception and tracing shared by <see cref="IDistributedLock"/> implementations.
    /// </summary>
    public abstract class DistributedLockBase : IDistributedLock
    {
        public const string ActivitySourceName = "VirtoCommerce.Platform.DistributedLock";

        private static readonly ActivitySource _activitySource = new(ActivitySourceName);

        protected DistributedLockBase(IOptions<DistributedLockOptions> options)
        {
            LockOptions = options.Value;
        }

        protected DistributedLockOptions LockOptions { get; }

        public virtual async Task<IDistributedLockHandle> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            var effectiveTimeout = timeout ?? LockOptions.DefaultTimeout;

            return await TryAcquireAsync(resource, effectiveTimeout, cancellationToken)
                ?? throw new DistributedLockTimeoutException(resource, effectiveTimeout);
        }

        public virtual async Task<IDistributedLockHandle> TryAcquireAsync(string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(resource);
            ArgumentOutOfRangeException.ThrowIfLessThan(timeout, TimeSpan.Zero);
            cancellationToken.ThrowIfCancellationRequested();

            using var activity = _activitySource.StartActivity("DistributedLock acquire");
            activity?.SetTag("vc.lock.resource", resource);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var handle = await TryAcquireCoreAsync(resource, timeout, cancellationToken);
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
                activity?.SetTag("vc.lock.wait_ms", stopwatch.Elapsed.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Acquires the lock within <paramref name="timeout"/> (already validated), or returns <c>null</c>.
        /// </summary>
        protected abstract Task<IDistributedLockHandle> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken);
    }
}
