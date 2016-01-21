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
using VirtoCommerce.Storefront.Model.Pricing.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class CatalogSearchServiceImpl : ICatalogSearchService
    {
        private readonly ICatalogModuleApi _catalogModuleApi;
        private readonly IPricingService _pricingService;
        private readonly IInventoryModuleApi _inventoryModuleApi;
        private readonly ISearchModuleApi _searchApi;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly Func<WorkContext> _workContextFactory;


        public CatalogSearchServiceImpl(Func<WorkContext> workContextFactory, ICatalogModuleApi catalogModuleApi, IPricingService pricingService, IInventoryModuleApi inventoryModuleApi, ISearchModuleApi searchApi, IPromotionEvaluator promotionEvaluator)
        {
            _workContextFactory = workContextFactory;
            _catalogModuleApi = catalogModuleApi;
            _pricingService = pricingService;
            _inventoryModuleApi = inventoryModuleApi;
            _searchApi = searchApi;
            _promotionEvaluator = promotionEvaluator;
        }

        public async Task<Product[]> GetProductsAsync(string[] ids, ItemResponseGroup responseGroup = ItemResponseGroup.ItemInfo)
        {
            var workContext = _workContextFactory();

            var retVal = (await _catalogModuleApi.CatalogModuleProductsGetProductByIdsAsync(ids.ToList())).Select(x=>x.ToWebModel(workContext.CurrentLanguage, workContext.CurrentCurrency)).ToArray();

            var allProducts = retVal.Concat(retVal.SelectMany(x => x.Variations)).ToArray();

            if (allProducts != null && allProducts.Any())
            {
                var taskList = new List<Task>();

                if ((responseGroup | ItemResponseGroup.ItemWithInventories) == responseGroup)
                {
                    taskList.Add(Task.Factory.StartNew(() => LoadProductsInventories(allProducts)));
                }

                if ((responseGroup | ItemResponseGroup.ItemWithPrices) == responseGroup)
                {
                    await _pricingService.EvaluateProductPricesAsync(allProducts);
                    if ((responseGroup | ItemResponseGroup.ItemWithDiscounts) == responseGroup)
                    {
                        await LoadProductsDiscountsAsync(allProducts);
                    }
                }

                Task.WaitAll(taskList.ToArray());
            }

            return retVal;
        }

        public async Task<CatalogSearchResult> SearchAsync(CatalogSearchCriteria criteria)
        {
            var retVal = new CatalogSearchResult();

            var workContext = _workContextFactory();

            var result = await _searchApi.SearchModuleSearchAsync(
                criteriaStoreId: workContext.CurrentStore.Id,
                criteriaKeyword: criteria.Keyword,
                criteriaResponseGroup: criteria.ResponseGroup.ToString(),
                criteriaSearchInChildren: criteria.SearchInChildren,
                criteriaCategoryId: criteria.CategoryId,
                criteriaCatalogId: criteria.CatalogId,
                criteriaCurrency: workContext.CurrentCurrency.Code,
                criteriaHideDirectLinkedCategories: true,
                criteriaTerms: criteria.Terms.ToStrings(),
                criteriaPricelistIds: workContext.CurrentPriceListIds.ToList(),
                criteriaSkip: criteria.PageSize * (criteria.PageNumber - 1),
                criteriaTake: criteria.PageSize,
                criteriaSort: criteria.SortBy);

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
                    var products = result.Products.Select(x => x.ToWebModel(workContext.CurrentLanguage, workContext.CurrentCurrency)).ToArray();
                    retVal.Products = new StorefrontPagedList<Product>(products, criteria.PageNumber, criteria.PageSize, result.ProductsTotalCount.Value, page => workContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());

                    await _pricingService.EvaluateProductPricesAsync(retVal.Products.ToArray());
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
    
        private async Task LoadProductsDiscountsAsync(Product[] products)
        {
            var workContext = _workContextFactory();
            var promotionContext = workContext.ToPromotionEvaluationContext();
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