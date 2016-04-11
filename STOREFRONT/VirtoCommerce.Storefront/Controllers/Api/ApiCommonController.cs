using System;
using System.Linq;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
using System.Net;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [HandleJsonError]
    public class ApiCommonController : StorefrontControllerBase
    {
        private readonly IStoreModuleApi _storeModuleApi;
        private readonly Country[] _countriesWithoutRegions;

        public ApiCommonController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IStoreModuleApi storeModuleApi)
            : base(workContext, urlBuilder)
        {
            _storeModuleApi = storeModuleApi;
            _countriesWithoutRegions = workContext.AllCountries
             .Select(c => new Country { Name = c.Name, Code2 = c.Code2, Code3 = c.Code3, RegionType = c.RegionType })
             .ToArray();
        }

        // GET: storefrontapi/countries
        [HttpGet]
        public ActionResult GetCountries()
        {
            return Json(_countriesWithoutRegions, JsonRequestBehavior.AllowGet);
        }

        // GET: storefrontapi/countries/{countryCode}/regions
        [HttpGet]
        public ActionResult GetCountryRegions(string countryCode)
        {
            var country = WorkContext.AllCountries.FirstOrDefault(c => c.Code2.Equals(countryCode, StringComparison.InvariantCultureIgnoreCase) || c.Code3.Equals(countryCode, StringComparison.InvariantCultureIgnoreCase));
            if (country != null)
            {
                return Json(country.Regions, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // POST: storefrontapi/feedback
        [HttpPost]
        public async Task<ActionResult> Feedback(ContactUsForm model)
        {
            await _storeModuleApi.StoreModuleSendDynamicNotificationAnStoreEmailAsync(model.ToServiceModel(WorkContext));

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}