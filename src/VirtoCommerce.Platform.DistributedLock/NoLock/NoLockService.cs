using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock.NoLock
{
    [Obsolete("No longer registered. Resolve IDistributedLock, or IDistributedLockService for existing code.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public class NoLockService : IDistributedLockService
    {
        public T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
        {
            return resolver();
        }

        public Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
        {
            return resolver();
        }
    }
}
