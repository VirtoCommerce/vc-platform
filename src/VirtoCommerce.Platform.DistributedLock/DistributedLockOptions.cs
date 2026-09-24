using System;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Distributed lock options, bound to the <c>DistributedLock</c> configuration section.
    /// </summary>
    public class DistributedLockOptions
    {
        /// <summary>
        /// Seconds the platform startup lock waits for another instance.
        /// </summary>
        public int WaitTime { get; set; } = 180;

        /// <summary>
        /// Wait used by <c>IDistributedLock.AcquireAsync</c> when no timeout is passed.
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Redis lock time-to-live. Extended automatically while the lock is held, so it only bounds how long
        /// a crashed holder blocks other instances.
        /// </summary>
        public TimeSpan Expiry { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Interval between Redis acquisition attempts while waiting.
        /// </summary>
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Optional prefix for Redis lock keys (<c>redlock:{KeyPrefix}:{resource}</c>) to isolate applications that share Redis.
        /// Changing it during a rolling deploy breaks mutual exclusion between old and new instances.
        /// </summary>
        public string KeyPrefix { get; set; }
    }
}
