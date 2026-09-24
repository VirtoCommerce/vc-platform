using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using VirtoCommerce.Platform.Core.DistributedLock;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DistributedLock;

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

    [Fact]
    public void Acquire_WhenFree_ReturnsHandleThatReleasesOnDispose()
    {
        var distributedLock = TestLocks.CreateInProcess();

        using (var handle = distributedLock.Acquire(Resource, cancellationToken: Token))
        {
            handle.Resource.Should().Be(Resource);
            distributedLock.TryAcquire(Resource, cancellationToken: Token).Should().BeNull();
        }

        using var after = distributedLock.TryAcquire(Resource, cancellationToken: Token);
        after.Should().NotBeNull();
    }

    [Fact]
    public void Acquire_WhenHeldPastTimeout_ThrowsDistributedLockTimeoutException()
    {
        var distributedLock = TestLocks.CreateInProcess();
        using var held = distributedLock.Acquire(Resource, cancellationToken: Token);

        var act = () => distributedLock.Acquire(Resource, TimeSpan.FromMilliseconds(50), Token);

        act.Should().Throw<DistributedLockTimeoutException>().Which.Resource.Should().Be(Resource);
    }

    [Fact]
    public void Execute_RunsActionUnderLockAndReleases()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var ran = false;

        distributedLock.Execute(Resource, () => { ran = true; }, cancellationToken: Token);

        ran.Should().BeTrue();
        using var after = distributedLock.TryAcquire(Resource, cancellationToken: Token);
        after.Should().NotBeNull();
    }

    [Fact]
    public void Execute_WithResult_ReturnsResult()
    {
        var distributedLock = TestLocks.CreateInProcess();

        var result = distributedLock.Execute(Resource, () => 42, cancellationToken: Token);

        result.Should().Be(42);
    }

    [Fact]
    public void Execute_WhenActionThrows_ReleasesLock()
    {
        var distributedLock = TestLocks.CreateInProcess();

        var act = () => distributedLock.Execute(Resource, () => throw new InvalidOperationException(), cancellationToken: Token);

        act.Should().Throw<InvalidOperationException>();
        using var after = distributedLock.TryAcquire(Resource, cancellationToken: Token);
        after.Should().NotBeNull();
    }

    [Fact]
    public void TryExecute_WhenHeld_ReturnsFalseAndSkipsAction()
    {
        var distributedLock = TestLocks.CreateInProcess();
        using var held = distributedLock.Acquire(Resource, cancellationToken: Token);
        var ran = false;

        var executed = distributedLock.TryExecute(Resource, () => { ran = true; }, cancellationToken: Token);

        executed.Should().BeFalse();
        ran.Should().BeFalse();
    }

    [Fact]
    public async Task Acquire_UnderSynchronizationContextWhileContended_DoesNotDeadlock()
    {
        var distributedLock = TestLocks.CreateInProcess();
        var held = await distributedLock.TryAcquireAsync(Resource, cancellationToken: Token);
        IDistributedLockHandle acquired = null;

        // A single-threaded context that never runs posted work, as when its only thread is blocked (desktop UI, some test runners).
        var thread = new Thread(() =>
        {
            SynchronizationContext.SetSynchronizationContext(new NonPumpingSynchronizationContext());
            acquired = distributedLock.Acquire(Resource, TimeSpan.FromSeconds(10), CancellationToken.None);
        })
        {
            IsBackground = true,
        };
        thread.Start();
        await Task.Delay(100, Token);
        await held.DisposeAsync();

        thread.Join(TimeSpan.FromSeconds(5)).Should().BeTrue("Acquire must not wait for the blocked synchronization context");
        acquired.Should().NotBeNull();
        acquired.Dispose();
    }

    private sealed class NonPumpingSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            // Dropped: the owning thread is blocked and never processes its queue.
        }
    }
}
