using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Core.DataContracts;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Binders;

namespace VirtoCommerce.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcSiteMapProviderConfig.Register(DependencyResolver.Current);

            ModelBinders.Binders[typeof(BrowseQuery)] = new BrowseQueryBinder();
            ModelBinders.Binders[typeof(CategoryPathModel)] = new CategoryPathModelBinder();
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

            //varyString += UserHelper.CustomerSession.Language;//allways vary by language

            //if (SettingsHelper.OutputCacheEnabled)
            {
                foreach (var key in custom.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.Equals(key, "registered", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += StoreHelper.CustomerSession.IsRegistered ? "registered" : "anonymous";
                    }
                    if (string.Equals(key, "storeparam", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += StoreHelper.CustomerSession.StoreId;
                    }
                    if (string.Equals(key, "currency", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += StoreHelper.CustomerSession.Currency;
                    }
                    //if (string.Equals(key, "pricelist", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    varyString += CacheHelper.CreateCacheKey("_pl", UserHelper.CustomerSession.Pricelists);
                    //}
                    if (string.Equals(key, "filters", StringComparison.OrdinalIgnoreCase))
                    {
                        var qs = Request.QueryString;
                        var pageSize = qs.AllKeys.Contains("pageSize") ? qs["pageSize"] : StoreHelper.GetCookieValue("pagesizecookie");
                        var sort = qs.AllKeys.Contains("sort") ? qs["sort"] : StoreHelper.GetCookieValue("sortcookie");
                        var sortorder = qs.AllKeys.Contains("sortorder") ? qs["sortorder"] : StoreHelper.GetCookieValue("sortordercookie") ?? "asc";

                        varyString += string.Format("filters_{0}_{1}_{2}", pageSize, sort, sortorder);
                    }

                    //if (string.Equals(key, "cart", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    //This method is called from System.Web.Caching module before customerId set 
                    //    if (string.IsNullOrEmpty(UserHelper.CustomerSession.CustomerId))
                    //    {
                    //        if (UserHelper.CustomerSession.IsRegistered)
                    //        {
                    //            var account = UserHelper.UserClient.GetAccountByUserName(UserHelper.CustomerSession.Username);
                    //            if (account != null)
                    //            {
                    //                UserHelper.CustomerSession.CustomerId = account.MemberId;
                    //            }
                    //        }
                    //    }

                    //    if (string.IsNullOrEmpty(UserHelper.CustomerSession.CustomerId))
                    //    {
                    //        UserHelper.CustomerSession.CustomerId = context.Request.AnonymousID ??
                    //                                                Guid.NewGuid().ToString();
                    //    }

                    //    var ch = new CartHelper(CartHelper.CartName);

                    //    if (ch.LineItems.Any())
                    //    {
                    //        varyString +=
                    //            new CartHelper(CartHelper.CartName).LineItems.Select(x => x.LineItemId)
                    //                                               .Aggregate((x, y) => x + y);
                    //    }

                    //}
                }
            }

            return varyString;
        }
    }
}
