using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using VirtoCommerce.ApiClient.DataContracts.Search;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Converters;
using VirtoCommerce.Web.Core.DataContracts;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Controllers
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;
    using VirtoCommerce.ApiClient.Session;

    [RoutePrefix("search")]
    public class SearchController : ControllerBase
    {

        [Route("")]
        public async Task<ActionResult> Index(BrowseQuery query)
        {
            RestoreSearchPreferences(query);

            var retVal = await SearchAsync(query);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                retVal.Title = String.Format("Searching by '{0}'", query.Search);
            }

            return View(retVal);
        }

        [Route("find")]
        public async Task<ActionResult> Find(string term)
        {
            ViewBag.Title = String.Format("Searching by '{0}'", term);

            var query = new BrowseQuery
            {
                Take = 15, //autocomplete returns first 15
                Search = term.EscapeSearchTerm()
            };
            var results = await SearchAsync(query, responseGroup: ItemResponseGroups.ItemInfo);

            var data = from i in results.Results
                       select new
                       {
                           url = Url.ItemUrl(i.CatalogItem.Id, i.CatalogItem.Outline, i.CatalogItem.MainProductId),
                           value = i.DisplayName
                       };
            return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
        }


        // GET: /Catalog/
        /// <summary>
        /// Displays the catalog by specified URL.
        /// </summary>
        /// <param name="category">The category code</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// ActionResult.
        /// </returns>
        /// <exception cref="System.Web.HttpException">404;Category not found</exception>
        [DonutOutputCache(CacheProfile = "CatalogCache", VaryByCustom = "currency;filters;pricelist")]
        public async Task<ActionResult> Category(CategoryPathModel category, BrowseQuery query)
        {
            var client = ClientContext.Clients.CreateBrowseClient();
            var cat = await client.GetCategoryAsync(category.Category);

            if (cat != null)
            {
                RestoreSearchPreferences(query);
                query.Outline = string.Join("/", cat.BuildOutline().Select(x => x.Key));
                var retVal = await SearchAsync(query);
                retVal.Title = cat.Name;

                if (SiteMaps.Current != null)
                {
                    var node = SiteMaps.Current.CurrentNode;

                    if (Request.UrlReferrer != null &&
                        Request.UrlReferrer.AbsoluteUri.StartsWith(Request.Url.GetLeftPart(UriPartial.Authority)))
                    {
                        if (node != null)
                        {
                            node.RootNode.Attributes["ShowBack"] = true;
                        }

                        if (Request.UrlReferrer.AbsoluteUri.Equals(Request.Url.AbsoluteUri))
                        {
                            StoreHelper.CustomerSession.LastShoppingPage = Url.Content("~/");
                        }
                        else
                        {
                            StoreHelper.CustomerSession.LastShoppingPage = Request.UrlReferrer.AbsoluteUri;
                        }

                    }

                    if (node != null)
                    {
                        node.Title = retVal.Title;
                    }
                }

                return View("Index", retVal);
            }

            throw new HttpException(404, "Category not found");
        }

        [ChildActionOnly]
        public ActionResult SearchItems(CategoryUrlModel categoryUrl)
        {
            var session = StoreHelper.CustomerSession;
            var query = new BrowseQuery
            {
                SortProperty = categoryUrl.SortField,
                Take = categoryUrl.ItemCount
            };

            if (categoryUrl.NewItemsOnly)
            {
                query.StartDateFrom = DateTime.UtcNow.AddMonths(-1);
            }

            if (!string.IsNullOrWhiteSpace(categoryUrl.CategoryCode))
            {
                var client = ClientContext.Clients.CreateBrowseClient();
                var category = Task.Run(() => client.GetCategoryByCodeAsync(categoryUrl.CategoryCode)).Result;

                if (category != null)
                {
                    query.Outline = string.Join("/", category.BuildOutline().Select(x => x.Key));
                }
            }

            //Need to run synchrously because of child action
            var model = Search(query);

            return PartialView(model.Results);
        }

        #region Private Helpers

        private static SearchResult CreateSearchResult(ProductSearchResult results, BrowseQuery query)
        {
            var retVal = new SearchResult
            {
                Results = results.Items.Select(x => x.ToWebModel()).ToList(),
                Pager =
                {
                    TotalCount = results.TotalCount,
                    RecordsPerPage = query.Take ?? BrowseQuery.DefaultPageSize,
                    SortValues = new[] { "Position", "Name", "Price", "Rating", "Reviews" },
                    SelectedSort = query.SortProperty,
                    SortOrder = query.SortDirection
                },
                Facets = results.Facets
            };

        
            retVal.Pager.StartingRecord = query.Skip ?? 0;
            retVal.Pager.DisplayStartingRecord = retVal.Pager.StartingRecord + 1;
            retVal.Pager.CurrentPage = retVal.Pager.StartingRecord / retVal.Pager.RecordsPerPage + 1;

            var end = retVal.Pager.StartingRecord + retVal.Pager.RecordsPerPage;    
            retVal.Pager.DisplayEndingRecord = end > results.TotalCount ? results.TotalCount : end;

         
            return retVal;
        }

        private async Task<SearchResult> SearchAsync(BrowseQuery query, ICustomerSession session = null, ItemResponseGroups responseGroup = ItemResponseGroups.ItemMedium)
        {
            query.SortProperty = string.IsNullOrEmpty(query.SortProperty) ? "position" : query.SortProperty;
            query.SortDirection = "desc".Equals(query.SortDirection, StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";

            session = session ?? StoreHelper.CustomerSession;
            var client = ClientContext.Clients.CreateBrowseClient(session.StoreId, session.Language);
            var results = await client.GetProductsAsync(query, responseGroup);
            var retVal = CreateSearchResult(results, query);

            return retVal;
        }

        private SearchResult Search(BrowseQuery query)
        {
            var session = StoreHelper.CustomerSession;
            return Task.Run(() => SearchAsync(query, session)).Result;
        }

        private void RestoreSearchPreferences(BrowseQuery parameters)
        {
            var pageSize = parameters.Take;
            var sort = parameters.SortProperty;
            var sortOrder = parameters.SortDirection;

            if (!pageSize.HasValue)
            {
                int parsedSize;
                if (Int32.TryParse(StoreHelper.GetCookieValue("pagesizecookie"), out parsedSize))
                {
                    pageSize = parsedSize;
                }
            }
            else
            {
                StoreHelper.SetCookie("pagesizecookie", pageSize.Value.ToString(CultureInfo.InvariantCulture), DateTime.Now.AddMonths(1));
            }

            if (!pageSize.HasValue)
            {
                pageSize = BrowseQuery.DefaultPageSize;
            }

            parameters.Take = pageSize;

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

            parameters.SortProperty = sort;
            parameters.SortDirection = sortOrder;
        }

        #endregion

      
    }
}