using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.ApiWebClient.Clients;
using VirtoCommerce.ApiWebClient.Customer;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Converters;
using VirtoCommerce.Web.Core.DataContracts;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : ControllerBase
    {

        [Route("")]
        public async Task<ActionResult> Index(BrowseQuery query)
        {

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                ViewBag.Title = String.Format("Searching by '{0}'", query.Search);
            }

            var retVal = await SearchAsync(query);

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
                var category = Task.Run(() => CatalogHelper.CatalogClient.GetCategoryByCodeAsync(categoryUrl.CategoryCode, session.CatalogId, session.Language)).Result;

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

        private static SearchResult CreateSearchResult(ResponseCollection<Product> results, BrowseQuery query)
        {
            var retVal = new SearchResult
            {
                Results = results.Items.Select(x => x.ToWebModel()).ToList(),
                Pager =
                {
                    TotalCount = results.TotalCount,
                    RecordsPerPage = query.Take ?? BrowseQuery.DefaultPageSize,
                    StartingRecord = query.Skip ??  0,
                    DisplayStartingRecord = query.Skip ?? 1,
                    SortValues = new[] { "Position", "Name", "Price", "Rating", "Reviews" },
                    SelectedSort = query.SortProperty,
                    SortOrder = query.SortDirection
                }
            };

            var end = query.Skip + query.Take ?? 0;
            retVal.Pager.CurrentPage = retVal.Pager.StartingRecord/retVal.Pager.RecordsPerPage + 1;
            retVal.Pager.DisplayEndingRecord = end > results.TotalCount ? results.TotalCount : end;

            return retVal;
        }

        private async Task<SearchResult> SearchAsync(BrowseQuery query, ICustomerSession session = null, ItemResponseGroups responseGroup = ItemResponseGroups.ItemMedium)
        {
            session = session ?? StoreHelper.CustomerSession;
            var results = await CatalogHelper.CatalogClient.GetProductsAsync(session.CatalogId, session.Language, query, responseGroup);
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