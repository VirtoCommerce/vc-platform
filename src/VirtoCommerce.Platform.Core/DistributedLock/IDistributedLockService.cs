using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DistributedLock
{
    /// <summary>
    /// Povides a unified mechanism for acquiring and managing distributed locks. It encapsulates the execution of critical sections of code in a distributed environment, ensuring that only one process can access the locked resource at a time. 
    /// </summary>
    public interface IDistributedLockService
    {
        /// <summary>
        /// Critical sections of code within a distributed lock, allowing only one process to execute the enclosed code block at a time.
        /// </summary>
        /// <typeparam name="T">The type of the result produced by resolver.</typeparam>
        /// <param name="resourceKey">The resource string to lock on.</param>
        /// <param name="resolver"></param>
        /// <param name="expireTime">The duration for which the lock will be held before it is automatically released. Recommended value: 1 minute.</param>
        /// <param name="waitTime">How long to block for until a lock can be acquired. Recommended value: 10 seconds.</param>
        /// <param name="retryTime">The time interval between retry attempts when trying to acquire the lock. Recommended value: 100 milliseconds.</param>
        /// <param name="cancellationToken">CancellationToken to abort waiting for blocking lock.</param>
        /// <returns>Returns the result of executing the resolver delegate under the acquired lock.</returns>
        /// <exception cref="VirtoCommerce.Platform.Core.Exceptions.PlatformException">Throws PlatformException if resource is currently unavailable due to high demand.</exception>
        T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? expireTime = null, TimeSpan? waitTime = null, TimeSpan? retryTime = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Critical sections of asynchronous code within a distributed lock, allowing only one process to execute the enclosed code block at a time.
        /// </summary>
        /// <typeparam name="T">The type of the result produced by resolver.</typeparam>
        /// <param name="resourceKey">The resource string to lock on.</param>
        /// <param name="resolver"></param>
        /// <param name="expireTime">The duration for which the lock will be held before it is automatically released. Recommended value: 1 minute.</param>
        /// <param name="waitTime">How long to block for until a lock can be acquired. Recommended value: 10 seconds.</param>
        /// <param name="retryTime">The time interval between retry attempts when trying to acquire the lock. Recommended value: 100 milliseconds.</param>
        /// <param name="cancellationToken">CancellationToken to abort waiting for blocking lock.</param>
        /// <returns>Returns the result of executing the resolver delegate under the acquired lock.</returns>
        /// <exception cref="VirtoCommerce.Platform.Core.Exceptions.PlatformException">Throws PlatformException if resource is currently unavailable due to high demand.</exception>
        Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? expireTime = null, TimeSpan? waitTime = null, TimeSpan? retryTime = null, CancellationToken? cancellationToken = null);
    }
}
