# Distributed lock — Platform implementation plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add the `IDistributedLock` API to Platform with Redis and in-process implementations, run the existing `IDistributedLockService` on it, and hide the startup lock from modules.

**Architecture:** Contracts live in `VirtoCommerce.Platform.Core.DistributedLock`. Implementations live in `VirtoCommerce.Platform.DistributedLock` and share `DistributedLockBase` for validation, default timeout, timeout exception and tracing. `AddRedis` registers the Redis implementation when a Redis connection string is configured and the in-process implementation otherwise; the existing `IDistributedLockService` becomes an adapter over `IDistributedLock`.

**Tech Stack:** .NET 10, RedLock.net 2.3.2, xUnit v3, FluentAssertions 7, Moq.

Spec: `docs/superpowers/specs/2026-09-24-unified-distributed-lock-design.md`.

Branch: `feat/VCST-6052` (from `dev`).

Build note: `Directory.Build.props` sets `TreatWarningsAsErrors`. Every Platform use of a member marked `[Obsolete(DiagnosticId = "VC0015")]` needs `#pragma warning disable VC0015`, unless the using member is itself obsolete.

---

## File structure

| File | Responsibility |
|---|---|
| Create `src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLock.cs` | Public lock contract |
| Create `src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLockHandle.cs` | Held-lock handle |
| Create `src/VirtoCommerce.Platform.Core/DistributedLock/DistributedLockTimeoutException.cs` | Timeout failure, derives from `PlatformException` |
| Create `src/VirtoCommerce.Platform.Core/DistributedLock/DistributedLockExtensions.cs` | `ExecuteAsync` / `TryExecuteAsync` helpers |
| Modify `src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLockService.cs` | Deprecation remarks |
| Modify `src/VirtoCommerce.Platform.DistributedLock/DistributedLockOptions.cs` | New settings |
| Create `src/VirtoCommerce.Platform.DistributedLock/DistributedLockBase.cs` | Shared validation, timeout, tracing |
| Create `src/VirtoCommerce.Platform.DistributedLock/InProcess/InProcessDistributedLock.cs` | Single-instance implementation |
| Create `src/VirtoCommerce.Platform.DistributedLock/Redis/RedisDistributedLock.cs` | Redis implementation |
| Create `src/VirtoCommerce.Platform.DistributedLock/DistributedLockServiceAdapter.cs` | Existing `IDistributedLockService` on `IDistributedLock` |
| Modify `src/VirtoCommerce.Platform.DistributedLock/Redis/DistributedLockService.cs`, `NoLock/NoLockService.cs` | `[Obsolete]`, no longer registered |
| Modify `src/VirtoCommerce.Platform.DistributedLock/IInternalDistributedLockService.cs`, `DistributedLockCondition.cs`, `NoLock/InternalNoLockService.cs`, `Redis/InternalDistributedLockService.cs` | Startup lock `[Obsolete]` + `[EditorBrowsable(Never)]`; message fix |
| Modify `src/VirtoCommerce.Platform.Web/Extensions/ApplicationBuilderExtensions.cs` | Suppress VC0015 for the startup lock |
| Modify `src/VirtoCommerce.Platform.Web/Redis/ServiceCollectionExtensions.cs` | Registration |
| Create `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/*.cs` | Tests |
| Create `docs/fundamentals/distributed-lock.md` | Developer guide |

---

### Task 1: Core contracts

**Files:**
- Create: `src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLock.cs`
- Create: `src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLockHandle.cs`
- Create: `src/VirtoCommerce.Platform.Core/DistributedLock/DistributedLockTimeoutException.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/DistributedLockTimeoutExceptionTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System;
using FluentAssertions;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

public class DistributedLockTimeoutExceptionTests
{
    [Fact]
    public void Constructor_SetsResourceTimeoutAndMessage()
    {
        var exception = new DistributedLockTimeoutException("cart:1", TimeSpan.FromSeconds(5));

        exception.Should().BeAssignableTo<PlatformException>();
        exception.Resource.Should().Be("cart:1");
        exception.Timeout.Should().Be(TimeSpan.FromSeconds(5));
        exception.Message.Should().Contain("cart:1").And.Contain("00:00:05");
    }
}
```

- [ ] **Step 2: Run the test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: build error `The type or namespace name 'DistributedLockTimeoutException' could not be found`.

- [ ] **Step 3: Create the contracts**

`IDistributedLock.cs`:

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DistributedLock
{
    /// <summary>
    /// Cluster-wide mutual exclusion for module and solution code.
    /// Redis-backed when <c>ConnectionStrings:RedisConnectionString</c> is configured; otherwise an in-process lock
    /// that serializes callers within one platform instance. Locks are not reentrant.
    /// </summary>
    /// <example>
    /// <code>
    /// await using var handle = await distributedLock.AcquireAsync($"cart:recalc:{cartId}", cancellationToken: cancellationToken);
    /// </code>
    /// </example>
    public interface IDistributedLock
    {
        /// <summary>
        /// Acquires the lock, waiting up to <paramref name="timeout"/> (default: <c>DistributedLock:DefaultTimeout</c>).
        /// </summary>
        /// <param name="resource">Lock name, for example <c>cart:recalc:{cartId}</c>. Must not contain secrets.</param>
        /// <param name="timeout">Maximum wait; <see cref="TimeSpan.Zero"/> tries once.</param>
        /// <param name="cancellationToken">Cancels the wait.</param>
        /// <returns>A handle that releases the lock when disposed.</returns>
        /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
        /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled while waiting.</exception>
        Task<IDistributedLockHandle> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tries to acquire the lock within <paramref name="timeout"/>. The default tries once without waiting.
        /// </summary>
        /// <returns>A handle that releases the lock when disposed, or <c>null</c> when the lock is held elsewhere.</returns>
        /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled while waiting.</exception>
        Task<IDistributedLockHandle> TryAcquireAsync(string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default);
    }
}
```

`IDistributedLockHandle.cs`:

```csharp
using System;

