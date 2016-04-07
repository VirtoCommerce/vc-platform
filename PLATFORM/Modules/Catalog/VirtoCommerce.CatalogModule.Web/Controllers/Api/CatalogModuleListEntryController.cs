using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Web.Security;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/listentries")]
    public class CatalogModuleListEntryController : CatalogBaseController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly IItemService _itemService;
        private readonly IBlobUrlResolver _blobUrlResolver;

        public CatalogModuleListEntryController(ICatalogSearchService searchService,
                                   ICategoryService categoryService,
                                   IItemService itemService, IBlobUrlResolver blobUrlResolver, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            : base(securityService, permissionScopeService)
        {
            _searchService = searchService;
            _categoryService = categoryService;
            _itemService = itemService;
            _blobUrlResolver = blobUrlResolver;
        }


        /// <summary>
        /// Searches for the items by complex criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(webModel.ListEntrySearchResult))]
        public IHttpActionResult ListItemsSearch(coreModel.SearchCriteria searchCriteria)
        {
            ApplyRestrictionsForCurrentUser(searchCriteria);

            searchCriteria.WithHidden = true;
            //Need search in children categories if user specify keyword
            if (!string.IsNullOrEmpty(searchCriteria.Keyword))
            {
                searchCriteria.SearchInChildren = true;
                searchCriteria.SearchInVariations = true;
            }

            var retVal = new webModel.ListEntrySearchResult();

            int categorySkip = 0;
            int categoryTake = 0;
            //Because products and categories represent in search result as two separated collections for handle paging request 
            //we should join two resulting collection artificially
            //search categories
            if ((searchCriteria.ResponseGroup & coreModel.SearchResponseGroup.WithCategories) == coreModel.SearchResponseGroup.WithCategories)
            {
                searchCriteria.ResponseGroup = searchCriteria.ResponseGroup & ~coreModel.SearchResponseGroup.WithProducts;
                var categoriesSearchResult = _searchService.Search(searchCriteria);
                var categoriesTotalCount = categoriesSearchResult.Categories.Count();

                categorySkip = Math.Min(categoriesTotalCount, searchCriteria.Skip);
                categoryTake = Math.Min(searchCriteria.Take, Math.Max(0, categoriesTotalCount - searchCriteria.Skip));
                var categories = categoriesSearchResult.Categories.Skip(categorySkip).Take(categoryTake).Select(x => new webModel.ListEntryCategory(x.ToWebModel(_blobUrlResolver))).ToList();

                retVal.TotalCount = categoriesTotalCount;
                retVal.ListEntries.AddRange(categories);

                searchCriteria.ResponseGroup = searchCriteria.ResponseGroup | coreModel.SearchResponseGroup.WithProducts;
            }

            //search products
            if ((searchCriteria.ResponseGroup & coreModel.SearchResponseGroup.WithProducts) == coreModel.SearchResponseGroup.WithProducts)
            {
                searchCriteria.ResponseGroup = searchCriteria.ResponseGroup & ~coreModel.SearchResponseGroup.WithCategories;
                searchCriteria.Skip = searchCriteria.Skip - categorySkip;
                searchCriteria.Take = searchCriteria.Take - categoryTake;
                var productsSearchResult = _searchService.Search(searchCriteria);

                var products = productsSearchResult.Products.Select(x => new webModel.ListEntryProduct(x.ToWebModel(_blobUrlResolver)));

                retVal.TotalCount += productsSearchResult.ProductsTotalCount;
                retVal.ListEntries.AddRange(products);
            }


            return Ok(retVal);
        }


        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <param name="links">The links.</param>
        [HttpPost]
        [Route("~/api/catalog/listentrylinks")]
        [ResponseType(typeof(void))]
        public IHttpActionResult CreateLinks(webModel.ListEntryLink[] links)
        {
            //Scope bound security check
            CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, links);

            InnerUpdateLinks(links, (x, y) => x.Links.Add(y));
            return StatusCode(HttpStatusCode.NoContent);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
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


        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <param name="links">The links.</param>
        [HttpPost]
        [Route("~/api/catalog/listentrylinks/delete")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteLinks(webModel.ListEntryLink[] links)
        {
            //Scope bound security check
            CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Delete, links);

            InnerUpdateLinks(links, (x, y) => x.Links.Remove(y));
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <param name="moveInfo">Move operation details</param>
        [HttpPost]
        [Route("move")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Move(webModel.MoveInfo moveInfo)
        {
            var categories = new List<coreModel.Category>();

            //Move  categories
            foreach (var listEntryCategory in moveInfo.ListEntries.Where(x => String.Equals(x.Type, webModel.ListEntryCategory.TypeName, StringComparison.InvariantCultureIgnoreCase)))
            {
                var category = _categoryService.GetById(listEntryCategory.Id, coreModel.CategoryResponseGroup.Info);
                if (category.CatalogId != moveInfo.Catalog)
                {
                    category.CatalogId = moveInfo.Catalog ?? String.Empty;
                }
                if (category.ParentId != moveInfo.Category)
                {
                    category.ParentId = moveInfo.Category ?? String.Empty;
                }
                categories.Add(category);
            }

            var products = new List<coreModel.CatalogProduct>();
            //Move products
            foreach (var listEntryProduct in moveInfo.ListEntries.Where(x => String.Equals(x.Type, webModel.ListEntryProduct.TypeName, StringComparison.InvariantCultureIgnoreCase)))
            {
                var product = _itemService.GetById(listEntryProduct.Id, Domain.Catalog.Model.ItemResponseGroup.ItemLarge);
                if (product.CatalogId != moveInfo.Catalog)
                {
                    product.CatalogId = moveInfo.Catalog ?? String.Empty;
                    product.CategoryId = null;
                    foreach (var variation in product.Variations)
                    {
                        variation.CatalogId = moveInfo.Catalog ?? String.Empty;
                        variation.CategoryId = null;
                    }

                }
                if (product.CategoryId != moveInfo.Category)
                {
                    product.CategoryId = moveInfo.Category ?? String.Empty;
                    foreach (var variation in product.Variations)
                    {
                        variation.CategoryId = moveInfo.Category ?? String.Empty;
                    }
                }
                products.Add(product);
                products.AddRange(product.Variations);
            }

            //Scope bound security check
            CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, categories);
            CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, products);

            if (categories.Any())
            {
                _categoryService.Update(categories.ToArray());
            }
            if (products.Any())
            {
                _itemService.Update(products.ToArray());
            }
            return Ok();
        }

        private void InnerUpdateLinks(webModel.ListEntryLink[] links, Action<coreModel.ILinkSupport, coreModel.CategoryLink> action)
        {
            var changedObjects = new List<coreModel.ILinkSupport>();
            foreach (var link in links)
            {
                coreModel.ILinkSupport changedObject;
                var newlink = new coreModel.CategoryLink
                {
                    CategoryId = link.CategoryId,
                    CatalogId = link.CatalogId
                };

                if (String.Equals(link.ListEntryType, webModel.ListEntryCategory.TypeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    changedObject = _categoryService.GetById(link.ListEntryId, coreModel.CategoryResponseGroup.Full);
                }
                else
                {
                    changedObject = _itemService.GetById(link.ListEntryId, coreModel.ItemResponseGroup.ItemLarge);
                }
                action(changedObject, newlink);
                changedObjects.Add(changedObject);
            }

            _categoryService.Update(changedObjects.OfType<coreModel.Category>().ToArray());
            _itemService.Update(changedObjects.OfType<coreModel.CatalogProduct>().ToArray());
        }


    }
}
