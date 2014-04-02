using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Web.Client.Helpers;

namespace Initial.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Provides an application-wide implementation of the <see cref="P:System.Web.UI.PartialCachingAttribute.VaryByCustom" /> property.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that contains information about the current Web request.</param>
        /// <param name="custom">The custom string that specifies which cached response is used to respond to the current request.</param>
        /// <returns>If the value of the <paramref name="custom" /> parameter is "browser", the browser's <see cref="P:System.Web.Configuration.HttpCapabilitiesBase.Type" />; otherwise, null.</returns>
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {

            var varyString = base.GetVaryByCustomString(context, custom) ?? string.Empty;

            if (SettingsHelper.OutputCacheEnabled)
            {
                foreach (var key in custom.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.Equals(key, "storeparam", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += StoreHelper.CustomerSession.StoreId;
                    }
                    if (string.Equals(key, "currency", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += StoreHelper.CustomerSession.Currency;
                    }
                    if (string.Equals(key, "pricelist", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += CacheHelper.CreateCacheKey("_pl", StoreHelper.CustomerSession.Pricelists);
                    }
                    if (string.Equals(key, "filters", StringComparison.OrdinalIgnoreCase))
                    {
                        var qs = Request.QueryString;
                        var pageSize = qs.AllKeys.Contains("pageSize") ? qs["pageSize"] : StoreHelper.GetCookieValue("pagesizecookie");
                        var sort = qs.AllKeys.Contains("sort") ? qs["sort"] : StoreHelper.GetCookieValue("sortcookie");
                        var sortorder = qs.AllKeys.Contains("sortorder") ? qs["sortorder"] : StoreHelper.GetCookieValue("sortordercookie") ?? "asc";

                        varyString += string.Format("filters_{0}_{1}_{2}", pageSize, sort, sortorder);
                    }                
                }
            }

            return varyString;
        }
    }
}