namespace VirtoCommerce.Platform.Core.DistributedLock
{
    /// <summary>
    /// Distributed lock condition information to notice the payload method about running conditions
    /// </summary>
    public enum DistributedLockCondition
    {
        /// <summary>
        /// Distributed lock was acquired instantly
        /// </summary>
        Instant,
        /// <summary>
        /// Distributed lock was acquired after awaiting for previous lock
        /// </summary>
        Delayed,
        /// <summary>
        /// No distributed lock was acquired because of the Redis server non-configured
        /// </summary>
        NoRedis
    }
}
