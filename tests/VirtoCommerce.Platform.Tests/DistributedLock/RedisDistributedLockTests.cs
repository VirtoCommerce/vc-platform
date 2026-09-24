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

namespace VirtoCommerce.Platform.Tests.DistributedLock;

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
    public async Task TryAcquireAsync_WithInfiniteTimeout_WaitsWithoutLimit()
    {
        var redLock = CreateRedLock(isAcquired: true);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, Expiry, TimeSpan.MaxValue, TimeSpan.FromMilliseconds(100), Token))
            .ReturnsAsync(redLock.Object);

        await using var handle = await CreateLock(factory.Object).TryAcquireAsync(Resource, Timeout.InfiniteTimeSpan, Token);

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
