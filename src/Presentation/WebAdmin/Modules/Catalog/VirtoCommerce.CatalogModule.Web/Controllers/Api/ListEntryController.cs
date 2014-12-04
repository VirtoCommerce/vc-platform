using System;
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
		private readonly IItemService _itemService;

		public ListEntryController(ICatalogSearchService searchService, ICategoryService categoryService, IItemService itemService)
        {
            _searchService = searchService;
            _categoryService = categoryService;
			_itemService = itemService;
        }

        [HttpPost]
        [ResponseType(typeof(webModel.ListEntrySearchResult))]
        public IHttpActionResult ListItemsSearch(webModel.ListEntrySearchCriteria criteria)
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
		public IHttpActionResult CreateLinks(webModel.ListEntryLink[] links)
        {
			InnerUpdateLinks(links, (x, y) => x.Links.Add(y));
            return StatusCode(HttpStatusCode.NoContent);
        }

		// GET: api/GetSlug/sasasa
		[ResponseType(typeof(string))]
		[HttpGet]
		public IHttpActionResult GetSlug(string text)
		{
			if(text == null)
			{
				return InternalServerError(new NullReferenceException("text"));
			}
			return Ok(text.GenerateSlug());
		}


        // POST: api/listentry/deleteLinks
        [HttpPost]
        [ResponseType(typeof(void))]
		public IHttpActionResult DeleteLinks(webModel.ListEntryLink[] links)
        {
			InnerUpdateLinks(links, (x,y)=> x.Links.Remove(y) );
            return StatusCode(HttpStatusCode.NoContent);
        }


		private void InnerUpdateLinks(webModel.ListEntryLink[] links, Action<moduleModel.ILinkSupport, moduleModel.CategoryLink> action)
		{
			var changedObjects = new List<moduleModel.ILinkSupport>();
			foreach (var link in links)
			{
				moduleModel.ILinkSupport changedObject;
				var newlink = new moduleModel.CategoryLink
				{
					CategoryId = link.CategoryId,
					CatalogId = link.CatalogId
				};

				if (String.Equals(link.ListEntryType, webModel.ListEntryCategory.TypeName, StringComparison.InvariantCultureIgnoreCase))
				{
					changedObject = _categoryService.GetById(link.ListEntryId);
				}
				else
				{
					changedObject = _itemService.GetById(link.ListEntryId, moduleModel.ItemResponseGroup.ItemLarge);
				}
				action(changedObject, newlink);
				changedObjects.Add(changedObject);
			}

			_categoryService.Update(changedObjects.OfType<moduleModel.Category>().ToArray());
			_itemService.Update(changedObjects.OfType<moduleModel.CatalogProduct>().ToArray());
		}

    }
}
