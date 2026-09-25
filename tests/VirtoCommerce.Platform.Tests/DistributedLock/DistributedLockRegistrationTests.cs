using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.InProcess;
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
}
