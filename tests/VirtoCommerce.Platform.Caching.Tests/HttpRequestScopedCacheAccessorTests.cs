using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests;

public class HttpRequestScopedCacheAccessorTests
{
    // A real HttpContextAccessor is used throughout rather than a mock: the whole point of the accessor is that
    // it reads the ambient context from an AsyncLocal instead of capturing anything, and only the real one has
    // that behaviour.
    private static ServiceProvider BuildProviderWithCache()
    {
        return new ServiceCollection()
            .AddScoped<IRequestScopedCache, RequestScopedCache>()
            .BuildServiceProvider();
    }

    [Fact]
    public void Cache_LiveRequestWithCacheRegistered_ReturnsThatRequestsCache()
    {
        using var provider = BuildProviderWithCache();
        using var scope = provider.CreateScope();
        var accessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext { RequestServices = scope.ServiceProvider },
        };

        var sut = new HttpRequestScopedCacheAccessor(accessor);

        Assert.Same(scope.ServiceProvider.GetRequiredService<IRequestScopedCache>(), sut.Cache);
    }

    [Fact]
    public void Cache_TwoRequests_ReturnsEachRequestsOwnCache()
    {
        // Pins that Cache memoizes nothing: it re-reads the ambient context on every access, so the result
        // follows the request rather than the first one seen. An implementation that cached the context or the
        // resolved cache in a field would hand out the first scope's cache forever.
        using var provider = BuildProviderWithCache();
        using var firstScope = provider.CreateScope();
        using var secondScope = provider.CreateScope();
        var accessor = new HttpContextAccessor();
        var sut = new HttpRequestScopedCacheAccessor(accessor);

        accessor.HttpContext = new DefaultHttpContext { RequestServices = firstScope.ServiceProvider };
        var first = sut.Cache;

        accessor.HttpContext = new DefaultHttpContext { RequestServices = secondScope.ServiceProvider };
        var second = sut.Cache;

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.NotSame(first, second);
    }

    [Fact]
    public void Cache_NoHttpContext_ReturnsNull()
    {
        // A background job: there is no ambient request to scope a per-request cache to, so an uncached path
        // is the correct outcome rather than a degraded one.
        var sut = new HttpRequestScopedCacheAccessor(new HttpContextAccessor());

        Assert.Null(sut.Cache);
    }

    [Fact]
    public void Cache_RequestServicesNotSet_ReturnsNull()
    {
        // A fabricated DefaultHttpContext leaves RequestServices null, which is why the second null-conditional
        // in the implementation is load-bearing rather than defensive.
        var accessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };

        var sut = new HttpRequestScopedCacheAccessor(accessor);

        Assert.Null(sut.Cache);
    }

    [Fact]
    public void Cache_CacheServiceNotRegistered_ReturnsNull()
    {
        using var provider = new ServiceCollection().BuildServiceProvider();
        var accessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext { RequestServices = provider },
        };

        var sut = new HttpRequestScopedCacheAccessor(accessor);

        Assert.Null(sut.Cache);
    }

    [Fact]
    public void Accessor_ResolvedFromRootUnderFullScopeValidation_IsNotACaptiveDependency()
    {
        // The change's central claim: a singleton may depend on the accessor even though the cache it hands out
        // is Scoped, because the cache is resolved per call from a request's own provider and never injected.
        // Asserted against the container rather than in prose - ValidateScopes would reject the contrast case of
        // constructor-injecting IRequestScopedCache into a singleton.
        var services = new ServiceCollection()
            .AddHttpContextAccessor()
            .AddScoped<IRequestScopedCache, RequestScopedCache>();
        services.AddSingleton<IRequestScopedCacheAccessor, HttpRequestScopedCacheAccessor>();

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true,
        });

        var sut = provider.GetRequiredService<IRequestScopedCacheAccessor>();

        Assert.NotNull(sut);
        // No ambient request on the root provider, so there is nothing to hand out - and no throw either.
        Assert.Null(sut.Cache);
    }

    [Fact]
    public void AddCaching_RegistersTheAccessorSingletonAndTheCacheScoped()
    {
        var services = new ServiceCollection();

        services.AddCaching(new ConfigurationBuilder().Build());

        var accessor = services.Single(x => x.ServiceType == typeof(IRequestScopedCacheAccessor));
        var cache = services.Single(x => x.ServiceType == typeof(IRequestScopedCache));

        Assert.Equal(typeof(HttpRequestScopedCacheAccessor), accessor.ImplementationType);
        Assert.Equal(ServiceLifetime.Singleton, accessor.Lifetime);
        Assert.Equal(ServiceLifetime.Scoped, cache.Lifetime);
        Assert.Contains(services, x => x.ServiceType == typeof(IHttpContextAccessor));
    }
}
