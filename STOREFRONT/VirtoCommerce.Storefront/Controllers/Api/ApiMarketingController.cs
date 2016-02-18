using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [HandleJsonError]
    public class ApiMarketingController : StorefrontControllerBase
    {
        private readonly IMarketingService _marketingService;
        public ApiMarketingController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IMarketingService marketingService)
            : base(workContext, urlBuilder)
        {
            _marketingService = marketingService;
        }

        // GET: storefrontapi/marketing/dynamiccontent/{placeName}
        [HttpGet]
        public async Task<ActionResult> GetDynamicContent(string placeName)
        {
            var htmlContent = await _marketingService.GetDynamicContentHtmlAsync(WorkContext.CurrentStore.Id, placeName);

            return Json(htmlContent);
        }
    }
}