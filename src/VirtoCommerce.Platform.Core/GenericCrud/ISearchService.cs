using System;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.GenericCrud
{
    /// <summary>
    /// Generic interface to use with search services
    /// </summary>
    /// <typeparam name="TCriteria"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public interface ISearchService<TCriteria, TResult, TModel>
        where TCriteria : SearchCriteriaBase
        where TResult : GenericSearchResult<TModel>
        where TModel : IEntity
    {
        /// <summary>
        /// Returns model instances that meet specified criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="clone">If false, returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.</param>
        /// <returns></returns>
        Task<TResult> SearchAsync(TCriteria criteria, bool clone = true);

        /// <summary>
        /// Returns model instances that meet specified criteria without using cache.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<TResult> SearchNoCacheAsync(TCriteria criteria)
        {
            throw new NotSupportedException("Underlying service does not support no cache search.");
        }
    }
}
