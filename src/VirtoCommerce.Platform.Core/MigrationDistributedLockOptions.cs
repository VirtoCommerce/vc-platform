using System;

namespace VirtoCommerce.Platform.Core
{
    /// <summary>
    /// Options for managing distributed lock access to apply migrations sequentially in multiinstance platform installations
    /// </summary>
    public class MigrationDistributedLockOptions
    {
        /// <summary>
        /// Total timeout to release acquired lock 
        /// </summary>
        public TimeSpan Expiry { get; set; } = new TimeSpan(0, 5, 0);
        /// <summary>
        /// Total time to wait until the lock is available
        /// </summary>
        public TimeSpan Wait { get; set; } = new TimeSpan(0, 3, 0);
        /// <summary>
        /// The span to acquire the lock in retries
        /// </summary>
        public TimeSpan Retry { get; set; } = new TimeSpan(0, 0, 3);
    }
}
