using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Distributed lock implemented thru Redis RedLock
    /// </summary>
    public class RedLockDistributedLockProvider : IDistributedLockProvider
    {
        protected readonly IConnectionMultiplexer _redisConnMultiplexer;
        protected readonly ILogger<RedLockDistributedLockProvider> _logger;
        protected readonly int _waitTime;

        /// <summary>
        /// Construct the provider
        /// </summary>
        /// <param name="redisConnMultiplexer">Connection multiplexer pointing to the Redis server, used for locking</param>
        /// <param name="options">Total time to wait until the lock is available</param>
        /// <param name="logger"></param>        
        public RedLockDistributedLockProvider(IConnectionMultiplexer redisConnMultiplexer, IOptions<DistributedLockOptions> options, ILogger<RedLockDistributedLockProvider> logger)
        {
            _redisConnMultiplexer = redisConnMultiplexer;
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
            using (var redlockFactory = RedLockFactory.Create(new RedLockMultiplexer[] { new RedLockMultiplexer(_redisConnMultiplexer) }))
            {
                var instantlyAcquired = false;
                var expiryTime = 120 + _waitTime;
                // Try to acquire distributed lock and giving up immediately if the lock is not available
                using (var redLock = redlockFactory.CreateLock(resourceId,
                    TimeSpan.FromSeconds(expiryTime) /* Successfully acquired lock expiration time */))
                {
                    if (redLock.IsAcquired)
                    {
                        instantlyAcquired = true;
                        _logger.LogInformation(@$"Distributed lock: run payload for resource {resourceId} instantly.");
                        payload(DistributedLockCondition.Instant);
                    }
                }

                if (!instantlyAcquired)
                {
                    // Try to acquire distributed lock with awaiting 
                    using (var redLock = redlockFactory.CreateLock(resourceId,
                        TimeSpan.FromSeconds(expiryTime) /* Successfully acquired lock expiration time */,
                        TimeSpan.FromSeconds(_waitTime) /* Total time to wait until the lock is available */,
                        TimeSpan.FromSeconds(3) /* The span to acquire the lock in retries */))
                    {
                        if (!redLock.IsAcquired)
                        {
                            // Lock not acquired even after migrationDistributedLockOptions.Wait
                            throw new PlatformException($"Can't acquire distributed lock for resource {this}. It seems that another Platform instance still has the lock, consider increasing wait timeout.");
                        }
                        _logger.LogInformation(@$"Distributed lock: run payload for resource {resourceId} after awaiting for previous lock.");
                        payload(DistributedLockCondition.Delayed);
                    }
                }
            }
        }
    }
}
