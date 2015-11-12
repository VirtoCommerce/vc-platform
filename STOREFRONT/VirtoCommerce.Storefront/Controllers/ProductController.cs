using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Converters;

namespace VirtoCommerce.Storefront.Controllers
{
    //[RoutePrefix("product")]
    public class ProductController : Controller
    {
        private readonly WorkContext _workContext;
        private readonly ICatalogModuleApi _catalogApi;
        private readonly IPricingModuleApi _pricingApi;
        private readonly IInventoryModuleApi _inventoryApi;

        public ProductController(WorkContext context, ICatalogModuleApi catalogApi, IPricingModuleApi pricingApi, IInventoryModuleApi inventoryApi)
        {
            _workContext = context;
            _catalogApi = catalogApi;
            _pricingApi = pricingApi;
            _inventoryApi = inventoryApi;
        }

        [HttpGet]
        //[Route("{productid}")]
        public async Task<ActionResult> ProductDetails(string productid)
        {
            await GetProduct(productid);
            return View("product", _workContext);
        }


        protected async Task GetProduct(string id)
        {
            var product = await _catalogApi.CatalogModuleProductsGetAsync(id);
            _workContext.CurrentProduct = product.ToWebModel();
            var ids = _workContext.CurrentProduct.Variations.Select(v => v.Id).ToList();
            ids.Add(id);
            foreach (var productId in ids)
            {
                var prices = await _pricingApi.PricingModuleGetProductPricesAsync(productId);
                foreach (var price in prices)
                {
                    if (_workContext.CurrentProduct.Id == price.ProductId && _workContext.CurrentCurrency.CurrencyCode.ToString() == price.Currency)
                        _workContext.CurrentProduct.Price = price.ToWebModel();

                    var variation = _workContext.CurrentProduct.Variations.FirstOrDefault(v => v.Id == price.ProductId);
                    if (variation != null)
                        variation.Price = price.ToWebModel();
                }
            }


            var inventories = await _inventoryApi.InventoryModuleGetProductsInventoriesAsync(ids);
            foreach (var inventory in inventories)
            {
                if (_workContext.CurrentProduct.Id == inventory.ProductId)
                    _workContext.CurrentProduct.Inventory = inventory.ToWebModel();

                var variation = _workContext.CurrentProduct.Variations.FirstOrDefault(v => v.Id == inventory.ProductId);
                if (variation != null)
                    variation.Inventory = inventory.ToWebModel();
            }
        }
    }
}