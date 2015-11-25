using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Services
{
    public class CatalogServiceImpl : ICatalogService
    {
        private readonly ICatalogModuleApi _catalogModuleApi;
        private readonly IPricingModuleApi _pricingModuleApi;
        private readonly IInventoryModuleApi _inventoryModuleApi;
        private readonly IMarketingModuleApi _marketingModuleApi;
        private readonly WorkContext _workContext;

        public CatalogServiceImpl(WorkContext workContext, ICatalogModuleApi catalogModuleApi, IPricingModuleApi pricingModuleApi, IInventoryModuleApi inventoryModuleApi,
                                  IMarketingModuleApi marketingModuleApi)
        {
            _workContext = workContext;
            _catalogModuleApi = catalogModuleApi;
            _pricingModuleApi = pricingModuleApi;
            _inventoryModuleApi = inventoryModuleApi;
            _marketingModuleApi = marketingModuleApi;
        }

        public async Task<Product> GetProductAsync(string id, ItemResponseGroup responseGroup = ItemResponseGroup.ItemInfo)
        {
            var item = (await _catalogModuleApi.CatalogModuleProductsGetAsync(id)).ToWebModel();

            var allProducts = new[] { item }.Concat(item.Variations).ToArray();

            var taskList = new List<Task>();

            if ((responseGroup | ItemResponseGroup.ItemWithPrices) == responseGroup)
            {
                taskList.Add(Task.Factory.StartNew(() => LoadProductsPrices(allProducts)));
            }
            if ((responseGroup | ItemResponseGroup.ItemWithInventories) == responseGroup)
            {
                taskList.Add(Task.Factory.StartNew(() => LoadProductsInventories(allProducts)));
            }

            Task.WaitAll(taskList.ToArray());

            return item;
        }

        public Task<SearchResult> SearchAsync(SearchCriteria criteria)
        {
            throw new NotImplementedException();
        }

        private void LoadProductsPrices(Product[] products)
        {
            var result =  _pricingModuleApi.PricingModuleEvaluatePrices(_workContext.CurrentStore.Id, _workContext.CurrentStore.Catalog, products.Select(x=> x.Id).ToList(), null, null, _workContext.CurrentCustomer.Id, null, _workContext.StorefrontUtcNow, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            foreach (var item in products)
            {
                item.Prices = result.Where(x => x.ProductId == item.Id).Select(x => x.ToWebModel()).ToList();
                item.Price = item.Prices.FirstOrDefault(x => x.Currency == _workContext.CurrentCurrency);
            }
        }

        private void LoadProductsInventories(Product[] products)
        {
            var inventories = _inventoryModuleApi.InventoryModuleGetProductsInventories(products.Select(x => x.Id).ToList());
            foreach (var item in products)
            {
                item.Inventory = inventories.Where(x=>x.ProductId == item.Id).Select(x=>x.ToWebModel()).FirstOrDefault();
            }
        }
    }
}