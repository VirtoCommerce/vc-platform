using System;
using System.Linq;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
using System.Net;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [HandleJsonError]
    public class ApiCommonController : StorefrontControllerBase
    {
        private readonly Country[] _countriesWithoutRegions;

        public ApiCommonController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
            : base(workContext, urlBuilder)
        {
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
    }
}