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
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class PricingController : StorefrontControllerBase
    {
        private readonly IMarketingService _marketingService;
        private readonly IPricingModuleApi _pricingApi;
        private readonly IPromotionEvaluator _promotionEvaluator;

        public PricingController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IMarketingService marketingService,
            IPromotionEvaluator promotionEvaluator, IPricingModuleApi pricingApi)
            : base(workContext, urlBuilder)
        {
            _marketingService = marketingService;
            _pricingApi = pricingApi;
            _promotionEvaluator = promotionEvaluator;
        }

        // POST: /pricing/actualprices
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
            var promotionContext = WorkContext.ToPromotionEvaluationContext();
            promotionContext.PromoEntries = GetPromoEntries(products, prices);

            foreach (var product in products)
            {
                product.Currency = WorkContext.CurrentCurrency;
                product.Price = prices.FirstOrDefault(p => p.ProductId == product.Id);
            }
            await _promotionEvaluator.EvaluateDiscountsAsync(promotionContext, products);

            return Json(prices, JsonRequestBehavior.AllowGet);
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