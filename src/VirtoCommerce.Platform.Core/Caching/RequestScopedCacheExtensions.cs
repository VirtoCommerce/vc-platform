using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class RequestScopedCacheExtensions
    {
        /// <summary>
        /// By-id form for items keyed by <see cref="IEntity.Id"/>: delegates to
        /// <see cref="IRequestScopedCache.GetOrLoadByIdsAsync{T}(string, ICollection{string}, Func{T, string}, Func{ICollection{string}, Task{IList{T}}})"/>.
        /// </summary>
        public static Task<IDictionary<string, T>> GetOrLoadByIdsAsync<T>(
            this IRequestScopedCache cache,
            string keyPrefix,
            ICollection<string> ids,
            Func<ICollection<string>, Task<IList<T>>> loadMissing)
            where T : class, IEntity
        {
            return cache.GetOrLoadByIdsAsync(keyPrefix, ids, static x => x.Id, loadMissing);
        }
    }
}
