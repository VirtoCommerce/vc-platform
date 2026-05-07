using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.DistributedLock.InMemory;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class InMemoryLockServiceTests
    {
        // Time budget the analyzer-friendly delays use throughout the file.
        // Kept short so the suite runs fast; tuned only for ordering, not throughput.
        private static readonly TimeSpan ShortDelay = TimeSpan.FromMilliseconds(50);
        private static readonly TimeSpan TinyDelay = TimeSpan.FromMilliseconds(20);
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(2);

        private static CancellationToken Ct => TestContext.Current.CancellationToken;

        // ---- Basic behavior --------------------------------------------------

        [Fact]
        public void Execute_ReturnsResolverResult()
        {
            using var sut = new InMemoryLockService();
            var result = sut.Execute("k", () => 42);
            result.Should().Be(42);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsResolverResult()
        {
            using var sut = new InMemoryLockService();
            var result = await sut.ExecuteAsync("k", () => Task.FromResult(42));
            result.Should().Be(42);
        }

        // ---- Argument validation --------------------------------------------

        [Fact]
        public void Execute_NullResourceKey_Throws()
        {
            using var sut = new InMemoryLockService();
            Action act = () => sut.Execute<int>(null!, () => 0);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Execute_NullResolver_Throws()
        {
            using var sut = new InMemoryLockService();
            Action act = () => sut.Execute<int>("k", null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task ExecuteAsync_NullResourceKey_Throws()
        {
            using var sut = new InMemoryLockService();
            Func<Task> act = () => sut.ExecuteAsync<int>(null!, () => Task.FromResult(0));
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ExecuteAsync_NullResolver_Throws()
        {
            using var sut = new InMemoryLockService();
            Func<Task> act = () => sut.ExecuteAsync<int>("k", null!);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        // ---- Mutual exclusion (same key) ------------------------------------

        [Fact]
        public async Task ExecuteAsync_SameKey_SerializesConcurrentCallers()
        {
            // Two concurrent callers on the same key must observe a critical-section
            // count of at most 1 at every observation point.
            using var sut = new InMemoryLockService();
            const string key = "cart:user-1";
            var inFlight = 0;
            var maxInFlight = 0;
            var maxLock = new object();

            async Task<int> Critical()
            {
                var current = Interlocked.Increment(ref inFlight);
                lock (maxLock)
                {
                    if (current > maxInFlight) maxInFlight = current;
                }
                await Task.Delay(ShortDelay, Ct);
                Interlocked.Decrement(ref inFlight);
                return current;
            }

            var t1 = sut.ExecuteAsync(key, Critical);
            var t2 = sut.ExecuteAsync(key, Critical);
            await Task.WhenAll(t1, t2);

            maxInFlight.Should().Be(1, "the in-memory lock should serialize callers on the same key");
            // Both calls completed and observed themselves alone in the critical section.
            (await t1).Should().Be(1);
            (await t2).Should().Be(1);
        }

        [Fact]
        public async Task ExecuteAsync_SameKey_PreservesFifoOrderingOfCompletion()
        {
            // The first caller to enter the lock should be the first to complete its
            // critical section. SemaphoreSlim doesn't strictly guarantee FIFO ordering
            // of waiters, but the second caller cannot finish before the first releases.
            using var sut = new InMemoryLockService();
            const string key = "k";
            var firstCompletedAt = DateTime.MaxValue;
            var secondCompletedAt = DateTime.MinValue;

            var first = sut.ExecuteAsync(key, async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(80), Ct);
                firstCompletedAt = DateTime.UtcNow;
                return 0;
            });

            // Give the first call a head-start so it definitely owns the lock
            // before the second one arrives.
            await Task.Delay(TinyDelay, Ct);

            var second = sut.ExecuteAsync(key, async () =>
            {
                secondCompletedAt = DateTime.UtcNow;
                await Task.Delay(TimeSpan.FromMilliseconds(10), Ct);
                return 0;
            });

            await Task.WhenAll(first, second);
            secondCompletedAt.Should().BeOnOrAfter(firstCompletedAt,
                "the second caller cannot enter the critical section until the first releases the lock");
        }

        // ---- Independence (different keys) ----------------------------------

        [Fact]
        public async Task ExecuteAsync_DifferentKeys_DoNotSerialize()
        {
            // Calls on different keys must run truly concurrently.
            using var sut = new InMemoryLockService();
            var concurrent = 0;
            var sawTwoConcurrent = false;
            var bothEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            async Task<int> Critical()
            {
                var n = Interlocked.Increment(ref concurrent);
                if (n >= 2)
                {
                    sawTwoConcurrent = true;
                    bothEntered.TrySetResult();
                }
                // Wait until both reached the critical section before releasing.
                await bothEntered.Task.WaitAsync(TestTimeout, Ct);
                Interlocked.Decrement(ref concurrent);
                return n;
            }

            var a = sut.ExecuteAsync("cart:user-A", Critical);
            var b = sut.ExecuteAsync("cart:user-B", Critical);
            await Task.WhenAll(a, b);

            sawTwoConcurrent.Should().BeTrue("different keys must not serialize");
        }

        // ---- Timeout / contention --------------------------------------------

        [Fact]
        public async Task ExecuteAsync_TryLockTimeoutExceeded_ThrowsPlatformException()
        {
            using var sut = new InMemoryLockService();
            const string key = "k";

            // Hold the lock indefinitely until released by the test.
            var release = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            var holder = sut.ExecuteAsync(key, () => release.Task);

            // Give the holder time to acquire.
            await Task.Delay(TinyDelay, Ct);

            Func<Task> contender = () => sut.ExecuteAsync(key,
                () => Task.FromResult(0),
                tryLockTimeout: TimeSpan.FromMilliseconds(50));

            await contender.Should().ThrowAsync<PlatformException>()
                .WithMessage("*currently unavailable*");

            release.SetResult(0);
            await holder;
        }

        // ---- Cancellation ----------------------------------------------------

        [Fact]
        public async Task ExecuteAsync_CancelledWhileWaiting_ThrowsOperationCanceled()
        {
            using var sut = new InMemoryLockService();
            const string key = "k";

            var release = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            var holder = sut.ExecuteAsync(key, () => release.Task);

            await Task.Delay(TinyDelay, Ct);

            using var cts = new CancellationTokenSource();
            var contender = sut.ExecuteAsync(key,
                () => Task.FromResult(0),
                cancellationToken: cts.Token);

            cts.Cancel();
            await FluentActions.Invoking(() => contender)
                .Should().ThrowAsync<OperationCanceledException>();

            // After cancellation, the holder must still be able to release and a
            // brand-new call on the same key must succeed (i.e. cancellation didn't
            // leak the lock).
            release.SetResult(0);
            await holder;

            var fresh = await sut.ExecuteAsync(key, () => Task.FromResult(7));
            fresh.Should().Be(7);
        }

        // ---- Resolver exception releases the lock ---------------------------

        [Fact]
        public async Task ExecuteAsync_ResolverThrows_LockIsStillReleased()
        {
            using var sut = new InMemoryLockService();
            const string key = "k";

            await FluentActions.Invoking(() =>
                    sut.ExecuteAsync<int>(key, () => throw new InvalidOperationException("boom")))
                .Should().ThrowAsync<InvalidOperationException>();

            // If the lock leaked, this call would block forever; bound it via the
            // test cancellation token so we fail fast instead of hanging.
            var ok = await sut.ExecuteAsync(key, () => Task.FromResult(1), cancellationToken: Ct);
            ok.Should().Be(1);
        }

        [Fact]
        public void Execute_SyncResolverThrows_LockIsStillReleased()
        {
            using var sut = new InMemoryLockService();
            const string key = "k";

            Action act = () => sut.Execute<int>(key, () => throw new InvalidOperationException("boom"));
            act.Should().Throw<InvalidOperationException>();

            // Should not deadlock; if the lock leaked, the call below would block forever.
            sut.Execute(key, () => 1).Should().Be(1);
        }

        // ---- Stress: many concurrent callers on same key --------------------

        [Fact]
        public async Task ExecuteAsync_ManyConcurrentCallersSameKey_AlwaysAtMostOneInside()
        {
            using var sut = new InMemoryLockService();
            const string key = "k";
            const int callers = 50;
            var inFlight = 0;
            var maxInFlight = 0;
            var maxLock = new object();
            var counter = 0;

            async Task<int> Critical()
            {
                var current = Interlocked.Increment(ref inFlight);
                lock (maxLock)
                {
                    if (current > maxInFlight) maxInFlight = current;
                }
                await Task.Yield();
                Interlocked.Decrement(ref inFlight);
                return Interlocked.Increment(ref counter);
            }

            var tasks = new Task[callers];
            for (var i = 0; i < callers; i++)
            {
                tasks[i] = sut.ExecuteAsync(key, Critical);
            }
            await Task.WhenAll(tasks);

            maxInFlight.Should().Be(1);
            counter.Should().Be(callers, "every caller must have run exactly once");
        }

        // ---- Disposal -------------------------------------------------------

        [Fact]
        public async Task Dispose_PreventsFurtherCalls()
        {
            var sut = new InMemoryLockService();
            await sut.ExecuteAsync("k", () => Task.FromResult(0));

            sut.Dispose();

            Action sync = () => sut.Execute("k", () => 0);
            sync.Should().Throw<ObjectDisposedException>();

            Func<Task> asyncCall = () => sut.ExecuteAsync("k", () => Task.FromResult(0));
            await asyncCall.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            var sut = new InMemoryLockService();
            sut.Dispose();
            // Second dispose must not throw.
            Action act = () => sut.Dispose();
            act.Should().NotThrow();
        }

        // ---- Independence between two service instances ---------------------

        [Fact]
        public async Task ExecuteAsync_DifferentInstances_DoNotShareLocks()
        {
            // Each InMemoryLockService instance owns its own dictionary, so two
            // separate instances must not block each other even on the same key.
            using var a = new InMemoryLockService();
            using var b = new InMemoryLockService();
            const string key = "k";
            var concurrent = 0;
            var sawTwo = false;
            var bothEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            async Task<int> Critical()
            {
                var n = Interlocked.Increment(ref concurrent);
                if (n >= 2)
                {
                    sawTwo = true;
                    bothEntered.TrySetResult();
                }
                await bothEntered.Task.WaitAsync(TestTimeout, Ct);
                Interlocked.Decrement(ref concurrent);
                return n;
            }

            var t1 = a.ExecuteAsync(key, Critical);
            var t2 = b.ExecuteAsync(key, Critical);
            await Task.WhenAll(t1, t2);

            sawTwo.Should().BeTrue("each instance has independent locks");
        }

        // ---- Mutual exclusion under thread contention -----------------------

        [Fact]
        public async Task Execute_ManyThreadsSameKey_AreSerialized()
        {
            // Hammer the lock from many threads on the same key. ConcurrentDictionary
            // guarantees a single stored SemaphoreSlim per key, so we should never
            // observe two threads inside the critical section at once.
            using var sut = new InMemoryLockService();
            const string key = "k";
            const int threadCount = 16;
            var inFlight = 0;
            var maxInFlight = 0;
            var maxLock = new object();
            var observed = new ConcurrentBag<int>();
            var ready = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            var tasks = new Task[threadCount];
            for (var i = 0; i < threadCount; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    await ready.Task.WaitAsync(TestTimeout, Ct);
                    sut.Execute(key, () =>
                    {
                        var current = Interlocked.Increment(ref inFlight);
                        lock (maxLock)
                        {
                            if (current > maxInFlight) maxInFlight = current;
                        }
                        observed.Add(Environment.CurrentManagedThreadId);
                        Thread.SpinWait(1000);
                        Interlocked.Decrement(ref inFlight);
                        return 0;
                    });
                }, Ct);
            }
            ready.SetResult();
            await Task.WhenAll(tasks);

            observed.Count.Should().Be(threadCount, "every thread must have run exactly once");
            maxInFlight.Should().Be(1, "the lock must serialize threads competing on the same key");
        }
    }
}
