using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
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
            var product = (await _catalogSearchService.GetProductsAsync(new[] { productId }, Model.Catalog.ItemResponseGroup.ItemSmall | Model.Catalog.ItemResponseGroup.ItemWithPrices)).FirstOrDefault();
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
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> ProductDetailsJson(string productId)
        {
            base.WorkContext.CurrentProduct = (await _catalogSearchService.GetProductsAsync(new [] { productId }, Model.Catalog.ItemResponseGroup.ItemLarge)).FirstOrDefault();
            return Json(base.WorkContext.CurrentProduct, JsonRequestBehavior.AllowGet);
        }
    }
}