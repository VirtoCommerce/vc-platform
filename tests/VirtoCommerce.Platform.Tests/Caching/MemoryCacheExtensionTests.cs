using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.MemoryCacheExtensionTests
{
    [Trait("Category", "Unit")]
    public class MemoryCacheExtensionTests
    {
        private static IMemoryCache BuildCache()
        {
            return new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public void GetOrCreateExclusive()
        {
            var sut = BuildCache();
            int counter = 0;
            Parallel.ForEach(Enumerable.Range(1, 10), i =>
            {
                var item = sut.GetOrCreateExclusive("test-key", cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(10);
                    return Interlocked.Increment(ref counter);
                });
                Assert.Equal(1, item);
            });
        }

        [Fact]
        public void GetOrCreateExclusiveAsync()
        {
            var sut = BuildCache();
            int counter = 0;
            Parallel.ForEach(Enumerable.Range(1, 10), async i =>
            {
                var item = await sut.GetOrCreateExclusiveAsync("test-key", cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(10);
                    return Task.FromResult(Interlocked.Increment(ref counter));
                });
                Assert.Equal(1, item);
            });
        }

        [Fact]
        public void Named_AsyncLock_Exclusive_Access_For_One_Thread()
        {
            var sut = BuildCache();
            int counter = 0;
            Parallel.ForEach(Enumerable.Range(1, 10), async i =>
            {
                var releaser = await AsyncLock.GetLockByKey((i % 2).ToString()).LockAsync();
                sut.GetOrCreate("test-key", cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(10);
                    return Interlocked.Increment(ref counter);
                });
                releaser.Dispose();
            });
            Assert.Equal(2, counter);
        }

    }
}
