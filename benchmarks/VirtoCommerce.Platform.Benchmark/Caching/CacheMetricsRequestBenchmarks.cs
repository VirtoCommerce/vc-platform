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

// Measure the entire middleware lifecycle, including genuinely incomplete downstream tasks. The
// controlled completion excludes thread-pool/I/O latency but forces the middleware's async box.
// Context, Activity and completion-source costs are identical on baseline and branch.
[MemoryDiagnoser]
[ThreadingDiagnoser]
public class CacheMetricsRequestBenchmarks
{
    private PlatformMemoryCache _cache;
    private MeterListener _listener;
    private ActivitySource _source;
    private ActivityListener _traces;
    private RequestDelegate _invoke;
    private TaskCompletionSource _pending;
    private string[] _keys;

    [Params("Off", "OffAfterRequest", "RecordOnly", "Recorded")]
    public string State { get; set; }

    [Params(0, 3)]
    public int Groups { get; set; }

    [Params(false, true)]
    public bool AsyncDownstream { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        if (State is "RecordOnly" or "Recorded")
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
        }
        _source = new ActivitySource("CacheMetrics.RequestBenchmark");
        if (State != "Off")
        {
            _traces = new ActivityListener
            {
                ShouldListenTo = source => ReferenceEquals(source, _source),
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => State == "RecordOnly"
                    ? ActivitySamplingResult.AllData : ActivitySamplingResult.AllDataAndRecorded,
            };
            ActivitySource.AddActivityListener(_traces);
        }
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
        // Exercise/validate the selected path outside timing. OffAfterRequest first completes an
        // actual sampled middleware invocation, then removes the tracing listener for measurements.
        CompleteRequest().GetAwaiter().GetResult();
        if (State == "OffAfterRequest")
        {
            _traces.Dispose();
            _traces = null;
        }
    }

    [Benchmark]
    public async Task CompleteRequest()
    {
        using var activity = _source.StartActivity("cache-request", ActivityKind.Server)
            ?? new Activity("cache-request").SetParentId("00-0123456789abcdef0123456789abcdef-0123456789abcdef-00").Start();
        var context = new DefaultHttpContext();
        context.Features.Set<IHttpActivityFeature>(new HttpActivityFeature { Activity = activity });
        var request = _invoke(context);
        _pending?.SetResult();
        await request;
    }

    private Task Handle(HttpContext context)
    {
        for (var i = 0; i < Groups; i++)
        {
            _cache.TryGetValue(_keys[i], out _);
        }
        _pending = AsyncDownstream ? new TaskCompletionSource() : null;
        return _pending?.Task ?? Task.CompletedTask;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cache.Dispose();
        _listener?.Dispose();
        _traces?.Dispose();
        _source.Dispose();
    }

    private sealed class HttpActivityFeature : IHttpActivityFeature
    {
        public Activity Activity { get; set; }
    }
}
