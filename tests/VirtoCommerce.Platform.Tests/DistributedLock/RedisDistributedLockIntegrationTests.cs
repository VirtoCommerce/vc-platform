using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.Redis;
using VirtoCommerce.Platform.Web.Redis;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DistributedLock;

/// <summary>
/// Runs the distributed lock against a real Redis. Each <see cref="PlatformInstance"/> is a separate DI container built by
/// <c>AddRedis</c>, with its own Redis connection and lock factory, as separate platform processes would have; they share only Redis.
/// Redis: <c>VC_TEST_REDIS_CONNECTION_STRING</c>, default <c>127.0.0.1:6379,ssl=False</c>. Tests are skipped when Redis is not reachable.
/// </summary>
[Trait("Category", "IntegrationTest")]
public class RedisDistributedLockIntegrationTests : IClassFixture<RedisDistributedLockIntegrationTests.RedisFixture>
{
    private static readonly TimeSpan WaitLimit = TimeSpan.FromSeconds(30);

    private readonly RedisFixture _redis;

    public RedisDistributedLockIntegrationTests(RedisFixture redis)
    {
        _redis = redis;
        Assert.SkipUnless(redis.IsAvailable, $"Redis is not reachable at '{redis.ConnectionString}'. Set VC_TEST_REDIS_CONNECTION_STRING to run these tests.");
    }

    private static CancellationToken Token => TestContext.Current.CancellationToken;

    [Fact]
    public void AddRedis_WithConnectionString_RegistersRedisLock()
    {
        using var instance = CreateInstance();

        instance.Lock.Should().BeOfType<RedisDistributedLock>();
        instance.Services.GetRequiredService<IDistributedLockService>().Should().BeOfType<DistributedLockServiceAdapter>();
    }

