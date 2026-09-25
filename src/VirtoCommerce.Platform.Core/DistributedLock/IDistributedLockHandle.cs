#nullable enable

using System;
using System.Threading;

namespace VirtoCommerce.Platform.Core.DistributedLock;

/// <summary>
/// A held distributed lock. Dispose it to release the lock; Redis locks are extended automatically while held.
/// </summary>
public interface IDistributedLockHandle : IAsyncDisposable, IDisposable
{
    /// <summary>The resource name passed when the lock was acquired.</summary>
    string Resource { get; }

    /// <summary>
    /// Cancelled when the lock is lost while the handle is still held, for example because a Redis lock could not be
    /// extended and expired. Another instance may then hold the lock, so stop the protected work.
    /// <see cref="CancellationToken.None"/> for locks that cannot be lost, such as the in-process lock.
    /// </summary>
    CancellationToken HandleLostToken { get; }
}
