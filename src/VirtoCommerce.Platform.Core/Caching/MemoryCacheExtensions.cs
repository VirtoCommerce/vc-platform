using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class MemoryCacheExtensions
    {
        private static readonly StringComparer _ignoreCase = StringComparer.OrdinalIgnoreCase;
        private static readonly ConcurrentDictionary<string, object> _lockLookup = new();

        // The four overloads below are deliberately explicit rather than one method with an optional
        // `createExpirationToken`. C# bakes optional arguments in at the CALL SITE, so turning a published
        // signature into an optional-parameter one removes the arity every already-compiled caller emitted
        // a reference to. Platform assemblies ship as NuGet and modules load as plugins without being
        // rebuilt in lockstep, so that would surface as a runtime MissingMethodException in modules that
        // still build and test clean.
        public static Task<IList<TItem>> GetOrLoadByIdsAsync<TItem>(
            this IMemoryCache memoryCache,
            string keyPrefix,
            IList<string> ids,
            Func<IList<string>, Task<IList<TItem>>> loadItems,
            Action<MemoryCacheEntryOptions, string, TItem> configureCache)
            where TItem : IEntity
        {
            return memoryCache.GetOrLoadByIdsCoreAsync(keyPrefix, ids, x => x.Id, loadItems, configureCache, createExpirationToken: null);
        }

        public static Task<IList<TItem>> GetOrLoadByIdsAsync<TItem>(
            this IMemoryCache memoryCache,
            string keyPrefix,
            IList<string> ids,
            Func<IList<string>, Task<IList<TItem>>> loadItems,
            Action<MemoryCacheEntryOptions, string, TItem> configureCache,
            Func<string, IChangeToken> createExpirationToken)
            where TItem : IEntity
        {
            return memoryCache.GetOrLoadByIdsCoreAsync(keyPrefix, ids, x => x.Id, loadItems, configureCache, createExpirationToken);
        }

        public static Task<IList<TItem>> GetOrLoadByIdsAsync<TItem>(
            this IMemoryCache memoryCache,
            string keyPrefix,
            IList<string> ids,
            Func<TItem, string> idSelector,
            Func<IList<string>, Task<IList<TItem>>> loadItems,
            Action<MemoryCacheEntryOptions, string, TItem> configureCache)
            where TItem : class
        {
            ArgumentNullException.ThrowIfNull(idSelector);

            return memoryCache.GetOrLoadByIdsCoreAsync(keyPrefix, ids, idSelector, loadItems, configureCache, createExpirationToken: null);
        }

        public static Task<IList<TItem>> GetOrLoadByIdsAsync<TItem>(
            this IMemoryCache memoryCache,
            string keyPrefix,
            IList<string> ids,
            Func<TItem, string> idSelector,
            Func<IList<string>, Task<IList<TItem>>> loadItems,
            Action<MemoryCacheEntryOptions, string, TItem> configureCache,
            Func<string, IChangeToken> createExpirationToken)
            where TItem : class
        {
            ArgumentNullException.ThrowIfNull(idSelector);

            return memoryCache.GetOrLoadByIdsCoreAsync(keyPrefix, ids, idSelector, loadItems, configureCache, createExpirationToken);
        }

        private static async Task<IList<TItem>> GetOrLoadByIdsCoreAsync<TItem>(
            this IMemoryCache memoryCache,
            string keyPrefix,
            IList<string> ids,
            Func<TItem, string> idSelector,
            Func<IList<string>, Task<IList<TItem>>> loadItems,
            Action<MemoryCacheEntryOptions, string, TItem> configureCache,
            Func<string, IChangeToken> createExpirationToken)
        {
            ids = DistinctNonEmpty(ids);

            var normalizedPrefix = CacheKey.Normalize(keyPrefix);

            var hits = new List<TItem>(ids.Count);
            var allCached = true;

            foreach (var id in ids)
            {
                if (memoryCache.TryGetValue(CacheKey.With(normalizedPrefix, CacheKey.Normalize(id)), out var cached))
                {
                    if (cached is not null)
                    {
                        hits.Add((TItem)cached);
                    }
                }
                else
                {
                    allCached = false;
                    break;
                }
            }

            if (allCached)
            {
                return hits;
            }

            IDictionary<string, TItem> result;

            using (await AsyncLock.GetLockByKey(normalizedPrefix).LockAsync())
            {
                if (!TryGetByIds(memoryCache, keyPrefix, ids, out result))
                {
                    var missingIds = ids
                        .Except(result.Keys)
                        .ToList();

                    // Capture the invalidation token BEFORE the load runs, not after. A writer's
                    // commit+invalidation can land while `loadItems` is in flight; if the token were
                    // minted afterwards (in `configureCache`, as before this fix), the invalidation that
                    // made the just-loaded value stale would already be gone from
                    // CancellableCacheRegion<T>'s key-token dictionary (InnerExpireTokenForKey removes it
                    // on cancel), so the freshly minted token would come back live/uncancelled and the
                    // stale value would be cached and served until the sliding TTL expires.
                    var preCapturedTokens = createExpirationToken is null
                        ? null
                        : missingIds.ToDictionary(x => x, createExpirationToken, _ignoreCase);

                    var items = await loadItems(missingIds) ?? Array.Empty<TItem>();

                    var itemsByIds = items
                        .Where(x => x != null)
                        .ToDictionary(idSelector, _ignoreCase);

                    foreach (var id in missingIds)
                    {
                        var cacheKey = CacheKey.With(normalizedPrefix, CacheKey.Normalize(id));

                        result[id] = memoryCache.GetOrCreateExclusive(cacheKey, options =>
                        {
                            var item = itemsByIds.GetValueSafe(id);

                            if (preCapturedTokens is not null)
                            {
                                options.AddExpirationToken(preCapturedTokens[id]);
                            }

                            configureCache(options, id, item);

                            return item;
                        });
                    }
                }
            }

            return result.Values
                .Where(x => x != null)
                .ToList();
        }

        [SuppressMessage("Major Code Smell", "S3267:Loops should be simplified using the \"Where\" LINQ method",
            Justification = "Perf-critical cache path: the explicit loop avoids the Where iterator and delegate allocation this method exists to eliminate.")]
        private static IList<string> DistinctNonEmpty(IList<string> ids)
        {
            if (ids is null || ids.Count == 0)
            {
                return Array.Empty<string>();
            }

            var distinct = new HashSet<string>(ids.Count, _ignoreCase);

            foreach (var id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    distinct.Add(id);
                }
            }

            return distinct.ToList();
        }

        public static bool TryGetByIds<TItem>(this IMemoryCache memoryCache, string keyPrefix, IList<string> ids, out IDictionary<string, TItem> result)
        {
            result = new Dictionary<string, TItem>(ids.Count, _ignoreCase);

            var normalizedPrefix = CacheKey.Normalize(keyPrefix);

            foreach (var id in ids)
            {
                var key = CacheKey.With(normalizedPrefix, CacheKey.Normalize(id));

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
            key = CacheKey.Normalize(key);
            if (!cache.TryGetValue(key, out var result))
            {
                using (await AsyncLock.GetLockByKey(key).LockAsync())
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
            key = CacheKey.Normalize(key);
            if (!cache.TryGetValue(key, out var result))
            {
                lock (_lockLookup.GetOrAdd(key, static _ => new object()))
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
