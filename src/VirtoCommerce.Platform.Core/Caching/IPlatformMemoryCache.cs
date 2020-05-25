using Microsoft.Extensions.Caching.Memory;

namespace VirtoCommerce.Platform.Core.Caching
{
    public interface IPlatformMemoryCache : IMemoryCache
    {
        MemoryCacheEntryOptions GetDefaultCacheEntryOptions();
    }
}
