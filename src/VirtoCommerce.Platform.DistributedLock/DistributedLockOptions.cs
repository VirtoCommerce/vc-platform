namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Distributed lock options
    /// </summary>
    public class DistributedLockOptions
    {
        /// <summary>
        /// Total time to wait until the lock is available
        /// </summary>
        public int WaitTime { get; set; } = 180;
    }
}
