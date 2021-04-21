using StackExchange.Redis;

namespace VirtoCommerce.Platform.Core.DistributedLock
{
    /// <summary>
    /// Simple static lock resource with specified string key
    /// </summary>
    public class StaticDistributedLockResource : DistributedLockResourceBase
    {
        /// <summary>
        /// Construct static lock resource with specified string key
        /// </summary>        
        /// <param name="redisConnMultiplexer">Connection multiplexer pointing to the Redis server, used for locking</param>
        /// <param name="waitTime">Total time to wait until the lock is available</param>
        /// <param name="resourceId"></param>
        public StaticDistributedLockResource(IConnectionMultiplexer redisConnMultiplexer, int waitTime, string resourceId) : base(redisConnMultiplexer, waitTime)
        {
            ResourceId = resourceId;
        }
    }
}
