using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.DistributedLock
{
    public interface IDistributedLockService
    {
        T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? expireTime = null, TimeSpan? waitTime = null, TimeSpan? retryTime = null, CancellationToken? cancellationToken = null);

        Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? expireTime = null, TimeSpan? waitTime = null, TimeSpan? retryTime = null, CancellationToken? cancellationToken = null);
    }
}
