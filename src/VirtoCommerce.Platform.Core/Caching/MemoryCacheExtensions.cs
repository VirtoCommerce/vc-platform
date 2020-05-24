using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class MemoryCacheExtensions
    {
        private static ConcurrentDictionary<string, object> _lockLookup = new ConcurrentDictionary<string, object>();
        /// <summary>
        ///  It is async thread-safe wrapper on IMemoryCache and guarantees that the cacheable delegates (cache miss) only executes once
        /// </summary>
        public static async Task<TItem> GetOrCreateExclusiveAsync<TItem>(this IMemoryCache cache, string key, Func<MemoryCacheEntryOptions, Task<TItem>> factory, bool cacheNullValue = true)
        {
            if (!cache.TryGetValue(key, out var result))
            {
                using (await AsyncLock.GetLockByKey(key).LockAsync())
                {
                    if (!cache.TryGetValue(key, out result))
                    {
                        var options = cache is IPlatformMemoryCache platformMemoryCache ? platformMemoryCache.GetDefaultCacheEntryOptions() : new MemoryCacheEntryOptions();
                        result = await factory(options);
                        if (result != null || cacheNullValue)
                        {
                            cache.Set(key, result, options);
                        }
                    }
                }
            }
            return (TItem)result;
        }

        /// <summary>
        ///  It is thread-safe wrapper on IMemoryCache and guarantees that the cacheable delegates (cache miss) only executes once
        /// </summary>
        public static TItem GetOrCreateExclusive<TItem>(this IMemoryCache cache, string key, Func<MemoryCacheEntryOptions, TItem> factory, bool cacheNullValue = true)
        {
            if (!cache.TryGetValue(key, out var result))
            {
                lock (_lockLookup.GetOrAdd(key, new object()))
                {
                    try
                    {
                        if (!cache.TryGetValue(key, out result))
                        {
                            var options = cache is IPlatformMemoryCache platformMemoryCache ? platformMemoryCache.GetDefaultCacheEntryOptions() : new MemoryCacheEntryOptions();
                            result = factory(options);
                            if (result != null || cacheNullValue)
                            {
                                cache.Set(key, result, options);
                            }
                        }
                    }
                    finally                        
                    {
                        _lockLookup.TryRemove(key, out var _);
                    }
                }
            }
            return (TItem)result;
        }


    }
}
