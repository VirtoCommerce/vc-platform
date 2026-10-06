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
        /// Redis lock time-to-live, renewed every <c>Expiry / 2</c> while the handle is held. It bounds two things:
        /// how long a crashed holder blocks other instances, and how long renewal may fail (for example during a Redis outage)
        /// before a live holder loses the lock (<c>IDistributedLockHandle.HandleLostToken</c>) and another instance can take it.
        /// </summary>
        public TimeSpan Expiry { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// First delay between Redis acquisition attempts while waiting. Later delays grow by 1.8 times, with ±20% jitter,
        /// up to <see cref="MaxRetryInterval"/>, so waiters do not poll a busy key in lockstep.
        /// </summary>
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Upper bound for the delay between Redis acquisition attempts while waiting.
        /// </summary>
        public TimeSpan MaxRetryInterval { get; set; } = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Redis lock time-to-live for platform startup synchronization (migrations and module <c>PostInitialize</c>).
        /// Longer than <see cref="Expiry"/> so a Redis brownout during a long migration does not let another instance start migrating.
        /// Default 5 minutes, the lease used before this setting existed (120 s + <see cref="WaitTime"/>).
        /// </summary>
        public TimeSpan StartupExpiry { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Optional prefix for Redis lock keys (<c>redlock:{KeyPrefix}:{resource}</c>) to isolate applications that share Redis.
        /// Changing it during a rolling deploy breaks mutual exclusion between old and new instances.
        /// </summary>
        public string KeyPrefix { get; set; }
    }
}
