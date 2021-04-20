using System;
using System.IO;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Modules
{
    /// <summary>
    /// Distributed lock for local storage.
    /// Creates some file with Guid to mark the storage. Next, this Guid used as a resource id for distributed lock.    
    /// </summary>
    public class LocalStorageDistributedLockResource : DistributedLockResourceBase
    {
        /// <summary>
        /// Constructs local storage with a try to mark storage by placing mark-file at specified path
        /// </summary>
        /// <param name="markerFilePath"></param>
        public LocalStorageDistributedLockResource(string markerFilePath)
        {
            try
            {
                if (!File.Exists(markerFilePath))
                {
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
                throw new PlatformException($"An IO error occurred while mark local modules storage", exc);
            }
        }
    }
}
