using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Pricing.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [HandleJsonError]
    public class ApiPricingController : StorefrontControllerBase
    {
        private readonly IMarketingService _marketingService;
        private readonly IPricingService _pricingService;
        private readonly IPromotionEvaluator _promotionEvaluator;

        public ApiPricingController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IMarketingService marketingService,
            IPromotionEvaluator promotionEvaluator, IPricingService pricingService)
            : base(workContext, urlBuilder)
        {
            _marketingService = marketingService;
            _pricingService = pricingService;
            _promotionEvaluator = promotionEvaluator;
        }

        // POST: storefrontapi/pricing/actualprices
        [HttpPost]
        public async Task<ActionResult> GetActualProductPrices(Product[] products)
        {
            if (products != null)
            {
                //Evaluate products prices
                await _pricingService.EvaluateProductPricesAsync(products);
                //Evaluate discounts
                var promotionContext = WorkContext.ToPromotionEvaluationContext(products);
                promotionContext.PromoEntries = products.Select(x => x.ToPromotionItem()).ToList();

                await _promotionEvaluator.EvaluateDiscountsAsync(promotionContext, products);
                var retVal = products.Select(x => x.Price).ToArray();

                return Json(retVal);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}