using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class ListEntryController : ApiController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly ICategoryService _categoryService;

        public ListEntryController(ICatalogSearchService searchService, ICategoryService categoryService)
        {
            _searchService = searchService;
            _categoryService = categoryService;
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
            var products = serviceResult.Products.Select(x => new webModel.ListEntryProduct(x.ToWebModel()));

            retVal.TotalCount = categories.Count() + serviceResult.TotalCount;
            retVal.ListEntries.AddRange(categories.Skip(start).Take(count));

            count -= categories.Count();

            retVal.ListEntries.AddRange(products.Take(count));

            return Ok(retVal);
        }

        // POST: api/listentry/createLinks
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult CreateLinks(webModel.CategoryLink[] links)
        {
            var categoryLinks = links.Where(x => x.SourceCategoryId != null);
            var categories = new List<moduleModel.Category>();
            foreach (var categoryLink in categoryLinks)
            {
                var category = _categoryService.GetById(categoryLink.SourceCategoryId);
                category.Links.Add(new moduleModel.CategoryLink { CategoryId = categoryLink.CategoryId, CatalogId = categoryLink.CatalogId });
                categories.Add(category);
            }
            _categoryService.Update(categories.ToArray());

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/listentry/deleteLinks
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteLinks(webModel.CategoryLink[] links)
        {
            var categoryLinks = links.Where(x => x.SourceCategoryId != null);
            var categories = new List<moduleModel.Category>();
            foreach (var categoryLink in categoryLinks)
            {
                var category = _categoryService.GetById(categoryLink.SourceCategoryId);
                var linkToRemove = category.Links.First(x => x.CatalogId == categoryLink.CatalogId && x.CategoryId == categoryLink.CategoryId);
                category.Links.Remove(linkToRemove);
                categories.Add(category);
            }
            _categoryService.Update(categories.ToArray());

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
