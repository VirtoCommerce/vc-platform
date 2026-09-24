#nullable enable

using System;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.DistributedLock;

/// <summary>
/// The distributed lock was not acquired in time. Derives from <see cref="PlatformException"/>,
/// so existing <c>catch (PlatformException)</c> blocks keep working.
/// </summary>
public sealed class DistributedLockTimeoutException : PlatformException
{
    private const string DefaultMessage = "The distributed lock was not acquired in time.";

    public DistributedLockTimeoutException()
        : base(DefaultMessage)
    {
    }

    public DistributedLockTimeoutException(string message)
        : base(message)
    {
    }

    public DistributedLockTimeoutException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DistributedLockTimeoutException(string resource, TimeSpan timeout)
        : base(FormatMessage(resource, timeout))
    {
        Resource = resource;
        Timeout = timeout;
    }

    /// <param name="resource">The lock name.</param>
    /// <param name="timeout">The wait that elapsed.</param>
    /// <param name="innerException">The lock store failure that caused the timeout, for example a network timeout.</param>
    public DistributedLockTimeoutException(string resource, TimeSpan timeout, Exception innerException)
        : base(FormatMessage(resource, timeout), innerException)
    {
        Resource = resource;
        Timeout = timeout;
    }

    /// <summary>The lock name, or <c>null</c> when the exception was created with a custom message.</summary>
    public string? Resource { get; }

    /// <summary>The wait that elapsed, or <see cref="TimeSpan.Zero"/> when the exception was created with a custom message.</summary>
    public TimeSpan Timeout { get; }

    private static string FormatMessage(string resource, TimeSpan timeout)
    {
        return $"Distributed lock '{resource}' was not acquired within {timeout}.";
    }
}
