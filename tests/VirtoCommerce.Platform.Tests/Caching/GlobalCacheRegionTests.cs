using System;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Caching
{
    [Trait("Category", "Unit")]
    public class GlobalCacheRegionTests: MemoryCacheTestsBase
    {
        private const string CacheKey = "test";

        [Fact]
        public void CacheEntry_Has_Global_Expiration_Token()
        {
            var platformMemoryCache = GetPlatformMemoryCache();
            var entry = platformMemoryCache.CreateEntry("test");
            Assert.Single(entry.ExpirationTokens);
        }

        [Fact]
        public void Global_Cache_Region_Expired_Successfully()
        {
            var platformMemoryCache = GetPlatformMemoryCache();
            var firstValue = GetSampleValueWithCache(platformMemoryCache);
            var secondValue = GetSampleValueWithCache(platformMemoryCache);
            GlobalCacheRegion.ExpireRegion();
            var thirdValue = GetSampleValueWithCache(platformMemoryCache);
            Assert.Equal(firstValue, secondValue);
            Assert.NotEqual(firstValue, thirdValue);
        }

        private DateTime GetSampleValueWithCache(IPlatformMemoryCache platformMemoryCache)
        {
            return platformMemoryCache.GetOrCreateExclusive(CacheKey, x => DateTime.Now);
        }
    }
}
