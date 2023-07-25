using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.DistributedLock.NoLock
{
    public class NoLockService : IDistributedLockService
    {
        public T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? expireTime = null, TimeSpan? waitTime = null, TimeSpan? retryTime = null, CancellationToken? cancellationToken = null)
        {
            return resolver();
        }

        public Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? expireTime = null, TimeSpan? waitTime = null, TimeSpan? retryTime = null, CancellationToken? cancellationToken = null)
        {
            return resolver();
        }
    }
}
