using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class SearchServiceExtensions
    {
        /// <summary>
        /// Returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.
        /// </summary>
        public static Task<TResult> SearchNoCloneAsync<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : Entity, ICloneable
        {
            return searchService.SearchAsync(searchCriteria, clone: false);
        }

        public static IAsyncEnumerable<TResult> SearchBatchesNoClone<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : Entity, ICloneable
        {
            return searchService.SearchBatches(searchCriteria, clone: false);
        }

        public static async IAsyncEnumerable<TResult> SearchBatches<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria, bool clone = true)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : Entity, ICloneable
        {
            int totalCount;

            do
            {
                var searchResult = await searchService.SearchAsync(searchCriteria, clone);

                if (searchCriteria.Take == 0 ||
                    searchResult.Results.Any())
                {
                    yield return searchResult;
                }

                if (searchCriteria.Take == 0)
                {
                    yield break;
                }

                totalCount = searchResult.TotalCount;
                searchCriteria.Skip += searchCriteria.Take;
            }
            while (searchCriteria.Skip < totalCount);
        }
    }
}
