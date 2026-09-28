using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests;

[Trait("Category", "Unit")]
[Collection(nameof(NotThreadSafeCollection))]
public class CacheMetricsTests : MemoryCacheTestsBase
{
    [Fact]
    public void MemoryCache_GroupsModelsWithoutKeysOrIds_AndPreservesNormalizedIdentity()
    {
        using var measurements = new Measurements();
        using var cache = GetPlatformMemoryCache();
        CacheKey.RegisterCacheName(typeof(ProductOwner), typeof(Product));
        CacheKey.RegisterCacheName(typeof(CategoryOwner), typeof(Category));

        for (var i = 0; i < 100; i++)
        {
            var productKey = CacheKey.With(typeof(ProductOwner), "GetAsync", $"private-product-{i}");
            Assert.False(cache.TryGetValue(productKey, out _));
            cache.Set(productKey, i);
            Assert.True(cache.TryGetValue(productKey.ToUpperInvariant(), out var value));
            Assert.Equal(i, value);

            var categoryKey = CacheKey.With(typeof(CategoryOwner), "GetAsync", $"private-category-{i}");
            Assert.False(cache.TryGetValue(categoryKey, out _));
        }

        measurements.AssertCounts(nameof(Product), hits: 100, misses: 100);
        measurements.AssertCounts(nameof(Category), hits: 0, misses: 100);
        measurements.AssertNames(nameof(Product), nameof(Category));
    }

    [Fact]
    public void MemoryCache_UsesKnownOwner_AndNeverUsesArbitraryPrefixesOrObjectStrings()
    {
        using var measurements = new Measurements();
        using var cache = GetPlatformMemoryCache();
        Assert.False(cache.TryGetValue(CacheKey.With(typeof(SettingsOwner), "secret-value"), out _));
        Assert.False(cache.TryGetValue("alice@example.com:private-data", out _));
        Assert.False(cache.TryGetValue("unknown-prefix:private-data", out _));
        Assert.False(cache.TryGetValue(new object(), out _));

        measurements.AssertCounts(nameof(SettingsOwner), hits: 0, misses: 1);
        measurements.AssertCounts(nameof(PlatformMemoryCache), hits: 0, misses: 3);
        measurements.AssertNames(nameof(SettingsOwner), nameof(PlatformMemoryCache));
    }

    [Fact]
    public void Redis_UsesLogicalGroupWithoutDoubleCounting_AndRemoveKeepsItsBehavior()
    {
        using var measurements = new Measurements();
        using var cache = new RedisPlatformMemoryCacheTests().GetRedisPlatformMemoryCache();
        CacheKey.RegisterCacheName(typeof(ProductOwner), typeof(Product));
        var key = CacheKey.With(typeof(ProductOwner), "GetAsync", "42");
        Assert.False(cache.TryGetValue(key, out _));
        cache.Set(key, 42);
        Assert.True(cache.TryGetValue(key, out _));
        cache.Remove(key.ToUpperInvariant());
        Assert.False(cache.TryGetValue(key, out _));

        measurements.AssertCounts(nameof(Product), hits: 1, misses: 2);
        measurements.AssertNames(nameof(Product));
    }

    private sealed class ProductOwner;
    private sealed class CategoryOwner;
    private sealed class SettingsOwner;
    private sealed class Product;
    private sealed class Category;

    [Fact]
    public async Task RequestCache_TypeOwnedKeysUseLogicalGroupsWithoutDynamicSuffixes()
    {
        using var measurements = new Measurements();
        var cache = new RequestScopedCache();
        CacheKey.RegisterCacheName(typeof(ProductOwner), typeof(Product));
        var prefix = CacheKey.With(typeof(ProductOwner), "GetAsync", "private-store-id");
        await cache.GetOrAddAsync(prefix, () => Task.FromResult(42));
        await cache.GetOrAddAsync<int>(prefix, () => throw new InvalidOperationException());
        await cache.GetOrLoadMapByIdsAsync<string>(prefix, ["private-id"], x => x,
            ids => Task.FromResult<IList<string>>(ids.ToList()));
        await cache.GetOrLoadMapByIdsAsync<string>(prefix, ["private-id"], x => x,
            _ => throw new InvalidOperationException());

        measurements.AssertCounts(nameof(Product), hits: 2, misses: 2);
        measurements.AssertNames(nameof(Product));
    }

    [Fact]
    public void MemoryCache_RecordsLookupResultsIncludingCachedNull()
    {
        using var measurements = new Measurements();
        using var cache = GetPlatformMemoryCache();
        Assert.False(cache.TryGetValue("missing", out _));
        cache.Set("null", (object)null);
        Assert.True(cache.TryGetValue("null", out _));
        measurements.AssertCounts(nameof(PlatformMemoryCache), hits: 1, misses: 1);
    }

    [Fact]
    public async Task ExclusiveHelpers_CountPhysicalLookupsIncludingLockRecheck()
    {
        using var measurements = new Measurements();
        using var cache = GetPlatformMemoryCache();
        cache.GetOrCreateExclusive("sync", _ => 1);
        cache.GetOrCreateExclusive<int>("sync", _ => throw new InvalidOperationException());
        await cache.GetOrCreateExclusiveAsync("async", _ => Task.FromResult(2));
        await cache.GetOrCreateExclusiveAsync<int>("async", _ => throw new InvalidOperationException());
        measurements.AssertCounts(nameof(PlatformMemoryCache), hits: 2, misses: 4);
    }

