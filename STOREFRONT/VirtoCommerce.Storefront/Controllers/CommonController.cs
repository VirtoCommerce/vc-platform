using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "CommonCachingProfile")]
    public class CommonController : StorefrontControllerBase
    {
        private readonly Country[] _countriesWithoutRegions;
        private readonly IStoreModuleApi _storeModuleApi;

        public CommonController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IStoreModuleApi storeModuleApi)
            : base(workContext, urlBuilder)
        {
            _storeModuleApi = storeModuleApi;
            _countriesWithoutRegions = workContext.AllCountries
                .Select(c => new Country { Name = c.Name, Code2 = c.Code2, Code3 = c.Code3, RegionType = c.RegionType })
                .ToArray();

        }

        /// <summary>
        /// GET : /contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult СontactUs(string viewName = "page.contact")
        {
            return View(viewName, base.WorkContext);
        }

        /// <summary>
        /// POST : /contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> СontactUs(ContactUsForm model, string viewName = "page.contact")
        {
            await _storeModuleApi.StoreModuleSendDynamicNotificationAnStoreEmailAsync(model.ToServiceModel(base.WorkContext));
            WorkContext.ContactUsForm = model;
            return View(viewName, base.WorkContext);
        }

        /// <summary>
        /// GET: common/setcurrency/{currency}
        /// Set current currency for current user session
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
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

        // GET: common/getcountries/json
        [HttpGet]
        public ActionResult GetCountries()
        {
            return Json(_countriesWithoutRegions, JsonRequestBehavior.AllowGet);
        }

        // GET: common/getregions/{countryCode}/json
        [HttpGet]
        public ActionResult GetRegions(string countryCode)
        {
            Country country = null;

            if (countryCode != null)
            {
                if (countryCode.Length == 3)
                {
                    country = WorkContext.AllCountries.FirstOrDefault(c => c.Code3.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
                }
                else if (countryCode.Length == 2)
                {
                    country = WorkContext.AllCountries.FirstOrDefault(c => c.Code2.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (country != null)
            {
                return Json(country.Regions, JsonRequestBehavior.AllowGet);
            }

            return HttpNotFound();
        }

        // GET: common/nostore
        [HttpGet]
        public ActionResult NoStore()
        {
            return View("NoStore");
        }

        // GET: /maintenance
        [HttpGet]
        public ActionResult Maintenance()
        {
            return View("maintenance", "empty", WorkContext);
        }
    }
}