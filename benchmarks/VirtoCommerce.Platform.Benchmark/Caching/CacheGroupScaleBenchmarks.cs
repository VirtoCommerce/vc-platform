using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Benchmark.Caching;

// Supplement the one-group hot path with a round-robin scan up to the attribution limit.
// Keys are pre-normalized to isolate grouping cost from key/string creation. Report ns/batch,
// or divide by Groups for ns/lookup; do not compare raw batch times across different group counts.
[MemoryDiagnoser]
[ThreadingDiagnoser]
public class CacheGroupScaleBenchmarks
{
    private PlatformMemoryCache _cache;
    private MeterListener _meter;
    private ActivitySource _source;
    private ActivityListener _traces;
    private Activity _activity;
    private IDisposable _scope;
    private string[] _keys;

    [Params(1, 16, 64)]
    public int Groups { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _meter = new MeterListener
        {
            InstrumentPublished = (instrument, listener) =>
            {
                if (instrument.Meter.Name == "VirtoCommerce.Platform.Caching")
                {
                    listener.EnableMeasurementEvents(instrument);
                }
            },
        };
        _meter.SetMeasurementEventCallback<long>(static (_, _, _, _) => { });
        _meter.Start();
        _source = new ActivitySource("CacheGroups.ScaleBenchmark");
        _traces = new ActivityListener
        {
            ShouldListenTo = source => ReferenceEquals(source, _source),
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
        };
        ActivitySource.AddActivityListener(_traces);
        _activity = _source.StartActivity("group-scale", ActivityKind.Server);
        var metricsType = typeof(PlatformMemoryCache).Assembly.GetType("VirtoCommerce.Platform.Caching.CacheRequestMetrics");
        _scope = (IDisposable)metricsType?.GetMethod("Begin", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, [_activity]);
        if (metricsType is not null && (_scope is null || !ReferenceEquals(_scope, metricsType.GetProperty("Current").GetValue(null))))
        {
            throw new InvalidOperationException("Expected an active sampled accumulator");
        }
        _cache = new PlatformMemoryCache(new MemoryCache(new MemoryCacheOptions()),
            Options.Create(new CachingOptions { CacheEnabled = true }), NullLogger<PlatformMemoryCache>.Instance);
        _keys = typeof(object).Assembly.GetExportedTypes().Where(type => !type.IsGenericType)
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .DistinctBy(type => type.Name, StringComparer.OrdinalIgnoreCase).Take(Groups)
            .Select(type => CacheKey.With(type, "scale-item").ToLowerInvariant()).ToArray();
        if (_keys.Length != Groups)
        {
            throw new InvalidOperationException("Insufficient distinct groups");
        }
        foreach (var key in _keys)
        {
            _cache.Set(key, 42);
            if (!_cache.TryGetValue(key, out _))
            {
                throw new InvalidOperationException("Expected a warm hit");
            }
        }
    }

    [Benchmark]
    public void ReadAllGroups()
    {
        foreach (var key in _keys)
        {
            _cache.TryGetValue(key, out _);
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _scope?.Dispose();
        if (_scope is not null)
        {
            using var detail = JsonDocument.Parse((string)_activity.GetTagItem("cache.lookup.summary"));
            if (detail.RootElement.GetArrayLength() != Groups)
            {
                throw new InvalidOperationException("Measured scope did not retain the expected groups");
            }
        }
        _activity.Dispose();
        _traces.Dispose();
        _source.Dispose();
        _meter.Dispose();
        _cache.Dispose();
    }
}
