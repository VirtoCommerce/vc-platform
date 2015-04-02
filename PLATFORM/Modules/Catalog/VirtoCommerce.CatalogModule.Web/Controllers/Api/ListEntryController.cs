using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CatalogModule.Web.Binders;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/listentries")]
    public class ListEntryController : ApiController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly IItemService _itemService;
        private readonly IAssetUrlResolver _assetUrlResolver;

        public ListEntryController(ICatalogSearchService searchService,
                                   ICategoryService categoryService,
                                   IItemService itemService, IAssetUrlResolver assetUrlResolver)
        {
            _searchService = searchService;
            _categoryService = categoryService;
            _itemService = itemService;
            _assetUrlResolver = assetUrlResolver;
        }
        // GET: api/catalog/listentries&q='some'&catalog=apple&category=cat1&respGroup=8&skip=0&take=20
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(webModel.ListEntrySearchResult))]
        public IHttpActionResult ListItemsSearch([ModelBinder(typeof(ListEntrySearchCriteriaBinder))]webModel.ListEntrySearchCriteria criteria)
        {
            var serviceResult = _searchService.Search(criteria.ToModuleModel());

            var retVal = new webModel.ListEntrySearchResult();

            var start = criteria.Start;
            var count = criteria.Count;

            // all categories
            var categories = serviceResult.Categories.Select(x => new webModel.ListEntryCategory(x.ToWebModel()));
            var products = serviceResult.Products.Select(x => new webModel.ListEntryProduct(x.ToWebModel(_assetUrlResolver)));

            retVal.TotalCount = categories.Count() + serviceResult.TotalCount;
            retVal.ListEntries.AddRange(categories.Skip(start).Take(count));

            count -= categories.Count();

            retVal.ListEntries.AddRange(products.Take(count));

            return Ok(retVal);
        }

        // POST: api/catalog/listentrylinks
        [HttpPost]
        [Route("~/api/catalog/listentrylinks")]
        [ResponseType(typeof(void))]
        public IHttpActionResult CreateLinks(webModel.ListEntryLink[] links)
        {
            InnerUpdateLinks(links, (x, y) => x.Links.Add(y));
            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/catalog/getslug
        [HttpGet]
        [Route("~/api/catalog/getslug")]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetSlug(string text)
        {
            if (text == null)
            {
                return InternalServerError(new NullReferenceException("text"));
            }
            return Ok(text.GenerateSlug());
        }


        // DELETE: api/catalog/listentrylinks
        [HttpPost]
        [Route("~/api/catalog/listentrylinks/delete")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteLinks(webModel.ListEntryLink[] links)
        {
            InnerUpdateLinks(links, (x, y) => x.Links.Remove(y));
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
