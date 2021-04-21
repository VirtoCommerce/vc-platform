using System;
using System.IO;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Modules
{
    /// <summary>
    /// Distributed lock for local storage.
    /// Creates a file with Guid to mark the storage. Next, this Guid is used as a resource id for distributed lock.
    /// </summary>
    public class LocalStorageDistributedLockResource : DistributedLockResourceBase
    {
        /// <summary>
        /// Try marking the storage by placing mark-file at the specified path
        /// </summary>
        /// <param name="redisConnMultiplexer">Connection multiplexer pointing to the Redis server, used for locking</param>
        /// <param name="waitTime">Total time to wait until the lock is available</param>
        /// <param name="markerFilePath"></param>
        public LocalStorageDistributedLockResource(IConnectionMultiplexer redisConnMultiplexer, int waitTime, string markerFilePath) : base(redisConnMultiplexer, waitTime)
        {
            try
            {
                if (File.Exists(markerFilePath))
                {
                    using (var stream = File.OpenText(markerFilePath))
                    {
                        ResourceId = stream.ReadToEnd();
                    }
                }
                else
                {
                    // Non-marked storage, mark by placing a file with resource id.
                    ResourceId = Guid.NewGuid().ToString();
                    using (var stream = File.CreateText(markerFilePath))
                    {
                        stream.Write(ResourceId);
                    }
                }
            }
            catch (IOException exc)
            {
                throw new PlatformException($"An IO error occurred while marking local modules storage.", exc);
            }
        }
    }
}
