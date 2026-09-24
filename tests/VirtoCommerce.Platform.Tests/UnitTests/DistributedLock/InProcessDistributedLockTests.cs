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
