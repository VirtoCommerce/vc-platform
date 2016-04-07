using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.SearchModule.Data.Services;
using VirtoCommerce.SearchModule.Web.Converters;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.SearchModule.Web.Services
{
    public class ItemBrowsingService : IItemBrowsingService
    {
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly IItemService _itemService;
        private readonly ISearchConnection _searchConnection;
        private readonly ISearchProvider _searchProvider;

        public ItemBrowsingService(IItemService itemService, ISearchProvider searchService, IBlobUrlResolver blobUrlResolver = null, ISearchConnection searchConnection = null)
        {
            _searchProvider = searchService;
            _searchConnection = searchConnection;
            _itemService = itemService;
            _blobUrlResolver = blobUrlResolver;
        }

        public moduleModel.SearchResult SearchItems(CatalogIndexedSearchCriteria criteria, moduleModel.ItemResponseGroup responseGroup)
        {
            CatalogItemSearchResults results;
            var items = Search(criteria, out results, responseGroup);

            var response = new moduleModel.SearchResult();

            response.Products.AddRange(items);
            response.ProductsTotalCount = results.TotalCount;

            // TODO need better way to find applied filter values
            var appliedFilters = criteria.CurrentFilters.SelectMany(x => x.GetValues()).Select(x => x.Id).ToArray();
            if (results.FacetGroups != null)
            {
                response.Aggregations = results.FacetGroups.Select(g => g.ToModuleModel(appliedFilters)).ToArray();
            }
            return response;
        }



        private IEnumerable<moduleModel.CatalogProduct> Search(CatalogIndexedSearchCriteria criteria, out CatalogItemSearchResults results, moduleModel.ItemResponseGroup responseGroup)
        {
            var items = new List<moduleModel.CatalogProduct>();
            var itemsOrderedList = new List<string>();

            var foundItemCount = 0;
            var dbItemCount = 0;
            var searchRetry = 0;

            //var myCriteria = criteria.Clone();
            var myCriteria = criteria;

            do
            {
                // Search using criteria, it will only return IDs of the items
                var scope = _searchConnection.Scope;
                var searchResults = _searchProvider.Search(scope, criteria) as SearchResults;
                var itemKeyValues = searchResults.GetKeyAndOutlineFieldValueMap<string>();
                results = new CatalogItemSearchResults(myCriteria, itemKeyValues, searchResults);

                searchRetry++;

                if (results.Items == null)
                {
                    continue;
                }

                //Get only new found itemIds
                var uniqueKeys = results.Items.Keys.Except(itemsOrderedList).ToArray();
                foundItemCount = uniqueKeys.Length;

                if (!results.Items.Any())
                {
                    continue;
                }

                itemsOrderedList.AddRange(uniqueKeys);

                // Now load items from repository
                var currentItems = _itemService.GetByIds(uniqueKeys.ToArray(), responseGroup, criteria.Catalog);

                var orderedList = currentItems.OrderBy(i => itemsOrderedList.IndexOf(i.Id));
                items.AddRange(orderedList);
                dbItemCount = currentItems.Length;

                //If some items where removed and search is out of sync try getting extra items
                if (foundItemCount > dbItemCount)
                {
                    //Retrieve more items to fill missing gap
                    myCriteria.RecordsToRetrieve += (foundItemCount - dbItemCount);
                }
            }
            while (foundItemCount > dbItemCount && results.Items.Any() && searchRetry <= 3 &&
                (myCriteria.RecordsToRetrieve + myCriteria.StartingRecord) < results.TotalCount);

            return items;
        }


        private string StripCatalogFromOutline(string outline, string catalog)
        {
            if (String.IsNullOrEmpty(outline))
            {
                return null;
            }

            if (outline.Length > catalog.Length + 1)
            {
                return outline.Substring(catalog.Length + 1);
            }

            return String.Empty;
        }


    }
}
