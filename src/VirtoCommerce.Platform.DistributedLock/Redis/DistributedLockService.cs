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
        private readonly TimeSpan _expiry = TimeSpan.FromSeconds(20);

        public DistributedLockService(IDistributedLockFactory distributedLockFactory)
        {
            _distributedLockFactory = distributedLockFactory;
        }

        public T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? expireTime = null, TimeSpan? waitTime = null, TimeSpan? retryTime = null, CancellationToken? cancellationToken = null)
        {
            using var redLock = waitTime.HasValue && retryTime.HasValue
                ? _distributedLockFactory.CreateLock(resourceKey, expireTime ?? _expiry, waitTime.Value, retryTime.Value, cancellationToken)
                : _distributedLockFactory.CreateLock(resourceKey, expireTime ?? _expiry);

            if (!redLock.IsAcquired)
            {
                throw new PlatformException("Service is busy.");
            }

            return resolver();
        }

        public async Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? expireTime = null, TimeSpan? waitTime = null, TimeSpan? retryTime = null, CancellationToken? cancellationToken = null)
        {
            var lockTask = waitTime.HasValue && retryTime.HasValue
                ? _distributedLockFactory.CreateLockAsync(resourceKey, expireTime ?? _expiry, waitTime.Value, retryTime.Value, cancellationToken)
                : _distributedLockFactory.CreateLockAsync(resourceKey, expireTime ?? _expiry);

            using var redLock = await lockTask;

            if (!redLock.IsAcquired)
            {
                throw new PlatformException("Service is busy.");
            }

            return await resolver();
        }
    }
}