    [Fact]
    public void Redis_RecordsEachLookupOnceThroughBaseClass()
    {
        using var measurements = new Measurements();
        using var cache = new RedisPlatformMemoryCacheTests().GetRedisPlatformMemoryCache();
        Assert.False(cache.TryGetValue("redis", out _));
        cache.Set("redis", 1);
        Assert.True(cache.TryGetValue("redis", out _));
        measurements.AssertCounts(nameof(PlatformMemoryCache), hits: 1, misses: 1);
    }

    [Fact]
    public async Task RequestCache_ConcurrentCallsShareOneMissAndInFlightHits()
    {
        using var measurements = new Measurements();
        var cache = new RequestScopedCache();
        var completion = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var callers = 0;
        var loads = 0;
        var calls = Enumerable.Range(0, 40).Select(_ => Task.Run(() =>
        {
            var result = cache.GetOrAddAsync("key", "search", () =>
            {
                Interlocked.Increment(ref loads);
                return completion.Task;
            });
            if (Interlocked.Increment(ref callers) == 40)
            {
                entered.SetResult();
            }
            return result;
        })).ToArray();
        await entered.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        completion.SetResult(42);
        Assert.All(await Task.WhenAll(calls), value => Assert.Equal(42, value));
        Assert.Equal(1, loads);
        measurements.AssertCounts("search", hits: 39, misses: 1);
    }

    [Fact]
    public async Task RequestCache_NameDoesNotChangeIdentityAndFaultIsAHit()
    {
        using var measurements = new Measurements();
        IRequestScopedCache cache = new RequestScopedCache();
        await Assert.ThrowsAsync<InvalidOperationException>(() => cache.GetOrAddAsync<int>("key", () => throw new InvalidOperationException()));
        await Assert.ThrowsAsync<InvalidOperationException>(() => cache.GetOrAddAsync("key", "named", () => Task.FromResult(42)));
        measurements.AssertCounts(nameof(RequestScopedCache), hits: 0, misses: 1);
        measurements.AssertCounts("named", hits: 1, misses: 0);
    }

    [Fact]
    public async Task RequestCache_ByIdsCountsReservationsDuplicatesAndNegativeHits()
    {
        using var measurements = new Measurements();
        var cache = new RequestScopedCache();
        await cache.GetOrLoadMapByIdsAsync<string>("products", ["a", "A", "b", null, ""], x => x, _ => Task.FromResult<IList<string>>(["a"]));
        await cache.GetOrLoadMapByIdsAsync<string>("products", ["a", "b"], x => x, _ => throw new InvalidOperationException());
        measurements.AssertCounts("products", hits: 3, misses: 2);
    }

    [Fact]
    public async Task RequestCache_OverlappingInFlightBatchesOnlyMissForReservationWinners()
    {
        using var measurements = new Measurements();
        var cache = new RequestScopedCache();
        var completion = new TaskCompletionSource<IList<string>>(TaskCreationOptions.RunContinuationsAsynchronously);
        var first = cache.GetOrLoadMapByIdsAsync("products", new[] { "a", "b" }, x => x, _ => completion.Task);
        var second = cache.GetOrLoadMapByIdsAsync<string>("products", ["b", "c"], x => x, ids => Task.FromResult<IList<string>>(ids.ToList()));
        completion.SetResult(["a", "b"]);
        await Task.WhenAll(first, second);
        measurements.AssertCounts("products", hits: 1, misses: 3);
    }

    [Fact]
    public async Task NamedOverload_DefaultInterfaceImplementationPreservesExistingImplementers()
    {
        IRequestScopedCache cache = new LegacyCache();
        Assert.Equal(42, await cache.GetOrAddAsync("key", "name", () => Task.FromResult(42)));
    }

    private sealed class LegacyCache : IRequestScopedCache
    {
        public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory) => factory();

        public Task<IDictionary<string, T>> GetOrLoadMapByIdsAsync<T>(string keyPrefix, ICollection<string> ids,
            Func<T, string> idSelector, Func<ICollection<string>, Task<IList<T>>> loadMissing) where T : class
        {
            throw new NotSupportedException();
        }
    }

    private sealed class Measurements : IDisposable
    {
        private readonly MeterListener _listener = new();
        private readonly ConcurrentQueue<(long Value, string Name, string Result)> _values = new();

        public Measurements()
        {
            _listener.InstrumentPublished = (instrument, listener) =>
            {
                if (instrument.Meter.Name == "VirtoCommerce.Platform.Caching" && instrument.Name == "virtocommerce.cache.requests")
                {
                    listener.EnableMeasurementEvents(instrument);
                }
            };
            _listener.SetMeasurementEventCallback<long>((_, value, tags, _) =>
            {
                var dimensions = tags.ToArray().ToDictionary(x => x.Key, x => x.Value);
                Assert.Equal(2, dimensions.Count);
                _values.Enqueue((value, (string)dimensions["cache.name"], (string)dimensions["cache.request.type"]));
            });
            _listener.Start();
        }

        public void AssertCounts(string name, long hits, long misses)
        {
            Assert.Equal(hits, _values.Where(x => x.Name == name && x.Result == "hit").Sum(x => x.Value));
            Assert.Equal(misses, _values.Where(x => x.Name == name && x.Result == "miss").Sum(x => x.Value));
        }

        public void AssertNames(params string[] names)
        {
            Assert.Equal(names.OrderBy(x => x), _values.Select(x => x.Name).Distinct().OrderBy(x => x));
        }

        public void Dispose() => _listener.Dispose();
    }
}
