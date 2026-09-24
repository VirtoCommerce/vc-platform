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
