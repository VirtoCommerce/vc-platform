#nullable enable

using System;

namespace VirtoCommerce.Platform.Core.DistributedLock;

/// <summary>
/// A held distributed lock. Dispose it to release the lock; Redis locks are extended automatically while held.
/// </summary>
public interface IDistributedLockHandle : IAsyncDisposable, IDisposable
{
    /// <summary>The resource name passed when the lock was acquired.</summary>
    string Resource { get; }
}
