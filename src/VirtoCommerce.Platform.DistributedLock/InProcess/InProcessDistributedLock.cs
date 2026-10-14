#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock.InProcess;

/// <summary>
/// <see cref="IDistributedLock"/> for a single platform instance: serializes callers within this process.
/// Used when Redis is not configured, and in unit tests.
/// </summary>
public sealed class InProcessDistributedLock : DistributedLockBase
{
    // SemaphoreSlim accepts at most int.MaxValue milliseconds; longer waits are treated as infinite.
    private static readonly TimeSpan _maxWait = TimeSpan.FromMilliseconds(int.MaxValue);

    private readonly ConcurrentDictionary<string, LockEntry> _entries = new(StringComparer.Ordinal);

    public InProcessDistributedLock(IOptions<DistributedLockOptions> options, ILogger<InProcessDistributedLock> logger)
        : base(options)
    {
        logger.LogInformation("Distributed lock: Redis is not configured. Locks serialize callers within this platform instance only.");
    }

    protected override async Task<IDistributedLockHandle?> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var entry = AddReference(resource);
        var acquired = false;

        try
        {
            var wait = timeout > _maxWait ? Timeout.InfiniteTimeSpan : timeout;
            acquired = await entry.Semaphore.WaitAsync(wait, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            if (!acquired)
            {
                RemoveReference(resource, entry);
            }
        }

        return acquired ? new Handle(this, resource, entry) : null;
    }

    private LockEntry AddReference(string resource)
    {
        while (true)
        {
            var entry = _entries.GetOrAdd(resource, static _ => new LockEntry());
            if (entry.TryAddReference())
            {
                return entry;
            }

            // The entry is being removed by its last user; retry with a fresh one.
            Thread.Yield();
        }
    }

    private void RemoveReference(string resource, LockEntry entry)
    {
        if (entry.RemoveReference())
        {
            _entries.TryRemove(new KeyValuePair<string, LockEntry>(resource, entry));
        }
    }

    private sealed class LockEntry
    {
        // Waiting and holding callers; -1 marks an entry that is being removed.
        private int _references;

        public SemaphoreSlim Semaphore { get; } = new(1, 1);

        public bool TryAddReference()
        {
            var current = Volatile.Read(ref _references);
            while (current >= 0)
            {
                var observed = Interlocked.CompareExchange(ref _references, current + 1, current);
                if (observed == current)
                {
                    return true;
                }

                current = observed;
            }

            return false;
        }

        /// <summary>Returns true when the last reference was removed and the entry must leave the dictionary.</summary>
        public bool RemoveReference()
        {
            return Interlocked.Decrement(ref _references) == 0
                && Interlocked.CompareExchange(ref _references, -1, 0) == 0;
        }
    }

    private sealed class Handle : IDistributedLockHandle
    {
        private readonly InProcessDistributedLock _owner;
        private readonly LockEntry _entry;
        private int _released;

        public Handle(InProcessDistributedLock owner, string resource, LockEntry entry)
        {
            _owner = owner;
            _entry = entry;
            Resource = resource;
        }

        public string Resource { get; }

        // An in-process lock cannot be lost while held.
        public CancellationToken HandleLostToken => CancellationToken.None;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _released, 1) == 0)
            {
                _entry.Semaphore.Release();
                _owner.RemoveReference(Resource, _entry);
            }
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
