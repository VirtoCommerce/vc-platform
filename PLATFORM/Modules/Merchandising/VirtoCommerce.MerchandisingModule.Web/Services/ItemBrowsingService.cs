using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Services
{
    public class ItemBrowsingService : IItemBrowsingService
    {
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly IItemService _itemService;
        private readonly ISearchConnection _searchConnection;
        private readonly ISearchProvider _searchProvider;

        public ItemBrowsingService(IItemService itemService, ISearchProvider searchService, CacheManager cacheManager, IBlobUrlResolver blobUrlResolver = null, ISearchConnection searchConnection = null)
        {
            _searchProvider = searchService;
            _searchConnection = searchConnection;
            _itemService = itemService;
            _blobUrlResolver = blobUrlResolver;
        }

        public ProductSearchResult SearchItems(CatalogIndexedSearchCriteria criteria, moduleModel.ItemResponseGroup responseGroup = moduleModel.ItemResponseGroup.ItemSmall)
        {
            CatalogItemSearchResults results;
            var items = Search(criteria, out results, responseGroup);
            var catalogItems = new List<Product>();

            // go through items
            foreach (var item in items)
            {
                var searchTags = results.Items[item.Id.ToLower()];

                var currentOutline = this.GetItemOutlineUsingContext(
                    searchTags[criteria.OutlineField].ToString(),
                    criteria.Catalog);
                var catalogItem = item.ToWebModel(_blobUrlResolver) as Product;
                catalogItem.Outline = this.StripCatalogFromOutline(currentOutline, criteria.Catalog);

                int reviewTotal;
                if (searchTags.ContainsKey(criteria.ReviewsTotalField)
                    && int.TryParse(searchTags[criteria.ReviewsTotalField].ToString(), out reviewTotal))
                {
                    catalogItem.ReviewsTotal = reviewTotal;
                }
                double reviewAvg;
                if (searchTags.ContainsKey(criteria.ReviewsAverageField)
                    && double.TryParse(searchTags[criteria.ReviewsAverageField].ToString(), out reviewAvg))
                {
                    catalogItem.Rating = reviewAvg;
                }

                catalogItems.Add(catalogItem);
            }

            var response = new ProductSearchResult();

            response.Items.AddRange(catalogItems);
            response.TotalCount = results.TotalCount;

            //TODO need better way to find applied filter values
            var appliedFilters = criteria.CurrentFilters.SelectMany(x => x.GetValues()).Select(x => x.Id).ToArray();
            response.Facets = results.FacetGroups.Select(g => g.ToWebModel(appliedFilters)).ToArray();
            return response;
        }



        /// <summary>
        ///     Gets the context item outline based on what customer is browsing
        /// </summary>
        /// <param name="itemOutline"></param>
        /// <returns></returns>
        private string GetItemOutlineUsingContext(string itemOutline, string prefixOutline)
        {
            if (String.IsNullOrEmpty(itemOutline))
            {
                return String.Empty;
            }

            var outline = itemOutline.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith(prefixOutline, StringComparison.OrdinalIgnoreCase)) ?? string.Empty;

            return outline;
        }

        private IEnumerable<moduleModel.CatalogProduct> Search(CatalogIndexedSearchCriteria criteria, out CatalogItemSearchResults results, moduleModel.ItemResponseGroup responseGroup = moduleModel.ItemResponseGroup.ItemSmall)
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
                var currentItems = _itemService.GetByIds(uniqueKeys.ToArray(), responseGroup);

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
