using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Converters;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductApiController : ApiController
    {
        private readonly WorkContext _workContext;
        private readonly ICatalogModuleApi _catalogApi;
        private readonly IPricingModuleApi _pricingApi;
        private readonly IInventoryModuleApi _inventoryApi;

        public ProductApiController(WorkContext workContext, ICatalogModuleApi catalogApi, IPricingModuleApi pricingApi, IInventoryModuleApi inventoryApi)
        {
            _workContext = workContext;
            _catalogApi = catalogApi;
            _pricingApi = pricingApi;
            _inventoryApi = inventoryApi;
        }

        [HttpGet]
        [ResponseType(typeof(Product))]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetProduct(string id)
        {
            await SetProduct(id);

            return Ok(_workContext.CurrentProduct);
        }

        protected async Task SetProduct(string id)
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