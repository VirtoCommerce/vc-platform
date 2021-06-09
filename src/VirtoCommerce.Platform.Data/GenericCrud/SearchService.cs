using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.GenericCrud
{
    /// <summary>
    /// Generic service to simplify search implementation.
    /// To implement the service for applied purpose, inherit your search service from this.
    /// </summary>
    /// <typeparam name="TCriteria">Search criteria type (a descendant of <see cref="SearchCriteriaBase"/>)</typeparam>
    /// <typeparam name="TResult">Search result (<see cref="GenericSearchResult<TModel>"/>)</typeparam>
    /// <typeparam name="TModel">The type of service layer model</typeparam>
    /// <typeparam name="TEntity">The type of data access layer entity (EF) </typeparam>
    public abstract class SearchService<TCriteria, TResult, TModel, TEntity> : ISearchService<TCriteria, TResult, TModel>
        where TCriteria : SearchCriteriaBase
        where TResult : GenericSearchResult<TModel>
        where TModel : Entity, ICloneable
        where TEntity : Entity, IDataEntity<TEntity, TModel>
    {
        protected readonly IPlatformMemoryCache _platformMemoryCache;
        protected readonly Func<IRepository> _repositoryFactory;
        protected readonly ICrudService<TModel> _crudService;

        /// <summary>
        /// Construct new SearchService
        /// </summary>
        /// <param name="repositoryFactory">Repository factory to get access to the data source</param>
        /// <param name="platformMemoryCache">The cache used to temporary store returned values</param>
        /// <param name="crudService">Crud service to get service-layer model instances (a descendant of <see cref="ICrudService<TModel>"/>)</param>
        protected SearchService(Func<IRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, ICrudService<TModel> crudService)
        {
            _platformMemoryCache = platformMemoryCache;
            _repositoryFactory = repositoryFactory;
            _crudService = crudService;
        }

        /// <summary>
        /// Search for model (service-layer) instances, related to specified criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual async Task<TResult> SearchAsync(TCriteria criteria)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(SearchAsync), criteria.GetCacheKey());
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                var result = AbstractTypeFactory<TResult>.TryCreateInstance();
                cacheEntry.AddExpirationToken(GenericSearchCachingRegion<TModel>.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    //Optimize performance and CPU usage
                    repository.DisableChangesTracking();

                    var sortInfos = BuildSortExpression(criteria);
                    var query = BuildQuery(repository, criteria);

                    var needExecuteCount = criteria.Take == 0;

                    if (criteria.Take > 0)
                    {
                        var ids = await query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id)
                                         .Select(x => x.Id)
                                         .Skip(criteria.Skip).Take(criteria.Take)
                                         .ToArrayAsync();

                        result.TotalCount = ids.Count();
                        // This reduces a load of a relational database by skipping count query in case of:
                        // - First page is reading (Skip is 0)
                        // - Count in reading result less than Take value.
                        if (criteria.Skip > 0 || result.TotalCount == criteria.Take)

                        {
                            needExecuteCount = true;
                        }
                        result.Results = (await _crudService.GetByIdsAsync(ids, criteria.ResponseGroup)).OrderBy(x => Array.IndexOf(ids, x.Id)).ToList();
                    }

                    if (needExecuteCount)
                    {
                        result.TotalCount = await query.CountAsync();
                    }

                    return result;
                }
            });
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
    }
}
