using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests;

[Trait("Category", "Unit")]
[Collection(nameof(NotThreadSafeCollection))]
public class CacheMetricsRegressionTests : MemoryCacheTestsBase
{
    private const string PrivatePrefix = "GetProductConfigurationQueryHandler:OptionProducts:store|USD|en-US|user-secret|organization-secret|true";

    [Fact]
    public async Task UnregisteredByIdPrefixDoesNotExportRequestData()
    {
        var names = new ConcurrentBag<string>();
        using var listener = Listen((_, _, tags, _) =>
        {
            foreach (var tag in tags)
            {
                if (tag.Key == "cache.name")
                {
                    names.Add((string)tag.Value);
                }
            }
        });
        var cache = new RequestScopedCache();
        await cache.GetOrLoadMapByIdsAsync<string>(PrivatePrefix, ["private-id"], x => x,
            ids => Task.FromResult<IList<string>>(ids.ToList()));
        await cache.GetOrLoadMapByIdsAsync<string>(PrivatePrefix, ["private-id"], x => x,
            _ => throw new InvalidOperationException("Already cached"));
        Assert.NotEmpty(names);
        Assert.All(names, name => Assert.Equal(nameof(RequestScopedCache), name));
    }

    [Fact]
    public async Task ThrowingListenerDoesNotFailLookupsOrLeaveByIdReservationsPending()
    {
        using var listener = Listen((_, _, _, _) => throw new InvalidOperationException("Listener failed"));
        using var platform = GetPlatformMemoryCache();
        Assert.False(platform.TryGetValue("missing", out _));
        platform.Set("present", 42);
        Assert.True(platform.TryGetValue("present", out _));
        var cache = new RequestScopedCache();
        Assert.Equal(42, await cache.GetOrAddAsync("key", () => Task.FromResult(42)));
        Assert.Equal(42, await cache.GetOrAddAsync<int>("key", () => throw new InvalidOperationException()));

        var loaded = new TaskCompletionSource<IList<string>>(TaskCreationOptions.RunContinuationsAsynchronously);
        var first = cache.GetOrLoadMapByIdsAsync<string>(PrivatePrefix, ["a", "b"], x => x, _ => loaded.Task);
        var second = cache.GetOrLoadMapByIdsAsync<string>(PrivatePrefix, ["b", "c"], x => x,
            ids => Task.FromResult<IList<string>>(ids.ToList()));
        loaded.SetResult(["a", "b"]);
        var results = await Task.WhenAll(first, second).WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        Assert.All(results, result => Assert.Equal(2, result.Count));
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(false, false)]
    [InlineData(true, false)]
    public async Task UnexportedActivityUsesOriginalDownstreamTask(bool recorded, bool allData)
    {
        using var activity = new Activity("server").Start();
        activity.ActivityTraceFlags = recorded ? ActivityTraceFlags.Recorded : ActivityTraceFlags.None;
        activity.IsAllDataRequested = allData;
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var middleware = new CacheMetricsMiddleware(context => completion.Task);
        var actual = middleware.InvokeAsync(Context(activity));
        completion.SetResult();
        await actual;
        Assert.Same(completion.Task, actual);
        Assert.Null(activity.GetTagItem("cache.hits"));
        Assert.Null(activity.GetTagItem("cache.misses"));
    }

    [Fact]
    public async Task SampledRequestWithNoLookupsHasNoCacheTags()
    {
        using var activity = new Activity("server").Start();
        activity.ActivityTraceFlags = ActivityTraceFlags.Recorded;
        await new CacheMetricsMiddleware(context => Task.CompletedTask).InvokeAsync(Context(activity));
        Assert.DoesNotContain(activity.TagObjects, tag => tag.Key.StartsWith("cache.", StringComparison.Ordinal));
        Assert.Empty(activity.Events);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(false, false)]
    [InlineData(true, false)]
    public void AccumulatorItselfRejectsUnexportedActivities(bool recorded, bool allData)
    {
        using var activity = new Activity("server").Start();
        activity.ActivityTraceFlags = recorded ? ActivityTraceFlags.Recorded : ActivityTraceFlags.None;
        activity.IsAllDataRequested = allData;
        var metricsType = typeof(CacheMetricsMiddleware).Assembly.GetType("VirtoCommerce.Platform.Caching.CacheRequestMetrics");
        var begin = metricsType.GetMethod("Begin", BindingFlags.Public | BindingFlags.Static);
        // Also pin the internal gate independently of the middleware's allocation-free fast path.
        using var result = (IDisposable)begin.Invoke(null, [activity]);
        Assert.Null(result);
    }

