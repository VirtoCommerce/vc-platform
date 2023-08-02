using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.DistributedLock.Redis
{
    /// <summary>
    /// Distributed lock implemented thru Redis RedLock
    /// Used for synchronizing multiple platform instances
    /// </summary>
    public class InternalDistributedLockService : IInternalDistributedLockService
    {
        private readonly IDistributedLockFactory _distributedLockFactory;
        private readonly ILogger<InternalDistributedLockService> _logger;
        private readonly int _waitTime;

        /// <summary>
        /// Construct the provider
        /// </summary>
        /// <param name="distributedLockFactory">RedLockFactory, constructed with connection multiplexer pointing to the Redis server, used for locking</param>
        /// <param name="options">Total time to wait until the lock is available</param>
        /// <param name="logger"></param>        
        public InternalDistributedLockService(IDistributedLockFactory distributedLockFactory, IOptions<DistributedLockOptions> options, ILogger<InternalDistributedLockService> logger)
        {
            _distributedLockFactory = distributedLockFactory;
            _waitTime = options.Value.WaitTime;
            _logger = logger;
        }

        /// <summary>
        /// Run payload method with distributed lock
        /// </summary>        
        /// <param name="resourceId">Identifier of locking resource</param>
        /// <param name="payload">Payload method to run under the acquired lock</param>
        public virtual void ExecuteSynchronized(string resourceId, Action<DistributedLockCondition> payload)
        {
            var instantlyAcquired = false;
            var expiryTime = 120 + _waitTime;
            // Try to acquire distributed lock and giving up immediately if the lock is not available
            using (var redLock = _distributedLockFactory.CreateLock(resourceId,
                TimeSpan.FromSeconds(expiryTime) /* Successfully acquired lock expiration time */))
            {
                if (redLock.IsAcquired)
                {
                    instantlyAcquired = true;
                    _logger.LogInformation("Distributed lock: run payload for resource {resourceId} instantly.", resourceId);
                    payload(DistributedLockCondition.Instant);
                }
            }

            if (!instantlyAcquired)
            {
                // Try to acquire distributed lock with awaiting 
                using (var redLock = _distributedLockFactory.CreateLock(resourceId,
                    TimeSpan.FromSeconds(expiryTime) /* Successfully acquired lock expiration time */,
                    TimeSpan.FromSeconds(_waitTime) /* Total time to wait until the lock is available */,
                    TimeSpan.FromSeconds(3) /* The span to acquire the lock in retries */))
                {
                    if (!redLock.IsAcquired)
                    {
                        // Lock not acquired even after migrationDistributedLockOptions.Wait
                        throw new PlatformException($"Can't acquire distributed lock for resource {this}. It seems that another Platform instance still has the lock, consider increasing wait timeout.");
                    }
                    _logger.LogInformation("Distributed lock: run payload for resource {resourceId} after awaiting for previous lock.", resourceId);
                    payload(DistributedLockCondition.Delayed);
                }
            }
        }
    }
}
