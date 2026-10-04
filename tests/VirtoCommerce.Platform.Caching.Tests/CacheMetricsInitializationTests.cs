using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests;

[Trait("Category", "Unit")]
[Collection(nameof(NotThreadSafeCollection))]
public class CacheMetricsInitializationTests
{
    [Theory]
    [InlineData("virtocommerce.cache.requests")]
    [InlineData("virtocommerce.cache.request.groups")]
    public async Task InstrumentPublicationFailureDoesNotPoisonCacheAccess(string failingInstrument)
    {
        using var isolated = new IsolatedCachingAssembly();
        var publishedMeters = new HashSet<Meter>();
        var callbacks = 0;
        var armed = false;
        using var listener = new MeterListener
        {
            InstrumentPublished = (instrument, _) =>
            {
                if (armed && instrument.Meter.Name == "VirtoCommerce.Platform.Caching")
                {
                    publishedMeters.Add(instrument.Meter);
                    if (instrument.Name == failingInstrument)
                    {
                        callbacks++;
                        throw new InvalidOperationException("Publication failed");
                    }
                }
            },
        };
        listener.Start();
        armed = true;
        try
        {
            using var platform = isolated.CreatePlatformCache();
            var request = isolated.CreateRequestCache();
            using var activities = new CacheTestActivitySource();
            using var activity = activities.Start("publication-failure");
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpActivityFeature>(new HttpActivityFeature { Activity = activity });
            await isolated.Middleware(_ => AssertCacheOperations(platform, request))(context);
            Assert.True(callbacks > 0, "The failing publication callback must actually run before the first cache access.");
            Assert.Equal(2L, activity.GetTagItem("cache.hits"));
            Assert.Equal(4L, activity.GetTagItem("cache.misses"));
            listener.Dispose();
            // A poisoned type stays poisoned after listener removal. Repeated access pins recovery.
            await AssertCacheOperations(platform, request);
        }
        finally
        {
            listener.Dispose();
            foreach (var meter in publishedMeters)
            {
                meter.Dispose();
            }
        }
    }

