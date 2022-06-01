using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class MemoryCacheExtensions
    {
        private static readonly StringComparer _ignoreCase = StringComparer.OrdinalIgnoreCase;
        private static readonly ConcurrentDictionary<string, object> _lockLookup = new ConcurrentDictionary<string, object>();

        public static async Task<IList<TItem>> GetOrLoadByIdsAsync<TItem>(
            this IMemoryCache memoryCache,
            string keyPrefix,
            IList<string> ids,
            Func<IList<string>, Task<IEnumerable<TItem>>> loadItems,
            Action<MemoryCacheEntryOptions, string, TItem> configureCache)
            where TItem : IEntity
        {
            ids = ids
                ?.Where(id => !string.IsNullOrEmpty(id))
                .Distinct(_ignoreCase)
                .ToArray()
                ?? Array.Empty<string>();

            if (!TryGetByIds<TItem>(memoryCache, keyPrefix, ids, out var result))
            {
                using (await AsyncLock.GetLockByKey(keyPrefix).GetReleaserAsync())
                {
                    if (!TryGetByIds(memoryCache, keyPrefix, ids, out result))
                    {
                        var missingIds = ids
                            .Except(result.Keys)
                            .ToList();

                        var items = await loadItems(missingIds);

                        var itemsByIds = items
                            .Where(x => x != null)
                            .ToDictionary(x => x.Id, _ignoreCase);

                        foreach (var id in missingIds)
                        {
                            var cacheKey = CacheKey.With(keyPrefix, id);

                            result[id] = memoryCache.GetOrCreateExclusive(cacheKey, options =>
                            {
                                var item = itemsByIds.GetValueSafe(id);
                                configureCache(options, id, item);
                                return item;
                            });
                        }
                    }
                }
            }

            return result.Values
                .Where(x => x != null)
                .ToList();
        }

        public static bool TryGetByIds<TItem>(this IMemoryCache memoryCache, string keyPrefix, IList<string> ids, out IDictionary<string, TItem> result)
        {
            result = new Dictionary<string, TItem>(_ignoreCase);

            foreach (var id in ids)
            {
                var key = CacheKey.With(keyPrefix, id);

                if (memoryCache.TryGetValue(key, out var itemFromCache))
                {
                    result[id] = (TItem)itemFromCache;
                }
            }

            return result.Keys.Count == ids.Count;
        }

        public static void RemoveByIds(this IMemoryCache memoryCache, string keyPrefix, IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var cacheKey = CacheKey.With(keyPrefix, id);
                memoryCache.Remove(cacheKey);
            }
        }

        /// <summary>
        ///  It is async thread-safe wrapper on IMemoryCache and guarantees that the cacheable delegates (cache miss) only executes once
        /// </summary>
        public static async Task<TItem> GetOrCreateExclusiveAsync<TItem>(this IMemoryCache cache, string key, Func<MemoryCacheEntryOptions, Task<TItem>> factory, bool cacheNullValue = true)
        {
            if (!cache.TryGetValue(key, out var result))
            {
                using (await AsyncLock.GetLockByKey(key).GetReleaserAsync())
                {
                    if (!cache.TryGetValue(key, out result))
                    {
                        var options = cache is IPlatformMemoryCache platformMemoryCache ? platformMemoryCache.GetDefaultCacheEntryOptions() : new MemoryCacheEntryOptions();
                        result = await factory(options);
                        if (!CacheDisabler.CacheDisabled && (result != null || cacheNullValue))
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
                            if (!CacheDisabler.CacheDisabled && (result != null || cacheNullValue))
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