namespace VirtoCommerce.Platform.Core.DistributedLock
{
    /// <summary>
    /// A held distributed lock. Dispose it to release the lock; Redis locks are extended automatically while held.
    /// </summary>
    public interface IDistributedLockHandle : IAsyncDisposable, IDisposable
    {
        /// <summary>The resource name passed when the lock was acquired.</summary>
        string Resource { get; }
    }
}
```

`DistributedLockTimeoutException.cs`:

```csharp
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
```

- [ ] **Step 4: Run the test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: PASS (1 test).

- [ ] **Step 5: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/DistributedLock tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock
git commit -m "feat(VCST-6052): add IDistributedLock contracts"
```

---

### Task 2: Options

**Files:**
- Modify: `src/VirtoCommerce.Platform.DistributedLock/DistributedLockOptions.cs`

- [ ] **Step 1: Replace the file**

```csharp
using System;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Distributed lock options, bound to the <c>DistributedLock</c> configuration section.
    /// </summary>
    public class DistributedLockOptions
    {
        /// <summary>
        /// Seconds the platform startup lock waits for another instance.
        /// </summary>
        public int WaitTime { get; set; } = 180;

        /// <summary>
        /// Wait used by <c>IDistributedLock.AcquireAsync</c> when no timeout is passed.
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Redis lock time-to-live. Extended automatically while the lock is held, so it only bounds how long
        /// a crashed holder blocks other instances.
        /// </summary>
        public TimeSpan Expiry { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Interval between Redis acquisition attempts while waiting.
        /// </summary>
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Optional prefix for Redis lock keys (<c>redlock:{KeyPrefix}:{resource}</c>) to isolate applications that share Redis.
        /// Changing it during a rolling deploy breaks mutual exclusion between old and new instances.
        /// </summary>
        public string KeyPrefix { get; set; }
    }
}
```

- [ ] **Step 2: Build**

Run: `dotnet build src/VirtoCommerce.Platform.DistributedLock/VirtoCommerce.Platform.DistributedLock.csproj`
Expected: `Build succeeded` with 0 errors.

- [ ] **Step 3: Commit**

```bash
git add src/VirtoCommerce.Platform.DistributedLock/DistributedLockOptions.cs
git commit -m "feat(VCST-6052): add distributed lock timeout, expiry, retry and key prefix options"
```

---

### Task 3: Base class and in-process lock

**Files:**
- Create: `src/VirtoCommerce.Platform.DistributedLock/DistributedLockBase.cs`
- Create: `src/VirtoCommerce.Platform.DistributedLock/InProcess/InProcessDistributedLock.cs`
- Create: `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/TestLocks.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/InProcessDistributedLockTests.cs`

- [ ] **Step 1: Write the test helper and failing tests**

`TestLocks.cs`:

```csharp
using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.InProcess;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

internal static class TestLocks
{
    public static InProcessDistributedLock CreateInProcess(DistributedLockOptions options = null)
    {
        return new InProcessDistributedLock(
            Microsoft.Extensions.Options.Options.Create(options ?? new DistributedLockOptions()),
            NullLogger<InProcessDistributedLock>.Instance);
    }
}
```

`InProcessDistributedLockTests.cs`:

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.DistributedLock;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

public class InProcessDistributedLockTests
{
    private const string Resource = "test:resource";

    private static CancellationToken Token => TestContext.Current.CancellationToken;

    [Fact]
    public async Task TryAcquireAsync_WhenFree_ReturnsHandleForResource()
    {
        var distributedLock = TestLocks.CreateInProcess();

        await using var handle = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        handle.Should().NotBeNull();
        handle.Resource.Should().Be(Resource);
    }

    [Fact]
    public async Task TryAcquireAsync_WhenHeld_ReturnsNullWithoutWaiting()
    {
        var distributedLock = TestLocks.CreateInProcess();
        await using var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        var second = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        second.Should().BeNull();
    }

    [Fact]
    public async Task TryAcquireAsync_WithTimeout_AcquiresAfterRelease()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        var waiting = distributedLock.TryAcquireAsync(Resource, TimeSpan.FromSeconds(5), Token);
        await held.DisposeAsync();
        await using var acquired = await waiting;

