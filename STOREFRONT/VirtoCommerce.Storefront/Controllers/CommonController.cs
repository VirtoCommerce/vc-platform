using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("common")]
    public class CommonController : StorefrontControllerBase
    {
        private readonly Country[] _countriesWithoutRegions;

        public CommonController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
            : base(workContext, urlBuilder)
        {
            _countriesWithoutRegions = workContext.AllCountries
                .Select(c => new Country { Name = c.Name, Code2 = c.Code2, Code3 = c.Code3 })
                .ToArray();
        }

        /// <summary>
        /// Set current currency for current user session
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Route("setcurrency/{currency}")]
        public ActionResult SetCurrency(string currency, string returnUrl = "")
        {
            var cookie = new HttpCookie(StorefrontConstants.CurrencyCookie)
            {
                HttpOnly = true,
                Value = currency
            };
            var cookieExpires = 24 * 365; // TODO make configurable
            cookie.Expires = DateTime.Now.AddHours(cookieExpires);
            HttpContext.Response.Cookies.Remove(StorefrontConstants.CurrencyCookie);
            HttpContext.Response.Cookies.Add(cookie);

            //home page  and prevent open redirection attack
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "~/";
            }

            return StoreFrontRedirect(returnUrl);
        }

        // GET: /getcountries/json
        [HttpGet]
        [Route("getcountries/json")]
        public ActionResult GetCountries()
        {
            return Json(_countriesWithoutRegions, JsonRequestBehavior.AllowGet);
        }

        // GET: /getregions/{countryCode}/json
        [HttpGet]
        [Route("getregions/{countryCode}/json")]
        public ActionResult GetRegions(string countryCode)
        {
            var country = WorkContext.AllCountries.FirstOrDefault(c => c.Code3.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

            return Json(country?.Regions, JsonRequestBehavior.AllowGet);
        }
    }
}