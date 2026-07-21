using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Caching
{
    /// <summary>
    /// Request-scoped cache for expensive loads: deduplicates a load that is re-issued with identical
    /// arguments many times within one request. Concurrent same-key calls share a single factory invocation.
    /// </summary>
    /// <remarks>
    /// Use this when many distinct items issue the same load (identical, or only a few distinct, argument
    /// combinations) and a per-node batching layer above it cannot dedup them: such a layer keys by its own
    /// node id, not by the load's arguments, so a load whose arguments repeat across differently-keyed nodes
    /// still fans out. This cache dedups by the load's own stable argument key instead.
    /// <br/><br/>
    /// Key rules: the key must include EVERY argument that affects the load's result (a missing one causes false
    /// cache hits), multi-value parts of the key must be sorted for order independence, and the key must carry a
    /// type/purpose-discriminating prefix so unrelated callers sharing the same scoped instance don't collide.
    /// <br/><br/>
    /// Failure semantics: a faulted load is cached for the remainder of the request and is not retried - every
    /// same-key call rethrows the original exception.
    /// <br/><br/>
    /// Warning: resolve consumers from the request/DI scope (a scoped dependency, or the current request's
    /// service provider) - never constructor-inject this cache into a singleton, or it is captured against the
    /// root scope and silently shared across all requests.
    /// </remarks>
    public interface IRequestScopedCache
    {
        /// <summary>
        /// Returns the result of a previous (possibly still in-flight) load registered under <paramref name="key"/>,
        /// or invokes <paramref name="factory"/> and caches its task for the remainder of the request.
        /// </summary>
        /// <typeparam name="T">Result type; every caller of the same key must use the same <typeparamref name="T"/>.</typeparam>
        /// <param name="key">Stable, order-independent key built from every load-affecting argument.</param>
        /// <param name="factory">The load to execute on a cache miss; runs at most once per key per request.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        /// <exception cref="InvalidCastException">The key was previously used with a different <typeparamref name="T"/>.</exception>
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory);

        /// <summary>
        /// Batch by-id variant: serves ids already loaded during this request from the cache and loads only the
        /// not-yet-cached ids in a single <paramref name="loadMissing"/> call, caching each item under its own
        /// (<paramref name="keyPrefix"/>, id) key - across overlapping id-sets, including concurrent callers,
        /// every id is loaded at most once per request.
        /// </summary>
        /// <remarks>
        /// Ids are matched case-insensitively (OrdinalIgnoreCase), matching the platform's id-cache convention
        /// (a load may return an entity whose <c>Id</c> casing differs from the requested id under a
        /// case-insensitive database collation).
        /// <br/><br/>
        /// An id the load does not return is negatively cached: omitted from results for the rest of the request,
        /// not retried. Loaded items with unrequested ids are ignored; duplicate ids - first wins. The returned
        /// dictionary is created per call and may be mutated; the item instances in it are shared within the
        /// request - treat them as read-only. <paramref name="loadMissing"/> must not call back into this cache
        /// for ids it was asked to load (it would await its own load).
        /// </remarks>
        /// <typeparam name="T">Item type, a reference type so a not-found id is representable as a cached null;
        /// every caller of the same (<paramref name="keyPrefix"/>, id) pair must use the same <typeparamref name="T"/>.</typeparam>
        /// <param name="keyPrefix">Type/purpose-discriminating prefix, unique per load kind.</param>
        /// <param name="ids">Identifiers to resolve; null/empty entries skipped, duplicates collapsed.</param>
        /// <param name="idSelector">Extracts the cache id from a loaded item.</param>
        /// <param name="loadMissing">Batch load for the not-yet-cached ids; a null result counts as empty.</param>
        /// <returns>Found ids mapped to their items; not-found ids are omitted.</returns>
        /// <exception cref="ArgumentNullException">Any argument is null.</exception>
        /// <exception cref="InvalidCastException">A (<paramref name="keyPrefix"/>, id) pair was previously used with a different <typeparamref name="T"/>.</exception>
        Task<IDictionary<string, T>> GetOrLoadByIdsAsync<T>(
            string keyPrefix,
            ICollection<string> ids,
            Func<T, string> idSelector,
            Func<ICollection<string>, Task<IList<T>>> loadMissing)
            where T : class;
    }
}
