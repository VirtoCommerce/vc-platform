using System;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Basic implementation of distributed lock resource
    /// </summary>
    public class DistributedLockResource
    {
        /// <summary>
        /// Resource identifier to use in the distributed lock
        /// </summary>
        protected readonly string _resourceId ;
        protected readonly IConnectionMultiplexer _redisConnMultiplexer;
        protected readonly int _waitTime;

        /// <summary>
        /// Construct static lock resource with specified string key
        /// </summary>        
        /// <param name="redisConnMultiplexer">Connection multiplexer pointing to the Redis server, used for locking</param>
        /// <param name="waitTime">Total time to wait until the lock is available</param>
        /// <param name="resourceId"></param>
        public DistributedLockResource(IConnectionMultiplexer redisConnMultiplexer, int waitTime, string resourceId)
        {
            _redisConnMultiplexer = redisConnMultiplexer;
            _waitTime = waitTime;
            _resourceId = resourceId;
        }

        /// <summary>
        /// Run payload method with distributed lock
        /// </summary>
        /// <param name="payload">Payload method to run under the acquired lock</param>
        public virtual void WithLock(Action<DistributedLockCondition> payload)
        {
            if (_redisConnMultiplexer != null)
            {
                using (var redlockFactory = RedLockFactory.Create(new RedLockMultiplexer[] { new RedLockMultiplexer(_redisConnMultiplexer) }))
                {
                    var instantlyAcquired = false;
                    var expiryTime = 120 + _waitTime;
                    // Try to acquire distributed lock and giving up immediately if the lock is not available
                    using (var redLock = redlockFactory.CreateLock(_resourceId,
                        TimeSpan.FromSeconds(expiryTime) /* Successfully acquired lock expiration time */))
                    {
                        if (redLock.IsAcquired)
                        {
                            instantlyAcquired = true;
                            payload(DistributedLockCondition.Instant);
                        }
                    }

                    if (!instantlyAcquired)
                    {
                        // Try to acquire distributed lock with awaiting 
                        using (var redLock = redlockFactory.CreateLock(_resourceId,
                            TimeSpan.FromSeconds(expiryTime) /* Successfully acquired lock expiration time */,
                            TimeSpan.FromSeconds(_waitTime) /* Total time to wait until the lock is available */,
                            TimeSpan.FromSeconds(3) /* The span to acquire the lock in retries */))
                        {
                            if (!redLock.IsAcquired)
                            {
                                // Lock not acquired even after migrationDistributedLockOptions.Wait
                                throw new PlatformException($"Can't acquire distributed lock for resource {this}. It seems that another Platform instance still has the lock, consider increasing wait timeout.");
                            }
                            payload(DistributedLockCondition.Delayed);
                        }
                    }
                }
            }
            else
            {
                // One-instance configuration, no Redis, just run the payload
                payload(DistributedLockCondition.NoRedis);
            }
        }

        public override string ToString()
        {
            return $@"{GetType().Name}-{_resourceId}";
        }
    }
}
