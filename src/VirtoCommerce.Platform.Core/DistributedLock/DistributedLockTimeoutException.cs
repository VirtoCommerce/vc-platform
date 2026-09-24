using System;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.DistributedLock
{
    /// <summary>
    /// The distributed lock was not acquired in time. Derives from <see cref="PlatformException"/>,
    /// so existing <c>catch (PlatformException)</c> blocks keep working.
    /// </summary>
    public sealed class DistributedLockTimeoutException : PlatformException
    {
        public DistributedLockTimeoutException(string resource, TimeSpan timeout)
            : base($"Distributed lock '{resource}' was not acquired within {timeout}.")
        {
            Resource = resource;
            Timeout = timeout;
        }

        public string Resource { get; }

        public TimeSpan Timeout { get; }
    }
}
