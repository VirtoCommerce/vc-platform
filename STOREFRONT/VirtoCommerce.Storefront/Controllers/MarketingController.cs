using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class MarketingController : StorefrontControllerBase
    {
        private readonly IMarketingModuleApi _marketingApi;
        private readonly IMarketingService _marketingService;
        private readonly IPricingModuleApi _pricingApi;

        public MarketingController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IMarketingModuleApi marketingApi,
            IMarketingService marketingService, IPricingModuleApi pricingApi)
            : base(workContext, urlBuilder)
        {
            _marketingApi = marketingApi;
            _marketingService = marketingService;
            _pricingApi = pricingApi;
        }

       // GET: /marketing/dynamiccontent/{placeName}/json
       [HttpGet]
       public async Task<ActionResult> GetDynamicContentJson(string placeName)
        {
            var htmlContent = await _marketingService.GetDynamicContentHtmlAsync(WorkContext.CurrentStore.Id, placeName);

            return Json(htmlContent, JsonRequestBehavior.AllowGet);
        }

        // POST: /marketing/actualprices
        [HttpPost]
        public async Task<ActionResult> GetActualProductPricesJson(Product[] products)
        {
            var prices = new List<ProductPrice>();

            if (products == null)
            {
                return Json(prices, JsonRequestBehavior.AllowGet);
            }

            var pricesResponse = await _pricingApi.PricingModuleEvaluatePricesAsync(
                evalContextProductIds: products.Select(p => p.Id).ToList(),
                evalContextCatalogId: WorkContext.CurrentStore.Catalog,
                evalContextCurrency: WorkContext.CurrentCurrency.Code,
                evalContextCustomerId: WorkContext.CurrentCustomer.Id,
                evalContextLanguage: WorkContext.CurrentLanguage.CultureName,
                evalContextStoreId: WorkContext.CurrentStore.Id);

            if (pricesResponse == null)
            {
                return Json(prices, JsonRequestBehavior.AllowGet);
            }

            prices = pricesResponse.Select(p => p.ToWebModel()).ToList();
            var promotionContext = new PromotionEvaluationContext
            {
                CartPromoEntries = GetCartPromoEntries(WorkContext.CurrentCart),
                Currency = WorkContext.CurrentCurrency,
                CustomerId = WorkContext.CurrentCustomer.Id,
                IsRegisteredUser = WorkContext.CurrentCustomer.HasAccount,
                Language = WorkContext.CurrentLanguage,
                PromoEntries = GetPromoEntries(products, prices),
                StoreId = WorkContext.CurrentStore.Id
            };

            var rewards = await _marketingService.EvaluatePromotionRewardsAsync(promotionContext);
            var validRewards = rewards.Where(r => r.RewardType == PromotionRewardType.CatalogItemAmountReward && r.IsValid);
            foreach (var price in prices)
            {
                var validReward = validRewards.FirstOrDefault(r => r.ProductId == price.ProductId);
                if (validReward != null)
                {
                    price.ActiveDiscount = validReward.ToDiscountWebModel(price.SalePrice.Amount, 1, price.Currency);
                }
            }

            return Json(prices, JsonRequestBehavior.AllowGet);
        }

        private ICollection<PromotionProductEntry> GetCartPromoEntries(ShoppingCart cart)
        {
            var cartPromoEntries = new List<PromotionProductEntry>();

            foreach (var lineItem in cart.Items)
            {
                cartPromoEntries.Add(new PromotionProductEntry
                {
                    CatalogId = lineItem.CatalogId,
                    CategoryId = lineItem.CategoryId,
                    Price = lineItem.SalePrice,
                    ProductId = lineItem.ProductId,
                    Quantity = lineItem.Quantity
                });
            }

            return cartPromoEntries;
        }

        private ICollection<PromotionProductEntry> GetPromoEntries(IEnumerable<Product> products, IEnumerable<ProductPrice> prices)
        {
            var promoEntries = new List<PromotionProductEntry>();

            foreach (var product in products)
            {
                var price = prices.FirstOrDefault(p => p.ProductId == product.Id);
                promoEntries.Add(new PromotionProductEntry
                {
                    CatalogId = product.CatalogId,
                    CategoryId = product.CategoryId,
                    Price = price != null ? price.SalePrice : null,
                    ProductId = product.Id,
                    Quantity = 1
                });
            }

            return promoEntries;
        }
    }
}