    [Fact]
    public async Task OutcomesAreEmittedOncePerGroupOnlyForSampledRequests()
    {
        var outcomes = new ConcurrentBag<(long Count, string Name, string Outcome)>();
        using var listener = Listen((instrument, count, tags, _) =>
        {
            if (instrument.Name == "virtocommerce.cache.request.groups")
            {
                var values = tags.ToArray().ToDictionary(x => x.Key, x => x.Value);
                Assert.Equal(2, values.Count);
                outcomes.Add((count, (string)values["cache.name"], (string)values["cache.outcome"]));
            }
        });

        foreach (var sampled in new[] { true, false })
        {
            using var cache = GetPlatformMemoryCache();
            var hitKey = CacheKey.With(typeof(HitOnlyOwner), "private-id");
            var mixedKey = CacheKey.With(typeof(MixedOwner), "private-id");
            var missKey = CacheKey.With(typeof(MissOnlyOwner), "private-id");
            cache.Set(hitKey, 1);
            using var activity = new Activity("server").Start();
            activity.ActivityTraceFlags = sampled ? ActivityTraceFlags.Recorded : ActivityTraceFlags.None;
            await new CacheMetricsMiddleware(context =>
            {
                cache.TryGetValue(hitKey, out _);
                cache.TryGetValue(mixedKey, out _);
                cache.Set(mixedKey, 1);
                cache.TryGetValue(mixedKey, out _);
                cache.TryGetValue(missKey, out _);
                return Task.CompletedTask;
            }).InvokeAsync(Context(activity));
            Assert.Empty(activity.Events);
        }

        Assert.Equal(3, outcomes.Count);
        Assert.Contains((1L, nameof(HitOnlyOwner), "hit_only"), outcomes);
        Assert.Contains((1L, nameof(MixedOwner), "mixed"), outcomes);
        Assert.Contains((1L, nameof(MissOnlyOwner), "miss_only"), outcomes);
    }

