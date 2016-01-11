using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class CatalogSearchServiceImpl : ICatalogSearchService
    {
        private readonly ICatalogModuleApi _catalogModuleApi;
        private readonly IPricingModuleApi _pricingModuleApi;
        private readonly IInventoryModuleApi _inventoryModuleApi;
        private readonly ISearchModuleApi _searchApi;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly WorkContext _workContext;


        public CatalogSearchServiceImpl(WorkContext workContext, ICatalogModuleApi catalogModuleApi, IPricingModuleApi pricingModuleApi, IInventoryModuleApi inventoryModuleApi, ISearchModuleApi searchApi, IPromotionEvaluator promotionEvaluator)
        {
            _workContext = workContext;
            _catalogModuleApi = catalogModuleApi;
            _pricingModuleApi = pricingModuleApi;
            _inventoryModuleApi = inventoryModuleApi;
            _searchApi = searchApi;
            _promotionEvaluator = promotionEvaluator;
        }

        public async Task<Product> GetProductAsync(string id, ItemResponseGroup responseGroup = ItemResponseGroup.ItemInfo)
        {
            var item = (await _catalogModuleApi.CatalogModuleProductsGetProductByIdAsync(id)).ToWebModel(_workContext.CurrentLanguage, _workContext.CurrentCurrency);

            var allProducts = new[] { item }.Concat(item.Variations).ToArray();

            var taskList = new List<Task>();

            if ((responseGroup | ItemResponseGroup.ItemWithPrices) == responseGroup)
            {
                var loadPricesTask = Task.Factory.StartNew(() =>
                {
                    LoadProductsPrices(allProducts);
                    if ((responseGroup | ItemResponseGroup.ItemWithDiscounts) == responseGroup)
                    {
                        LoadProductsDiscountsAsync(allProducts).Wait();
                    }
                });
                taskList.Add(loadPricesTask);
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

            string sort = "manual";
            string sortOrder = "asc";

            if (!string.IsNullOrEmpty(criteria.SortBy))
            {
                var splittedSortBy = criteria.SortBy.Split('-');
                if (splittedSortBy.Length > 1)
                {
                    sort = splittedSortBy[0].Equals("title", StringComparison.OrdinalIgnoreCase) ? "name" : splittedSortBy[0];
                    sortOrder = splittedSortBy[1].IndexOf("descending", StringComparison.OrdinalIgnoreCase) >= 0 ? "desc" : "asc";
                }
            }

            var result = await _searchApi.SearchModuleSearchAsync(
                criteriaStoreId: _workContext.CurrentStore.Id,
                criteriaKeyword: criteria.Keyword,
                criteriaResponseGroup: criteria.ResponseGroup.ToString(),
                criteriaSearchInChildren: true,
                criteriaCategoryId: criteria.CategoryId,
                criteriaCatalogId: criteria.CatalogId,
                criteriaCurrency: _workContext.CurrentCurrency.Code,
                criteriaHideDirectLinkedCategories: true,
                criteriaTerms: criteria.Terms.ToStrings(),
                criteriaPricelistIds: _workContext.CurrentPriceListIds.ToList(),
                criteriaSkip: criteria.PageSize * (criteria.PageNumber - 1),
                criteriaTake: criteria.PageSize,
                criteriaSort: sort,
                criteriaSortOrder: sortOrder);

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
                    retVal.Products = new StorefrontPagedList<Product>(products, criteria.PageNumber, criteria.PageSize, result.ProductsTotalCount.Value, page => _workContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());

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
            var result = _pricingModuleApi.PricingModuleEvaluatePrices(
                   evalContextStoreId: _workContext.CurrentStore.Id,
                   evalContextCatalogId: _workContext.CurrentStore.Catalog,
                   evalContextProductIds: products.Select(x => x.Id).ToList(),
                   evalContextCustomerId: _workContext.CurrentCustomer.Id,
                   evalContextCertainDate: _workContext.StorefrontUtcNow,
                   evalContextPricelistIds: _workContext.CurrentPriceListIds.ToList());

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

        private async Task LoadProductsDiscountsAsync(Product[] products)
        {
            var promotionContext = _workContext.ToPromotionEvaluationContext();
            promotionContext.PromoEntries = products.Select(x => x.ToPromotionItem()).ToList();
            await _promotionEvaluator.EvaluateDiscountsAsync(promotionContext, products);
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