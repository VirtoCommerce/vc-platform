using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    [Trait("Category", "Unit")]
    [Collection(nameof(NotThreadSafeCollection))]
    public class MemoryCacheExtensionTests : MemoryCacheTestsBase
    {
        // Cache keys with different casing to test case insensitivity
        private readonly string[] _cacheKeys =
        [
            "Abcdefghij",
            "aBcdefghij",
            "abCdefghij",
            "abcDefghij",
            "abcdEfghij",
            "abcdeFghij",
            "abcdefGhij",
            "abcdefgHij",
            "abcdefghIj",
            "abcdefghiJ",
        ];

        [Fact]
        public void GetOrCreateExclusive()
        {
            var sut = GetPlatformMemoryCache();
            var counter = 0;
            Parallel.ForEach(Enumerable.Range(0, _cacheKeys.Length), i =>
            {
                var item = sut.GetOrCreateExclusive(_cacheKeys[i], cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(10);
                    return Interlocked.Increment(ref counter);
                });
                Assert.Equal(1, item);
            });
        }

        [Fact]
        public Task GetOrCreateExclusiveAsync()
        {
            var sut = GetPlatformMemoryCache();
            var counter = 0;
            var tasks = new List<Task>();
            for (var threadNumber = 0; threadNumber < _cacheKeys.Length; threadNumber++)
            {
                var i = threadNumber;
                var task = Task.Run(async () =>
                {
                    var item = await sut.GetOrCreateExclusiveAsync(_cacheKeys[i], cacheEntry =>
                    {
                        cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(10);
                        return Task.FromResult(Interlocked.Increment(ref counter));
                    });
                    Assert.Equal(1, item);
                }, TestContext.Current.CancellationToken);
                tasks.Add(task);
            }
            return Task.WhenAll(tasks);
        }

        [Fact]
        public void Named_AsyncLock_Exclusive_Access_For_One_Thread()
        {
            var sut = GetPlatformMemoryCache();
            var counter = 0;
            Parallel.ForEach(Enumerable.Range(1, 10), async i =>
            {
                var releaser = await AsyncLock.GetLockByKey((i % 2).ToString()).LockAsync();
                sut.GetOrCreate($@"test-key {i % 2}", cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(10);
                    return Interlocked.Increment(ref counter);
                });
                releaser.Dispose();
            });
            Assert.Equal(2, counter);
        }

        [Fact]
        public async Task GetOrLoadByIdsAsync_WithIdSelector_CachesPerIdKeyedByNonEntityField_AndLoadsOnlyMissing()
        {
            var sut = GetPlatformMemoryCache();
            const string keyPrefix = "settings-like";
            var loadCalls = new List<string[]>();

            Task<IList<TestCacheItem>> LoadItems(IList<string> ids)
            {
                loadCalls.Add(ids.ToArray());
                IList<TestCacheItem> items = ids.Select(id => new TestCacheItem(id)).ToList();
                return Task.FromResult(items);
            }

            static void ConfigureCache(MemoryCacheEntryOptions options, string id, TestCacheItem item)
            {
                options.SlidingExpiration = TimeSpan.FromSeconds(10);
            }

            // First (cold) call: both ids come from the batch loader, keyed by the non-IEntity Name field.
            var first = await sut.GetOrLoadByIdsAsync(keyPrefix, ["Alpha", "Beta"], x => x.Name, LoadItems, ConfigureCache);

            Assert.Equal(["Alpha", "Beta"], first.Select(x => x.Name).OrderBy(x => x));
            Assert.Single(loadCalls);
            Assert.Equal(["Alpha", "Beta"], loadCalls[0].OrderBy(x => x));

            // Second call: "Alpha" is served warm from its own per-id entry; only "Gamma" hits the loader.
            var second = await sut.GetOrLoadByIdsAsync(keyPrefix, ["Alpha", "Gamma"], x => x.Name, LoadItems, ConfigureCache);

            Assert.Equal(["Alpha", "Gamma"], second.Select(x => x.Name).OrderBy(x => x));
            Assert.Equal(2, loadCalls.Count);
            Assert.Equal(["Gamma"], loadCalls[1]);
        }

        [Fact]
        public async Task GetOrLoadByIds_TryGetByIds_RemoveByIds_RoundTrip_IsCaseInsensitive()
        {
            var sut = GetPlatformMemoryCache();
            const string keyPrefix = "MixedCase-Prefix";

            static Task<IList<TestCacheItem>> LoadItems(IList<string> ids)
            {
                IList<TestCacheItem> items = ids.Select(id => new TestCacheItem(id)).ToList();
                return Task.FromResult(items);
            }

            static void ConfigureCache(MemoryCacheEntryOptions options, string id, TestCacheItem item)
            {
                options.SlidingExpiration = TimeSpan.FromSeconds(10);
            }

            await sut.GetOrLoadByIdsAsync(keyPrefix, ["Alpha", "Beta"], x => x.Name, LoadItems, ConfigureCache);

            // A differently-cased prefix and ids resolve to the same stored entries.
            var hit = sut.TryGetByIds<TestCacheItem>("mixedcase-prefix", ["alpha", "beta"], out var found);

            Assert.True(hit);
            Assert.Equal(["Alpha", "Beta"], found.Values.Select(x => x.Name).OrderBy(x => x));

            // RemoveByIds builds raw keys, but the cache normalizes them, so the normalized entries still evict.
            sut.RemoveByIds(keyPrefix, ["Alpha", "Beta"]);

            Assert.False(sut.TryGetByIds<TestCacheItem>(keyPrefix, ["Alpha", "Beta"], out _));
        }

        [Fact]
        public void DefaultCachingOptions_Are_Applied()
        {
            var defaultOptions = Options.Create(new CachingOptions() { CacheSlidingExpiration = TimeSpan.FromMilliseconds(10) });
            var logger = new Moq.Mock<ILogger<PlatformMemoryCache>>();
            var sut = new PlatformMemoryCache(CreateCache(), defaultOptions, logger.Object);

            sut.GetOrCreateExclusive("test-key", cacheOptions =>
            {
                Assert.Equal(cacheOptions.SlidingExpiration, TimeSpan.FromMilliseconds(10));
                return 1;
            });
            Thread.Sleep(100);
            var result = sut.GetOrCreateExclusive("test-key", cacheOptions =>
            {
                return 2;
            });
            Assert.Equal(2, result);
        }

        private sealed record TestCacheItem(string Name);
    }
}
