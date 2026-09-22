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
        var factory = provider.GetRequiredService<IScopedFactory<Service>>();

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
        var factory = provider.GetRequiredService<IScopedFactory<Service>>();

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
    public void Constructor_WithoutScope_OwnsTheServiceItself()
    {
        var dependency = new DisposableDependency(new Recorder());

        var scoped = new ScopedService<DisposableDependency>(dependency);
        scoped.ServiceProvider.Should().BeNull();
        scoped.Dispose();
        scoped.Dispose();

        dependency.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void Constructor_WithoutScope_NonDisposableService_DisposeIsNoOp()
    {
        var service = new Service(new DisposableDependency(new Recorder()));

        var scoped = new ScopedService<Service>(service);
        scoped.Dispose();

        scoped.Service.Should().BeSameAs(service);
        service.Dependency.IsDisposed.Should().BeFalse("the wrapper owns the service, not what the service holds");
    }

    [Fact]
    public async Task Dispose_AsyncOnlyService_IsNoOpAndKeepsOwnershipForDisposeAsync()
    {
        var service = new AsyncOnlyService();
        var scoped = new ScopedService<AsyncOnlyService>(service);

        scoped.Dispose();
        service.IsDisposed.Should().BeFalse("Dispose never throws and cannot release an async-only owner");

        await scoped.DisposeAsync();
        service.IsDisposed.Should().BeTrue("ownership survived the synchronous Dispose");
    }

    [Fact]
    public async Task DisposeAsync_AsyncOnlyService_Disposes()
    {
        var service = new AsyncOnlyService();
        var scoped = new ScopedService<AsyncOnlyService>(service);

        await scoped.DisposeAsync();

        service.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void DelegateFactory_FromFunc_WrapperDisposesTheProducedInstance()
    {
        var dependency = new DisposableDependency(new Recorder());
        var factory = new DelegateScopedFactory<DisposableDependency>(() => dependency);

        using (var scoped = factory.Create())
        {
            scoped.Service.Should().BeSameAs(dependency);
            dependency.IsDisposed.Should().BeFalse();
        }

        dependency.IsDisposed.Should().BeTrue("legacy Func<T> callers disposed what the factory returned, and so does the wrapper");
    }

    [Fact]
    public void DelegateFactory_FromScopedServiceDelegate_UsesTheProducedWrapper()
    {
        using var provider = BuildProvider();
        var factory = new DelegateScopedFactory<Service>(() =>
        {
            var scope = provider.CreateScope();
            return new ScopedService<Service>(scope.ServiceProvider.GetRequiredService<Service>(), scope);
        });

        var scoped = factory.Create();
        scoped.Dispose();

        scoped.Service.Dependency.IsDisposed.Should().BeTrue();
    }

    private static ServiceProvider BuildProvider(Action<IServiceCollection> configure = null)
    {
        var services = new ServiceCollection();
        services.AddSingleton(typeof(IScopedFactory<>), typeof(ScopedFactory<>));
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

    public sealed class DisposableDependency(Recorder recorder) : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
            recorder.Disposed++;
        }
    }

    public sealed class AsyncOnlyService : IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return default;
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
