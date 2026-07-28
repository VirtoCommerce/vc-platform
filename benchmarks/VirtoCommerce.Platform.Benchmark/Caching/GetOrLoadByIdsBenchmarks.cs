using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Benchmark.Caching;

// Measures the ALL-HIT read path of GetOrLoadByIdsAsync: the cache is warmed once in GlobalSetup, so
// every measured invocation is a pure cache read (TryGetByIds succeeds, the loader is never invoked).
// The realistic long prefix exercises CacheKey.With/Normalize per-id key building, which is otherwise
// invisible with a short synthetic prefix.
[MemoryDiagnoser]
public class GetOrLoadByIdsBenchmarks
{
    private const string _prefix = "VirtoCommerce.Platform.SomeLongTypeName.Cache:GetByNames";

    [Params(1, 10, 100)]
    public int Count { get; set; }

    private IMemoryCache _cache;
    private string[] _ids;

    [GlobalSetup]
    public void Setup()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
        _ids = Enumerable.Range(0, Count).Select(i => $"id-{i}").ToArray();

        // Warm the cache once so every subsequent read is an all-hit; the loader below only ever
        // executes here, not during the measured [Benchmark] invocations.
        _cache.GetOrLoadByIdsAsync(_prefix, _ids, x => x.Id, LoadItems, ConfigureCache).GetAwaiter().GetResult();
    }

    [Benchmark]
    public Task<IList<BenchCacheItem>> AllHitRead()
        => _cache.GetOrLoadByIdsAsync(_prefix, _ids, x => x.Id, LoadItems, ConfigureCache);

    private static Task<IList<BenchCacheItem>> LoadItems(IList<string> ids)
        => Task.FromResult<IList<BenchCacheItem>>(ids.Select(id => new BenchCacheItem { Id = id }).ToList());

    private static void ConfigureCache(MemoryCacheEntryOptions options, string id, BenchCacheItem item)
    {
        options.SlidingExpiration = TimeSpan.FromMinutes(10);
    }

    // Must be at least as accessible as the public [Benchmark] method that returns it (CS0050).
    public sealed class BenchCacheItem
    {
        public string Id { get; set; }
    }
}
