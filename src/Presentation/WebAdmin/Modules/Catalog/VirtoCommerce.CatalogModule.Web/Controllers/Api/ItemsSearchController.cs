using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Services;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class ItemsSearchController : ApiController
    {
        private readonly ICatalogSearchService _searchService;

		public ItemsSearchController(ICatalogSearchService searchService)
        {
			_searchService = searchService;
        }

        [HttpPost]
		[ResponseType(typeof(webModel.ListEntrySearchResult))]
		public IHttpActionResult ListItemsSearch(webModel.SearchCriteria criteria)
        {
            var serviceResult = _searchService.Search(criteria.ToModuleModel());

			var retVal = new webModel.ListEntrySearchResult();

            var start = criteria.Start;
            var count = criteria.Count;

            // all categories
			var categories = serviceResult.Categories.Select(x => new webModel.ListEntryCategory(x.ToWebModel()));
			var products = serviceResult.Products.Select(x => new webModel.ListEntryProduct( x.ToWebModel()));
      
            retVal.TotalCount = categories.Count() + serviceResult.TotalCount;
			retVal.ListEntries.AddRange(categories.Skip(start).Take(count));

            count -= categories.Count();

			retVal.ListEntries.AddRange(products.Take(count));

            return Ok(retVal);
        }

    }
}
