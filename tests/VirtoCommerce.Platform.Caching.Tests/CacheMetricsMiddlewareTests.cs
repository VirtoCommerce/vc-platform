using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests;

[Trait("Category", "Unit")]
[Collection(nameof(NotThreadSafeCollection))]
public class CacheMetricsMiddlewareTests : MemoryCacheTestsBase, IDisposable
{
    private readonly CacheTestActivitySource _activities = new();

    public void Dispose()
    {
        _activities.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task CapturesBothCachesOnServerActivityIncludingChildActivitiesWithoutMeterListener()
    {
        using var server = Sampled("server");
        using var outerChild = new Activity("module").Start();
        using var cache = GetPlatformMemoryCache();
        CacheKey.RegisterCacheName(typeof(RequestMetricsProductOwner), typeof(Product));
        var key = CacheKey.With(typeof(RequestMetricsProductOwner), "private-product-id");
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
        var summary = Assert.Single(Summary(server));
        Assert.Equal(nameof(Product), summary[0].GetString());
        Assert.Equal(3L, summary[1].GetInt64());
        Assert.Equal(3L, summary[2].GetInt64());
        Assert.Empty(server.Events);
        Assert.Null(outerChild.GetTagItem("cache.hits"));
        Assert.DoesNotContain("private-product-id", summary.ToString());
        Assert.DoesNotContain("secret-id", summary.ToString());
    }

    [Fact]
    public async Task ParallelRequestsAndParallelLookupsDoNotShareOrLoseCounts()
    {
        var requests = Enumerable.Range(1, 8).Select(async requestNumber =>
        {
            using var server = Sampled($"request-{requestNumber}");
            var middleware = CreateMiddleware(async httpContext =>
            {
                var cache = new RequestScopedCache();
                await Task.WhenAll(Enumerable.Range(0, requestNumber * 25).Select(i => Task.Run(async () =>
                {
                    await cache.GetOrAddAsync(i.ToString(), () => Task.FromResult(i));
                    await cache.GetOrAddAsync<int>(i.ToString(), () => throw new Exception());
                })));
            });
            await middleware.InvokeAsync(Context(server));
            AssertCounts(server, requestNumber * 25, requestNumber * 25);
            Assert.Single(Summary(server));
        });

        await Task.WhenAll(requests);
    }

    [Fact]
    public async Task ExceptionStillRecordsTotalsAndDoesNotLeakIntoFollowingRequest()
    {
        using var failed = Sampled("failed");
        var failure = new InvalidOperationException("Expected");
        var middleware = CreateMiddleware(async httpContext =>
        {
            var cache = new RequestScopedCache();
            await cache.GetOrAddAsync<int>("key", () => throw failure);
        });

        Assert.Same(failure, await Assert.ThrowsAsync<InvalidOperationException>(() => middleware.InvokeAsync(Context(failed))));
        AssertCounts(failed, 0, 1);

        var outside = new RequestScopedCache();
        await outside.GetOrAddAsync("outside", () => Task.FromResult(1));
        AssertCounts(failed, 0, 1);
        using var next = Sampled("next");
        await CreateMiddleware(httpContext => Task.CompletedTask).InvokeAsync(Context(next));
        AssertCounts(next, 0, 0);
        Assert.Empty(next.Events);
    }

    [Fact]
    public async Task DetachedWorkCannotChangeCompletedRequest()
    {
        using var server = Sampled("server");
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task detached = null;
        var middleware = CreateMiddleware(httpContext =>
        {
            detached = Task.Run(async () =>
            {
                await release.Task;
                await new RequestScopedCache().GetOrAddAsync("late-key", () => Task.FromResult(1));
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
        using var server = Sampled("server");
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
    public async Task GroupLimitBoundsSummaryWithoutLosingOverallCounts()
    {
        using var server = Sampled("server");
        var middleware = CreateMiddleware(async httpContext =>
        {
            var cache = new RequestScopedCache();
            foreach (var type in typeof(object).Assembly.GetExportedTypes().DistinctBy(x => x.Name).Take(80))
            {
                var key = CacheKey.With(type, "item");
                await cache.GetOrAddAsync(key, () => Task.FromResult(1));
                await cache.GetOrAddAsync<int>(key, () => throw new Exception());
            }
        });

        await middleware.InvokeAsync(Context(server));
        AssertCounts(server, 80, 80);
        Assert.Equal(64, Summary(server).Length);
        Assert.Empty(server.Events);
        Assert.Equal(true, server.GetTagItem("cache.groups.truncated"));
    }

    [Fact]
    public async Task RedisInheritedLookupIsCountedOnce()
    {
        using var server = Sampled("server");
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
        Assert.Single(Summary(server));
    }

    private static CacheMetricsMiddleware CreateMiddleware(Func<HttpContext, Task> handler)
    {
        return new CacheMetricsMiddleware(context => handler(context));
    }

    private static DefaultHttpContext Context(Activity activity)
    {
        var context = new DefaultHttpContext();
        context.Features.Set<IHttpActivityFeature>(new HttpActivityFeature { Activity = activity });
        return context;
    }

    private static void AssertCounts(Activity activity, long hits, long misses)
    {
        if (hits == 0 && misses == 0)
        {
            Assert.Null(activity.GetTagItem("cache.hits"));
            Assert.Null(activity.GetTagItem("cache.misses"));
            return;
        }
        Assert.Equal(hits, activity.GetTagItem("cache.hits"));
        Assert.Equal(misses, activity.GetTagItem("cache.misses"));
    }

    private static JsonElement[][] Summary(Activity activity)
    {
        return JsonSerializer.Deserialize<JsonElement[][]>((string)activity.GetTagItem("cache.lookup.summary"));
    }

    private Activity Sampled(string name)
    {
        return _activities.Start(name);
    }

    private sealed class HttpActivityFeature : IHttpActivityFeature
    {
        public Activity Activity { get; set; }
    }

    private sealed class RequestMetricsProductOwner;
    private sealed class Product;
}
