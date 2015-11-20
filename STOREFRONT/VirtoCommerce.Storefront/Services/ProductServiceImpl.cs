using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Converters;

namespace VirtoCommerce.Storefront.Services
{
    public class ProductServiceImpl : IProductService
    {
        private readonly ICatalogModuleApi _catalogModuleApi;
        private readonly IPricingModuleApi _pricingModuleApi;
        private readonly IInventoryModuleApi _inventoryModuleApi;
        private readonly IMarketingModuleApi _marketingModuleApi;

        public ProductServiceImpl(
            ICatalogModuleApi catalogModuleApi,
            IPricingModuleApi pricingModuleApi,
            IInventoryModuleApi inventoryModuleApi,
            IMarketingModuleApi marketingModuleApi)
        {
            _catalogModuleApi = catalogModuleApi;
            _pricingModuleApi = pricingModuleApi;
            _inventoryModuleApi = inventoryModuleApi;
            _marketingModuleApi = marketingModuleApi;
        }

        public async Task<Product> GetProductById(string id, string currencyCode, ItemResponseGroup responseGroup = ItemResponseGroup.ItemInfo)
        {
            var item = (await _catalogModuleApi.CatalogModuleProductsGetAsync(id)).ToWebModel();

            var taskList = new List<Task>();

            if ((responseGroup | ItemResponseGroup.ItemWithPrices) == responseGroup)
            {
                taskList.Add(Task.Factory.StartNew(() => GetPrices(item, currencyCode)));
            }
            if ((responseGroup | ItemResponseGroup.ItemWithInventories) == responseGroup)
            {
                taskList.Add(Task.Factory.StartNew(() => GetInventories(item)));
            }

            Task.WaitAll(taskList.ToArray());

            return item;
        }

        private void GetPrices(Product product, string currencyCode)
        {
            var ids = product.Variations.Select(v => v.Id).ToList();
            ids.Add(product.Id);
            foreach (var productId in ids)
            {
                var prices = _pricingModuleApi.PricingModuleGetProductPrices(productId);
                foreach (var price in prices.Where(p => p.Currency == currencyCode))
                {
                    if (product.Id == price.ProductId)
                        product.Price = price.ToWebModel();

                    var variation = product.Variations.FirstOrDefault(v => v.Id == price.ProductId);
                    if (variation != null)
                        variation.Price = price.ToWebModel();
                }
            }
        }

        private void GetInventories(Product product)
        {
            var ids = product.Variations.Select(v => v.Id).ToList();
            ids.Add(product.Id);
            var inventories = _inventoryModuleApi.InventoryModuleGetProductsInventories(ids);
            foreach (var inventory in inventories)
            {
                if (product.Id == inventory.ProductId)
                    product.Inventory = inventory.ToWebModel();

                var variation = product.Variations.FirstOrDefault(v => v.Id == inventory.ProductId);
                if (variation != null)
                    variation.Inventory = inventory.ToWebModel();
            }
        }
    }
}