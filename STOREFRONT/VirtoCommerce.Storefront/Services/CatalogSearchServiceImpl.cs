using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class CatalogSearchServiceImpl : ICatalogSearchService
    {
        private readonly ICatalogModuleApi _catalogModuleApi;
        private readonly IPricingModuleApi _pricingModuleApi;
        private readonly IInventoryModuleApi _inventoryModuleApi;
        private readonly IMarketingModuleApi _marketingModuleApi;
        private readonly ISearchModuleApi _searchApi;
        private readonly WorkContext _workContext;

        public CatalogSearchServiceImpl(WorkContext workContext, ICatalogModuleApi catalogModuleApi, IPricingModuleApi pricingModuleApi, IInventoryModuleApi inventoryModuleApi,
                                  IMarketingModuleApi marketingModuleApi, ISearchModuleApi searchApi)
        {
            _workContext = workContext;
            _catalogModuleApi = catalogModuleApi;
            _pricingModuleApi = pricingModuleApi;
            _inventoryModuleApi = inventoryModuleApi;
            _marketingModuleApi = marketingModuleApi;
            _searchApi = searchApi;
        }

        public async Task<Product> GetProductAsync(string id, ItemResponseGroup responseGroup = ItemResponseGroup.ItemInfo)
        {
            var item = (await _catalogModuleApi.CatalogModuleProductsGetAsync(id)).ToWebModel(_workContext.CurrentLanguage, _workContext.CurrentCurrency);

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

        public async Task<CatalogSearchResult> SearchAsync(CatalogSearchCriteria criteria)
        {
            var retVal = new CatalogSearchResult();

            var result = await _searchApi.SearchModuleSearchAsync(
                criteriaStoreId: _workContext.CurrentStore.Id,
                criteriaResponseGroup: criteria.ResponseGroup.ToString(),
                criteriaSearchInChildren: true,
                criteriaCategoryId: criteria.CategoryId,
                criteriaCatalogId: criteria.CatalogId,
                criteriaCurrency: _workContext.CurrentCurrency.Code,
                criteriaHideDirectLinkedCategories: true,
                criteriaSkip: criteria.PageSize * (criteria.PageNumber - 1),
                criteriaTake: criteria.PageSize);

            if (criteria.CategoryId != null)
            {
                var category = await _catalogModuleApi.CatalogModuleCategoriesGetAsync(criteria.CategoryId);
                if (category != null)
                {
                    retVal.Category = category.ToWebModel();
                }
            }

            if (result != null)
            {
                if (result.Products != null && result.Products.Any())
                {
                    var products = result.Products.Select(x => x.ToWebModel(_workContext.CurrentLanguage, _workContext.CurrentCurrency)).ToArray();
                    retVal.Products = new StorefrontPagedList<Product>(products, criteria.PageNumber, criteria.PageSize, result.ProductsTotalCount.Value, page => _workContext.RequestUrl.AddParameter("page", page.ToString()).ToString());

                    LoadProductsPrices(retVal.Products.ToArray());
                    LoadProductsInventories(retVal.Products.ToArray());
                }

                if (result.Categories != null && result.Categories.Any())
                {
                    retVal.Categories = result.Categories.Select(x => x.ToWebModel());
                }

                if (result.Aggregations != null)
                {
                    retVal.Aggregations = result.Aggregations.Select(x => x.ToWebModel()).ToArray();
                }
            }

            return retVal;
        }

        private void LoadProductsPrices(Product[] products)
        {
            var result = _pricingModuleApi.PricingModuleEvaluatePrices(_workContext.CurrentStore.Id, _workContext.CurrentStore.Catalog, products.Select(x => x.Id).ToList(), null, null, _workContext.CurrentCustomer.Id, null, _workContext.StorefrontUtcNow);
            foreach (var item in products)
            {
                item.Prices = result.Where(x => x.ProductId == item.Id).Select(x => x.ToWebModel()).ToList();
                var price = item.Prices.FirstOrDefault(x => x.Currency.Equals(_workContext.CurrentCurrency));
                if (price != null)
                {
                    item.Price = price;
                }
            }
        }

        private void LoadProductsInventories(Product[] products)
        {
            var inventories = _inventoryModuleApi.InventoryModuleGetProductsInventories(products.Select(x => x.Id).ToList());
            foreach (var item in products)
            {
                item.Inventory = inventories.Where(x => x.ProductId == item.Id).Select(x => x.ToWebModel()).FirstOrDefault();
            }
        }
    }
}