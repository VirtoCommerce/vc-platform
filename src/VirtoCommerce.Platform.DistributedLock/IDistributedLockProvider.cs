using System;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Interface for distributed lock implementations
    /// </summary>
    public interface IDistributedLockProvider
    {
        /// <summary>
        /// Run payload method with a distributed lock
        /// </summary>        
        /// <param name="resourceId">Identifier of locking resource</param>
        /// <param name="payload">Payload method to run under the acquired lock</param>
        public void ExecuteSynchronized(string resourceId, Action<DistributedLockCondition> payload);
    }
}
