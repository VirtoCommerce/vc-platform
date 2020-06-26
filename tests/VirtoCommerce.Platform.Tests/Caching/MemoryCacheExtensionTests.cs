using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Caching
{
    [Trait("Category", "Unit")]
    public class MemoryCacheExtensionTests: MemoryCacheTestsBase
    {

        [Fact]
        public void GetOrCreateExclusive()
        {
            var sut = CreateCache();
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
            var sut = CreateCache();
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
            var sut = CreateCache();
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

    }
}
