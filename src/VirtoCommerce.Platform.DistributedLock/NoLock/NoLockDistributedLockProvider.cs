using System;
using Microsoft.Extensions.Logging;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Distributed lock provider that implements bypass mode (no distributed lock)
    /// </summary>
    public class NoLockDistributedLockProvider : IDistributedLockProvider
    {
        protected readonly ILogger<NoLockDistributedLockProvider> _logger;


        /// <summary>
        /// Construct the provider
        /// </summary>
        /// <param name="logger"></param>
        public NoLockDistributedLockProvider(ILogger<NoLockDistributedLockProvider> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Run payload with no lock
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="payload"></param>
        public virtual void ExecuteSynhronized(string resourceId, Action<DistributedLockCondition> payload)
        {
            _logger.LogInformation(@$"Distributed lock: run payload for resource {resourceId} without lock.");
            payload(DistributedLockCondition.NoLock);
        }
    }
}
