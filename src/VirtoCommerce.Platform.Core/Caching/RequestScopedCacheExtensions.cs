using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class RequestScopedCacheExtensions
    {
        /// <summary>
        /// Values-only, by-<see cref="IEntity.Id"/> form: returns just the resolved items (not-found ids omitted),
        /// for callers that do not need the id map.
        /// </summary>
        public static async Task<ICollection<T>> GetOrLoadByIdsAsync<T>(
            this IRequestScopedCache cache,
            string keyPrefix,
            ICollection<string> ids,
            Func<ICollection<string>, Task<IList<T>>> loadMissing)
            where T : class, IEntity
        {
            return (await GetOrLoadMapByIdsAsync(cache, keyPrefix, ids, loadMissing)).Values;
        }

        /// <summary>
        /// Values-only form of <see cref="IRequestScopedCache.GetOrLoadMapByIdsAsync{T}(string, ICollection{string}, Func{T, string}, Func{ICollection{string}, Task{IList{T}}})"/>:
        /// returns just the resolved items (not-found ids omitted), for callers that do not need the id map.
        /// </summary>
        public static async Task<ICollection<T>> GetOrLoadByIdsAsync<T>(
            this IRequestScopedCache cache,
            string keyPrefix,
            ICollection<string> ids,
            Func<T, string> idSelector,
            Func<ICollection<string>, Task<IList<T>>> loadMissing)
            where T : class
        {
            return (await cache.GetOrLoadMapByIdsAsync(keyPrefix, ids, idSelector, loadMissing)).Values;
        }

        /// <summary>
        /// By-id form for items keyed by <see cref="IEntity.Id"/>: delegates to
        /// <see cref="IRequestScopedCache.GetOrLoadMapByIdsAsync{T}(string, ICollection{string}, Func{T, string}, Func{ICollection{string}, Task{IList{T}}})"/>.
        /// </summary>
        public static Task<IDictionary<string, T>> GetOrLoadMapByIdsAsync<T>(
            this IRequestScopedCache cache,
            string keyPrefix,
            ICollection<string> ids,
            Func<ICollection<string>, Task<IList<T>>> loadMissing)
            where T : class, IEntity
        {
            return cache.GetOrLoadMapByIdsAsync(keyPrefix, ids, static x => x.Id, loadMissing);
        }
    }
}
