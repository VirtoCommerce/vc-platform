using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests;

[Trait("Category", "Unit")]
[Collection(nameof(NotThreadSafeCollection))]
public class CacheMetricsMiddlewareTests : MemoryCacheTestsBase
{
    [Fact]
    public async Task CapturesBothCachesOnServerActivityIncludingChildActivitiesWithoutMeterListener()
    {
        using var server = new Activity("server").Start();
        using var outerChild = new Activity("module").Start();
        using var cache = GetPlatformMemoryCache();
        CacheKey.RegisterCacheName(typeof(ProductOwner), typeof(Product));
        var key = CacheKey.With(typeof(ProductOwner), "private-product-id");
        var middleware = CreateMiddleware(async httpContext =>
        {
            using var child = new Activity("graphql").Start();
            Assert.False(cache.TryGetValue(key, out _));
            cache.Set(key, 42);
            Assert.True(cache.TryGetValue(key, out _));
            var requestCache = new RequestScopedCache();
            await requestCache.GetOrAddAsync(key, () => Task.FromResult(42));
            await requestCache.GetOrAddAsync<int>(key, () => throw new Exception("Must be cached"));
            await requestCache.GetOrLoadMapByIdsAsync<string>(key, ["secret-id"], x => x,
                ids => Task.FromResult<IList<string>>(ids.ToList()));
            await requestCache.GetOrLoadMapByIdsAsync<string>(key, ["secret-id"], x => x,
                _ => throw new Exception("Must be cached"));
            Assert.Null(child.GetTagItem("cache.hits"));
        });

        await middleware.InvokeAsync(Context(server));

        AssertCounts(server, 3, 3);
        var summary = Assert.Single(server.Events);
        Assert.Equal("cache.lookup.summary", summary.Name);
        Assert.Equal(nameof(Product), summary.Tags.Single(x => x.Key == "cache.name").Value);
        Assert.Equal(3L, summary.Tags.Single(x => x.Key == "cache.hits").Value);
        Assert.Equal(3L, summary.Tags.Single(x => x.Key == "cache.misses").Value);
        Assert.Null(outerChild.GetTagItem("cache.hits"));
        Assert.DoesNotContain("private-product-id", string.Join(',', summary.Tags));
        Assert.DoesNotContain("secret-id", string.Join(',', summary.Tags));
    }

    [Fact]
    public async Task ParallelRequestsAndParallelLookupsDoNotShareOrLoseCounts()
    {
        var requests = Enumerable.Range(1, 8).Select(async requestNumber =>
        {
            using var server = new Activity($"request-{requestNumber}").Start();
            var middleware = CreateMiddleware(async httpContext =>
            {
                var cache = new RequestScopedCache();
                await Task.WhenAll(Enumerable.Range(0, requestNumber * 25).Select(i => Task.Run(async () =>
                {
                    await cache.GetOrAddAsync(i.ToString(), "Products", () => Task.FromResult(i));
                    await cache.GetOrAddAsync<int>(i.ToString(), "Products", () => throw new Exception());
                })));
            });
            await middleware.InvokeAsync(Context(server));
            AssertCounts(server, requestNumber * 25, requestNumber * 25);
            Assert.Single(server.Events);
        });

        await Task.WhenAll(requests);
    }

    [Fact]
    public async Task ExceptionStillRecordsTotalsAndDoesNotLeakIntoFollowingRequest()
    {
        using var failed = new Activity("failed").Start();
        var failure = new InvalidOperationException("Expected");
        var middleware = CreateMiddleware(async httpContext =>
        {
            var cache = new RequestScopedCache();
            await cache.GetOrAddAsync<int>("key", "Products", () => throw failure);
        });

        Assert.Same(failure, await Assert.ThrowsAsync<InvalidOperationException>(() => middleware.InvokeAsync(Context(failed))));
        AssertCounts(failed, 0, 1);

        var outside = new RequestScopedCache();
        await outside.GetOrAddAsync("outside", () => Task.FromResult(1));
        AssertCounts(failed, 0, 1);
        using var next = new Activity("next").Start();
        await CreateMiddleware(httpContext => Task.CompletedTask).InvokeAsync(Context(next));
        AssertCounts(next, 0, 0);
        Assert.Empty(next.Events);
    }

    [Fact]
    public async Task DetachedWorkCannotChangeCompletedRequest()
    {
        using var server = new Activity("server").Start();
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task detached = null;
        var middleware = CreateMiddleware(httpContext =>
        {
            detached = Task.Run(async () =>
            {
                await release.Task;
                await new RequestScopedCache().GetOrAddAsync("late-key", "Products", () => Task.FromResult(1));
            });
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(Context(server));
        release.SetResult();
        await detached;
        AssertCounts(server, 0, 0);
        Assert.Empty(server.Events);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task MissingOrUnsampledServerActivityDoesNotCreateTraceData(bool unsampled)
    {
        using var server = new Activity("server").Start();
        server.IsAllDataRequested = !unsampled;
        var context = unsampled ? Context(server) : new DefaultHttpContext();
        using var cache = GetPlatformMemoryCache();
        var middleware = CreateMiddleware(httpContext =>
        {
            cache.TryGetValue("key", out _);
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(context);
        Assert.Null(server.GetTagItem("cache.hits"));
        Assert.Null(server.GetTagItem("cache.misses"));
        Assert.Empty(server.Events);
    }

    [Fact]
    public async Task GroupLimitBoundsEventsWithoutLosingOverallCounts()
    {
        using var server = new Activity("server").Start();
        var middleware = CreateMiddleware(async httpContext =>
        {
            var cache = new RequestScopedCache();
            for (var i = 0; i < 80; i++)
            {
                await cache.GetOrAddAsync(i.ToString(), $"Group{i}", () => Task.FromResult(i));
                await cache.GetOrAddAsync<int>(i.ToString(), $"Group{i}", () => throw new Exception());
            }
        });

        await middleware.InvokeAsync(Context(server));
        AssertCounts(server, 80, 80);
        Assert.Equal(64, server.Events.Count());
        Assert.Equal(true, server.GetTagItem("cache.groups.truncated"));
    }

    [Fact]
    public async Task RedisInheritedLookupIsCountedOnce()
    {
        using var server = new Activity("server").Start();
        using var cache = new RedisPlatformMemoryCacheTests().GetRedisPlatformMemoryCache();
        var middleware = CreateMiddleware(httpContext =>
        {
            Assert.False(cache.TryGetValue("key", out _));
            cache.Set("key", 42);
            Assert.True(cache.TryGetValue("key", out _));
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(Context(server));
        AssertCounts(server, 1, 1);
        Assert.Single(server.Events);
    }

    private static CacheMetricsMiddleware CreateMiddleware(Func<HttpContext, Task> handler)
    {
        return new CacheMetricsMiddleware(context => handler(context));
    }

    private static HttpContext Context(Activity activity)
    {
        var context = new DefaultHttpContext();
        context.Features.Set<IHttpActivityFeature>(new HttpActivityFeature { Activity = activity });
        return context;
    }

    private static void AssertCounts(Activity activity, long hits, long misses)
    {
        Assert.Equal(hits, activity.GetTagItem("cache.hits"));
        Assert.Equal(misses, activity.GetTagItem("cache.misses"));
    }

    private sealed class HttpActivityFeature : IHttpActivityFeature
    {
        public Activity Activity { get; set; }
    }

    private sealed class ProductOwner;
    private sealed class Product;
}
