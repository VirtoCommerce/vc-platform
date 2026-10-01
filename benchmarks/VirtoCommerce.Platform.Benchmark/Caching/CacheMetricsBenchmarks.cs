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
    private IDisposable _requestMetrics;
    private string _typedKey;
    private string _prefix;
    private string[] _ids;
    private Action<int> _parallelKeys;
    private Action<int> _parallelLookups;
    private static readonly Func<Task<int>> _factory = () => Task.FromResult(42);

    [Params("Off", "Meter", "Request")]
    public string State { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        if (State != "Off")
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

        if (State == "Request")
        {
            _activity = new Activity("cache-benchmark").Start();
            _activity.ActivityTraceFlags = ActivityTraceFlags.Recorded;
            var metricsType = typeof(PlatformMemoryCache).Assembly.GetType("VirtoCommerce.Platform.Caching.CacheRequestMetrics");
            _requestMetrics = (IDisposable)metricsType?.GetMethod("Begin", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, [_activity]);
            Console.WriteLine($"Request accumulator available: {_requestMetrics is not null}");
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

    [GlobalCleanup]
    public void Cleanup()
    {
        _requestMetrics?.Dispose();
        _activity?.Dispose();
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
