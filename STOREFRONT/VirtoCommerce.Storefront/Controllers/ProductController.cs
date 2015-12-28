using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "ProductCachingProfile")]
    public class ProductController : StorefrontControllerBase
    {
        private readonly ICatalogSearchService _productService;
        private readonly ICatalogSearchService _searchService;

        public ProductController(WorkContext context, IStorefrontUrlBuilder urlBuilder, ICatalogSearchService productService, ICatalogSearchService searchService)
            : base(context, urlBuilder)
        {
            _productService = productService;
            _searchService = searchService;
        }

        /// <summary>
        /// GET: /product/{productId}
        /// This action used by storefront to get product details by product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ProductDetails(string productId)
        {
            var product = await _productService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemInfo | Model.Catalog.ItemResponseGroup.ItemWithPrices);
            WorkContext.CurrentProduct = product;

            WorkContext.CurrentCatalogSearchCriteria.CategoryId = product.CategoryId;
            WorkContext.CurrentCatalogSearchResult = await _searchService.SearchAsync(WorkContext.CurrentCatalogSearchCriteria);

            return View("product", WorkContext);
        }

        /// <summary>
        /// GET: /product/{productId}/json
        /// This action used by js 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ProductDetailsJson(string productId)
        {
            base.WorkContext.CurrentProduct = await _productService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemLarge);
            return Json(base.WorkContext.CurrentProduct, JsonRequestBehavior.AllowGet);
        }
    }
}