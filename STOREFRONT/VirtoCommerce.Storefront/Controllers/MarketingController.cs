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
    public class MarketingController : StorefrontControllerBase
    {
        private readonly IMarketingService _marketingService;
        private readonly IPricingModuleApi _pricingApi;
        private readonly IPromotionEvaluator _promotionEvaluator;

        public MarketingController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IMarketingService marketingService,
            IPromotionEvaluator promotionEvaluator, IPricingModuleApi pricingApi)
            : base(workContext, urlBuilder)
        {
            _marketingService = marketingService;
            _pricingApi = pricingApi;
            _promotionEvaluator = promotionEvaluator;
        }

       // GET: /marketing/dynamiccontent/{placeName}/json
       [HttpGet]
       public async Task<ActionResult> GetDynamicContentJson(string placeName)
        {
            var htmlContent = await _marketingService.GetDynamicContentHtmlAsync(WorkContext.CurrentStore.Id, placeName);

            return Json(htmlContent, JsonRequestBehavior.AllowGet);
        }
    }
}