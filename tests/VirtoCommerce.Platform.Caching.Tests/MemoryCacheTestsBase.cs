using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace VirtoCommerce.Platform.Caching.Tests
{
    public class MemoryCacheTestsBase
    {
        private readonly Mock<ILogger<PlatformMemoryCache>> _logMock;

        public IOptions<CachingOptions> CachingOptions => new OptionsWrapper<CachingOptions>(new CachingOptions { CacheEnabled = true });

        public MemoryCacheTestsBase()
        {
            _logMock = new Mock<ILogger<PlatformMemoryCache>>();
        }

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

        public PlatformMemoryCache GetPlatformMemoryCache()
        {
            return new PlatformMemoryCache(CreateCache(), CachingOptions, _logMock.Object);
        }
    }
}
