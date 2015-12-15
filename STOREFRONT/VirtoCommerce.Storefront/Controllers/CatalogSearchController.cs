using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("search")]
    public class CatalogSearchController : StorefrontControllerBase
    {
        private readonly ICatalogSearchService _searchService;
        private readonly IMarketingModuleApi _marketingApi;

        public CatalogSearchController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICatalogSearchService searchService,
            IMarketingModuleApi marketingApi)
            : base(workContext, urlBuilder)
        {
            _searchService = searchService;
            _marketingApi = marketingApi;
        }

        /// <summary>
        /// This method called from SeoRoute when url contains slug for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ActionResult> CategoryBrowsing(string categoryId)
        {
            WorkContext.CurrentCatalogSearchCriteria.CategoryId = categoryId;
            WorkContext.CurrentCatalogSearchResult = await _searchService.SearchAsync(WorkContext.CurrentCatalogSearchCriteria);

            return View("collection", WorkContext);
        }

        // GET: /search/{categoryId}/actualproductprices/json
        [HttpGet]
        [Route("{categoryId}/actualproductprices/json")]
        public async Task<ActionResult> ActualProductPricesJson(string categoryId)
        {
            var searchCriteria = new CatalogSearchCriteria
            {
                CategoryId = categoryId
            };

            var products = new List<Product>();

            var searchResult = await _searchService.SearchAsync(searchCriteria);
            if (searchResult.Products != null)
            {
                products = searchResult.Products.ToList();
            }

            var validRewards = await EvaluatePromotionRewardsAsync(products, WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency, WorkContext.CurrentLanguage);
            foreach (var product in products)
            {
                var productReward = validRewards.FirstOrDefault(r => !string.IsNullOrEmpty(r.RewardType) &&
                    r.RewardType.Equals("CatalogItemAmountReward") &&
                    r.ProductId == product.Id);
                if (productReward != null)
                {
                    product.Price.ActiveDiscount = productReward.ToDiscountWebModel(product.Price.SalePrice.Amount, WorkContext.CurrentCurrency);
                    product.Price.SalePrice -= product.Price.ActiveDiscount.Amount;
                }
            }

            return Json(products.Select(p => p.Price), JsonRequestBehavior.AllowGet);
        }

        private async Task<ICollection<VirtoCommerceMarketingModuleWebModelPromotionReward>> EvaluatePromotionRewardsAsync(ICollection<Product> products, Store store, Customer customer, Currency currency, Language language)
        {
            var rewards = new List<VirtoCommerceMarketingModuleWebModelPromotionReward>();

            var promotionContext = new VirtoCommerceDomainMarketingModelPromotionEvaluationContext
            {
                Currency = currency.Code,
                CustomerId = customer.Id,
                Language = language.CultureName,
                StoreId = store.Id,
                PromoEntries = products.Select(p => p.ToPromotionItem()).ToList()
            };

            var allRewards = await _marketingApi.MarketingModulePromotionEvaluatePromotionsAsync(promotionContext);
            var validRewards = allRewards.Where(r => r.IsValid.HasValue && r.IsValid.Value);
            if (validRewards.Any())
            {
                rewards = validRewards.ToList();
            }

            return rewards;
        }
    }
}