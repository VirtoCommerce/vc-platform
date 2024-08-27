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
        public static Task<IList<TModel>> SearchAllNoCloneAsync<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : IEntity
        {
            return searchService.SearchAllAsync(searchCriteria, clone: false);
        }

        public static async Task<IList<TModel>> SearchAllAsync<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria, bool clone = true)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : IEntity
        {
            var result = new List<TModel>();

            await foreach (var searchResult in searchService.SearchBatchesAsync(searchCriteria, clone))
            {
                result.AddRange(searchResult.Results);
            }

            return result;
        }

        /// <summary>
        /// Returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.
        /// </summary>
        public static Task<TResult> SearchNoCloneAsync<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : IEntity
        {
            return searchService.SearchAsync(searchCriteria, clone: false);
        }

        /// <summary>
        /// Returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.
        /// </summary>
        public static IAsyncEnumerable<TResult> SearchBatchesNoCloneAsync<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : IEntity
        {
            return searchService.SearchBatchesAsync(searchCriteria, clone: false);
        }

        public static async IAsyncEnumerable<TResult> SearchBatchesAsync<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria, bool clone = true)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : IEntity
        {
            int totalCount;
            searchCriteria = searchCriteria.CloneTyped();

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
