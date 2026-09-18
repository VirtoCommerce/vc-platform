using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests;

[Trait("Category", "Unit")]
public class ScopedServiceTests
{
    [Fact]
    public void Create_ResolvesServiceInOwnScope_DisposeReleasesScope()
    {
        using var provider = BuildProvider();
        var factory = provider.GetRequiredService<IScopedServiceFactory<Service>>();

        var scoped = factory.Create();
        scoped.Service.Dependency.IsDisposed.Should().BeFalse();

        scoped.Dispose();
        scoped.Dispose();

        scoped.Service.Dependency.IsDisposed.Should().BeTrue();
        provider.GetRequiredService<Recorder>().Disposed.Should().Be(1);
    }

    [Fact]
    public async Task DisposeAsync_ReleasesScope()
    {
        using var provider = BuildProvider();

        var scoped = provider.CreateScopedService<Service>();
        await scoped.DisposeAsync();

        scoped.Service.Dependency.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void Create_EveryCallGetsItsOwnScope()
    {
        using var provider = BuildProvider();
        var factory = provider.GetRequiredService<IScopedServiceFactory<Service>>();

        using var first = factory.Create();
        using var second = factory.Create();

        first.Service.Dependency.Should().NotBeSameAs(second.Service.Dependency);
    }

    [Fact]
    public void ServiceProvider_ResolvesFromTheSameScopeAsService()
    {
        using var provider = BuildProvider();

        using var scoped = provider.CreateScopedService<Service>();

        scoped.ServiceProvider.GetRequiredService<DisposableDependency>().Should().BeSameAs(scoped.Service.Dependency);
    }

    [Fact]
    public void Create_ResolutionFails_DisposesScope()
    {
        using var provider = BuildProvider(services => services.AddTransient<ThrowingService>());

        var act = () => provider.CreateScopedService<ThrowingService>();

        act.Should().Throw<InvalidOperationException>().WithMessage("boom");
        provider.GetRequiredService<Recorder>().Disposed.Should().Be(1, "the dependency created before the failure belongs to the abandoned scope");
    }

    [Fact]
    public void Constructor_WithoutScope_DisposeIsNoOp()
    {
        var service = new Service(new DisposableDependency(new Recorder()));

        var scoped = new ScopedService<Service>(service);
        scoped.Dispose();

        scoped.Service.Should().BeSameAs(service);
        scoped.ServiceProvider.Should().BeNull();
        service.Dependency.IsDisposed.Should().BeFalse();
    }

    private static ServiceProvider BuildProvider(Action<IServiceCollection> configure = null)
    {
        var services = new ServiceCollection();
        services.AddSingleton(typeof(IScopedServiceFactory<>), typeof(ScopedServiceFactory<>));
        services.AddSingleton<Recorder>();
        services.AddScoped<DisposableDependency>();
        services.AddTransient<Service>();
        configure?.Invoke(services);

        return services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
    }

    public class Recorder
    {
        public int Disposed { get; set; }
    }

    public class DisposableDependency(Recorder recorder) : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
            recorder.Disposed++;
        }
    }

    public class Service(DisposableDependency dependency)
    {
        public DisposableDependency Dependency { get; } = dependency;
    }

    public class ThrowingService
    {
        public ThrowingService(DisposableDependency dependency)
        {
            _ = dependency;
            throw new InvalidOperationException("boom");
        }
    }
}
