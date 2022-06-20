using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.GenericCrud
{
    public static class SearchServiceExtensions
    {
        public static async IAsyncEnumerable<TResult> SearchBatches<TCriteria, TResult, TModel>(this ISearchService<TCriteria, TResult, TModel> searchService, TCriteria searchCriteria)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : Entity, ICloneable
        {
            int totalCount;

            do
            {
                var searchResult = await searchService.SearchAsync(searchCriteria);

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
