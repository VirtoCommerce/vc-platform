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
    [RoutePrefix("product")]
    public class ProductController : StorefrontControllerBase
    {
        private readonly ICatalogSearchService _productService;

        public ProductController(WorkContext context, IStorefrontUrlBuilder urlBuilder, ICatalogSearchService productService)
            : base(context, urlBuilder)
        {
            _productService = productService;
        }

        //This method created specially for routing from SeoRoute.cs (because ASP.NET routing does not work for  actions with RouteAttributes)
        //http://stackoverflow.com/questions/19545898/asp-net-web-api-2-attribute-routing-controller-name
        [HttpGet]
        public async Task<ActionResult> ProductDetails(string productId)
        {
            return await GetProductDetails(productId);
        }

        /// <summary>
        /// This action used by storefront to get product details by product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{productId}")]
        public async Task<ActionResult> GetProductDetails(string productId)
        {
            base.WorkContext.CurrentProduct = await _productService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemInfo);
            return View("product", base.WorkContext);
        }

        [HttpGet]
        [Route("{productId}/json")]
        public async Task<ActionResult> GetProductJsonById(string productId)
        {
            base.WorkContext.CurrentProduct = await _productService.GetProductAsync(productId, Model.Catalog.ItemResponseGroup.ItemLarge);
            return Json(base.WorkContext.CurrentProduct, JsonRequestBehavior.AllowGet);
        }
    }
}