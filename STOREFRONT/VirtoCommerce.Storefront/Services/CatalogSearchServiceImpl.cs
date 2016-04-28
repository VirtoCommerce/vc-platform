using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
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

        #region ICatalogSearchService Members
        public async Task<Product[]> GetProductsAsync(string[] ids, ItemResponseGroup responseGroup = ItemResponseGroup.ItemInfo)
        {
            var workContext = _workContextFactory();

            var retVal = (await _catalogModuleApi.CatalogModuleProductsGetProductByIdsAsync(ids.ToList(), ((int)responseGroup).ToString())).Select(x => x.ToWebModel(workContext.CurrentLanguage, workContext.CurrentCurrency, workContext.CurrentStore)).ToArray();

            var allProducts = retVal.Concat(retVal.SelectMany(x => x.Variations)).ToArray();

            if (!allProducts.IsNullOrEmpty())
            {
                var taskList = new List<Task>();

                if (responseGroup.HasFlag(ItemResponseGroup.ItemAssociations))
                {
                    taskList.Add(LoadProductsAssociationsAsync(allProducts));
                }

                if (responseGroup.HasFlag(ItemResponseGroup.Inventory))
                {
                    taskList.Add(LoadProductsInventoriesAsync(allProducts));
                }

                if (responseGroup.HasFlag(ItemResponseGroup.ItemWithPrices))
                {
                    await _pricingService.EvaluateProductPricesAsync(allProducts);
                    if ((responseGroup | ItemResponseGroup.ItemWithDiscounts) == responseGroup)
                    {
                        await LoadProductsDiscountsAsync(allProducts);
                    }
                }

                await Task.WhenAll(taskList.ToArray());
            }

            return retVal;
        }

        public async Task<Category[]> GetCategoriesAsync(string[] ids, CategoryResponseGroup responseGroup = CategoryResponseGroup.Info)
        {
            var workContext = _workContextFactory();

            var retVal = (await _catalogModuleApi.CatalogModuleCategoriesGetCategoriesByIdsAsync(ids.ToList(), ((int)responseGroup).ToString())).Select(x => x.ToWebModel(workContext.CurrentLanguage, workContext.CurrentStore)).ToArray();

            return retVal;
        }

        /// <summary>
        /// Async search categories by given criteria 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<IPagedList<Category>> SearchCategoriesAsync(CatalogSearchCriteria criteria)
        {
            var workContext = _workContextFactory();
            criteria = criteria.Clone();
            //exclude products
            criteria.ResponseGroup = criteria.ResponseGroup & (~CatalogSearchResponseGroup.WithProducts);
            //include categories
            criteria.ResponseGroup = criteria.ResponseGroup | CatalogSearchResponseGroup.WithCategories;
            var searchCriteria = criteria.ToServiceModel(workContext);
            var result = await _catalogModuleApi.CatalogModuleSearchSearchAsync(searchCriteria);

            //API temporary does not support paginating request to categories (that's uses PagedList with superset instead StaticPagedList)
            return new PagedList<Category>(result.Categories.Select(x => x.ToWebModel(workContext.CurrentLanguage, workContext.CurrentStore)), criteria.PageNumber, criteria.PageSize);
        }

        /// <summary>
        /// search categories by given criteria 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IPagedList<Category> SearchCategories(CatalogSearchCriteria criteria)
        {
            var workContext = _workContextFactory();
            criteria = criteria.Clone();
            //exclude products
            criteria.ResponseGroup = criteria.ResponseGroup & (~CatalogSearchResponseGroup.WithProducts);
            //include categories
            criteria.ResponseGroup = criteria.ResponseGroup | CatalogSearchResponseGroup.WithCategories;
            var searchCriteria = criteria.ToServiceModel(workContext);
            var categories = _catalogModuleApi.CatalogModuleSearchSearch(searchCriteria).Categories.Select(x => x.ToWebModel(workContext.CurrentLanguage, workContext.CurrentStore)).ToList();

            //API temporary does not support paginating request to categories (that's uses PagedList with superset)
            return new PagedList<Category>(categories, criteria.PageNumber, criteria.PageSize);
        }

        /// <summary>
        /// Async search products by given criteria 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<CatalogSearchResult> SearchProductsAsync(CatalogSearchCriteria criteria)
        {
            criteria = criteria.Clone();
            //exclude categories
            criteria.ResponseGroup = criteria.ResponseGroup & (~CatalogSearchResponseGroup.WithCategories);
            //include products
            criteria.ResponseGroup = criteria.ResponseGroup | CatalogSearchResponseGroup.WithProducts;

            var workContext = _workContextFactory();
            var searchCriteria = criteria.ToServiceModel(workContext);
            var result = await _searchApi.SearchModuleSearchAsync(searchCriteria);
            var products = result.Products.Select(x => x.ToWebModel(workContext.CurrentLanguage, workContext.CurrentCurrency, workContext.CurrentStore)).ToList();

            if (!products.IsNullOrEmpty())
            {
                var taskList = new List<Task>();
                taskList.Add(LoadProductsInventoriesAsync(products));
                taskList.Add(_pricingService.EvaluateProductPricesAsync(products));
                await Task.WhenAll(taskList.ToArray());
            }

            return new CatalogSearchResult
            {
                Products = new StaticPagedList<Product>(products, criteria.PageNumber, criteria.PageSize, result.ProductsTotalCount.Value),
                Aggregations = !result.Aggregations.IsNullOrEmpty() ? result.Aggregations.Select(x => x.ToWebModel(workContext.CurrentLanguage.CultureName)).ToArray() : new Aggregation[] { }
            };
        }


        /// <summary>
        /// Search products by given criteria 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public CatalogSearchResult SearchProducts(CatalogSearchCriteria criteria)
        {
            var workContext = _workContextFactory();
            criteria = criteria.Clone();
            //exclude categories
            criteria.ResponseGroup = criteria.ResponseGroup & (~CatalogSearchResponseGroup.WithCategories);
            //include products
            criteria.ResponseGroup = criteria.ResponseGroup | CatalogSearchResponseGroup.WithProducts;

            var searchCriteria = criteria.ToServiceModel(workContext);

            var result = _searchApi.SearchModuleSearch(searchCriteria);
            var products = result.Products.Select(x => x.ToWebModel(workContext.CurrentLanguage, workContext.CurrentCurrency, workContext.CurrentStore)).ToList();

            //Unable to make parallel call because its synchronous method (in future this information pricing and inventory will be getting from search index) and this lines can be removed
            _pricingService.EvaluateProductPrices(products);
            LoadProductsInventories(products);

            return new CatalogSearchResult
            {
                Products = new StaticPagedList<Product>(products, criteria.PageNumber, criteria.PageSize, result.ProductsTotalCount.Value),
                Aggregations = !result.Aggregations.IsNullOrEmpty() ? result.Aggregations.Select(x => x.ToWebModel(workContext.CurrentLanguage.CultureName)).ToArray() : new Aggregation[] { }
            };
        }

        #endregion

        private void LoadProductsDiscounts(IEnumerable<Product> products)
        {
            var workContext = _workContextFactory();
            var promotionContext = workContext.ToPromotionEvaluationContext(products);
            promotionContext.PromoEntries = products.Select(x => x.ToPromotionItem()).ToList();
            _promotionEvaluator.EvaluateDiscounts(promotionContext, products);
        }

        private async Task LoadProductsDiscountsAsync(IEnumerable<Product> products)
        {
            var workContext = _workContextFactory();
            var promotionContext = workContext.ToPromotionEvaluationContext(products);
            promotionContext.PromoEntries = products.Select(x => x.ToPromotionItem()).ToList();
            await _promotionEvaluator.EvaluateDiscountsAsync(promotionContext, products);
        }

        private async Task LoadProductsAssociationsAsync(IEnumerable<Product> products)
        {
            var allAssociations = products.SelectMany(x => x.Associations);
            var associatedProducts = await GetProductsAsync(allAssociations.Select(x=>x.ProductId).Distinct().ToArray(), ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemWithPrices | ItemResponseGroup.Seo);
            foreach (var association in allAssociations)
            {
                association.Product = associatedProducts.FirstOrDefault(x => x.Id == association.ProductId);
            }
        }
        private async Task LoadProductsInventoriesAsync(IEnumerable<Product> products)
        {
            var inventories = await _inventoryModuleApi.InventoryModuleGetProductsInventoriesAsync(products.Select(x => x.Id).ToList());
            foreach (var item in products)
            {
                item.Inventory = inventories.Where(x => x.ProductId == item.Id).Select(x => x.ToWebModel()).FirstOrDefault();
            }
        }

        private void LoadProductsInventories(IEnumerable<Product> products)
        {
            var inventories = _inventoryModuleApi.InventoryModuleGetProductsInventories(products.Select(x => x.Id).ToList());
            foreach (var item in products)
            {
                item.Inventory = inventories.Where(x => x.ProductId == item.Id).Select(x => x.ToWebModel()).FirstOrDefault();
            }
        }


    }
}