        acquired.Should().NotBeNull();
    }

    [Fact]
    public async Task TryAcquireAsync_DifferentResources_DoNotBlockEachOther()
    {
        var distributedLock = TestLocks.CreateInProcess();
        await using var first = await distributedLock.TryAcquireAsync("test:a", cancellationToken: Token);

        await using var second = await distributedLock.TryAcquireAsync("test:b", cancellationToken: Token);

        second.Should().NotBeNull();
    }

    [Fact]
    public async Task AcquireAsync_WhenHeldPastTimeout_ThrowsDistributedLockTimeoutException()
    {
        var distributedLock = TestLocks.CreateInProcess();
        await using var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        var act = () => distributedLock.AcquireAsync(Resource, TimeSpan.FromMilliseconds(50), Token);

        var exception = (await act.Should().ThrowAsync<DistributedLockTimeoutException>()).Which;
        exception.Resource.Should().Be(Resource);
        exception.Timeout.Should().Be(TimeSpan.FromMilliseconds(50));
    }

    [Fact]
    public async Task AcquireAsync_WithoutTimeout_UsesDefaultTimeout()
    {
        var distributedLock = TestLocks.CreateInProcess(new DistributedLockOptions { DefaultTimeout = TimeSpan.FromMilliseconds(50) });
        await using var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        var act = () => distributedLock.AcquireAsync(Resource, cancellationToken: Token);

        (await act.Should().ThrowAsync<DistributedLockTimeoutException>()).Which.Timeout.Should().Be(TimeSpan.FromMilliseconds(50));
    }

    [Fact]
    public async Task TryAcquireAsync_WhenCancelledWhileWaiting_ThrowsAndKeepsLockUsable()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(Token);

        var waiting = distributedLock.TryAcquireAsync(Resource, TimeSpan.FromSeconds(30), cancellation.Token);
        await cancellation.CancelAsync();
        var act = () => waiting;

        await act.Should().ThrowAsync<OperationCanceledException>();
        await held.DisposeAsync();
        await using var after = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);
        after.Should().NotBeNull();
    }

    [Fact]
    public async Task Dispose_Twice_ReleasesOnlyOnce()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var first = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        first.Dispose();
        first.Dispose();
        await using var second = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);
        var third = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        second.Should().NotBeNull();
        third.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task TryAcquireAsync_InvalidResource_ThrowsArgumentException(string resource)
    {
        var act = () => TestLocks.CreateInProcess().TryAcquireAsync(resource, cancellationToken: Token);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task TryAcquireAsync_NegativeTimeout_ThrowsArgumentOutOfRangeException()
    {
        var act = () => TestLocks.CreateInProcess().TryAcquireAsync(Resource, TimeSpan.FromSeconds(-1), Token);

        await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: build error `The type or namespace name 'InProcess' does not exist in the namespace 'VirtoCommerce.Platform.DistributedLock'`.

- [ ] **Step 3: Implement the base class**

`DistributedLockBase.cs`:

```csharp
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Validation, default timeout, timeout exception and tracing shared by <see cref="IDistributedLock"/> implementations.
    /// </summary>
    public abstract class DistributedLockBase : IDistributedLock
    {
        public const string ActivitySourceName = "VirtoCommerce.Platform.DistributedLock";

        private static readonly ActivitySource _activitySource = new(ActivitySourceName);

        protected DistributedLockBase(IOptions<DistributedLockOptions> options)
        {
            LockOptions = options.Value;
        }

        protected DistributedLockOptions LockOptions { get; }

        public virtual async Task<IDistributedLockHandle> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            var effectiveTimeout = timeout ?? LockOptions.DefaultTimeout;

            return await TryAcquireAsync(resource, effectiveTimeout, cancellationToken)
                ?? throw new DistributedLockTimeoutException(resource, effectiveTimeout);
        }

        public virtual async Task<IDistributedLockHandle> TryAcquireAsync(string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(resource);
            ArgumentOutOfRangeException.ThrowIfLessThan(timeout, TimeSpan.Zero);
            cancellationToken.ThrowIfCancellationRequested();

            using var activity = _activitySource.StartActivity("DistributedLock acquire");
            activity?.SetTag("vc.lock.resource", resource);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var handle = await TryAcquireCoreAsync(resource, timeout, cancellationToken);
                activity?.SetTag("vc.lock.outcome", handle is null ? "timeout" : "acquired");
                return handle;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                activity?.SetTag("vc.lock.outcome", "cancelled");
                throw;
            }
            finally
            {
                activity?.SetTag("vc.lock.wait_ms", stopwatch.Elapsed.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Acquires the lock within <paramref name="timeout"/> (already validated), or returns <c>null</c>.
        /// </summary>
        protected abstract Task<IDistributedLockHandle> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken);
    }
}
```

- [ ] **Step 4: Implement the in-process lock**

`InProcess/InProcessDistributedLock.cs`:

```csharp
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock.InProcess
{
    /// <summary>
    /// <see cref="IDistributedLock"/> for a single platform instance: serializes callers within this process.
    /// Used when Redis is not configured, and in unit tests.
    /// </summary>
    public sealed class InProcessDistributedLock : DistributedLockBase
    {
        private readonly ConcurrentDictionary<string, LockEntry> _entries = new(StringComparer.Ordinal);

        public InProcessDistributedLock(IOptions<DistributedLockOptions> options, ILogger<InProcessDistributedLock> logger)
            : base(options)
        {
            logger.LogInformation("Distributed lock: Redis is not configured. Locks serialize callers within this platform instance only.");
        }

        protected override async Task<IDistributedLockHandle> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var entry = AddReference(resource);
            var acquired = false;

            try
            {
                acquired = await entry.Semaphore.WaitAsync(timeout, cancellationToken);
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
}
```

- [ ] **Step 5: Run the tests to verify they pass**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: PASS (13 tests).

- [ ] **Step 6: Commit**

```bash
git add src/VirtoCommerce.Platform.DistributedLock tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock
git commit -m "feat(VCST-6052): add in-process distributed lock"
```

---

### Task 4: Extension helpers

**Files:**
- Create: `src/VirtoCommerce.Platform.Core/DistributedLock/DistributedLockExtensions.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/DistributedLockExtensionsTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using VirtoCommerce.Platform.Core.DistributedLock;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

public class DistributedLockExtensionsTests
{
    private const string Resource = "test:resource";

    private static CancellationToken Token => TestContext.Current.CancellationToken;

    [Fact]
    public async Task ExecuteAsync_ReturnsResultAndReleasesLock()
    {
        var distributedLock = TestLocks.CreateInProcess();

        var result = await distributedLock.ExecuteAsync(Resource, _ => Task.FromResult(42), cancellationToken: Token);

        result.Should().Be(42);
        await using var after = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);
        after.Should().NotBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_PassesCancellationTokenToAction()
    {
        var distributedLock = TestLocks.CreateInProcess();
        CancellationToken received = default;

        await distributedLock.ExecuteAsync(Resource, cancellationToken =>
        {
            received = cancellationToken;
            return Task.CompletedTask;
        }, cancellationToken: Token);

        received.Should().Be(Token);
    }

    [Fact]
    public async Task ExecuteAsync_WhenActionThrows_ReleasesLock()
    {
        var distributedLock = TestLocks.CreateInProcess();

        var act = () => distributedLock.ExecuteAsync(Resource, _ => Task.FromException(new InvalidOperationException()), cancellationToken: Token);

        await act.Should().ThrowAsync<InvalidOperationException>();
        await using var after = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);
        after.Should().NotBeNull();
    }

    [Fact]
    public async Task TryExecuteAsync_WhenFree_RunsActionAndReturnsTrue()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var ran = false;

        var executed = await distributedLock.TryExecuteAsync(Resource, _ =>
        {
            ran = true;
            return Task.CompletedTask;
        }, cancellationToken: Token);

        executed.Should().BeTrue();
        ran.Should().BeTrue();
    }

    [Fact]
    public async Task TryExecuteAsync_WhenHeld_ReturnsFalseAndSkipsAction()
    {
        var distributedLock = TestLocks.CreateInProcess();
        await using var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);
        var ran = false;

        var executed = await distributedLock.TryExecuteAsync(Resource, _ =>
        {
            ran = true;
            return Task.CompletedTask;
        }, cancellationToken: Token);

        executed.Should().BeFalse();
        ran.Should().BeFalse();
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: build error `'InProcessDistributedLock' does not contain a definition for 'ExecuteAsync'`.

- [ ] **Step 3: Implement the helpers**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DistributedLock
{
    public static class DistributedLockExtensions
    {
        /// <summary>
        /// Runs <paramref name="action"/> under the lock and returns its result.
        /// </summary>
        /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
        public static async Task<T> ExecuteAsync<T>(this IDistributedLock distributedLock, string resource,
            Func<CancellationToken, Task<T>> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(distributedLock);
            ArgumentNullException.ThrowIfNull(action);

            await using var handle = await distributedLock.AcquireAsync(resource, timeout, cancellationToken);
            return await action(cancellationToken);
        }

        /// <summary>
        /// Runs <paramref name="action"/> under the lock.
        /// </summary>
        /// <exception cref="DistributedLockTimeoutException">The lock was not acquired within <paramref name="timeout"/>.</exception>
        public static async Task ExecuteAsync(this IDistributedLock distributedLock, string resource,
            Func<CancellationToken, Task> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(distributedLock);
            ArgumentNullException.ThrowIfNull(action);

            await using var handle = await distributedLock.AcquireAsync(resource, timeout, cancellationToken);
            await action(cancellationToken);
        }

        /// <summary>
        /// Runs <paramref name="action"/> only if the lock is acquired within <paramref name="timeout"/> (default: no wait).
        /// </summary>
        /// <returns><c>true</c> if the action ran; <c>false</c> if the lock was held elsewhere.</returns>
        public static async Task<bool> TryExecuteAsync(this IDistributedLock distributedLock, string resource,
            Func<CancellationToken, Task> action, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(distributedLock);
            ArgumentNullException.ThrowIfNull(action);

            await using var handle = await distributedLock.TryAcquireAsync(resource, timeout, cancellationToken);
            if (handle is null)
            {
                return false;
            }

            await action(cancellationToken);
            return true;
        }
    }
}
```

- [ ] **Step 4: Run the tests to verify they pass**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: PASS (18 tests).

- [ ] **Step 5: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/DistributedLock/DistributedLockExtensions.cs tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/DistributedLockExtensionsTests.cs
git commit -m "feat(VCST-6052): add ExecuteAsync and TryExecuteAsync lock helpers"
```

---

### Task 5: Redis lock

**Files:**
- Create: `src/VirtoCommerce.Platform.DistributedLock/Redis/RedisDistributedLock.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/RedisDistributedLockTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RedLockNet;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.Redis;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

public class RedisDistributedLockTests
{
    private const string Resource = "test:resource";
    private static readonly TimeSpan Expiry = TimeSpan.FromSeconds(30);

    private static CancellationToken Token => TestContext.Current.CancellationToken;

    [Fact]
    public async Task TryAcquireAsync_WithoutTimeout_TriesOnceWithConfiguredExpiry()
    {
        var redLock = CreateRedLock(isAcquired: true);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, Expiry)).ReturnsAsync(redLock.Object);

        await using var handle = await CreateLock(factory.Object).TryAcquireAsync(Resource, cancellationToken: Token);

        handle.Should().NotBeNull();
        handle.Resource.Should().Be(Resource);
        factory.Verify(x => x.CreateLockAsync(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken?>()), Times.Never);
    }

    [Fact]
    public async Task TryAcquireAsync_WithTimeout_WaitsWithRetryIntervalAndCancellation()
    {
        var redLock = CreateRedLock(isAcquired: true);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, Expiry, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(100), Token))
            .ReturnsAsync(redLock.Object);

        await using var handle = await CreateLock(factory.Object).TryAcquireAsync(Resource, TimeSpan.FromSeconds(5), Token);

        handle.Should().NotBeNull();
    }

    [Fact]
    public async Task TryAcquireAsync_WithKeyPrefix_PrefixesRedisKeyButKeepsResourceName()
    {
        var redLock = CreateRedLock(isAcquired: true);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync("prod-eu:" + Resource, Expiry)).ReturnsAsync(redLock.Object);

        await using var handle = await CreateLock(factory.Object, new DistributedLockOptions { KeyPrefix = "prod-eu" })
            .TryAcquireAsync(Resource, cancellationToken: Token);

        handle.Resource.Should().Be(Resource);
    }

    [Fact]
    public async Task TryAcquireAsync_WhenNotAcquired_ReturnsNullAndDisposesRedLock()
    {
        var redLock = CreateRedLock(isAcquired: false);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, Expiry)).ReturnsAsync(redLock.Object);

        var handle = await CreateLock(factory.Object).TryAcquireAsync(Resource, cancellationToken: Token);

        handle.Should().BeNull();
        redLock.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task DisposeAsync_ReleasesRedLock()
    {
        var redLock = CreateRedLock(isAcquired: true);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, Expiry)).ReturnsAsync(redLock.Object);
        var handle = await CreateLock(factory.Object).TryAcquireAsync(Resource, cancellationToken: Token);

        await handle.DisposeAsync();

        redLock.Verify(x => x.DisposeAsync(), Times.Once);
    }

    private static Mock<IRedLock> CreateRedLock(bool isAcquired)
    {
        var redLock = new Mock<IRedLock>();
        redLock.SetupGet(x => x.IsAcquired).Returns(isAcquired);
        redLock.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
        return redLock;
    }

    private static RedisDistributedLock CreateLock(IDistributedLockFactory factory, DistributedLockOptions options = null)
    {
        return new RedisDistributedLock(
            factory,
            Microsoft.Extensions.Options.Options.Create(options ?? new DistributedLockOptions()),
            NullLogger<RedisDistributedLock>.Instance);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: build error `The type or namespace name 'RedisDistributedLock' could not be found`.

- [ ] **Step 3: Implement the Redis lock**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock.Redis
{
    /// <summary>
    /// <see cref="IDistributedLock"/> on Redis (RedLock.net). Held locks are extended automatically until the handle is disposed.
    /// </summary>
    public sealed class RedisDistributedLock : DistributedLockBase
    {
        private readonly IDistributedLockFactory _lockFactory;

        public RedisDistributedLock(IDistributedLockFactory lockFactory, IOptions<DistributedLockOptions> options, ILogger<RedisDistributedLock> logger)
            : base(options)
        {
            _lockFactory = lockFactory;
            logger.LogInformation("Distributed lock: using Redis.");
        }

        protected override async Task<IDistributedLockHandle> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var key = string.IsNullOrEmpty(LockOptions.KeyPrefix) ? resource : $"{LockOptions.KeyPrefix}:{resource}";

            var redLock = timeout == TimeSpan.Zero
                ? await _lockFactory.CreateLockAsync(key, LockOptions.Expiry)
                : await _lockFactory.CreateLockAsync(key, LockOptions.Expiry, timeout, LockOptions.RetryInterval, cancellationToken);

            if (redLock.IsAcquired)
            {
                return new Handle(resource, redLock);
            }

            await redLock.DisposeAsync();
            return null;
        }

        private sealed class Handle : IDistributedLockHandle
        {
            private readonly IRedLock _redLock;

            public Handle(string resource, IRedLock redLock)
            {
                Resource = resource;
                _redLock = redLock;
            }

            public string Resource { get; }

            public void Dispose()
            {
                _redLock.Dispose();
            }

            public ValueTask DisposeAsync()
            {
                return _redLock.DisposeAsync();
            }
        }
    }
}
```

- [ ] **Step 4: Run the tests to verify they pass**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: PASS (23 tests).

- [ ] **Step 5: Commit**

```bash
git add src/VirtoCommerce.Platform.DistributedLock/Redis/RedisDistributedLock.cs tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/RedisDistributedLockTests.cs
git commit -m "feat(VCST-6052): add Redis distributed lock"
```

---

### Task 6: Adapter for the existing `IDistributedLockService`

**Files:**
- Create: `src/VirtoCommerce.Platform.DistributedLock/DistributedLockServiceAdapter.cs`
- Modify: `src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLockService.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/DistributedLockServiceAdapterTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.DistributedLock;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

