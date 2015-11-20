using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        private readonly WorkContext _workContext;
        private readonly IProductService _productService;

        public ProductController(WorkContext context, IProductService productService)
        {
            _workContext = context;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> ProductDetails(string productid)
        {
            _workContext.CurrentProduct = await _productService.GetProductById(productid, _workContext.CurrentCurrency.Code, Model.Catalog.ItemResponseGroup.ItemLarge);
            return View("product", _workContext);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetProductById(string id)
        {
            _workContext.CurrentProduct = await _productService.GetProductById(id, _workContext.CurrentCurrency.Code, Model.Catalog.ItemResponseGroup.ItemLarge);
            return Json(_workContext.CurrentProduct, JsonRequestBehavior.AllowGet);
        }
    }
}