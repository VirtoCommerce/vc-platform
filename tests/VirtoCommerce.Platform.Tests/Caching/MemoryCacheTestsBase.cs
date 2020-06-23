using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Caching;

namespace VirtoCommerce.Platform.Tests.Caching
{
    public class MemoryCacheTestsBase
    {
        private readonly Mock<ILogger<PlatformMemoryCache>> _logMock;

        public Mock<IOptions<CachingOptions>> CachingOptionsMock { get; }

        public MemoryCacheTestsBase()
        {
            CachingOptionsMock = new Mock<IOptions<CachingOptions>>();
            CachingOptionsMock.Setup(x => x.Value).Returns(new CachingOptions { CacheEnabled = true });
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
            });
        }

        public PlatformMemoryCache GetPlatformMemoryCache()
        {
            return new PlatformMemoryCache(CreateCache(), CachingOptionsMock.Object, _logMock.Object);
        }
    }
}
