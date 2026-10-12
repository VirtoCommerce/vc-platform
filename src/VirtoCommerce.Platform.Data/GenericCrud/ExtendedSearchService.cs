using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Data.GenericCrud
{
    /// <summary>
    /// A <see cref="SearchService{TCriteria, TResult, TModel, TEntity}"/> that additionally implements
    /// <see cref="IExtendedSearchService{TCriteria, TResult, TModel}"/> — the optimized "search all" operations
    /// that read all matching identifiers with a single query (no per-page counting, no offset paging)
    /// and load the models by identifiers in batches of <see cref="CrudOptions.SearchAllBatchSize"/>.
    /// <para>
    /// These operations produce correct results only when the result membership is fully defined by
    /// <see cref="SearchService{TCriteria, TResult, TModel, TEntity}.BuildQuery"/>/
    /// <see cref="SearchService{TCriteria, TResult, TModel, TEntity}.GetOrderedQueryAsync"/>. Derive from this class
    /// only when that holds; if a service's <see cref="SearchService{TCriteria, TResult, TModel, TEntity}.SearchAsync"/>
    /// changes the result membership, either stay on <see cref="SearchService{TCriteria, TResult, TModel, TEntity}"/>
    /// or override the members below with an implementation consistent with the custom search.
    /// </para>
    /// </summary>
    /// <typeparam name="TCriteria">Search criteria type (a descendant of <see cref="SearchCriteriaBase"/>)</typeparam>
    /// <typeparam name="TResult">Search result (<see cref="GenericSearchResult{TModel}"/>)</typeparam>
    /// <typeparam name="TModel">The type of service layer model</typeparam>
    /// <typeparam name="TEntity">The type of data access layer entity (EF) </typeparam>
    public abstract class ExtendedSearchService<TCriteria, TResult, TModel, TEntity>
        : SearchService<TCriteria, TResult, TModel, TEntity>, IExtendedSearchService<TCriteria, TResult, TModel>
        where TCriteria : SearchCriteriaBase
        where TResult : GenericSearchResult<TModel>
        where TModel : IEntity, ICloneable
        where TEntity : class, IEntity, IDataEntity<TEntity, TModel>
    {
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly Func<IRepository> _repositoryFactory;
        private readonly CrudOptions _crudOptions;

        protected ExtendedSearchService(
            Func<IRepository> repositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            ICrudService<TModel> crudService,
            IOptions<CrudOptions> crudOptions)
            : base(repositoryFactory, platformMemoryCache, crudService, crudOptions)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _crudOptions = crudOptions.Value;
        }

        /// <inheritdoc cref="IExtendedSearchService{TCriteria, TResult, TModel}.SearchAllAsync" />
        public virtual async Task<IList<TModel>> SearchAllAsync(TCriteria criteria, bool clone = true, CancellationToken cancellationToken = default)
        {
            // Skip/Take are ignored by SearchAll*; normalize them so paging never leaks into the
            // result assembly (e.g. a ProcessSearchResultAsync override that slices by criteria.Skip/Take).
            criteria = WithoutPaging(criteria);

            var ids = await SearchAllIdsAsync(criteria, cancellationToken);

            var result = new List<TModel>();

            foreach (var batchIds in ids.Paginate(_crudOptions.SearchAllBatchSize))
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Assemble each batch through the same pipeline as a regular search page. Running
                // ProcessSearchResultAsync per batch (rather than once over the whole set) keeps any
                // result-dependent enrichment inside it bounded to SearchAllBatchSize items.
                var batchResult = await CreateSearchResultAsync(batchIds, ids.Count, criteria, clone);

                result.AddRange(batchResult.Results);
            }

            return result;
        }

        /// <inheritdoc cref="IExtendedSearchService{TCriteria, TResult, TModel}.SearchAllIdsAsync" />
        public virtual Task<IList<string>> SearchAllIdsAsync(TCriteria criteria, CancellationToken cancellationToken = default)
        {
            criteria = WithoutPaging(criteria);
            ValidateSearchCriteria(criteria);

            return GetOrCreateCachedAsync(nameof(SearchAllIdsAsync), criteria,
                ct => SearchAllIdsNoCacheAsync(criteria, ct), cancellationToken);
        }

        /// <inheritdoc cref="IExtendedSearchService{TCriteria, TResult, TModel}.CountAllAsync" />
        public virtual Task<int> CountAllAsync(TCriteria criteria, CancellationToken cancellationToken = default)
        {
            criteria = WithoutPaging(criteria);
            ValidateSearchCriteria(criteria);

            return GetOrCreateCachedAsync(nameof(CountAllAsync), criteria, async ct =>
            {
                using var repository = _repositoryFactory();
                return await BuildQuery(repository, criteria).CountAsync(ct);
            }, cancellationToken);
        }

        /// <inheritdoc cref="IExtendedSearchService{TCriteria, TResult, TModel}.ExistsAsync" />
        public virtual Task<bool> ExistsAsync(TCriteria criteria, CancellationToken cancellationToken = default)
        {
            criteria = WithoutPaging(criteria);
            ValidateSearchCriteria(criteria);

            return GetOrCreateCachedAsync(nameof(ExistsAsync), criteria, async ct =>
            {
                using var repository = _repositoryFactory();
                return await BuildQuery(repository, criteria).AnyAsync(ct);
            }, cancellationToken);
        }

        protected virtual async Task<IList<string>> SearchAllIdsNoCacheAsync(TCriteria criteria, CancellationToken cancellationToken)
        {
            using var repository = _repositoryFactory();

            var query = BuildQuery(repository, criteria);
            var orderedQuery = await GetOrderedQueryAsync(query, criteria);

            return await orderedQuery
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Returns a copy of the criteria with paging reset (Skip = Take = 0). SearchAll* operations
        /// return every matching record, so paging must not affect their results or cache keys.
        /// The original instance is returned unchanged when it already carries no paging.
        /// </summary>
        private static TCriteria WithoutPaging(TCriteria criteria)
        {
            if (criteria.Skip == 0 && criteria.Take == 0)
            {
                return criteria;
            }

            criteria = criteria.CloneTyped();
            criteria.Skip = 0;
            criteria.Take = 0;

            return criteria;
        }

        /// <summary>
        /// Wraps a query result in the shared exclusive-cache scaffolding used by the SearchAll* operations
        /// (cache key from the operation name and criteria, plus the standard expiration token).
        /// </summary>
        private Task<T> GetOrCreateCachedAsync<T>(string operationName, TCriteria criteria, Func<CancellationToken, Task<T>> factory, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKey.With(GetType(), operationName, criteria.GetCacheKey());

            return _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, cacheOptions =>
            {
                cacheOptions.AddExpirationToken(CreateCacheToken(criteria));
                return factory(cancellationToken);
            });
        }
    }
}
