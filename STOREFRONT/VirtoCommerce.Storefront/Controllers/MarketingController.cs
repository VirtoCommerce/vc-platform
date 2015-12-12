using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("marketing")]
    public class MarketingController : StorefrontControllerBase
    {
        private readonly IMarketingModuleApi _marketingApi;

        public MarketingController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IMarketingModuleApi marketingApi)
            : base(workContext, urlBuilder)
        {
            _marketingApi = marketingApi;
        }

       // GET: /marketing/dynamiccontent/{placeName}/json
       [HttpGet]
       [Route("dynamiccontent/{placeName}/json")]
        public async Task<ActionResult> GetDynamicContentJson(string placeName)
        {
            var dynamicContent = await _marketingApi.MarketingModuleDynamicContentEvaluateDynamicContentAsync(WorkContext.CurrentStore.Id, placeName);

            return Json(dynamicContent, JsonRequestBehavior.AllowGet);
        }
    }
}