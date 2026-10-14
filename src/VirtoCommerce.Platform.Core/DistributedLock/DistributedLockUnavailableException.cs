#nullable enable

using System;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.DistributedLock;

/// <summary>
/// The lock store could not be reached, for example Redis is down, so it is unknown whether the resource is free.
/// Thrown by both <c>AcquireAsync</c> and <c>TryAcquireAsync</c>: a <c>null</c> result or a
/// <see cref="DistributedLockTimeoutException"/> always means another holder, never an outage.
/// Derives from <see cref="PlatformException"/>.
/// </summary>
public sealed class DistributedLockUnavailableException : PlatformException
{
    private const string DefaultMessage = "The distributed lock store is unavailable.";

    public DistributedLockUnavailableException()
        : base(DefaultMessage)
    {
    }

    public DistributedLockUnavailableException(string message)
        : base(message)
    {
    }

    public DistributedLockUnavailableException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <param name="resource">The lock name.</param>
    /// <param name="details">What the lock store reported, for example the per-instance results.</param>
    /// <param name="innerException">The lock store failure, when there is one.</param>
    public DistributedLockUnavailableException(string resource, string details, Exception? innerException = null)
        : base($"Distributed lock '{resource}' could not be acquired because the lock store is unavailable: {details}.", innerException)
    {
        Resource = resource;
    }

    /// <summary>The lock name, or <c>null</c> when the exception was created with a custom message.</summary>
    public string? Resource { get; }
}
