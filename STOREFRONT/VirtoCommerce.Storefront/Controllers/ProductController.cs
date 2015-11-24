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
            WorkContext.CurrentProduct = await _productService.GetProduct(productid, WorkContext.CurrentCurrency.Code, Model.Catalog.ItemResponseGroup.ItemLarge);
            return View("product", WorkContext);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetProductById(string id)
        {
            WorkContext.CurrentProduct = await _productService.GetProduct(id, WorkContext.CurrentCurrency.Code, Model.Catalog.ItemResponseGroup.ItemLarge);
            return Json(WorkContext.CurrentProduct, JsonRequestBehavior.AllowGet);
        }
    }
}
