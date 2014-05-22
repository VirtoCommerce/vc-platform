using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.PlatformTools;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.Web.Client.Caching;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Controllers
{
    using System.Data.Entity.Core.Metadata.Edm;

    using VirtoCommerce.Web.Client.Extensions.Filters;
    using VirtoCommerce.Web.Client.Services.Filters;

    /// <summary>
    /// Class SearchController.
    /// </summary>
    public class SearchController : ControllerBase
    {
        /// <summary>
        /// The _catalog client
        /// </summary>
        private readonly CatalogClient _catalogClient;
        /// <summary>
        /// The _marketing
        /// </summary>
        private readonly MarketingHelper _marketing;
        /// <summary>
        /// The _price list client
        /// </summary>
        private readonly PriceListClient _priceListClient;
        /// <summary>
        /// The _store client
        /// </summary>
        private readonly StoreClient _storeClient;

        private readonly ISearchFilterService _searchFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController" /> class.
        /// </summary>
        /// <param name="marketing">The marketing.</param>
        /// <param name="priceListClient">The price list client.</param>
        /// <param name="storeClient">The store client.</param>
        /// <param name="catalogClient">The catalog client.</param>
        /// <param name="searchFilter">The search filter.</param>
        public SearchController(MarketingHelper marketing, PriceListClient priceListClient, StoreClient storeClient,
                                CatalogClient catalogClient, ISearchFilterService searchFilter)
        {
            _marketing = marketing;
            _priceListClient = priceListClient;
            _storeClient = storeClient;
            _catalogClient = catalogClient;
            _searchFilter = searchFilter;
        }

        /// <summary>
        /// Searches by keywords.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "SearchCache", VaryByCustom = "storeparam")]
        public ActionResult Index(SearchParameters parameters)
        {

            Logger.Info("New search started: " + parameters.FreeSearch);
            ViewBag.Title = String.Format("Searching by '{0}'", parameters.FreeSearch);

            var criteria = new CatalogItemSearchCriteria
            {
                SearchPhrase = parameters.FreeSearch,
                IsFuzzySearch = true,
                Catalog = UserHelper.CustomerSession.CatalogId
            };

            RestoreSearchPreferences(parameters);
            var results = SearchResults(criteria, parameters);
            return View(results);
        }

        /// <summary>
        /// Searches within category.
        /// </summary>
        /// <param name="cat">The category.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="name">Partial view name</param>
        /// <param name="criteria">Search criteria</param>
        /// <param name="savePreferences"></param>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly]
        public ActionResult SearchResultsWithinCategory(CategoryModel cat, SearchParameters parameters, string name = "Index", CatalogItemSearchCriteria criteria = null, bool savePreferences = true)
        {
            criteria = criteria ?? new CatalogItemSearchCriteria();
            if (cat != null)
            {
                ViewBag.Title = cat.DisplayName;
                criteria.Outlines.Add(String.Format("{0}*", _catalogClient.BuildCategoryOutline(UserHelper.CustomerSession.CatalogId, cat.Category)));
            }

            if (savePreferences)
            {
                RestoreSearchPreferences(parameters);
            }

            var results = SearchResults(criteria, parameters);
            return PartialView(name, results);
        }

        /// <summary>
        /// Finds the specified term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "SearchCache", VaryByCustom = "storeparam")]
        public ActionResult Find(string term)
        {
            Logger.Info("New search started: " + term);
            ViewBag.Title = String.Format("Searching by '{0}'".Localize(), term);

            var parameters = new SearchParameters { PageSize = 15 };
            var criteria = new CatalogItemSearchCriteria { SearchPhrase = term.EscapeSearchTerm(), IsFuzzySearch = true };
            var results = SearchResults(criteria, parameters);

            var data = from i in results.CatalogItems
                       select new { url = Url.ItemUrl(i.CatalogItem.Item, i.CatalogItem.ParentItemId), value = i.DisplayName };
            return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Prices(Dictionary<string, string> itemAndOutine)
        {
            var session = UserHelper.CustomerSession;

            var itemIdArray = itemAndOutine.Keys.ToArray();
            var prices = _priceListClient.GetLowestPrices(session.Pricelists, itemIdArray, 1);

            var retVal = new List<PriceModel>();

            if (prices != null && prices.Any())
            {
                var currentItems = _catalogClient.GetItems(itemIdArray);
                foreach (var item in currentItems)
                {
                    var outline = itemAndOutine[item.ItemId];
                    if (string.IsNullOrEmpty(outline))
                    {

                        outline = CatalogHelper.OutlineBuilder.BuildCategoryOutline(session.CatalogId, item.ItemId).ToString();
                    }

                    var lowestPrice =
                   (from p in prices
                    where p.ItemId.Equals(item.ItemId, StringComparison.OrdinalIgnoreCase)
                    select p).SingleOrDefault();
                    if (lowestPrice != null)
                    {
                        var tags = new Hashtable
							{
								{
									"Outline",
									outline
								}
							};
                        var priceModel = _marketing.GetItemPriceModel(item, lowestPrice, tags);
                        retVal.Add(priceModel);
                    }
                }
            }

            //return Json(retVal.ToArray(), JsonRequestBehavior.AllowGet);
            return PartialView(retVal);
        }

        #region Private Helpers

        /// <summary>
        /// Searches by given parameters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>CatalogItemSearchModel.</returns>
        private CatalogItemSearchModel SearchResults(CatalogItemSearchCriteria criteria, SearchParameters parameters)
        {
            var pageNumber = parameters.PageIndex;
            var pageSize = parameters.PageSize;
            criteria.Locale = UserHelper.CustomerSession.Language;
            criteria.Catalog = UserHelper.CustomerSession.CatalogId;
            criteria.RecordsToRetrieve = pageSize;
            criteria.StartingRecord = (pageNumber - 1) * pageSize;
            criteria.Pricelists = UserHelper.CustomerSession.Pricelists;

            return GetModelFromCriteria(criteria, parameters);
        }

        private List<Item> Search(CatalogItemSearchCriteria criteria, bool cacheResults, out CatalogItemSearchResults results)
        {
            var items = new List<Item>();
            var itemsOrderedList = new List<string>();

            int foundItemCount;
            int dbItemCount = 0;
            int searchRetry = 0;

            var myCriteria = criteria.Clone();

            do
            {
                // Search using criteria, it will only return IDs of the items
                results = _catalogClient.SearchItems(myCriteria, cacheResults);
                searchRetry++;

                //Get only new found itemIds
                var uniqueKeys = results.Items.Keys.Except(itemsOrderedList).ToArray();
                foundItemCount = uniqueKeys.Length;

                if (!results.Items.Any()) continue;

                itemsOrderedList.AddRange(uniqueKeys);
                // Now load items from repository
                var currentItems = _catalogClient.GetItems(uniqueKeys.ToArray(), cacheResults,
                                                           ItemResponseGroups.ItemAssets |
                                                           ItemResponseGroups.ItemProperties | ItemResponseGroups.ItemEditorialReviews);

                items.AddRange(currentItems.OrderBy(i => itemsOrderedList.IndexOf(i.ItemId)));
                dbItemCount = currentItems.Length;

                //If some items where removed and search is out of sync try getting extra items
                if (foundItemCount > dbItemCount)
                {
                    //Retrieve more items to fill missing gap
                    myCriteria.RecordsToRetrieve += (foundItemCount - dbItemCount);
                }
            } while (foundItemCount > dbItemCount && results.Items.Any() && searchRetry <= 3 &&
                (myCriteria.RecordsToRetrieve + myCriteria.StartingRecord) < results.TotalCount);

            return items;
        }

        /// <summary>
        /// Restores the search preferences from cookies.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        private void RestoreSearchPreferences(SearchParameters parameters)
        {
            var pageSize = parameters.PageSize;
            var sort = parameters.Sort;
            var sortOrder = parameters.SortOrder;

            if (pageSize == 0)
            {
                Int32.TryParse(StoreHelper.GetCookieValue("pagesizecookie"), out pageSize);
            }
            else
            {
                StoreHelper.SetCookie(
                    "pagesizecookie", pageSize.ToString(CultureInfo.InvariantCulture), DateTime.Now.AddMonths(1));
            }

            if (pageSize == 0)
            {
                pageSize = SearchParameters.DefaultPageSize;
            }

            parameters.PageSize = pageSize;

            if (String.IsNullOrEmpty(sort))
            {
                sort = StoreHelper.GetCookieValue("sortcookie");
            }
            else
            {
                StoreHelper.SetCookie("sortcookie", sort, DateTime.Now.AddMonths(1));
            }

            if (String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = StoreHelper.GetCookieValue("sortordercookie");
            }
            else
            {
                StoreHelper.SetCookie("sortordercookie", sortOrder, DateTime.Now.AddMonths(1));
            }

            parameters.Sort = sort;
            parameters.SortOrder = sortOrder;
        }


        /// <summary>
        /// Gets the model from criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>CatalogItemSearchModel.</returns>
        private CatalogItemSearchModel GetModelFromCriteria(CatalogItemSearchCriteria criteria,
                                                            SearchParameters parameters)
        {
            criteria.Currency = UserHelper.CustomerSession.Currency;

            var dataSource = CreateDataModel(criteria, parameters, true);
            return dataSource;
        }

        /// <summary>
        /// Creates the data model.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cacheResults">if set to <c>true</c> [cache results].</param>
        /// <returns>CatalogItemSearchModel.</returns>
        private CatalogItemSearchModel CreateDataModel(CatalogItemSearchCriteria criteria, SearchParameters parameters,
                                                       bool cacheResults)
        {
            var session = UserHelper.CustomerSession;

            // Create a model
            var dataSource = new CatalogItemSearchModel();

            // Now fill in filters
            var searchHelper = _searchFilter;

            var filters = searchHelper.Filters;

            // Add all filters
            foreach (var filter in filters)
            {
                // Check if we already filtering by the key and value
                /*
                var facet = (from f in parameters.Facets
                    where f.Key.Equals(filter.Key, StringComparison.OrdinalIgnoreCase)
                    select f).SingleOrDefault();

                if (facet.Value != null) // now check if value already filtered
                {
                    if (facet.Value.Intersect(filter.GetValues().Select(x => x.Id)).Any())
                    {
                        continue;
                    }
                }
                 * */

                criteria.Add(filter);
            }

            // Get selected filters
            var facets = parameters.Facets;
            dataSource.SelectedFilters = new List<SelectedFilterModel>();
            if (facets.Count != 0)
            {
                foreach (var key in facets.Keys)
                {
                    var filter = filters.SingleOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
                        && (!(x is PriceRangeFilter) || ((PriceRangeFilter)x).Currency.Equals(StoreHelper.CustomerSession.Currency, StringComparison.OrdinalIgnoreCase)));

                    var appliedFilter = searchHelper.Convert(filter, facets[key]);

                    foreach (var val in appliedFilter.GetValues())
                    {
                        criteria.Apply(appliedFilter);
                        dataSource.SelectedFilters.Add(
                            new SelectedFilterModel(searchHelper.Convert(filter), searchHelper.Convert(val)));
                    }
                }
            }

            // Perform search
            var sort = string.IsNullOrEmpty(parameters.Sort) ? "position" : parameters.Sort;
            var sortOrder = parameters.SortOrder;

            var isDescending = "desc".Equals(sortOrder, StringComparison.OrdinalIgnoreCase);

            SearchSort sortObject = null;

            if (!sort.Equals("position", StringComparison.OrdinalIgnoreCase))
            {
                if (sort.Equals("price", StringComparison.OrdinalIgnoreCase))
                {
                    if (session.Pricelists != null)
                    {
                        sortObject = new SearchSort(session.Pricelists.Select(priceList =>
                            new SearchSortField(
                                String.Format("price_{0}_{1}",
                                    criteria.Currency.ToLower(),
                                    priceList.ToLower()))
                            {
                                IgnoredUnmapped = true,
                                IsDescending = isDescending,
                                DataType = SearchSortField.DOUBLE
                            })
                            .ToArray());
                    }
                }
                else
                {
                    sortObject = new SearchSort(sort.ToLower(), isDescending);
                }
            }

            // Put default sort order if none is set
            if (sortObject == null)
            {
                sortObject = CatalogItemSearchCriteria.DefaultSortOrder;
            }

            criteria.Sort = sortObject;
            CatalogItemSearchResults results;
            // Search using criteria, it will only return IDs of the items
            var items = Search(criteria, cacheResults, out results).ToArray();
            var itemsIdsArray = items.Select(i => i.ItemId).ToArray();

            // Now load items with appropriate 
            var itemModelList = new List<CatalogItemWithPriceModel>();
            if (items.Any())
            {

                // Now convert it to the model

                var prices = _priceListClient.GetLowestPrices(session.Pricelists, itemsIdsArray, 1);
                var availabilities = _catalogClient.GetItemAvailability(itemsIdsArray,
                                                   UserHelper.StoreClient.GetCurrentStore().FulfillmentCenterId);

                foreach (var item in items)
                {
                    PriceModel priceModel = null;
                    ItemAvailabilityModel availabilityModel = null;
                    var catalogIdPath = UserHelper.CustomerSession.CatalogId + "/";
                    var searchTags = results.Items[item.ItemId.ToLower()].ToPropertyDictionary();

                    //Cache outline
                    HttpContext.Items["browsingoutline_" + item.Code.ToLower()] = searchTags[criteria.BrowsingOutlineField].ToString();
                    var currentOutline = searchTags[criteria.OutlineField].ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .FirstOrDefault(x => x.StartsWith(catalogIdPath, StringComparison.OrdinalIgnoreCase)) ?? string.Empty;

                    if (prices != null && prices.Any())
                    {
                        var lowestPrice =
                            (from p in prices
                             where p.ItemId.Equals(item.ItemId, StringComparison.OrdinalIgnoreCase)
                             select p).SingleOrDefault();
                        if (lowestPrice != null)
                        {
                            var tags = new Hashtable
							{
								{
									"Outline",
									currentOutline
								}
							};
                            priceModel = _marketing.GetItemPriceModel(item, lowestPrice, tags);
                        }
                    }

                    if (availabilities != null && availabilities.Any())
                    {
                        var availability =
                            (from a in availabilities
                             where a.ItemId.Equals(item.ItemId, StringComparison.OrdinalIgnoreCase)
                             select a).SingleOrDefault();

                        availabilityModel = new ItemAvailabilityModel(availability);
                    }

                    var itemModel = new CatalogItemWithPriceModel(CatalogHelper.CreateItemModel(item), priceModel, availabilityModel)
                    {
                        SearchOutline = currentOutline
                    };
                    itemModelList.Add(itemModel);
                }
            }

            dataSource.FilterGroups = searchHelper.Convert(results.FacetGroups);
            dataSource.CatalogItems = itemModelList.ToArray();
            dataSource.Criteria = criteria;

            // Create pager
            var pager = new PagerModel
            {
                TotalCount = results.TotalCount,
                CurrentPage = criteria.StartingRecord / criteria.RecordsToRetrieve + 1,
                RecordsPerPage = criteria.RecordsToRetrieve,
                StartingRecord = criteria.StartingRecord,
                DisplayStartingRecord = criteria.StartingRecord + 1,
                SortValues = new[] { "Position", "Name", "Price" },
                SelectedSort = sort,
                SortOrder = isDescending ? "desc" : "asc"
            };

            var end = criteria.StartingRecord + criteria.RecordsToRetrieve;
            pager.DisplayEndingRecord = end > results.TotalCount ? results.TotalCount : end;

            dataSource.Pager = pager;

            // Query similar words
            /*
            if (count == 0)
                dataSource.Suggestions = GetSuggestions();
             * */
            //}

            return dataSource;
        }

        #endregion

    }
}