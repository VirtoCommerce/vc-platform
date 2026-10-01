using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.Platform.Benchmark.Caching;

// Keep this source compatible with the pre-metrics baseline. Only setup uses reflection to opt in
// to the new internal accumulator; the measured methods call the original public cache APIs directly.
[MemoryDiagnoser]
[ThreadingDiagnoser]
public class CacheMetricsBenchmarks
{
    private const int Workers = 8;
    private const int ReadsPerWorker = 1024;
    private readonly ParallelOptions _parallel = new() { MaxDegreeOfParallelism = Workers };
    private PlatformMemoryCache _cache;
    private RequestScopedCache _requestCache;
    private MeterListener _listener;
    private Activity _activity;
    private ActivitySource _source;
    private ActivityListener _traces;
    private IDisposable _requestMetrics;
    private string _typedKey;
    private string _prefix;
    private string[] _ids;
    private Action<int> _parallelKeys;
    private Action<int> _parallelLookups;
    private static readonly Func<Task<int>> _factory = () => Task.FromResult(42);

    [Params("Off", "OffAfterRequest", "Meter", "Request")]
    public string State { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        if (State is "Meter" or "Request")
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

        if (State is "Request" or "OffAfterRequest")
        {
            _source = new ActivitySource("CacheMetrics.Benchmark");
            _traces = new ActivityListener
            {
                ShouldListenTo = source => ReferenceEquals(source, _source),
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            };
            ActivitySource.AddActivityListener(_traces);
            _activity = _source.StartActivity("cache-benchmark", ActivityKind.Server);
            var metricsType = typeof(PlatformMemoryCache).Assembly.GetType("VirtoCommerce.Platform.Caching.CacheRequestMetrics");
            if (State == "OffAfterRequest")
            {
                CompleteSampledRequest(metricsType, _activity).GetAwaiter().GetResult();
                if (metricsType is not null && (!(bool)metricsType.GetProperty("HasStarted").GetValue(null)
                    || metricsType.GetProperty("Current").GetValue(null) is not null))
                {
                    throw new InvalidOperationException("OffAfterRequest requires HasStarted=true and Current=null");
                }
                _activity.Dispose();
                _activity = null;
                _traces.Dispose();
                _source.Dispose();
            }
            else
            {
                _requestMetrics = BeginRequest(metricsType, _activity);
            }
        }

        _cache = new PlatformMemoryCache(new MemoryCache(new MemoryCacheOptions()),
            Options.Create(new CachingOptions { CacheEnabled = true }), NullLogger<PlatformMemoryCache>.Instance);
        _requestCache = new RequestScopedCache();
        _typedKey = CacheKey.With(typeof(CacheMetricsBenchmarks), "warm-item");
        _prefix = CacheKey.With(typeof(CacheMetricsBenchmarks), "batch");
        _ids = Enumerable.Range(0, 50).Select(i => $"id-{i}").ToArray();
        _cache.Set(_typedKey, 42);
        _cache.Set("untyped-key", 42);
        _requestCache.GetOrAddAsync(_typedKey, _factory).GetAwaiter().GetResult();
        _requestCache.GetOrLoadMapByIdsAsync<string>(_prefix, _ids, static x => x, LoadStrings).GetAwaiter().GetResult();
        _cache.GetOrLoadByIdsAsync<string>(_prefix, _ids, static x => x, LoadStrings, static (_, _, _) => { }).GetAwaiter().GetResult();
        _ = new BenchCrudService();

        _parallelKeys = _ =>
        {
            for (var i = 0; i < ReadsPerWorker; i++)
            {
                GC.KeepAlive(CacheKey.With(typeof(CacheMetricsBenchmarks), "warm-item"));
            }
        };
        _parallelLookups = _ =>
        {
            for (var i = 0; i < ReadsPerWorker; i++)
            {
                _cache.TryGetValue(_typedKey, out var value);
                GC.KeepAlive(value);
            }
        };
        Parallel.For(0, Workers, _parallel, _parallelLookups);
    }

    private static IDisposable BeginRequest(Type metricsType, Activity activity)
    {
        var scope = (IDisposable)metricsType?.GetMethod("Begin", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, [activity]);
        Console.WriteLine($"Request accumulator available: {scope is not null}");
        if (metricsType is not null && scope is null)
        {
            throw new InvalidOperationException("Expected a sampled accumulator on an instrumented revision");
        }
        return scope;
    }

    private static async Task CompleteSampledRequest(Type metricsType, Activity activity)
    {
        // As in the middleware, the async builder restores the caller's ExecutionContext. Off after
        // this request measures HasStarted=true with no stale/completed accumulator in the caller.
        using var scope = BeginRequest(metricsType, activity);
        await Task.CompletedTask;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _requestMetrics?.Dispose();
        _activity?.Dispose();
        _traces?.Dispose();
        _source?.Dispose();
        _listener?.Dispose();
        _cache.Dispose();
    }

    [Benchmark]
    public string TypedKey() => CacheKey.With(typeof(CacheMetricsBenchmarks), "warm-item");

    [Benchmark(OperationsPerInvoke = Workers * ReadsPerWorker)]
    public void TypedKeyEightThreads() => Parallel.For(0, Workers, _parallel, _parallelKeys);

    [Benchmark]
    public bool TypedHit() => _cache.TryGetValue(_typedKey, out _);

    [Benchmark]
    public bool UntypedHit() => _cache.TryGetValue("untyped-key", out _);

    [Benchmark(OperationsPerInvoke = Workers * ReadsPerWorker)]
    public void TypedHitEightThreads() => Parallel.For(0, Workers, _parallel, _parallelLookups);

    [Benchmark]
    public int ExclusiveWarmHit() => _cache.GetOrCreateExclusive<int>(_typedKey, static _ => throw new InvalidOperationException("Expected a hit"));

    [Benchmark]
    public Task<IList<string>> PlatformWarmBatch() => _cache.GetOrLoadByIdsAsync<string>(_prefix, _ids, static x => x,
        static _ => throw new InvalidOperationException("Expected all hits"), static (_, _, _) => { });

    [Benchmark]
    public Task<int> RequestByKeyHit() => _requestCache.GetOrAddAsync(_typedKey, _factory);

    [Benchmark]
    public Task<IDictionary<string, string>> RequestWarmBatch() => _requestCache.GetOrLoadMapByIdsAsync<string>(_prefix, _ids,
        static x => x, static _ => throw new InvalidOperationException("Expected all hits"));

    [Benchmark]
    public BenchCrudService CrudConstructor() => new();

    private static Task<IList<string>> LoadStrings(ICollection<string> ids) => Task.FromResult<IList<string>>(ids.ToList());

    public sealed class BenchModel : Entity, ICloneable
    {
        public object Clone() => MemberwiseClone();
    }

    public sealed class BenchEntity : Entity, IDataEntity<BenchEntity, BenchModel>
    {
        public BenchModel ToModel(BenchModel model) => throw new NotSupportedException();
        public BenchEntity FromModel(BenchModel model, PrimaryKeyResolvingMap pkMap) => throw new NotSupportedException();
        public void Patch(BenchEntity target) => throw new NotSupportedException();
    }

    public sealed class BenchEvent(IEnumerable<GenericChangedEntry<BenchModel>> entries) : GenericChangedEntryEvent<BenchModel>(entries);

    public sealed class BenchCrudService() : CrudService<BenchModel, BenchEntity, BenchEvent, BenchEvent>(null, null, null)
    {
        protected override Task<IList<BenchEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
            => throw new NotSupportedException();
    }
}
