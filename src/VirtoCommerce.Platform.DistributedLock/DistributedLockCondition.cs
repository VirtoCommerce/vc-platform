namespace VirtoCommerce.Platform.DistributedLock
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
        /// Distributed lock wasn't acquired because of run in bypass (no distributed lock)
        /// </summary>
        NoLock
    }
}
