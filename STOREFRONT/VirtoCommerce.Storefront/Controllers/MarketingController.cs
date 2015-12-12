using System;
using System.Linq;
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
            string htmlContent = null;
            var dynamicContentResult = await _marketingApi.MarketingModuleDynamicContentEvaluateDynamicContentAsync(WorkContext.CurrentStore.Id, placeName);
            if (dynamicContentResult != null)
            {
                var htmlDynamicContent = dynamicContentResult.FirstOrDefault(dc => !string.IsNullOrEmpty(dc.ContentType) && dc.ContentType.Equals("Html", StringComparison.OrdinalIgnoreCase));
                if (htmlDynamicContent != null)
                {
                    var dynamicProperty = htmlDynamicContent.DynamicProperties.FirstOrDefault(dp => !string.IsNullOrEmpty(dp.Name) && dp.Name.Equals("Html", StringComparison.OrdinalIgnoreCase));
                    if (dynamicProperty != null && dynamicProperty.Values.Any(v => v.Value != null))
                    {
                        htmlContent = dynamicProperty.Values.First().Value.ToString();
                    }
                }
            }

            return Json(htmlContent, JsonRequestBehavior.AllowGet);
        }
    }
}