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

        // GET: /marketing/dynamiccontent/json
        //[HttpGet]
        //[Route("dynamiccontent/json")]
        //public async Task<ActionResult> GetDynamicContentJson(string[] placeNames)
        //{
        //    return Json("", JsonRequestBehavior.AllowGet);
        //}
    }
}