using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Data.GenericCrud
{
    /// <summary>
    /// Generic service to simplify search implementation.
    /// To implement the service for applied purpose, inherit your search service from this.
    /// <para>
    /// <see cref="SearchAllAsync"/> reads the identifiers of all matching data with a single query
    /// (no count query, no offset paging) and then loads the models by identifiers in batches
    /// of <see cref="CrudOptions.SearchAllBatchSize"/>. Compared to Skip/Take paging via
    /// <see cref="SearchAsync"/>, this avoids re-evaluating the search query and counting the records
    /// for every batch, and makes the returned data immune to page drift caused by concurrent modifications.
    /// A derived class that overrides <see cref="SearchAsync"/> in a way that changes the result membership
    /// must also override <see cref="SearchAllAsync"/> and <see cref="CountAllAsync"/> to keep them consistent.
    /// </para>
    /// </summary>
    /// <typeparam name="TCriteria">Search criteria type (a descendant of <see cref="SearchCriteriaBase"/>)</typeparam>
    /// <typeparam name="TResult">Search result (<see cref="GenericSearchResult{TModel}"/>)</typeparam>
    /// <typeparam name="TModel">The type of service layer model</typeparam>
    /// <typeparam name="TEntity">The type of data access layer entity (EF) </typeparam>
    public abstract class SearchService<TCriteria, TResult, TModel, TEntity> : IExtendedSearchService<TCriteria, TResult, TModel>
        where TCriteria : SearchCriteriaBase
        where TResult : GenericSearchResult<TModel>
        where TModel : IEntity, ICloneable
        where TEntity : class, IEntity, IDataEntity<TEntity, TModel>
    {
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly Func<IRepository> _repositoryFactory;
        private readonly ICrudService<TModel> _crudService;
        private readonly CrudOptions _crudOptions;

        /// <summary>
        /// Construct new SearchService
        /// </summary>
        /// <param name="repositoryFactory">Repository factory to get access to the data source</param>
        /// <param name="platformMemoryCache">The cache used to temporary store returned values</param>
        /// <param name="crudService">Crud service to get service-layer model instances (a descendant of <see cref="ICrudService{TModel}"/>)</param>
        /// <param name="crudOptions"></param>
        protected SearchService(
            Func<IRepository> repositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            ICrudService<TModel> crudService,
            IOptions<CrudOptions> crudOptions)
        {
            _platformMemoryCache = platformMemoryCache;
            _repositoryFactory = repositoryFactory;
            _crudService = crudService;
            _crudOptions = crudOptions.Value;
        }

        /// <summary>
        /// Returns model instances that meet specified criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="clone">If false, returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.</param>
        /// <returns></returns>
        public virtual async Task<TResult> SearchAsync(TCriteria criteria, bool clone = true)
        {
            ValidateSearchCriteria(criteria);

            var cacheKey = CacheKey.With(GetType(), nameof(SearchAsync), criteria.GetCacheKey());

            var idsResult = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheOptions =>
            {
                cacheOptions.AddExpirationToken(CreateCacheToken(criteria));
                return await SearchIdsNoCacheAsync(criteria);
            });

            return await CreateSearchResultAsync(idsResult.Results, idsResult.TotalCount, criteria, clone);
        }

        /// <summary>
        /// Builds a <typeparamref name="TResult"/> for the given ordered identifiers: loads the models,
        /// restores the identifier order (<see cref="ICrudService{TModel}.GetAsync"/> returns them in cache order),
        /// and runs <see cref="ProcessSearchResultAsync"/>. Shared by <see cref="SearchAsync"/> (one page) and
        /// <see cref="SearchAllAsync"/> (one batch) so both assemble and post-process results identically.
        /// </summary>
        protected virtual async Task<TResult> CreateSearchResultAsync(IList<string> ids, int totalCount, TCriteria criteria, bool clone)
        {
            var result = AbstractTypeFactory<TResult>.TryCreateInstance();
            result.TotalCount = totalCount;

            if (ids.Count > 0)
            {
                var models = await _crudService.GetAsync(ids, criteria.ResponseGroup, clone);
                result.Results.AddRange(models.OrderBy(x => ids.IndexOf(x.Id)));
            }

            return await ProcessSearchResultAsync(result, criteria);
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

        protected virtual void ValidateSearchCriteria(TCriteria criteria)
        {
            var resultWindow = criteria.Skip + criteria.Take;

            if (resultWindow > _crudOptions.MaxResultWindow)
            {
                throw new PlatformException($"Results window {resultWindow} exceeds maximum allowed value {_crudOptions.MaxResultWindow}");
            }
        }

        protected virtual IChangeToken CreateCacheToken(TCriteria criteria)
        {
            return GenericSearchCachingRegion<TModel>.CreateChangeToken();
        }

        protected virtual async Task<GenericSearchResult<string>> SearchIdsNoCacheAsync(TCriteria criteria)
        {
            var result = AbstractTypeFactory<GenericSearchResult<string>>.TryCreateInstance();
            result.Results = Array.Empty<string>();

            using var repository = _repositoryFactory();
            var query = BuildQuery(repository, criteria);
            var needExecuteCount = criteria.Take == 0;

            if (criteria.Take > 0)
            {
                var orderedQuery = await GetOrderedQueryAsync(query, criteria);

                result.Results = await orderedQuery
                    .Select(x => x.Id)
                    .Skip(criteria.Skip)
                    .Take(criteria.Take)
                    .ToListAsync();

                result.TotalCount = result.Results.Count;

                // This reduces a load of a relational database by skipping count query in case of:
                // - First page is reading (Skip is 0)
                // - Count in reading result less than Take value.
                if (criteria.Skip > 0 || result.TotalCount == criteria.Take)
                {
                    needExecuteCount = true;
                }
            }

            if (needExecuteCount)
            {
                result.TotalCount = await query.CountAsync();
            }

            return result;
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

        protected virtual Task<IOrderedQueryable<TEntity>> GetOrderedQueryAsync(IQueryable<TEntity> query, TCriteria criteria)
        {
            var orderedQuery = query
                    .OrderBySortInfos(BuildSortExpression(criteria))
                    .ThenBy(x => x.Id);

            return Task.FromResult(orderedQuery);
        }

        /// <summary>
        /// Custom search service must override this method to implement search criteria to query transformation for repository call
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected abstract IQueryable<TEntity> BuildQuery(IRepository repository, TCriteria criteria);

        /// <summary>
        /// Build sort expression. Override to add custom sorting.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected virtual IList<SortInfo> BuildSortExpression(TCriteria criteria)
        {
            return criteria.SortInfos;
        }

        /// <summary>
        /// Post-read processing of the search result instance.
        /// A good place to make some additional actions, tune result data.
        /// Override to add some search result data changes, calculations, etc...
        /// </summary>
        /// <param name="result"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected virtual Task<TResult> ProcessSearchResultAsync(TResult result, TCriteria criteria)
        {
            return Task.FromResult(result);
        }
    }
}
