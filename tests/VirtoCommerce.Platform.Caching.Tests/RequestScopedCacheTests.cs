using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests;

public class RequestScopedCacheTests
{
    [Fact]
    public async Task GetOrAddAsync_SameKey_FactoryExecutedOnce()
    {
        var sut = new RequestScopedCache();
        var callCount = 0;

        Task<string> Factory()
        {
            Interlocked.Increment(ref callCount);
            return Task.FromResult("value");
        }

        var first = await sut.GetOrAddAsync("key", Factory);
        var second = await sut.GetOrAddAsync("key", Factory);

        Assert.Equal("value", first);
        Assert.Equal("value", second);
        // The second call for the same key must be served from the cached result, not re-invoke the factory.
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task GetOrAddAsync_DifferentKeys_FactoryExecutedPerKey()
    {
        var sut = new RequestScopedCache();
        var callCount = 0;

        Task<int> Factory()
        {
            return Task.FromResult(Interlocked.Increment(ref callCount));
        }

        var first = await sut.GetOrAddAsync("key-1", Factory);
        var second = await sut.GetOrAddAsync("key-2", Factory);

        Assert.Equal(1, first);
        Assert.Equal(2, second);
        // Distinct keys must not share a cached result.
        Assert.Equal(2, callCount);
    }

    [Fact]
    public async Task GetOrAddAsync_ConcurrentSameKey_FactoryExecutedExactlyOnce()
    {
        // The single-flight guarantee: a burst of concurrent callers racing on the same key must
        // all observe the same in-flight Task rather than each starting its own factory invocation.
        var sut = new RequestScopedCache();
        var callCount = 0;
        var release = new TaskCompletionSource();

        async Task<int> Factory()
        {
            Interlocked.Increment(ref callCount);
            await release.Task;
            return 42;
        }

        const int callers = 20;
        var tasks = new Task<int>[callers];
        for (var i = 0; i < callers; i++)
        {
            // Task.Run forces the callers onto distinct pool threads so they race on GetOrAdd for real,
            // instead of entering it cooperatively from one thread.
            tasks[i] = Task.Run(() => sut.GetOrAddAsync("key", Factory));
        }

        // Give every caller a chance to reach GetOrAddAsync before releasing the factory.
        await Task.Delay(20, TestContext.Current.CancellationToken);
        release.SetResult();

        var results = await Task.WhenAll(tasks);

        // Concurrent callers on the same key must share a single in-flight factory invocation.
        Assert.Equal(1, callCount);
        Assert.All(results, x => Assert.Equal(42, x));
    }

    [Fact]
    public async Task GetOrAddAsync_DifferentResultTypesPerKey_BothResolveCorrectly()
    {
        var sut = new RequestScopedCache();

        var stringResult = await sut.GetOrAddAsync("string-key", () => Task.FromResult("text"));
        var intResult = await sut.GetOrAddAsync("int-key", () => Task.FromResult(7));

        Assert.Equal("text", stringResult);
        Assert.Equal(7, intResult);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_LoadsAllMissingIdsInOneBatch()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();

        var result = await sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b", "c"], x => x.Id, CreateLoader(batches));

        AssertSameIds(["a", "b", "c"], Assert.Single(batches));
        AssertSameIds(["a", "b", "c"], result.Keys);
        Assert.Equal("a", result["a"].Id);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_OverlappingCall_LoadsOnlyNotYetCachedIds()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();
        var loadMissing = CreateLoader(batches);

        var first = await sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], x => x.Id, loadMissing);
        var second = await sut.GetOrLoadMapByIdsAsync("prefix", ["b", "c"], x => x.Id, loadMissing);
        var third = await sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], x => x.Id, loadMissing);

        // The second call must load only the ids the first call did not cache, and the third must load nothing.
        Assert.Equal(2, batches.Count);
        AssertSameIds(["c"], batches[1]);
        AssertSameIds(["b", "c"], second.Keys);
        // An overlapping id must be served from the cache, not re-loaded.
        Assert.Same(first["b"], second["b"]);
        Assert.Same(first["a"], third["a"]);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_IdCasingDiffers_MatchesCaseInsensitively()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();
        // The loader returns the entity with a different id casing than requested (as a
        // case-insensitive DB collation would): request "ABC", stored id "abc".
        var loadMissing = CreateLoader(batches, x => new Item(x.ToLowerInvariant()));

        var first = await sut.GetOrLoadMapByIdsAsync("prefix", ["ABC"], x => x.Id, loadMissing);
        var second = await sut.GetOrLoadMapByIdsAsync("prefix", ["abc"], x => x.Id, loadMissing);

        // The loaded item publishes to the requested reservation despite the casing difference,
        // and a differently-cased repeat dedups against the cached entry instead of re-loading.
        Assert.Single(batches);
        Assert.NotNull(first["ABC"]);
        Assert.Same(first["ABC"], second["abc"]);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_NotFoundId_OmittedAndNegativelyCached()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();
        // The loader never returns an item for "ghost".
        var loadMissing = CreateLoader(batches, x => x == "ghost" ? null : new Item(x));

        var first = await sut.GetOrLoadMapByIdsAsync("prefix", ["a", "ghost"], x => x.Id, loadMissing);
        var second = await sut.GetOrLoadMapByIdsAsync("prefix", ["a", "ghost"], x => x.Id, loadMissing);

        AssertSameIds(["a"], first.Keys);
        AssertSameIds(["a"], second.Keys);
        // A not-found id must be negatively cached for the request, not re-loaded.
        Assert.Single(batches);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_NullEmptyAndDuplicateIds_NormalizedWithinCall()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();

        var result = await sut.GetOrLoadMapByIdsAsync("prefix", [null, "", "a", "a", "b"], x => x.Id, CreateLoader(batches));

        AssertSameIds(["a", "b"], Assert.Single(batches));
        AssertSameIds(["a", "b"], result.Keys);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_EmptyIds_ReturnsEmptyWithoutLoad()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();

        var result = await sut.GetOrLoadMapByIdsAsync("prefix", Array.Empty<string>(), x => x.Id, CreateLoader(batches));

        Assert.Empty(result);
        Assert.Empty(batches);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_FaultedLoad_CachedForTheRequestAndRethrown()
    {
        var sut = new RequestScopedCache();
        var callCount = 0;

        Task<IList<Item>> LoadMissing(ICollection<string> missingIds)
        {
            Interlocked.Increment(ref callCount);
            throw new InvalidOperationException("boom");
        }

        Func<Task<IDictionary<string, Item>>> firstCall = () => sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], x => x.Id, LoadMissing);
        Func<Task<IDictionary<string, Item>>> secondCall = () => sut.GetOrLoadMapByIdsAsync("prefix", ["a"], x => x.Id, LoadMissing);

        var firstEx = await Assert.ThrowsAsync<InvalidOperationException>(firstCall);
        var secondEx = await Assert.ThrowsAsync<InvalidOperationException>(secondCall);
        Assert.Equal("boom", firstEx.Message);
        Assert.Equal("boom", secondEx.Message);
        // A faulted load must be cached for the request - same-id calls rethrow without re-loading.
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_NullLoaderResult_TreatedAsEmptyAndNegativelyCached()
    {
        var sut = new RequestScopedCache();
        var callCount = 0;

        Task<IList<Item>> LoadMissing(ICollection<string> missingIds)
        {
            Interlocked.Increment(ref callCount);
            return Task.FromResult<IList<Item>>(null);
        }

        var first = await sut.GetOrLoadMapByIdsAsync("prefix", ["a"], x => x.Id, LoadMissing);
        var second = await sut.GetOrLoadMapByIdsAsync("prefix", ["a"], x => x.Id, LoadMissing);

        Assert.Empty(first);
        Assert.Empty(second);
        // Ids from a null (empty) load result are negatively cached for the request, not re-loaded.
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_InFlightLoad_SharedByLaterCallInsteadOfSecondLoad()
    {
        var sut = new RequestScopedCache();
        var release = new TaskCompletionSource();
        var callCount = 0;

        async Task<IList<Item>> LoadMissing(ICollection<string> missingIds)
        {
            Interlocked.Increment(ref callCount);
            await release.Task;
            return missingIds.Select(x => new Item(x)).ToList();
        }

        // The first call dispatches the load synchronously up to the await, then stays in flight.
        var firstTask = sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], x => x.Id, LoadMissing);
        var secondTask = sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], x => x.Id, LoadMissing);

        Assert.False(firstTask.IsCompleted);
        Assert.False(secondTask.IsCompleted);
        release.SetResult();

        var first = await firstTask;
        var second = await secondTask;

        // The second caller must await the in-flight load, not start its own.
        Assert.Equal(1, callCount);
        Assert.Same(first["a"], second["a"]);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_ConcurrentOverlappingCallers_EachIdLoadedAtMostOnce()
    {
        // Regression guard for per-caller full-batch amplification: under concurrent overlapping
        // misses, each id must be loaded by exactly one caller, so the total load across all
        // callers is the distinct union - never a caller's full missing set re-loaded.
        const int callers = 8;
        const int windowSize = 12;
        const int windowStride = 4;
        var sut = new RequestScopedCache();
        var release = new TaskCompletionSource();
        var batches = new List<string[]>();

        async Task<IList<Item>> LoadMissing(ICollection<string> missingIds)
        {
            lock (batches)
            {
                batches.Add([.. missingIds]);
            }

            await release.Task;
            return missingIds.Select(x => new Item(x)).ToList();
        }

        var tasks = new Task<IDictionary<string, Item>>[callers];
        for (var i = 0; i < callers; i++)
        {
            var callerIds = Enumerable.Range(i * windowStride, windowSize).Select(x => $"id-{x}").ToArray();
            tasks[i] = Task.Run(() => sut.GetOrLoadMapByIdsAsync("prefix", callerIds, x => x.Id, LoadMissing));
        }

        // Give every caller a chance to reserve its ids before any load completes.
        await Task.Delay(20, TestContext.Current.CancellationToken);
        release.SetResult();

        var results = await Task.WhenAll(tasks);

        var idSpace = (callers - 1) * windowStride + windowSize;
        var loadedIds = batches.SelectMany(x => x).ToList();
        // An id must never appear in two batches within one request.
        Assert.Equal(loadedIds.Count, loadedIds.Distinct().Count());
        // The union of all batches must be exactly the distinct union of requested ids.
        AssertSameIds(Enumerable.Range(0, idSpace).Select(x => $"id-{x}"), loadedIds);
        // Each caller dispatches at most one batch.
        Assert.True(batches.Count <= callers);

        for (var i = 0; i < callers; i++)
        {
            AssertSameIds(Enumerable.Range(i * windowStride, windowSize).Select(x => $"id-{x}"), results[i].Keys);
        }

        // Overlapping ids resolve to the same shared instance across callers.
        Assert.Same(results[0]["id-4"], results[1]["id-4"]);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_TupleKeys_DoNotCollideAcrossPrefixesOrWithByKeyEntries()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();
        var loadMissing = CreateLoader(batches);

        // "P" + "A:B" and "P:A" + "B" would alias under naive string concatenation.
        var first = await sut.GetOrLoadMapByIdsAsync("P", ["A:B"], x => x.Id, loadMissing);
        var second = await sut.GetOrLoadMapByIdsAsync("P:A", ["B"], x => x.Id, loadMissing);
        var byKey = await sut.GetOrAddAsync("P:A:B", () => Task.FromResult("by-key value"));

        // Entries under different prefixes must not alias.
        Assert.Equal(2, batches.Count);
        Assert.Equal("A:B", first["A:B"].Id);
        Assert.Equal("B", second["B"].Id);
        Assert.Equal("by-key value", byKey);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_ExtraAndDuplicateLoadedItems_FirstWinsAndExtrasIgnored()
    {
        var sut = new RequestScopedCache();
        var requestedFirst = new Item("a");
        var requestedDuplicate = new Item("a");
        var batches = new List<string[]>();
        var loadMissing = CreateLoader(batches);

        var result = await sut.GetOrLoadMapByIdsAsync(
            "prefix",
            ["a"],
            x => x.Id,
            _ => Task.FromResult<IList<Item>>([requestedFirst, requestedDuplicate, new Item("x")]));

        AssertSameIds(["a"], result.Keys);
        // The first loaded item for an id wins.
        Assert.Same(requestedFirst, result["a"]);

        // The unrequested extra item must not have been cached.
        var extra = await sut.GetOrLoadMapByIdsAsync("prefix", ["x"], x => x.Id, loadMissing);
        AssertSameIds(["x"], Assert.Single(batches));
        Assert.NotNull(extra["x"]);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_SamePrefixAndIdWithDifferentType_ThrowsInvalidCast()
    {
        var sut = new RequestScopedCache();

        await sut.GetOrLoadMapByIdsAsync("prefix", ["a"], x => x.Id, CreateLoader([]));

        Func<Task<IDictionary<string, OtherItem>>> act = () => sut.GetOrLoadMapByIdsAsync<OtherItem>("prefix", ["a"], x => x.Id, _ => Task.FromResult<IList<OtherItem>>([]));

        await Assert.ThrowsAsync<InvalidCastException>(act);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_TypeMismatchOnOneId_LeavesInnocentOwnedIdsRetryable()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();

        // Cache "b" under a different type first, so a later Item-typed request for it throws on the cast.
        await sut.GetOrLoadMapByIdsAsync("prefix", ["b"], x => x.Id, _ => Task.FromResult<IList<OtherItem>>([new OtherItem("b")]));

        // This call owns "a" (processed first), then hits the type mismatch on "b" and must throw.
        Func<Task<IDictionary<string, Item>>> mismatch = () => sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], x => x.Id, CreateLoader(batches));
        await Assert.ThrowsAsync<InvalidCastException>(mismatch);

        // The innocent "a" must NOT be negatively cached by "b"'s type conflict: a later correctly-typed
        // call re-reserves and loads it successfully.
        var recovered = await sut.GetOrLoadMapByIdsAsync("prefix", ["a"], x => x.Id, CreateLoader(batches));
        Assert.Equal("a", recovered["a"].Id);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_InvalidArguments_Throw()
    {
        var sut = new RequestScopedCache();
        var loadMissing = CreateLoader([]);

        Func<Task<IDictionary<string, Item>>> emptyPrefix = () => sut.GetOrLoadMapByIdsAsync("", ["a"], x => x.Id, loadMissing);
        Func<Task<IDictionary<string, Item>>> nullIds = () => sut.GetOrLoadMapByIdsAsync("prefix", null, x => x.Id, loadMissing);
        Func<Task<IDictionary<string, Item>>> nullSelector = () => sut.GetOrLoadMapByIdsAsync<Item>("prefix", ["a"], null, loadMissing);
        Func<Task<IDictionary<string, Item>>> nullLoader = () => sut.GetOrLoadMapByIdsAsync<Item>("prefix", ["a"], x => x.Id, null);

        await Assert.ThrowsAsync<ArgumentException>(emptyPrefix);
        await Assert.ThrowsAsync<ArgumentNullException>(nullIds);
        await Assert.ThrowsAsync<ArgumentNullException>(nullSelector);
        await Assert.ThrowsAsync<ArgumentNullException>(nullLoader);
    }

    [Fact]
    public async Task GetOrLoadMapByIdsAsync_EntityOverload_KeysByEntityIdAndSharesEntries()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();

        Task<IList<EntityItem>> LoadMissing(ICollection<string> missingIds)
        {
            lock (batches)
            {
                batches.Add([.. missingIds]);
            }

            return Task.FromResult<IList<EntityItem>>(missingIds.Select(x => new EntityItem { Id = x }).ToList());
        }

        var viaEntityOverload = await sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], LoadMissing);
        var viaCoreOverload = await sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], x => x.Id, LoadMissing);

        // The IEntity overload must delegate to the core overload and share its cache entries.
        Assert.Single(batches);
        AssertSameIds(["a", "b"], viaEntityOverload.Keys);
        Assert.Same(viaEntityOverload["a"], viaCoreOverload["a"]);
    }

    [Fact]
    public async Task GetOrLoadByIdsAsync_ReturnsResolvedItemValuesSharingTheMapCache()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();

        var values = await sut.GetOrLoadByIdsAsync(
            "prefix",
            ["a", "b", "ghost"],
            x => x.Id,
            CreateLoader(batches, x => x == "ghost" ? null : new Item(x)));

        // The values-only overload returns the resolved items, with not-found ids omitted.
        AssertSameIds(["a", "b"], values.Select(x => x.Id));

        // It shares the underlying cache with the map overload: the same ids resolve without a reload,
        // to the same instances.
        var map = await sut.GetOrLoadMapByIdsAsync("prefix", ["a", "b"], x => x.Id, CreateLoader(batches));
        Assert.Single(batches);
        Assert.Same(map["a"], values.Single(x => x.Id == "a"));
    }

    [Fact]
    public async Task GetOrLoadByIdsAsync_EntityOverload_ReturnsResolvedItemValues()
    {
        var sut = new RequestScopedCache();
        var batches = new List<string[]>();

        Task<IList<EntityItem>> LoadMissing(ICollection<string> missingIds)
        {
            lock (batches)
            {
                batches.Add([.. missingIds]);
            }

            return Task.FromResult<IList<EntityItem>>(missingIds.Select(x => new EntityItem { Id = x }).ToList());
        }

        var values = await sut.GetOrLoadByIdsAsync("prefix", ["a", "b"], LoadMissing);

        // The IEntity values-only overload keys by entity Id and returns just the items.
        AssertSameIds(["a", "b"], values.Select(x => x.Id));
        Assert.Single(batches);
    }

    private static void AssertSameIds(IEnumerable<string> expected, IEnumerable<string> actual)
    {
        Assert.Equal(
            expected.OrderBy(x => x, StringComparer.Ordinal),
            actual.OrderBy(x => x, StringComparer.Ordinal));
    }

    private static Func<ICollection<string>, Task<IList<Item>>> CreateLoader(
        List<string[]> batches,
        Func<string, Item> createItem = null)
    {
        createItem ??= x => new Item(x);

        return missingIds =>
        {
            lock (batches)
            {
                batches.Add([.. missingIds]);
            }

            var items = missingIds.Select(createItem).Where(x => x is not null).ToList();

            return Task.FromResult<IList<Item>>(items);
        };
    }

    private sealed record Item(string Id);

    private sealed record OtherItem(string Id);

    private sealed class EntityItem : Entity
    {
    }
}
