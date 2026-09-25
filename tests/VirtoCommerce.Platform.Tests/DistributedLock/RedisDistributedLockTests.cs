using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RedLockNet;
using VirtoCommerce.Platform.Core.DistributedLock;
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
    public async Task TryAcquireAsync_WithoutTimeout_MakesOneAttemptWithConfiguredExpiry()
    {
        var factory = CreateFactory(RedLockStatus.Acquired);

        await using var handle = await CreateLock(factory.Object).TryAcquireAsync(Resource, cancellationToken: Token);

        handle.Should().NotBeNull();
        handle.Resource.Should().Be(Resource);
        factory.Verify(x => x.CreateLockAsync(Resource, Expiry), Times.Once);
        factory.Verify(x => x.CreateLockAsync(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken?>()), Times.Never);
    }

    [Fact]
    public async Task TryAcquireAsync_WithoutTimeout_WhenHeldElsewhere_ReturnsNullAfterOneAttempt()
    {
        var redLocks = new[] { CreateRedLock(RedLockStatus.Conflicted) };
        var factory = CreateFactory(redLocks);

        var handle = await CreateLock(factory.Object).TryAcquireAsync(Resource, cancellationToken: Token);

        handle.Should().BeNull();
        factory.Verify(x => x.CreateLockAsync(Resource, Expiry), Times.Once);
        redLocks[0].Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task TryAcquireAsync_WithTimeout_RetriesUntilAcquired()
    {
        var factory = CreateFactory(RedLockStatus.Conflicted, RedLockStatus.Conflicted, RedLockStatus.Acquired);

        await using var handle = await CreateLock(factory.Object, FastRetries()).TryAcquireAsync(Resource, TimeSpan.FromSeconds(5), Token);

        handle.Should().NotBeNull();
        factory.Verify(x => x.CreateLockAsync(Resource, Expiry), Times.Exactly(3));
    }

    [Fact]
    public async Task TryAcquireAsync_WithTimeout_WhenStillHeld_ReturnsNullAfterTimeout()
    {
        var factory = CreateFactory(RedLockStatus.Conflicted);

        var handle = await CreateLock(factory.Object, FastRetries()).TryAcquireAsync(Resource, TimeSpan.FromMilliseconds(200), Token);

        handle.Should().BeNull();
        factory.Verify(x => x.CreateLockAsync(Resource, Expiry), Times.AtLeast(2));
    }

    [Fact]
    public async Task TryAcquireAsync_WhenRedisUnavailable_ThrowsUnavailableWithoutWaiting()
    {
        var redLocks = new[] { CreateRedLock(RedLockStatus.NoQuorum) };
        var factory = CreateFactory(redLocks);

        var act = () => CreateLock(factory.Object).TryAcquireAsync(Resource, TimeSpan.FromSeconds(30), Token);

        (await act.Should().ThrowAsync<DistributedLockUnavailableException>()).Which.Resource.Should().Be(Resource);
        factory.Verify(x => x.CreateLockAsync(Resource, Expiry), Times.Once);
        redLocks[0].Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task AcquireAsync_WhenRedisUnavailable_ThrowsUnavailableNotTimeout()
    {
        var factory = CreateFactory(RedLockStatus.NoQuorum);

        var act = () => CreateLock(factory.Object).AcquireAsync(Resource, cancellationToken: Token);

        await act.Should().ThrowAsync<DistributedLockUnavailableException>();
    }

    [Fact]
    public async Task TryAcquireAsync_WhenCancelledWhileWaiting_ThrowsOperationCanceledException()
    {
        var factory = CreateFactory(RedLockStatus.Conflicted);
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(Token);
        cancellation.CancelAfter(TimeSpan.FromMilliseconds(100));

        var act = () => CreateLock(factory.Object, FastRetries()).TryAcquireAsync(Resource, Timeout.InfiniteTimeSpan, cancellation.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task TryAcquireAsync_WithKeyPrefix_PrefixesRedisKeyButKeepsResourceName()
    {
        var redLock = CreateRedLock(RedLockStatus.Acquired);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync("prod-eu:" + Resource, Expiry)).ReturnsAsync(redLock.Object);

        await using var handle = await CreateLock(factory.Object, new DistributedLockOptions { KeyPrefix = "prod-eu" })
            .TryAcquireAsync(Resource, cancellationToken: Token);

        handle.Resource.Should().Be(Resource);
    }

    [Fact]
    public async Task DisposeAsync_ReleasesRedLock()
    {
        var redLocks = new[] { CreateRedLock(RedLockStatus.Acquired) };
        var handle = await CreateLock(CreateFactory(redLocks).Object).TryAcquireAsync(Resource, cancellationToken: Token);

        await handle.DisposeAsync();

        redLocks[0].Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task HandleLostToken_IsCancelledWhenRenewalFails()
    {
        var acquired = true;
        var redLock = new Mock<IRedLock>();
        redLock.SetupGet(x => x.IsAcquired).Returns(() => acquired);
        redLock.SetupGet(x => x.Status).Returns(() => acquired ? RedLockStatus.Acquired : RedLockStatus.NoQuorum);
        redLock.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, It.IsAny<TimeSpan>())).ReturnsAsync(redLock.Object);
        var options = new DistributedLockOptions { Expiry = TimeSpan.FromMilliseconds(400) };

        await using var handle = await CreateLock(factory.Object, options).TryAcquireAsync(Resource, cancellationToken: Token);
        handle.HandleLostToken.IsCancellationRequested.Should().BeFalse();

        acquired = false;
        var lost = await WaitForCancellation(handle.HandleLostToken, TimeSpan.FromSeconds(5));

        lost.Should().BeTrue();
    }

    [Fact]
    public async Task HandleLostToken_IsCancelledWhenRenewalsStopWithoutStatusChange()
    {
        // RedLock can fail to renew without changing Status (for example on a closed connection); ExtendCount then stops.
        var redLock = CreateRedLock(RedLockStatus.Acquired);
        redLock.SetupGet(x => x.ExtendCount).Returns(0);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, It.IsAny<TimeSpan>())).ReturnsAsync(redLock.Object);
        var options = new DistributedLockOptions { Expiry = TimeSpan.FromMilliseconds(400) };

        await using var handle = await CreateLock(factory.Object, options).TryAcquireAsync(Resource, cancellationToken: Token);
        var lost = await WaitForCancellation(handle.HandleLostToken, TimeSpan.FromSeconds(5));

        lost.Should().BeTrue();
    }

    [Fact]
    public async Task HandleLostToken_StaysActiveWhileRenewalsSucceed()
    {
        var extendCount = 0;
        var redLock = CreateRedLock(RedLockStatus.Acquired);
        redLock.SetupGet(x => x.ExtendCount).Returns(() => Interlocked.Increment(ref extendCount));
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, It.IsAny<TimeSpan>())).ReturnsAsync(redLock.Object);
        var options = new DistributedLockOptions { Expiry = TimeSpan.FromMilliseconds(400) };

        await using var handle = await CreateLock(factory.Object, options).TryAcquireAsync(Resource, cancellationToken: Token);
        var lost = await WaitForCancellation(handle.HandleLostToken, TimeSpan.FromSeconds(1));

        lost.Should().BeFalse();
    }

    [Fact]
    public async Task DisposeAsync_WhileLossCheckIsRunning_DoesNotReportLoss()
    {
        // RedLock reports Unlocked after release; a check that started before release must not see that as a loss.
        var acquired = true;
        var blockCheck = false;
        using var checkEntered = new ManualResetEventSlim();
        using var releaseCheck = new ManualResetEventSlim();
        var redLock = new Mock<IRedLock>();
        redLock.SetupGet(x => x.IsAcquired).Returns(() => Volatile.Read(ref acquired));
        redLock.SetupGet(x => x.Status).Returns(() => Volatile.Read(ref acquired) ? RedLockStatus.Acquired : RedLockStatus.Unlocked);
        redLock.SetupGet(x => x.ExtendCount).Returns(() =>
        {
            if (Volatile.Read(ref blockCheck))
            {
                checkEntered.Set();
                releaseCheck.Wait(TimeSpan.FromSeconds(10));
            }

            return 0;
        });
        redLock.Setup(x => x.DisposeAsync()).Returns(() =>
        {
            Volatile.Write(ref acquired, false);
            return ValueTask.CompletedTask;
        });
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(Resource, It.IsAny<TimeSpan>())).ReturnsAsync(redLock.Object);
        var logger = new RecordingLogger();
        var options = new DistributedLockOptions { Expiry = TimeSpan.FromSeconds(30) };

        var handle = await CreateLock(factory.Object, options, logger).TryAcquireAsync(Resource, cancellationToken: Token);
        Volatile.Write(ref blockCheck, true);
        checkEntered.Wait(TimeSpan.FromSeconds(10), Token).Should().BeTrue();

        var release = Task.Run(async () => await handle.DisposeAsync(), Token);
        await Task.Delay(TimeSpan.FromMilliseconds(100), Token);
        Volatile.Write(ref blockCheck, false);
        releaseCheck.Set();
        await release;
        await Task.Delay(TimeSpan.FromMilliseconds(300), Token);

        logger.Warnings.Should().BeEmpty();
        redLock.Verify(x => x.DisposeAsync(), Times.Once);
    }

    private static async Task<bool> WaitForCancellation(CancellationToken token, TimeSpan limit)
    {
        try
        {
            await Task.Delay(limit, token);
            return false;
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            return true;
        }
    }

    private static DistributedLockOptions FastRetries()
    {
        return new DistributedLockOptions { RetryInterval = TimeSpan.FromMilliseconds(10), MaxRetryInterval = TimeSpan.FromMilliseconds(20) };
    }

    private static Mock<IRedLock> CreateRedLock(RedLockStatus status)
    {
        var redLock = new Mock<IRedLock>();
        redLock.SetupGet(x => x.IsAcquired).Returns(status == RedLockStatus.Acquired);
        redLock.SetupGet(x => x.Status).Returns(status);
        redLock.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
        return redLock;
    }

    private static Mock<IDistributedLockFactory> CreateFactory(params RedLockStatus[] statuses)
    {
        return CreateFactory(Array.ConvertAll(statuses, CreateRedLock));
    }

    // Returns the given locks in order, then keeps returning the last one.
    private static Mock<IDistributedLockFactory> CreateFactory(Mock<IRedLock>[] redLocks)
    {
        var next = 0;
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLockAsync(It.IsAny<string>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(() => redLocks[Math.Min(Interlocked.Increment(ref next) - 1, redLocks.Length - 1)].Object);
        return factory;
    }

    private static RedisDistributedLock CreateLock(IDistributedLockFactory factory, DistributedLockOptions options = null, ILogger<RedisDistributedLock> logger = null)
    {
        return new RedisDistributedLock(
            factory,
            Microsoft.Extensions.Options.Options.Create(options ?? new DistributedLockOptions()),
            logger ?? NullLogger<RedisDistributedLock>.Instance);
    }

    private sealed class RecordingLogger : ILogger<RedisDistributedLock>
    {
        public ConcurrentQueue<string> Warnings { get; } = new();

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel >= LogLevel.Warning)
            {
                Warnings.Enqueue(formatter(state, exception));
            }
        }
    }
}
