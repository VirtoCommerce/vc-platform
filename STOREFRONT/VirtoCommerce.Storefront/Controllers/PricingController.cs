using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Pricing.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class PricingController : StorefrontControllerBase
    {
        private readonly IMarketingService _marketingService;
        private readonly IPricingService _pricingService;
        private readonly IPromotionEvaluator _promotionEvaluator;


        public PricingController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IMarketingService marketingService,
            IPromotionEvaluator promotionEvaluator, IPricingService pricingService)
            : base(workContext, urlBuilder)
        {
            _marketingService = marketingService;
            _pricingService = pricingService;
            _promotionEvaluator = promotionEvaluator;
        }

        // POST: /pricing/actualprices
        [HttpPost]
        public async Task<ActionResult> GetActualProductPricesJson(Product[] products)
        {
            if(products == null)
            {
                throw new ArgumentNullException("products");
            }
            //Evaluate products prices
            await _pricingService.EvaluateProductPricesAsync(products);
            //Evaluate discounts
            var promotionContext = WorkContext.ToPromotionEvaluationContext();
            promotionContext.PromoEntries = products.Select(x => x.ToPromotionItem()).ToList();
            await _promotionEvaluator.EvaluateDiscountsAsync(promotionContext, products);
            var retVal = products.Select(x => x.Price).ToArray();
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

     

    }
}