using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.InProcess;
using VirtoCommerce.Platform.DistributedLock.Redis;
using VirtoCommerce.Platform.Web.Redis;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DistributedLock;

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
        provider.GetRequiredKeyedService<IDistributedLock>(VirtoCommerce.Platform.Web.Redis.ServiceCollectionExtensions.StartupLockServiceKey)
            .Should().BeSameAs(provider.GetRequiredService<IDistributedLock>(), "in-process locks have no lease, so startup shares the default lock");
    }

    [Fact]
    public void AddRedis_WithConnectionString_StartupLockCopiesEveryOptionAndUsesStartupExpiry()
    {
        // Every public option gets a distinct non-default value, so an option the startup copy forgets is caught.
        var options = new DistributedLockOptions();
        var seed = 1;
        foreach (var property in OptionProperties())
        {
            property.SetValue(options, DistinctValue(property.PropertyType, seed++));
        }

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton(Options.Create(options));
        // Nothing listens on port 1; the multiplexer is created without connecting.
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:RedisConnectionString"] = "127.0.0.1:1,abortConnect=false,connectTimeout=100",
            })
            .Build();
        services.AddRedis(configuration);

        using var provider = services.BuildServiceProvider();
        using var redis = provider.GetRequiredService<IConnectionMultiplexer>();
        var defaultLock = provider.GetRequiredService<IDistributedLock>();
        var startupLock = provider.GetRequiredKeyedService<IDistributedLock>(VirtoCommerce.Platform.Web.Redis.ServiceCollectionExtensions.StartupLockServiceKey);

        defaultLock.Should().BeOfType<RedisDistributedLock>();
        startupLock.Should().BeOfType<RedisDistributedLock>().And.NotBeSameAs(defaultLock);
        var startupOptions = LockOptionsOf(startupLock);
        foreach (var property in OptionProperties())
        {
            var expected = property.Name == nameof(DistributedLockOptions.Expiry) ? options.StartupExpiry : property.GetValue(options);
            property.GetValue(startupOptions).Should().Be(expected, "the startup lock must keep {0}", property.Name);
        }

        LockOptionsOf(defaultLock).Expiry.Should().Be(options.Expiry);
    }

    private static IEnumerable<PropertyInfo> OptionProperties()
    {
        return typeof(DistributedLockOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    private static object DistinctValue(Type type, int seed)
    {
        if (type == typeof(TimeSpan))
        {
            return TimeSpan.FromSeconds(1000 + seed);
        }

        if (type == typeof(int))
        {
            return 1000 + seed;
        }

        if (type == typeof(string))
        {
            return $"value-{seed}";
        }

        throw new NotSupportedException($"Add a distinct value for option type {type} to this test.");
    }

    private static DistributedLockOptions LockOptionsOf(IDistributedLock distributedLock)
    {
        var property = typeof(DistributedLockBase).GetProperty("LockOptions", BindingFlags.NonPublic | BindingFlags.Instance)!;
        return (DistributedLockOptions)property.GetValue(distributedLock)!;
    }
}