    [Fact]
    public async Task Lock_HeldByOneInstance_ExcludesAnotherInstanceUntilReleased()
    {
        using var first = CreateInstance();
        using var second = CreateInstance();
        var resource = NewResource();

        var held = await first.Lock.TryAcquireAsync(resource, cancellationToken: Token);
        var whileHeld = await second.Lock.TryAcquireAsync(resource, cancellationToken: Token);
        var waiting = second.Lock.TryAcquireAsync(resource, WaitLimit, Token);
        await held!.DisposeAsync();
        await using var afterRelease = await waiting;

        held.Should().NotBeNull();
        whileHeld.Should().BeNull();
        afterRelease.Should().NotBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ConcurrentCallersAcrossInstances_RunOneAtATime()
    {
        var options = new DistributedLockOptions { RetryInterval = TimeSpan.FromMilliseconds(20) };
        var instances = Enumerable.Range(0, 3).Select(_ => CreateInstance(options)).ToArray();
        try
        {
            var resource = NewResource();
            var probe = new ConcurrencyProbe();

            var callers = Enumerable.Range(0, 30).Select(index => instances[index % instances.Length].Lock.ExecuteAsync(
                resource,
                ct => probe.EnterAsync(TimeSpan.FromMilliseconds(10), ct),
                TimeSpan.FromSeconds(60),
                Token));
            await Task.WhenAll(callers);

            probe.MaxConcurrency.Should().Be(1);
            probe.Completed.Should().Be(30);
        }
        finally
        {
            foreach (var instance in instances)
            {
                instance.Dispose();
            }
        }
    }

    [Fact]
    public async Task Lock_HeldLongerThanExpiry_IsExtendedAutomatically()
    {
        var options = new DistributedLockOptions { Expiry = TimeSpan.FromSeconds(1) };
        using var first = CreateInstance(options);
        using var second = CreateInstance(options);
        var resource = NewResource();

        await using (var held = await first.Lock.TryAcquireAsync(resource, cancellationToken: Token))
        {
            held.Should().NotBeNull();
            await Task.Delay(TimeSpan.FromSeconds(3), Token);

            (await second.Lock.TryAcquireAsync(resource, cancellationToken: Token)).Should().BeNull("the holder is alive, so its lock keeps being extended");
            held.HandleLostToken.IsCancellationRequested.Should().BeFalse("renewals keep succeeding");
        }

        await using var afterRelease = await second.Lock.TryAcquireAsync(resource, WaitLimit, Token);
        afterRelease.Should().NotBeNull();
    }

    [Fact]
    public async Task Lock_HolderLosesItsConnection_IsAcquiredByAnotherInstanceAfterExpiry()
    {
        var options = new DistributedLockOptions { Expiry = TimeSpan.FromSeconds(1) };
        using var crashed = CreateInstance(options);
        using var survivor = CreateInstance(options);
        var resource = NewResource();

        // Keep the handle undisposed: a crashed process never releases its lock.
        var held = await crashed.Lock.TryAcquireAsync(resource, cancellationToken: Token);
        held.Should().NotBeNull();
        await crashed.Redis.CloseAsync(allowCommandsToComplete: false);

        await using var recovered = await survivor.Lock.TryAcquireAsync(resource, WaitLimit, Token);

        recovered.Should().NotBeNull("the lock expires once the crashed holder stops extending it");
        // The holder learns about the loss when its renewal fails, which on a dropped connection can take up to the
        // StackExchange.Redis syncTimeout (5 s by default); with this short expiry that is after the survivor acquires.
        var lost = await WaitForCancellation(held.HandleLostToken, WaitLimit);
        lost.Should().BeTrue("the holder that lost its connection is told the lock is gone");
    }

    [Fact]
    public async Task TryAcquireAsync_WhenRedisUnreachable_ThrowsUnavailableInsteadOfReportingBusy()
    {
        // Nothing listens on port 1; short timeouts keep the failing attempt brief.
        using var unreachable = new PlatformInstance("127.0.0.1:1,abortConnect=false,connectTimeout=500,syncTimeout=500,asyncTimeout=500", new DistributedLockOptions());

        var act = () => unreachable.Lock.TryAcquireAsync(NewResource(), TimeSpan.FromSeconds(30), Token);

        await act.Should().ThrowAsync<DistributedLockUnavailableException>();
    }

    [Fact]
    public async Task KeyPrefix_IsolatesApplicationsSharingRedis()
    {
        var application = $"vc-tests-{Guid.NewGuid():N}";
        using var first = CreateInstance(new DistributedLockOptions { KeyPrefix = application + "-a" });
        using var sameApplication = CreateInstance(new DistributedLockOptions { KeyPrefix = application + "-a" });
        using var otherApplication = CreateInstance(new DistributedLockOptions { KeyPrefix = application + "-b" });
        var resource = NewResource();

        await using var held = await first.Lock.TryAcquireAsync(resource, cancellationToken: Token);
        var sameApplicationAttempt = await sameApplication.Lock.TryAcquireAsync(resource, cancellationToken: Token);
        await using var otherApplicationAttempt = await otherApplication.Lock.TryAcquireAsync(resource, cancellationToken: Token);

        held.Should().NotBeNull();
        sameApplicationAttempt.Should().BeNull();
        otherApplicationAttempt.Should().NotBeNull();
    }

    [Fact]
    public async Task LegacyService_WhileAnotherInstanceHoldsLock_ThrowsPlatformException()
    {
        using var first = CreateInstance();
        using var second = CreateInstance();
        var resource = NewResource();
        var legacyService = second.Services.GetRequiredService<IDistributedLockService>();

        await using var held = await first.Lock.TryAcquireAsync(resource, cancellationToken: Token);
        var act = () => legacyService.ExecuteAsync(resource, () => Task.FromResult(1), cancellationToken: Token);

        held.Should().NotBeNull();
        await act.Should().ThrowAsync<PlatformException>();
    }

    [Fact]
    public async Task StartupLock_HeldByPreviousRelease_ExcludesNewRelease()
    {
        // Rolling deploy: an instance on the previous release holds the startup lock through the old service,
        // and an instance on this release takes the same resource through IDistributedLock.
        using var previousRelease = CreateInstance();
        using var newRelease = CreateInstance();
        var resource = $"vc-tests:startup-compat:{Guid.NewGuid():N}";
#pragma warning disable VC0015 // Exercising the previous-release startup lock
        var previousStartupLock = previousRelease.Services.GetRequiredService<IInternalDistributedLockService>();
#pragma warning restore VC0015
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();

        var previousStartup = Task.Run(() => previousStartupLock.ExecuteSynchronized(resource, _ =>
        {
            entered.Set();
            release.Wait(WaitLimit);
        }), Token);
        entered.Wait(WaitLimit, Token).Should().BeTrue();

        var whileHeld = await newRelease.Lock.TryAcquireAsync(resource, cancellationToken: Token);
        var waiting = newRelease.Lock.TryAcquireAsync(resource, WaitLimit, Token);
        release.Set();
        await previousStartup;
        await using var afterRelease = await waiting;

        whileHeld.Should().BeNull();
        afterRelease.Should().NotBeNull();
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

    private static string NewResource()
    {
        return $"vc-tests:lock:{Guid.NewGuid():N}";
    }

    private PlatformInstance CreateInstance(DistributedLockOptions options = null)
    {
        return new PlatformInstance(_redis.ConnectionString, options ?? new DistributedLockOptions());
    }

    /// <summary>
    /// One simulated platform instance: its own container, Redis connection and lock factory.
    /// </summary>
    private sealed class PlatformInstance : IDisposable
    {
        public PlatformInstance(string connectionString, DistributedLockOptions options)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:RedisConnectionString"] = connectionString,
                })
                .Build();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(Options.Create(options));
            services.AddRedis(configuration);
            Services = services.BuildServiceProvider();
        }

        public ServiceProvider Services { get; }

        public IDistributedLock Lock => Services.GetRequiredService<IDistributedLock>();

        public IConnectionMultiplexer Redis => Services.GetRequiredService<IConnectionMultiplexer>();

        public void Dispose()
        {
            // The container does not own the multiplexer instance registered by AddRedis.
            var redis = Redis;
            Services.Dispose();
            redis.Dispose();
        }
    }

    public sealed class RedisFixture : IAsyncLifetime
    {
        public string ConnectionString { get; } =
            Environment.GetEnvironmentVariable("VC_TEST_REDIS_CONNECTION_STRING") ?? "127.0.0.1:6379,ssl=False";

        public bool IsAvailable { get; private set; }

        public async ValueTask InitializeAsync()
        {
            var options = ConfigurationOptions.Parse(ConnectionString);
            options.AbortOnConnectFail = false;
            options.ConnectTimeout = 2000;

            await using var connection = await ConnectionMultiplexer.ConnectAsync(options);
            IsAvailable = connection.IsConnected;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
