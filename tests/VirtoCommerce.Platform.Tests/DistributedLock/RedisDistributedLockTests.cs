using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        redLocks[0].Verify(x => x.DisposeAsync(), Times.Never, "RedLock already released a lock it did not acquire");
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
        redLocks[0].Verify(x => x.DisposeAsync(), Times.Never, "a second unlock would wait out another command timeout");
    }

    [Fact]
    public async Task TryAcquireAsync_WhenAcquisitionOutlivedTheLease_ThrowsUnavailable()
    {
        // Expired: the key was set, but acquisition took longer than Expiry. That is not contention.
        var factory = CreateFactory(RedLockStatus.Expired);

        var act = () => CreateLock(factory.Object).TryAcquireAsync(Resource, cancellationToken: Token);

        await act.Should().ThrowAsync<DistributedLockUnavailableException>();
    }

    [Theory]
    [InlineData(100, 0.0, 80)]
    [InlineData(100, 0.5, 100)]
    [InlineData(100, 0.99999, 120)]
    [InlineData(1900, 0.99999, 2000)]
    [InlineData(2000, 0.5, 2000)]
    public void GetRetryWait_AppliesJitterThenCap(int delayMs, double random, int expectedMs)
    {
        var wait = RedisDistributedLock.GetRetryWait(TimeSpan.FromMilliseconds(delayMs), TimeSpan.FromSeconds(2), random);

        wait.TotalMilliseconds.Should().BeApproximately(expectedMs, 0.1);
    }

    [Fact]
    public void GetNextRetryDelay_GrowsByFactorUpToCap()
    {
        var max = TimeSpan.FromSeconds(2);
        var delays = new List<double>();
        var delay = TimeSpan.FromMilliseconds(100);
        for (var i = 0; i < 8; i++)
        {
            delays.Add(delay.TotalMilliseconds);
            delay = RedisDistributedLock.GetNextRetryDelay(delay, max);
        }

        delays.Select(x => Math.Round(x, 1)).Should().Equal(100, 180, 324, 583.2, 1049.8, 1889.6, 2000, 2000);
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
        // Renewals keep advancing ExtendCount, so only the failed Status can cancel the token.
        var failing = new FailingRenewal();
        var options = new DistributedLockOptions { Expiry = TimeSpan.FromSeconds(2) };

        await using var handle = await CreateLock(failing.Factory, options).TryAcquireAsync(Resource, cancellationToken: Token);
        (await WaitForCancellation(handle.HandleLostToken, TimeSpan.FromSeconds(1))).Should().BeFalse();

        failing.Fail();
        var lost = await WaitForCancellation(handle.HandleLostToken, TimeSpan.FromSeconds(5));

        lost.Should().BeTrue();
    }

    [Fact]
    public async Task HandleLostToken_WhenACallbackThrows_IsStillCancelledAndTheErrorIsLogged()
    {
        // Cancellation runs on a timer thread, where an unhandled exception would terminate the process.
        var failing = new FailingRenewal();
        var logger = new RecordingLogger();
        var options = new DistributedLockOptions { Expiry = TimeSpan.FromMilliseconds(400) };

        await using var handle = await CreateLock(failing.Factory, options, logger).TryAcquireAsync(Resource, cancellationToken: Token);
        handle.HandleLostToken.Register(() => throw new InvalidOperationException("callback failed"));

        failing.Fail();
        var lost = await WaitForCancellation(handle.HandleLostToken, TimeSpan.FromSeconds(5));
        await Task.Delay(TimeSpan.FromMilliseconds(100), Token);

        lost.Should().BeTrue();
        logger.Errors.Should().ContainSingle().Which.Should().BeOfType<AggregateException>();
    }

    [Fact]
    public async Task HandleLostToken_IsCancelledWhenRenewalsStopWithoutStatusChange()
    {
        // RedLock can fail to renew without changing Status (for example on a closed multiplexer); ExtendCount then stops.
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

    // A held RedLock whose renewals succeed until Fail() is called.
    private sealed class FailingRenewal
    {
        private int _extendCount;
        private volatile bool _failed;

        public FailingRenewal()
        {
            var redLock = new Mock<IRedLock>();
            redLock.SetupGet(x => x.IsAcquired).Returns(() => !_failed);
            redLock.SetupGet(x => x.Status).Returns(() => _failed ? RedLockStatus.NoQuorum : RedLockStatus.Acquired);
            redLock.SetupGet(x => x.ExtendCount).Returns(() => _failed ? Volatile.Read(ref _extendCount) : Interlocked.Increment(ref _extendCount));
            redLock.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
            var factory = new Mock<IDistributedLockFactory>();
            factory.Setup(x => x.CreateLockAsync(Resource, It.IsAny<TimeSpan>())).ReturnsAsync(redLock.Object);
            Factory = factory.Object;
        }

        public IDistributedLockFactory Factory { get; }

        public void Fail()
        {
            _failed = true;
        }
    }

    private sealed class RecordingLogger : ILogger<RedisDistributedLock>
    {
        public ConcurrentQueue<string> Warnings { get; } = new();

        public ConcurrentQueue<Exception> Errors { get; } = new();

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
            {
                Warnings.Enqueue(formatter(state, exception));
            }
            else if (logLevel >= LogLevel.Error && exception is not null)
            {
                Errors.Enqueue(exception);
            }
        }
    }
}
