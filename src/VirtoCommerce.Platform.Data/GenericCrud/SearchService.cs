using System;
using System.Collections.Generic;
using System.Linq;
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
    /// </summary>
    /// <typeparam name="TCriteria">Search criteria type (a descendant of <see cref="SearchCriteriaBase"/>)</typeparam>
    /// <typeparam name="TResult">Search result (<see cref="GenericSearchResult{TModel}"/>)</typeparam>
    /// <typeparam name="TModel">The type of service layer model</typeparam>
    /// <typeparam name="TEntity">The type of data access layer entity (EF) </typeparam>
    public abstract class SearchService<TCriteria, TResult, TModel, TEntity> : ISearchService<TCriteria, TResult, TModel>
        where TCriteria : SearchCriteriaBase
        where TResult : GenericSearchResult<TModel>
        where TModel : Entity, ICloneable
        where TEntity : Entity, IDataEntity<TEntity, TModel>
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

            var result = AbstractTypeFactory<TResult>.TryCreateInstance();
            result.TotalCount = idsResult.TotalCount;

            if (idsResult.Results.Any())
            {
                var models = await _crudService.GetAsync(idsResult.Results, criteria.ResponseGroup, clone);
                result.Results.AddRange(models.OrderBy(x => idsResult.Results.IndexOf(x.Id)));
            }

            return await ProcessSearchResultAsync(result, criteria);
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
                result.Results = await query
                    .OrderBySortInfos(BuildSortExpression(criteria))
                    .ThenBy(x => x.Id)
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