    [Fact]
    public async Task ThrowingCompletionListenerPreservesOriginalDownstreamException()
    {
        using var listener = Listen((instrument, _, _, _) =>
        {
            if (instrument.Name == "virtocommerce.cache.request.groups")
            {
                throw new InvalidOperationException("Listener failed at completion");
            }
        });
        using var activity = new Activity("server").Start();
        activity.ActivityTraceFlags = ActivityTraceFlags.Recorded;
        var expected = new InvalidOperationException("Original downstream error");
        using var cache = GetPlatformMemoryCache();
        var middleware = new CacheMetricsMiddleware(context =>
        {
            cache.TryGetValue("missing", out _);
            throw expected;
        });
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => middleware.InvokeAsync(Context(activity)));
        Assert.Same(expected, actual);
        Assert.Equal(1L, activity.GetTagItem("cache.misses"));
    }

    [Fact]
    public async Task NestedMiddlewareRestoresOuterRequestThroughAsyncExecutionContext()
    {
        using var outer = new Activity("outer").Start();
        outer.ActivityTraceFlags = ActivityTraceFlags.Recorded;
        using var inner = new Activity("inner").Start();
        inner.ActivityTraceFlags = ActivityTraceFlags.Recorded;
        using var cache = GetPlatformMemoryCache();
        var innerMiddleware = new CacheMetricsMiddleware(async context =>
        {
            await Task.Yield();
            cache.TryGetValue("inner-only", out _);
        });
        await new CacheMetricsMiddleware(async context =>
        {
            cache.TryGetValue("outer-before", out _);
            await innerMiddleware.InvokeAsync(Context(inner));
            cache.TryGetValue("outer-after", out _);
        }).InvokeAsync(Context(outer));
        Assert.Equal(2L, outer.GetTagItem("cache.misses"));
        Assert.Equal(1L, inner.GetTagItem("cache.misses"));
    }

    [Fact]
    public async Task SummaryByteBudgetPreservesJsonAndTotalsWithoutPerGroupEvents()
    {
        long outcomes = 0;
        using var listener = Listen((instrument, count, _, _) =>
        {
            if (instrument.Name == "virtocommerce.cache.request.groups")
            {
                Interlocked.Add(ref outcomes, count);
            }
        });
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("CacheTelemetryBudgetTests"), AssemblyBuilderAccess.Run);
        var module = assembly.DefineDynamicModule("Groups");
        var keys = Enumerable.Range(0, 64).Select(index =>
        {
            var type = module.DefineType($"BudgetGroup{index}_\"_{new string('я', 160)}", TypeAttributes.Public).CreateType();
            return CacheKey.With(type, "private-key");
        }).ToArray();
        using var activity = new Activity("server").Start();
        activity.ActivityTraceFlags = ActivityTraceFlags.Recorded;
        using var cache = GetPlatformMemoryCache();
        await new CacheMetricsMiddleware(context =>
        {
            foreach (var key in keys)
            {
                cache.TryGetValue(key, out _);
            }
            return Task.CompletedTask;
        }).InvokeAsync(Context(activity));
        var summary = Assert.IsType<string>(activity.GetTagItem("cache.lookup.summary"));
        Assert.InRange(Encoding.UTF8.GetByteCount(summary), 1, 8192);
        using var json = JsonDocument.Parse(summary);
        Assert.InRange(json.RootElement.GetArrayLength(), 1, 63);
        Assert.Equal(64L, activity.GetTagItem("cache.misses"));
        Assert.Equal(64L, outcomes);
        Assert.Equal(true, activity.GetTagItem("cache.groups.truncated"));
        Assert.Empty(activity.Events);
        Assert.DoesNotContain("private-key", summary);
    }

    [Fact]
    public async Task RacingDetachedLookupsCannotChangeCompletedSnapshot()
    {
        using var activity = new Activity("server").Start();
        activity.ActivityTraceFlags = ActivityTraceFlags.Recorded;
        using var cache = GetPlatformMemoryCache();
        var firstHundred = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var finishedLookups = 0;
        Task workers = null;
        await new CacheMetricsMiddleware(async context =>
        {
            workers = Task.WhenAll(Enumerable.Range(0, 8).Select(worker => Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    cache.TryGetValue("race", out _);
                    if (Interlocked.Increment(ref finishedLookups) == 100)
                    {
                        firstHundred.SetResult();
                    }
                }
            })));
            await firstHundred.Task;
        }).InvokeAsync(Context(activity));

        var snapshot = activity.TagObjects.ToArray();
        var misses = Assert.IsType<long>(activity.GetTagItem("cache.misses"));
        Assert.InRange(misses, 100L, 80000L);
        using var json = JsonDocument.Parse(Assert.IsType<string>(activity.GetTagItem("cache.lookup.summary")));
        Assert.Equal(misses, Assert.Single(json.RootElement.EnumerateArray())[2].GetInt64());
        await workers.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        Assert.Equal(snapshot, activity.TagObjects.ToArray());
    }

    private sealed class HitOnlyOwner;
    private sealed class MixedOwner;
    private sealed class MissOnlyOwner;

    private static MeterListener Listen(MeasurementCallback<long> callback)
    {
        var listener = new MeterListener
        {
            InstrumentPublished = (instrument, subscriber) =>
            {
                if (instrument.Meter.Name == "VirtoCommerce.Platform.Caching")
                {
                    subscriber.EnableMeasurementEvents(instrument);
                }
            },
        };
        listener.SetMeasurementEventCallback(callback);
        listener.Start();
        return listener;
    }

    private static DefaultHttpContext Context(Activity activity)
    {
        var context = new DefaultHttpContext();
        context.Features.Set<IHttpActivityFeature>(new HttpActivityFeature { Activity = activity });
        return context;
    }

    private sealed class HttpActivityFeature : IHttpActivityFeature
    {
        public Activity Activity { get; set; }
    }
}
