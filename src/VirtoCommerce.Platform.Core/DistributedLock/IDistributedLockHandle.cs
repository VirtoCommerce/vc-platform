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
    /// Cancelled when the lock is detected as lost while the handle is still held, for example because a Redis lock
    /// could not be extended. Another instance may then hold the lock, so stop the protected work.
    /// Detection is best effort and after the fact: another instance may acquire the lock shortly before this token is cancelled.
    /// Callbacks registered on it run on a lock timer thread; keep them short and non-blocking.
    /// <see cref="CancellationToken.None"/> for locks that cannot be lost, such as the in-process lock.
    /// </summary>
    CancellationToken HandleLostToken { get; }
}
