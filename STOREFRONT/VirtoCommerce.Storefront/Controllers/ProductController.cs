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
        private readonly ICatalogService _productService;

        public ProductController(WorkContext context, IStorefrontUrlBuilder urlBuilder, ICatalogService productService)
            : base(context, urlBuilder)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> ProductDetails(string productid)
        {
            base.WorkContext.CurrentProduct = await _productService.GetProductAsync(productid, Model.Catalog.ItemResponseGroup.ItemLarge);
            return View("product", base.WorkContext);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetProductById(string id)
        {
            base.WorkContext.CurrentProduct = await _productService.GetProductAsync(id, Model.Catalog.ItemResponseGroup.ItemLarge);
            return Json(base.WorkContext.CurrentProduct, JsonRequestBehavior.AllowGet);
        }
    }
}