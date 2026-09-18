using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests;

[Trait("Category", "Unit")]
public class ServiceProviderExtensionsTests
{
    [Fact]
    public void ResolveInOwnScope_ScopeOwner_DisposesScopeWithService()
    {
        using var provider = BuildProvider(services => services.AddTransient<OwnerService>());

        var service = provider.ResolveInOwnScope<OwnerService>();
        service.Dependency.IsDisposed.Should().BeFalse();

        service.Dispose();
        service.Dispose();

        service.Dependency.IsDisposed.Should().BeTrue();
        provider.GetRequiredService<Recorder>().Disposed.Should().Be(1);
    }

    [Fact]
    public void ResolveInOwnScope_NotAScopeOwner_KeepsScopeAliveWithService()
    {
        using var provider = BuildProvider(services => services.AddTransient<PlainService>());

        var service = provider.ResolveInOwnScope<PlainService>();

        service.Dependency.IsDisposed.Should().BeFalse();
    }

    [Fact]
    public void ResolveInOwnScope_ResolutionFails_DisposesScope()
    {
        using var provider = BuildProvider(services => services.AddTransient<ThrowingService>());

        var act = () => provider.ResolveInOwnScope<ThrowingService>();

        act.Should().Throw<InvalidOperationException>().WithMessage("boom");
        provider.GetRequiredService<Recorder>().Disposed.Should().Be(1, "the dependency created before the failure belongs to the abandoned scope");
    }

    private static ServiceProvider BuildProvider(Action<IServiceCollection> configure)
    {
        var services = new ServiceCollection();
        services.AddSingleton<Recorder>();
        services.AddScoped<DisposableDependency>();
        configure(services);

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

    public class PlainService(DisposableDependency dependency)
    {
        public DisposableDependency Dependency { get; } = dependency;
    }

    public class OwnerService(DisposableDependency dependency) : IServiceScopeOwner, IDisposable
    {
        private IServiceScope _ownedScope;

        public DisposableDependency Dependency { get; } = dependency;

        public void OwnScope(IServiceScope scope)
        {
            _ownedScope = scope;
        }

        public void Dispose()
        {
            var scope = _ownedScope;
            _ownedScope = null;
            scope?.Dispose();
        }
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
