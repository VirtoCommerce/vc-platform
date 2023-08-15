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
        /// Executes critical sections of code within a distributed lock, allowing only one process to execute the enclosed code block at a time.
        /// </summary>
        /// <typeparam name="T">The type of the result produced by resolver.</typeparam>
        /// <param name="resourceKey">The resource string to lock on.</param>
        /// <param name="resolver">The delegate to execute within the lock.</param>
        /// <param name="lockTimeout">The maximum duration the resource will be locked. Default is 30 seconds.</param>
        /// <param name="tryLockTimeout">The duration to attempt acquiring the locked resource. Null throws an exception immediately if the resource is locked.</param>
        /// <param name="retryInterval">The interval between lock acquisition retries. Used with tryLockTimeout, otherwise ignored.</param>
        /// <param name="cancellationToken">A CancellationToken to abort waiting for a blocking lock.</param>
        /// <returns>The result of executing the resolver delegate under the acquired lock.</returns>
        /// <exception cref="VirtoCommerce.Platform.Core.Exceptions.PlatformException">Thrown if the resource is currently unavailable due to high demand.</exception>
        T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Executes asynchronous critical sections of code within a distributed lock, allowing only one process to execute the enclosed code block at a time.
        /// </summary>
        /// <typeparam name="T">The type of the result produced by resolver.</typeparam>
        /// <param name="resourceKey">The resource string to lock on.</param>
        /// <param name="resolver">The asynchronous delegate to execute within the lock.</param>
        /// <param name="lockTimeout">The maximum duration the resource will be locked. Default is 30 seconds.</param>
        /// <param name="tryLockTimeout">The duration to attempt acquiring the locked resource. Null throws an exception immediately if the resource is locked.</param>
        /// <param name="retryInterval">The interval between lock acquisition retries. Used with tryLockTimeout, otherwise ignored.</param>
        /// <param name="cancellationToken">A CancellationToken to abort waiting for a blocking lock.</param>
        /// <returns>A task representing the asynchronous execution of the resolver delegate under the acquired lock.</returns>
        /// <exception cref="VirtoCommerce.Platform.Core.Exceptions.PlatformException">Thrown if the resource is currently unavailable due to high demand.</exception>
        Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null);
    }
}
