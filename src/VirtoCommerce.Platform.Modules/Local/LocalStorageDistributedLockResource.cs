using System;
using System.IO;
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
        /// <param name="markerFilePath"></param>
        public LocalStorageDistributedLockResource(string markerFilePath)
        {
            try
            {
                if (!File.Exists(markerFilePath))
                {
                    // Non-marked storage, mark by placing a file with unique id.
                    using (var stream = File.CreateText(markerFilePath))
                    {
                        stream.Write(Guid.NewGuid());
                    }
                }

                using (var stream = File.OpenText(markerFilePath))
                {
                    ResourceId = stream.ReadToEnd();
                }
            }
            catch (Exception exc)
            {
                throw new PlatformException($"An IO error occurred while marking local modules storage.", exc);
            }
        }
    }
}