public class DistributedLockServiceAdapterTests
{
    private const string Resource = "test:resource";

    private static CancellationToken Token => TestContext.Current.CancellationToken;

    [Fact]
    public async Task ExecuteAsync_WhenHeldAndNoTryLockTimeout_ThrowsPlatformExceptionWithoutWaiting()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var service = new DistributedLockServiceAdapter(distributedLock);
        await using var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        var act = () => service.ExecuteAsync(Resource, () => Task.FromResult(1), cancellationToken: Token);

        await act.Should().ThrowAsync<PlatformException>();
    }

    [Fact]
    public async Task ExecuteAsync_WithTryLockTimeout_WaitsForRelease()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var service = new DistributedLockServiceAdapter(distributedLock);
        var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);

        var running = service.ExecuteAsync(Resource, () => Task.FromResult(7), tryLockTimeout: TimeSpan.FromSeconds(5), cancellationToken: Token);
        await held.DisposeAsync();

        (await running).Should().Be(7);
    }

    [Fact]
    public void Execute_RunsResolverUnderLockAndReleases()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var service = new DistributedLockServiceAdapter(distributedLock);

        service.Execute(Resource, () => 3, cancellationToken: Token).Should().Be(3);
        service.Execute(Resource, () => 4, cancellationToken: Token).Should().Be(4);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: build error `The type or namespace name 'DistributedLockServiceAdapter' could not be found`.

