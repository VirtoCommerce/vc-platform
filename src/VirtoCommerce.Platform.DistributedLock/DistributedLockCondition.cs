using System;
using System.ComponentModel;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Distributed lock condition information to notice the payload method about running conditions
    /// </summary>
    [Obsolete("Platform startup synchronization only. Use IDistributedLock from VirtoCommerce.Platform.Core.DistributedLock.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    [EditorBrowsable(EditorBrowsableState.Never)]
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
