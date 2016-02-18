using System;
using System.Linq;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;

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


        // GET: storefrontapi/{countryCode}/regions
        [HttpGet]
        public ActionResult GetRegions(string countryCode)
        {
            var country = WorkContext.AllCountries.FirstOrDefault(c => c.Code3.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
            if (country != null)
            {
                return Json(country.Regions, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }
    }
}