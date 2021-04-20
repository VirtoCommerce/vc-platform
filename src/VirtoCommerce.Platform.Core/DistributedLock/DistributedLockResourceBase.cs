using System;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.DistributedLock
{
    /// <summary>
    /// Basic implementation of distributed lock resource
    /// </summary>
    public class DistributedLockResourceBase
    {
        /// <summary>
        /// Resource identifier to use in the distributed lock
        /// </summary>
        protected string ResourceId { get; set; } = string.Empty;

        /// <summary>
        /// Run payload method with distributed lock
        /// </summary>
        /// <param name="redisConnMultiplexer">Connection multiplexer pointing to the Redis server, used for locking</param>
        /// <param name="waitTime">Total time to wait until the lock is available</param>
        /// <param name="payload">Payload method to run under the acquired lock</param>
        public virtual void WithLock(IConnectionMultiplexer redisConnMultiplexer, int waitTime, Action<DistributedLockCondition> payload)
        {
            if (redisConnMultiplexer != null)
            {
                using (var redlockFactory = RedLockFactory.Create(new RedLockMultiplexer[] { new RedLockMultiplexer(redisConnMultiplexer) }))
                {
                    var instantlyAcquired = false;
                    var expiryTime = 120 + waitTime;
                    // Try to acquire distributed lock giving up immediately if the lock is not available
                    using (var redLock = redlockFactory.CreateLock(ResourceId,
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
                        using (var redLock = redlockFactory.CreateLock(ResourceId,
                            TimeSpan.FromSeconds(expiryTime) /* Successfully acquired lock expiration time */,
                            TimeSpan.FromSeconds(waitTime) /* Total time to wait until the lock is available */,
                            TimeSpan.FromSeconds(3) /* The span to acquire the lock in retries */))
                        {
                            if (redLock.IsAcquired)
                            {
                                payload(DistributedLockCondition.Delayed);
                            }
                            else
                            {
                                // Lock not acquired even after migrationDistributedLockOptions.Wait
                                throw new PlatformException($"Can't acquire distribute lock for resource {this}. It seems another platform instance still has the lock. Consider to increase wait timeout.");
                            }
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
            return $@"{GetType().Name}-{ResourceId}";
        }
    }
}
