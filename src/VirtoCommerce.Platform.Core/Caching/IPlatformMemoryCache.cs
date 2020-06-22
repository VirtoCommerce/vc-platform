using System;
using Microsoft.Extensions.Caching.Memory;

namespace VirtoCommerce.Platform.Core.Caching
{
    public interface IPlatformMemoryCache : IMemoryCache
    {
        MemoryCacheEntryOptions GetDefaultCacheEntryOptions();

        public bool CacheEnabled { get; set; }

        public TimeSpan? AbsoluteExpiration { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }
    }
}
