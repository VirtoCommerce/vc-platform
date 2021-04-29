using System;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Caching;

namespace VirtoCommerce.Platform.Web.Tests.Cache
{
    public partial class PlatformWebMockHelper
    {
        public static class MemoryCacheMockHelper
        {
            public static IMemoryCache CreateCache()
            {
                return CreateCache(new SystemClock());
            }

            public static IMemoryCache CreateCache(ISystemClock clock)
            {
                return new MemoryCache(new MemoryCacheOptions()
                {
                    Clock = clock,
                    ExpirationScanFrequency = TimeSpan.FromSeconds(1)
                });
            }

            public static PlatformMemoryCache GetPlatformMemoryCache()
            {
                var logMock = new Mock<ILogger<PlatformMemoryCache>>();

                var cachingOptions = new OptionsWrapper<CachingOptions>(new CachingOptions
                {
                    CacheEnabled = true
                });

                return new PlatformMemoryCache(CreateCache(), cachingOptions, logMock.Object);
            }
        }
    }
}
