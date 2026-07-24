using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Caching;

namespace VirtoCommerce.Platform.Tests.Common
{
    public static class MemoryCacheMockHelper
    {
        public static IMemoryCache CreateCache()
        {
            return CreateCache(new SystemClock());
        }

        public static IMemoryCache CreateCache(ISystemClock clock)
        {
            return new MemoryCache(new MemoryCacheOptions
            {
                Clock = clock,
                ExpirationScanFrequency = TimeSpan.FromSeconds(1),
            });
        }

        public static PlatformMemoryCache GetPlatformMemoryCache()
        {
            return GetPlatformMemoryCache(new CachingOptions { CacheEnabled = true });
        }

        public static PlatformMemoryCache GetPlatformMemoryCache(CachingOptions options)
        {
            return new PlatformMemoryCache(CreateCache(), Options.Create(options), new Mock<ILogger<PlatformMemoryCache>>().Object);
        }
    }
}