- [ ] **Step 3: Implement the adapter**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock
{
    /// <summary>
    /// Runs existing <see cref="IDistributedLockService"/> callers on <see cref="IDistributedLock"/>.
    /// <c>tryLockTimeout</c> is the wait (<c>null</c> tries once). <c>lockTimeout</c> and <c>retryInterval</c> are ignored:
    /// <c>DistributedLock:Expiry</c> and <c>DistributedLock:RetryInterval</c> apply, and held Redis locks are extended automatically.
    /// </summary>
    public class DistributedLockServiceAdapter : IDistributedLockService
    {
        private readonly IDistributedLock _distributedLock;

        public DistributedLockServiceAdapter(IDistributedLock distributedLock)
        {
            _distributedLock = distributedLock;
        }

        public virtual T Execute<T>(string resourceKey, Func<T> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
        {
            // Synchronous legacy entry point; IDistributedLock is async only.
            using var handle = _distributedLock
                .AcquireAsync(resourceKey, tryLockTimeout ?? TimeSpan.Zero, cancellationToken ?? CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            return resolver();
        }

        public virtual async Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver, TimeSpan? lockTimeout = null, TimeSpan? tryLockTimeout = null, TimeSpan? retryInterval = null, CancellationToken? cancellationToken = null)
        {
            await using var handle = await _distributedLock.AcquireAsync(resourceKey, tryLockTimeout ?? TimeSpan.Zero, cancellationToken ?? CancellationToken.None);

            return await resolver();
        }
    }
}
```

- [ ] **Step 4: Add the deprecation remarks to `IDistributedLockService`**

In `src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLockService.cs`, replace the interface summary:

```csharp
    /// <summary>
    /// Povides a unified mechanism for acquiring and managing distributed locks. It encapsulates the execution of critical sections of code in a distributed environment, ensuring that only one process can access the locked resource at a time. 
    /// </summary>
```

with:

```csharp
    /// <summary>
    /// Runs a delegate under a distributed lock.
    /// </summary>
    /// <remarks>
    /// Deprecated: use <see cref="IDistributedLock"/>. This interface is kept for existing callers and runs on
    /// <see cref="IDistributedLock"/>; <c>lockTimeout</c> and <c>retryInterval</c> are ignored.
    /// </remarks>
```

- [ ] **Step 5: Run the tests to verify they pass**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: PASS (26 tests).

- [ ] **Step 6: Commit**

```bash
git add src/VirtoCommerce.Platform.DistributedLock/DistributedLockServiceAdapter.cs src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLockService.cs tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/DistributedLockServiceAdapterTests.cs
git commit -m "feat(VCST-6052): run IDistributedLockService on IDistributedLock"
```

---

### Task 7: Hide the startup lock

**Files:**
- Modify: `src/VirtoCommerce.Platform.DistributedLock/IInternalDistributedLockService.cs`
- Modify: `src/VirtoCommerce.Platform.DistributedLock/DistributedLockCondition.cs`
- Modify: `src/VirtoCommerce.Platform.DistributedLock/NoLock/InternalNoLockService.cs`
- Modify: `src/VirtoCommerce.Platform.DistributedLock/Redis/InternalDistributedLockService.cs`
- Modify: `src/VirtoCommerce.Platform.Web/Extensions/ApplicationBuilderExtensions.cs:97-102`
- Modify: `src/VirtoCommerce.Platform.Web/Redis/ServiceCollectionExtensions.cs:33,38`
- Test: `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/StartupLockTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RedLockNet;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.DistributedLock;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

public class StartupLockTests
{
    [Theory]
    [InlineData("VirtoCommerce.Platform.DistributedLock.IInternalDistributedLockService")]
    [InlineData("VirtoCommerce.Platform.DistributedLock.DistributedLockCondition")]
    [InlineData("VirtoCommerce.Platform.DistributedLock.InternalNoLockService")]
    [InlineData("VirtoCommerce.Platform.DistributedLock.Redis.InternalDistributedLockService")]
    public void StartupLockTypes_AreObsoleteAndHiddenFromIntelliSense(string typeName)
    {
        var type = typeof(DistributedLockOptions).Assembly.GetType(typeName, throwOnError: true);

        type.GetCustomAttribute<ObsoleteAttribute>().Should().NotBeNull();
        type.GetCustomAttribute<EditorBrowsableAttribute>()!.State.Should().Be(EditorBrowsableState.Never);
    }

    [Fact]
    public void ExecuteSynchronized_WhenLockNotAcquired_NamesResourceInError()
    {
        var redLock = new Mock<IRedLock>();
        redLock.SetupGet(x => x.IsAcquired).Returns(false);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLock(It.IsAny<string>(), It.IsAny<TimeSpan>())).Returns(redLock.Object);
        factory.Setup(x => x.CreateLock(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken?>()))
            .Returns(redLock.Object);
#pragma warning disable VC0015 // Testing the Platform startup lock
        var service = new VirtoCommerce.Platform.DistributedLock.Redis.InternalDistributedLockService(
            factory.Object,
            Microsoft.Extensions.Options.Options.Create(new DistributedLockOptions { WaitTime = 0 }),
            NullLogger<VirtoCommerce.Platform.DistributedLock.Redis.InternalDistributedLockService>.Instance);

        var act = () => service.ExecuteSynchronized("startup-resource", _ => { });
#pragma warning restore VC0015

        act.Should().Throw<PlatformException>().WithMessage("*startup-resource*");
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock.StartupLockTests"`
Expected: FAIL — the four theory cases fail on `ObsoleteAttribute` being null, and `ExecuteSynchronized_WhenLockNotAcquired_NamesResourceInError` fails because the message contains the type name instead of `startup-resource`.

- [ ] **Step 3: Mark the startup lock types**

Add to each of the four types, directly above the type declaration, and add `using System.ComponentModel;` (and `using System;` where missing) to each file:

```csharp
    [Obsolete("Platform startup synchronization only. Use IDistributedLock from VirtoCommerce.Platform.Core.DistributedLock.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    [EditorBrowsable(EditorBrowsableState.Never)]
```

Types: `IInternalDistributedLockService` (`IInternalDistributedLockService.cs`), `DistributedLockCondition` (`DistributedLockCondition.cs`), `InternalNoLockService` (`NoLock/InternalNoLockService.cs`), `InternalDistributedLockService` (`Redis/InternalDistributedLockService.cs`).

- [ ] **Step 4: Fix the error message**

In `Redis/InternalDistributedLockService.cs`, replace:

```csharp
                        throw new PlatformException($"Can't acquire distributed lock for resource {this}. It seems that another Platform instance still has the lock, consider increasing wait timeout.");
```

with:

```csharp
                        throw new PlatformException($"Can't acquire distributed lock for resource {resourceId}. It seems that another Platform instance still has the lock, consider increasing wait timeout.");
```

- [ ] **Step 5: Suppress VC0015 at the two Platform call sites**

In `src/VirtoCommerce.Platform.Web/Extensions/ApplicationBuilderExtensions.cs`, replace:

```csharp
            var distributedLockProvider = app.ApplicationServices.GetRequiredService<IInternalDistributedLockService>();
            distributedLockProvider.ExecuteSynchronized(nameof(Startup), _ => payload());
```

with:

```csharp
#pragma warning disable VC0015 // Platform startup lock is internal to Platform
            var distributedLockProvider = app.ApplicationServices.GetRequiredService<IInternalDistributedLockService>();
            distributedLockProvider.ExecuteSynchronized(nameof(Startup), _ => payload());
#pragma warning restore VC0015
```

In `src/VirtoCommerce.Platform.Web/Redis/ServiceCollectionExtensions.cs`, wrap each startup-lock registration:

```csharp
#pragma warning disable VC0015 // Platform startup lock is internal to Platform
                services.AddSingleton<IInternalDistributedLockService, InternalDistributedLockService>();
#pragma warning restore VC0015
```

```csharp
#pragma warning disable VC0015 // Platform startup lock is internal to Platform
                services.AddSingleton<IInternalDistributedLockService, InternalNoLockService>();
#pragma warning restore VC0015
```

`LocalStorageModuleCatalog` needs no change: it is already `[Obsolete]`, so it may use obsolete types.

- [ ] **Step 6: Build and run the tests**

Run: `dotnet build VirtoCommerce.Platform.sln`
Expected: `Build succeeded`, 0 errors. If another file reports `VC0015` for a startup-lock type, wrap that use the same way.

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: PASS (31 tests).

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.DistributedLock src/VirtoCommerce.Platform.Web tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/StartupLockTests.cs
git commit -m "feat(VCST-6052): hide the platform startup lock from module code"
```

---

### Task 8: Registration

**Files:**
- Modify: `src/VirtoCommerce.Platform.Web/Redis/ServiceCollectionExtensions.cs`
- Modify: `src/VirtoCommerce.Platform.DistributedLock/Redis/DistributedLockService.cs`
- Modify: `src/VirtoCommerce.Platform.DistributedLock/NoLock/NoLockService.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/DistributedLockRegistrationTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.InProcess;
using VirtoCommerce.Platform.Web.Redis;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

public class DistributedLockRegistrationTests
{
    [Fact]
    public void AddRedis_WithoutConnectionString_RegistersInProcessLockAndAdapter()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddRedis(new ConfigurationBuilder().Build());

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IDistributedLock>().Should().BeOfType<InProcessDistributedLock>();
        provider.GetRequiredService<IDistributedLockService>().Should().BeOfType<DistributedLockServiceAdapter>();
        provider.GetRequiredService<IDistributedLockService>().Should().BeSameAs(provider.GetRequiredService<IDistributedLockService>());
    }
}
```

- [ ] **Step 2: Run the test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~DistributedLockRegistrationTests"`
Expected: FAIL — `No service for type 'VirtoCommerce.Platform.Core.DistributedLock.IDistributedLock' has been registered.`

- [ ] **Step 3: Replace the registration**

Replace `src/VirtoCommerce.Platform.Web/Redis/ServiceCollectionExtensions.cs` with:

```csharp
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.InProcess;
using VirtoCommerce.Platform.DistributedLock.Redis;

namespace VirtoCommerce.Platform.Web.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");

            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                services.AddSingleton<IConnectionMultiplexer>(redis);
                services.AddSingleton(redis.GetSubscriber());
                services.AddDataProtection()
                        .SetApplicationName("VirtoCommerce.Platform")
                        .PersistKeysToStackExchangeRedis(redis, "VirtoCommerce-Keys");

                var redLockFactory = RedLockFactory.Create(new[] { new RedLockMultiplexer(redis) });
                services.AddSingleton<IDistributedLockFactory>(redLockFactory);

#pragma warning disable VC0015 // Platform startup lock is internal to Platform
                services.AddSingleton<IInternalDistributedLockService, InternalDistributedLockService>();
#pragma warning restore VC0015
                services.AddSingleton<IDistributedLock, RedisDistributedLock>();
            }
            else
            {
#pragma warning disable VC0015 // Platform startup lock is internal to Platform
                services.AddSingleton<IInternalDistributedLockService, InternalNoLockService>();
#pragma warning restore VC0015
                services.AddSingleton<IDistributedLock, InProcessDistributedLock>();
            }

            services.AddSingleton<IDistributedLockService, DistributedLockServiceAdapter>();

            return services;
        }
    }
}
```

- [ ] **Step 4: Mark the replaced implementations obsolete**

In `src/VirtoCommerce.Platform.DistributedLock/Redis/DistributedLockService.cs` and `src/VirtoCommerce.Platform.DistributedLock/NoLock/NoLockService.cs`, add above the class declaration:

```csharp
    [Obsolete("No longer registered. Resolve IDistributedLock, or IDistributedLockService for existing code.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
```

(`using System;` is already present in both files.)

- [ ] **Step 5: Build and run the tests**

Run: `dotnet build VirtoCommerce.Platform.sln`
Expected: `Build succeeded`, 0 errors.

Run: `dotnet test tests/VirtoCommerce.Platform.Tests/VirtoCommerce.Platform.Tests.csproj --filter "FullyQualifiedName~UnitTests.DistributedLock"`
Expected: PASS (32 tests).

- [ ] **Step 6: Commit**

```bash
git add src/VirtoCommerce.Platform.Web/Redis/ServiceCollectionExtensions.cs src/VirtoCommerce.Platform.DistributedLock tests/VirtoCommerce.Platform.Tests/UnitTests/DistributedLock/DistributedLockRegistrationTests.cs
git commit -m "feat(VCST-6052): register IDistributedLock with Redis and in-process implementations"
```

---

### Task 9: Developer guide

**Files:**
- Create: `docs/fundamentals/distributed-lock.md`

- [ ] **Step 1: Write the guide**

````markdown
# Distributed lock

Use `IDistributedLock` (`VirtoCommerce.Platform.Core.DistributedLock`) when code must not run concurrently for the same resource, across all platform instances.

- With `ConnectionStrings:RedisConnectionString`, locks are held in Redis and exclude every instance.
- Without Redis, locks serialize callers inside the current instance only.
- Locks are not reentrant. Resource names follow `{module}:{entity}:{id}`; they appear in Redis and traces, so never put secrets in them.

## Scenarios

### Serialize changes to one entity

```csharp
await _distributedLock.ExecuteAsync($"cart:recalc:{cartId}",
    ct => RecalculateAndSaveAsync(cartId, ct), cancellationToken: cancellationToken);
```

`ExecuteAsync` waits up to `DistributedLock:DefaultTimeout` (10 s) and throws `DistributedLockTimeoutException` if the lock stays busy.

### Consume a single-use token

```csharp
await using var handle = await _distributedLock.TryAcquireAsync($"ucp:handoff:{keyHash}", TimeSpan.FromSeconds(10), cancellationToken);
if (handle is null)
{
    return Conflict(); // retryable: another request is using the token
}

return await ConsumeTokenAsync(keyHash, cancellationToken);
```

### Skip work that another instance is already doing

```csharp
if (!await _distributedLock.TryExecuteAsync("catalog:reindex", ReindexAsync, cancellationToken: cancellationToken))
{
    _logger.LogInformation("Reindex is already running on another instance; skipped.");
}
```

### Hold a lock across several steps and release it early

```csharp
await using (await _distributedLock.AcquireAsync($"order:number:{storeId}", TimeSpan.FromSeconds(30), cancellationToken))
{
    var next = await ReserveNumberAsync(storeId, cancellationToken);
    await SaveCounterAsync(storeId, next, cancellationToken);
}

await NotifyAsync(cancellationToken);
```

### Unit tests

Use the in-process implementation instead of a mock:

```csharp
var distributedLock = new InProcessDistributedLock(
    Options.Create(new DistributedLockOptions()),
    NullLogger<InProcessDistributedLock>.Instance);
```

## Failures

| Situation | Result |
|---|---|
| `AcquireAsync` / `ExecuteAsync` cannot get the lock in time | `DistributedLockTimeoutException` (derives from `PlatformException`, exposes `Resource` and `Timeout`) |
| `TryAcquireAsync` / `TryExecuteAsync` cannot get the lock in time | `null` / `false` |
| The cancellation token is cancelled while waiting | `OperationCanceledException` |

## Configuration

```json
"DistributedLock": {
  "DefaultTimeout": "00:00:10",
  "Expiry": "00:00:30",
  "RetryInterval": "00:00:00.1",
  "KeyPrefix": ""
}
```

| Key | Default | Meaning |
|---|---|---|
| `DefaultTimeout` | `00:00:10` | Wait for `AcquireAsync` and `ExecuteAsync` without an explicit timeout |
| `Expiry` | `00:00:30` | Redis lock TTL. Extended automatically while held; bounds only how long a crashed holder blocks others |
| `RetryInterval` | `00:00:00.1` | Interval between Redis acquisition attempts while waiting |
| `KeyPrefix` | empty | Isolates applications sharing one Redis: keys become `redlock:{KeyPrefix}:{resource}`. Changing it during a rolling deploy breaks mutual exclusion between old and new instances |
| `WaitTime` | `180` | Seconds the platform startup synchronization waits; not used by `IDistributedLock` |

## Tracing

Each acquisition emits a `DistributedLock acquire` span from the `VirtoCommerce.Platform.DistributedLock` activity source, with `vc.lock.resource`, `vc.lock.outcome` (`acquired`, `timeout`, `cancelled`) and `vc.lock.wait_ms`.

## Migrating from `IDistributedLockService`

`VirtoCommerce.Platform.Core.DistributedLock.IDistributedLockService` is deprecated and now runs on `IDistributedLock`:

| Old parameter | Now |
|---|---|
| `tryLockTimeout` | The wait; `null` tries once |
| `lockTimeout` | Ignored; `DistributedLock:Expiry` with automatic extension applies |
| `retryInterval` | Ignored; `DistributedLock:RetryInterval` applies |
| `cancellationToken` | Cancels the wait |

```csharp
// Before
await _distributedLockService.ExecuteAsync($"loyalty-balance:{userId}", () => UpdateBalanceAsync(userId),
    TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30), TimeSpan.FromMilliseconds(200), cancellationToken);

// After
await _distributedLock.ExecuteAsync($"loyalty-balance:{userId}", ct => UpdateBalanceAsync(userId, ct),
    TimeSpan.FromSeconds(30), cancellationToken);
```

Without Redis, the old interface now serializes callers inside the instance instead of running them concurrently.

`IInternalDistributedLockService` and `DistributedLockCondition` are reserved for platform startup and are marked obsolete; do not use them in modules.
````

- [ ] **Step 2: Commit**

```bash
git add docs/fundamentals/distributed-lock.md
git commit -m "docs(VCST-6052): add distributed lock developer guide"
```

---

### Task 10: Full verification

- [ ] **Step 1: Build the solution**

Run: `dotnet build VirtoCommerce.Platform.sln`
Expected: `Build succeeded`, 0 warnings, 0 errors.

- [ ] **Step 2: Run all Platform unit tests**

Run: `dotnet test VirtoCommerce.Platform.sln --filter "Category!=IntegrationTest"`
Expected: all tests pass; the `UnitTests.DistributedLock` namespace contributes 32.

- [ ] **Step 3: Commit the spec and plans**

```bash
git add docs/superpowers/specs/2026-09-24-unified-distributed-lock-design.md docs/superpowers/plans/2026-09-24-distributed-lock-*.md
git commit -m "docs(VCST-6052): add distributed lock design and plans"
```
