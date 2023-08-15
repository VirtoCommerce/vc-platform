using System;
using System.Threading;
using System.Threading.Tasks;
using RedLockNet;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.DistributedLock.Redis
{
    public class DistributedLockService : IDistributedLockService
    {
        private readonly IDistributedLockFactory _distributedLockFactory;
        private readonly TimeSpan _expiry = TimeSpan.FromSeconds(30);

        public DistributedLockService(IDistributedLockFactory distributedLockFactory)
        {
            _distributedLockFactory = distributedLockFactory;
        }

        public T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
        {
            using var redLock = tryLockTimeout.HasValue && retryInterval.HasValue
                ? _distributedLockFactory.CreateLock(resourceKey, lockTimeout ?? _expiry, tryLockTimeout.Value, retryInterval.Value, cancellationToken)
                : _distributedLockFactory.CreateLock(resourceKey, lockTimeout ?? _expiry);

            if (!redLock.IsAcquired)
            {
                throw new PlatformException($"Resource `{resourceKey}' is currently unavailable due to high demand.");
            }

            return resolver();
        }

        public async Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
        {
            var lockTask = tryLockTimeout.HasValue && retryInterval.HasValue
                ? _distributedLockFactory.CreateLockAsync(resourceKey, lockTimeout ?? _expiry, tryLockTimeout.Value, retryInterval.Value, cancellationToken)
                : _distributedLockFactory.CreateLockAsync(resourceKey, lockTimeout ?? _expiry);

            using var redLock = await lockTask;

            if (!redLock.IsAcquired)
            {
                throw new PlatformException($"Resource `{resourceKey}' is currently unavailable due to high demand.");
            }

            return await resolver();
        }
    }
}
