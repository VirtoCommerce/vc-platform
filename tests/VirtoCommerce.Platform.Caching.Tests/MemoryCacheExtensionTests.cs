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

        [Fact]
        public async Task GetOrLoadByIdsAsync_DoesNotCacheMissingIds()
        {
            // arrange
            var sut = GetPlatformMemoryCache();
            var loadCallCount = 0;
            var ids = new[] { "missing-id" };

            Task<IList<TestEntity>> LoadItems(IList<string> missingIds)
            {
                loadCallCount++;
                return Task.FromResult<IList<TestEntity>>(Array.Empty<TestEntity>());
            }

            // act — first call: miss, loadItems returns nothing, null must NOT be cached
            var first = await sut.GetOrLoadByIdsAsync(nameof(GetOrLoadByIdsAsync_DoesNotCacheMissingIds), ids, LoadItems, ConfigureCache);

            // act — second call with same id: must hit loadItems again, not a cached null
            var second = await sut.GetOrLoadByIdsAsync(nameof(GetOrLoadByIdsAsync_DoesNotCacheMissingIds), ids, LoadItems, ConfigureCache);

            // assert
            Assert.Empty(first);
            Assert.Empty(second);
            Assert.Equal(2, loadCallCount);
        }

        [Fact]
        public async Task GetOrLoadByIdsAsync_CachesPresentIds()
        {
            // arrange
            var sut = GetPlatformMemoryCache();
            var loadCallCount = 0;
            var ids = new[] { "present-id" };

            Task<IList<TestEntity>> LoadItems(IList<string> missingIds)
            {
                loadCallCount++;
                return Task.FromResult<IList<TestEntity>>([new TestEntity { Id = "present-id" }]);
            }

            // act
            var first = await sut.GetOrLoadByIdsAsync(nameof(GetOrLoadByIdsAsync_CachesPresentIds), ids, LoadItems, ConfigureCache);
            var second = await sut.GetOrLoadByIdsAsync(nameof(GetOrLoadByIdsAsync_CachesPresentIds), ids, LoadItems, ConfigureCache);

            // assert
            Assert.Single(first);
            Assert.Single(second);
            Assert.Equal(1, loadCallCount);
        }

        private static void ConfigureCache(MemoryCacheEntryOptions options, string id, TestEntity item)
        {
            options.SlidingExpiration = TimeSpan.FromMinutes(1);
        }

        private sealed class TestEntity : IEntity
        {
            public string Id { get; set; }
        }
    }
}
