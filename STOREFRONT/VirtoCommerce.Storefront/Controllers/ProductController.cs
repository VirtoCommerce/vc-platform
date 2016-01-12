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
        private readonly ICatalogSearchService _catalogSearchService;

        public ProductController(WorkContext context, IStorefrontUrlBuilder urlBuilder, ICatalogSearchService catalogSearchService)
            : base(context, urlBuilder)
        {
            _catalogSearchService = catalogSearchService;
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
            var product = await _catalogSearchService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemInfo | Model.Catalog.ItemResponseGroup.ItemWithPrices);
            WorkContext.CurrentProduct = product;

            WorkContext.CurrentCatalogSearchCriteria.CategoryId = product.CategoryId;
            WorkContext.CurrentCatalogSearchResult = await _catalogSearchService.SearchAsync(WorkContext.CurrentCatalogSearchCriteria);

            return View("product", WorkContext);
        }

        /// <summary>
        /// GET: /product/{productId}/json
        /// This action used by js 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> ProductDetailsJson(string productId)
        {
            base.WorkContext.CurrentProduct = await _catalogSearchService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemLarge);
            return Json(base.WorkContext.CurrentProduct, JsonRequestBehavior.AllowGet);
        }
    }
}