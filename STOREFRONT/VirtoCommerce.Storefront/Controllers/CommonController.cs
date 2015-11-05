using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("common")]
    public class CommonController : Controller
    {
        private WorkContext _workContext;
        private IStorefrontUrlBuilder _storefrontUrlBuilder;
        public CommonController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
        {
            _workContext = workContext;
            _storefrontUrlBuilder = urlBuilder;
        }

        [Route("setcurrency/{currency}")]
        public ActionResult SetCurrency(string currency, string returnUrl = "")
        {
          
            var cookie = new HttpCookie(StorefrontConstants.CurrencyCookie);
            cookie.HttpOnly = true;
            cookie.Value = currency;
            var cookieExpires = 24 * 365; //TODO make configurable
            cookie.Expires = DateTime.Now.AddHours(cookieExpires);
            HttpContext.Response.Cookies.Remove(StorefrontConstants.CurrencyCookie);
            HttpContext.Response.Cookies.Add(cookie);

            //home page  and prevent open redirection attack
            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                returnUrl = _storefrontUrlBuilder.ToAbsolute("~/", _workContext.CurrentStore.Id, _workContext.CurrentLanguage.CultureName);

            return Redirect(returnUrl);
        }

    }
}