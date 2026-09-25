#nullable enable

using System;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.DistributedLock;

/// <summary>
/// The lock was lost while the protected action was running (<see cref="IDistributedLockHandle.HandleLostToken"/>),
/// so another instance may have run the same work concurrently. Thrown by the <c>Execute*</c> and <c>TryExecute*</c>
/// extensions after the action has completed or stopped; the work may already be done, so this is a signal, not a rollback.
/// Derives from <see cref="PlatformException"/>.
/// </summary>
public sealed class DistributedLockLostException : PlatformException
{
    private const string DefaultMessage = "The distributed lock was lost while held.";

    public DistributedLockLostException()
        : base(DefaultMessage)
    {
    }

    public DistributedLockLostException(string message)
        : base(message)
    {
    }

    public DistributedLockLostException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <param name="resource">The lock name.</param>
    /// <param name="details">What happened to the protected action, for example that it completed after the loss.</param>
    /// <param name="innerException">The exception the action stopped with, for example an <see cref="OperationCanceledException"/>.</param>
    public DistributedLockLostException(string resource, string details, Exception? innerException = null)
        : base($"Distributed lock '{resource}' was lost while held: {details}.", innerException)
    {
        Resource = resource;
    }

    /// <summary>The lock name, or <c>null</c> when the exception was created with a custom message.</summary>
    public string? Resource { get; }
}
