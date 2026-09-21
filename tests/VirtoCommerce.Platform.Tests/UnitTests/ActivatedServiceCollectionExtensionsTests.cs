using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests;

[Trait("Category", "Unit")]
public class ActivatedServiceCollectionExtensionsTests
{
    [Fact]
    public void AddActivated_TypeWithLegacyConstructor_ResolvesThroughTheMarkedConstructor()
    {
        var services = ConfigureBoth();
        services.AddActivated<IService, ServiceWithLegacyConstructor>(ServiceLifetime.Singleton);
        using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IService>().Via.Should().Be("scoped factory");
    }

    [Fact]
    public void PlainRegistration_TypeWithLegacyConstructor_IsAmbiguousForTheContainer()
    {
        // Documents why AddActivated exists: with both constructors resolvable the container gives up.
        var services = ConfigureBoth();
        services.AddSingleton<IService, ServiceWithLegacyConstructor>();
        using var provider = services.BuildServiceProvider();

        var act = () => provider.GetRequiredService<IService>();

        act.Should().Throw<InvalidOperationException>().WithMessage("*ambiguous*");
    }

    [Fact]
    public void TryAddActivated_ExistingRegistrationWins()
    {
        var services = ConfigureBoth();
        services.AddSingleton<IService, OtherService>();
        services.TryAddActivated<IService, ServiceWithLegacyConstructor>(ServiceLifetime.Singleton);
        using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IService>().Should().BeOfType<OtherService>();
    }

    [Fact]
    public void AddActivated_HonorsLifetime()
    {
        var services = ConfigureBoth();
        services.AddActivated<IService, ServiceWithLegacyConstructor>(ServiceLifetime.Transient);
        using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IService>().Should().NotBeSameAs(provider.GetRequiredService<IService>());
    }

    private static ServiceCollection ConfigureBoth()
    {
        var services = new ServiceCollection();
        services.AddSingleton(typeof(IScopedServiceFactory<>), typeof(ScopedServiceFactory<>));
        services.AddTransient<Dependency>();
        services.AddTransient<Func<Dependency>>(provider => () => provider.GetRequiredService<Dependency>());
        return services;
    }

    public interface IService
    {
        string Via { get; }
    }

    public class Dependency;

    public class OtherService : IService
    {
        public string Via => nameof(OtherService);
    }

    public class ServiceWithLegacyConstructor : IService
    {
        [Obsolete("legacy")]
        public ServiceWithLegacyConstructor(Func<Dependency> factory)
        {
            _ = factory;
            Via = "legacy Func";
        }

        [ActivatorUtilitiesConstructor]
        public ServiceWithLegacyConstructor(IScopedServiceFactory<Dependency> factory)
        {
            _ = factory;
            Via = "scoped factory";
        }

        public string Via { get; }
    }
}