    [Fact]
    public async Task IncomingRecordedLegacyActivityDoesNotStartRequestMetrics()
    {
        using var isolated = new IsolatedCachingAssembly();
        using var activity = new Activity("legacy-server")
            .SetParentId("00-0123456789abcdef0123456789abcdef-0123456789abcdef-01").Start();
        Assert.True(activity.Recorded);
        Assert.True(activity.IsAllDataRequested);
        Assert.False(activity.Source.HasListeners());
        Assert.False(isolated.HasStarted);
        using var direct = isolated.Begin(activity);
        Assert.Null(direct);
        Assert.False(isolated.HasStarted);

        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var middleware = isolated.Middleware(_ => completion.Task);
        var context = new DefaultHttpContext();
        context.Features.Set<IHttpActivityFeature>(new HttpActivityFeature { Activity = activity });
        var actual = middleware(context);
        completion.SetResult();
        await actual;
        Assert.Same(completion.Task, actual);
        Assert.False(isolated.HasStarted);
        Assert.DoesNotContain(activity.TagObjects, tag => tag.Key.StartsWith("cache.", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task IncomingRecordedLegacyActivityWithListenerDoesNotStartRequestMetrics(bool throughMiddleware)
    {
        using var isolated = new IsolatedCachingAssembly();
        using var listener = new ActivityListener
        {
            ShouldListenTo = source => string.IsNullOrEmpty(source.Name),
        };
        ActivitySource.AddActivityListener(listener);
        using var activity = new Activity("legacy-server")
            .SetParentId("00-0123456789abcdef0123456789abcdef-0123456789abcdef-01").Start();
        Assert.True(activity.Recorded);
        Assert.True(activity.IsAllDataRequested);
        Assert.Empty(activity.Source.Name);
        Assert.True(activity.Source.HasListeners());
        Assert.False(isolated.HasStarted);

        if (throughMiddleware)
        {
            var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var middleware = isolated.Middleware(_ => completion.Task);
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpActivityFeature>(new HttpActivityFeature { Activity = activity });
            var actual = middleware(context);
            completion.SetResult();
            await actual;
            Assert.Same(completion.Task, actual);
        }
        else
        {
            using var direct = isolated.Begin(activity);
            Assert.Null(direct);
        }

        Assert.False(isolated.HasStarted);
        Assert.DoesNotContain(activity.TagObjects, tag => tag.Key.StartsWith("cache.", StringComparison.Ordinal));
    }

    private static async Task AssertCacheOperations(IPlatformMemoryCache platform, IRequestScopedCache request)
    {
        Assert.False(platform.TryGetValue("absent", out _));
        Assert.True(platform.TryGetValue("present", out var value));
        Assert.Equal(42, value);
        Assert.Equal(42, await request.GetOrAddAsync("key", () => Task.FromResult(42)));
        Assert.Equal(42, await request.GetOrAddAsync<int>("key", () => throw new InvalidOperationException("Expected a hit")));
        var loaded = await request.GetOrLoadMapByIdsAsync<string>("private-prefix", ["a", "b"], x => x,
            ids => Task.FromResult<IList<string>>(ids.ToList()));
        Assert.Equal(2, loaded.Count);
    }

    private sealed class HttpActivityFeature : IHttpActivityFeature
    {
        public Activity Activity { get; set; }
    }

    // Fresh platform statics without relying on xUnit execution order or resetting production fields.
    // Framework/Core types are shared with the default context, including MeterListener and interfaces.
    private sealed class IsolatedCachingAssembly : IDisposable
    {
        private readonly AssemblyLoadContext _context = new("CacheMetrics initialization test", isCollectible: true);
        private readonly Assembly _assembly;

        public IsolatedCachingAssembly()
        {
            _assembly = _context.LoadFromAssemblyPath(typeof(PlatformMemoryCache).Assembly.Location);
        }

        public bool HasStarted => (bool)MetricsType.GetProperty("HasStarted").GetValue(null);
        private Type MetricsType => _assembly.GetType("VirtoCommerce.Platform.Caching.CacheRequestMetrics", throwOnError: true);
        public IDisposable Begin(Activity activity) => (IDisposable)MetricsType.GetMethod("Begin").Invoke(null, [activity]);

        public IPlatformMemoryCache CreatePlatformCache()
        {
            var cacheType = _assembly.GetType(typeof(PlatformMemoryCache).FullName, throwOnError: true);
            var optionsType = _assembly.GetType(typeof(CachingOptions).FullName, throwOnError: true);
            var options = Activator.CreateInstance(optionsType);
            optionsType.GetProperty("CacheEnabled").SetValue(options, true);
            var wrapper = Activator.CreateInstance(typeof(OptionsWrapper<>).MakeGenericType(optionsType), options);
            var logger = Activator.CreateInstance(typeof(NullLogger<>).MakeGenericType(cacheType));
            var memory = new MemoryCache(new MemoryCacheOptions());
            // Populate the backing store directly: this test isolates metric initialization and must
            // not register the isolated GlobalCacheRegion type in the shared Core cancellation event.
            memory.Set("present", 42);
            return (IPlatformMemoryCache)Activator.CreateInstance(cacheType, memory, wrapper, logger);
        }

        public IRequestScopedCache CreateRequestCache() => (IRequestScopedCache)Activator.CreateInstance(
            _assembly.GetType(typeof(RequestScopedCache).FullName, throwOnError: true));

        public RequestDelegate Middleware(RequestDelegate next)
        {
            var type = _assembly.GetType(typeof(CacheMetricsMiddleware).FullName, throwOnError: true);
            return type.GetMethod("InvokeAsync").CreateDelegate<RequestDelegate>(Activator.CreateInstance(type, next));
        }

        public void Dispose() => _context.Unload();
    }
}
