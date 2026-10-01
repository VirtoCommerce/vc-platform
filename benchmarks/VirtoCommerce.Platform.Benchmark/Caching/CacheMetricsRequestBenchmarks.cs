using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Benchmark.Caching;

// Include accumulator initialization and completion: warmed lookup benchmarks deliberately exclude
// these costs. Activity/context creation is identical on baseline and branch. The baseline binds
// directly to the handler when the middleware does not exist; reflection is setup-only.
[MemoryDiagnoser]
[ThreadingDiagnoser]
public class CacheMetricsRequestBenchmarks
{
    private PlatformMemoryCache _cache;
    private MeterListener _listener;
    private RequestDelegate _invoke;
    private string[] _keys;

    [Params("RecordOnly", "Recorded")]
    public string State { get; set; }

    [Params(0, 3)]
    public int Groups { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _listener = new MeterListener
        {
            InstrumentPublished = (instrument, subscriber) =>
            {
                if (instrument.Meter.Name == "VirtoCommerce.Platform.Caching")
                {
                    subscriber.EnableMeasurementEvents(instrument);
                }
            },
        };
        _listener.SetMeasurementEventCallback<long>(static (_, _, _, _) => { });
        _listener.Start();
        _cache = new PlatformMemoryCache(new MemoryCache(new MemoryCacheOptions()),
            Options.Create(new CachingOptions { CacheEnabled = true }), NullLogger<PlatformMemoryCache>.Instance);
        _keys = [CacheKey.With(typeof(string), "item"), CacheKey.With(typeof(int), "item"), CacheKey.With(typeof(bool), "item")];
        foreach (var key in _keys)
        {
            _cache.Set(key, 42);
        }

        RequestDelegate handler = Handle;
        var middlewareType = typeof(PlatformMemoryCache).Assembly.GetType("VirtoCommerce.Platform.Caching.CacheMetricsMiddleware");
        _invoke = middlewareType is null ? handler : middlewareType.GetMethod("InvokeAsync", BindingFlags.Instance | BindingFlags.Public)
            .CreateDelegate<RequestDelegate>(Activator.CreateInstance(middlewareType, handler));
    }

    [Benchmark]
    public async Task CompleteRequest()
    {
        using var activity = new Activity("cache-request").Start();
        activity.ActivityTraceFlags = State == "Recorded" ? ActivityTraceFlags.Recorded : ActivityTraceFlags.None;
        activity.IsAllDataRequested = true;
        var context = new DefaultHttpContext();
        context.Features.Set<IHttpActivityFeature>(new HttpActivityFeature { Activity = activity });
        await _invoke(context);
    }

    private Task Handle(HttpContext context)
    {
        for (var i = 0; i < Groups; i++)
        {
            _cache.TryGetValue(_keys[i], out _);
        }
        return Task.CompletedTask;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cache.Dispose();
        _listener.Dispose();
    }

    private sealed class HttpActivityFeature : IHttpActivityFeature
    {
        public Activity Activity { get; set; }
    }
}
