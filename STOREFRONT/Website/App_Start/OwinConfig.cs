#region
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Stores;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Extensions;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Routing;
using VirtoCommerce.Web.Models.Services;

#endregion

namespace VirtoCommerce.Web
{
    public static class OwinConfig
    {
        #region Public Methods and Operators
        public static IAppBuilder UseSiteContext(this IAppBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            app.Use(typeof(SiteContextDataOwinMiddleware));

            return app;
        }
        #endregion
    }

    public class SiteContextDataOwinMiddleware : OwinMiddleware
    {
        #region Constructors and Destructors
        public SiteContextDataOwinMiddleware(OwinMiddleware next)
            : base(next)
        {
        }
        #endregion

        const string anonymousCookieName = "vc-anonymous-id";

        #region Public Methods and Operators
        public override async Task Invoke(IOwinContext context)
        {
            var customerService = new CustomerService();
            var commerceService = new CommerceService();
            var ctx = SiteContext.Current;

            ctx.LoginProviders = GetExternalLoginProviders(context).ToArray();

            // Need to load language for all files, since translations are used within css and js
            // the order of execution is very important when initializing context
            // 1st: initialize some sort of context, especially get a list of all shops first
            // other methods will rely on that to be performance efficient
            // 2nd: find current shop from url, which context with shops will be used for
            ctx.Shops = await commerceService.GetShopsAsync();

            // Get current language
            var language = this.GetLanguage(context).ToSpecificLangCode();
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(language);
            ctx.Language = language;

            var shop = this.GetStore(context, language);

            if (shop == null)
            {
                throw new HttpException(404, "Store Not Found");
            }

            ctx.Shop = shop;
            ctx.Themes = await commerceService.GetThemesAsync();

            // if language is not set, set it to default shop language
            if (String.IsNullOrEmpty(ctx.Language))
            {
                language = shop.DefaultLanguage;
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(language);
                ctx.Language = language;
            }

            if (!this.IsResourceFile()) // only load settings for resource files, no need for other contents
            {
                if (context.Authentication.User != null && context.Authentication.User.Identity.IsAuthenticated)
                {
                    ctx.Customer = await customerService.GetCustomerAsync(
                        context.Authentication.User.Identity.Name, shop.StoreId);

                    context.Response.Cookies.Delete(anonymousCookieName);
                }
                else
                {
                    string cookie = context.Request.Cookies[anonymousCookieName];

                    if (string.IsNullOrEmpty(cookie))
                    {
                        cookie = Guid.NewGuid().ToString();
                        context.Response.Cookies.Append(anonymousCookieName, cookie);
                    }

                    ctx.Customer = new Customer() { Id = cookie };
                }

                // TODO: detect if shop exists, user has access
                // TODO: store anonymous customer id in cookie and update and merge cart once customer is logged in

                ctx.Linklists = await commerceService.GetListsAsync();
                ctx.PageTitle = ctx.Shop.Name;
                ctx.Collections = await commerceService.GetCollectionsAsync();
                ctx.Pages = new PageCollection();
                ctx.Forms = commerceService.GetForms();
                ctx.Cart = await commerceService.GetCurrentCartAsync(); 
                ctx.PriceLists = await commerceService.GetPriceListsAsync(ctx.Shop.Catalog, ctx.Shop.Currency, new TagQuery());
                ctx.Theme = commerceService.GetTheme(this.ResolveTheme(shop, context));

                // update theme files
                await commerceService.UpdateThemeCacheAsync();
            }
            else
            {
                ctx.Theme = commerceService.GetTheme(this.ResolveTheme(shop, context));
            }

            ctx.Settings = commerceService.GetSettings(
                ctx.Theme.ToString(),
                context.Request.Path.HasValue && context.Request.Path.Value.Contains(".scss") ? "''" : null);

            ctx.CountryOptionTags = commerceService.GetCountryTags();

            context.Set("vc_sitecontext", ctx);

            await this.Next.Invoke(context);
        }
        #endregion

        #region Methods
        protected virtual string GetLanguage(IOwinContext context)
        {
            string language = null;

            var mvcContext = new HttpContextWrapper(HttpContext.Current);
            var routeData = RouteTable.Routes.GetRouteData(mvcContext);

            if (routeData != null)
            {
                language = this.GetLanguageFromRoute(routeData.Values);
            }

            if (string.IsNullOrEmpty(language))
            {
                language = this.GetLanguageFromUrl(context.Request.Uri.Segments);
            }

            if (string.IsNullOrEmpty(language))
            {
                language = "en-US";
            }

            //if (string.IsNullOrEmpty(language))
            //{
            //    language = SiteContext.Current.Shop.DefaultLanguage;
            //}

            return language;
        }

        protected virtual Shop GetStore(IOwinContext context, string language)
        {
            var loadDefault = true;
            var mvcContext = new HttpContextWrapper(HttpContext.Current);
            var routeData = RouteTable.Routes.GetRouteData(mvcContext);

            string storeId = null;

            if (routeData != null)
            {
                storeId = this.GetStoreIdFromRoute(routeData.Values);
            }

            if (string.IsNullOrEmpty(storeId))
            {
                storeId = this.GetStoreIdFromUrl(context.Request.Uri.Segments);
            }

            Shop store = null;
            if (String.IsNullOrEmpty(storeId))
            {
                // try getting store from URL
                //storeid = storeClient.GetStoreIdByUrl(context.Request.Url.AbsoluteUri);
                if (String.IsNullOrEmpty(storeId))
                {
                    // try getting store from the cookie
                    // storeid = StoreHelper.GetCookieValue(StoreCookie, false);

                    // try getting default store from settings
                    if (String.IsNullOrEmpty(storeId))
                    {
                        storeId = ConfigurationManager.AppSettings["DefaultStore"];
                    }
                }
            }

            if (!String.IsNullOrEmpty(storeId))
            {
                store = SiteContext.Current.GetShopBySlug(storeId, language);

                if (store != null)
                {
                    if (store.State != StoreState.Closed)
                    {
                        loadDefault = false;
                    }
                    else
                    {
                        store = null;
                    }
                }
                else
                {
                    return null;
                }
            }

            if (store == null)
            {
                if (loadDefault)
                {
                    //StoreHelper.ClearCookie(StoreCookie, String.Empty, false);
                    storeId = ConfigurationManager.AppSettings["DefaultStore"];
                    store =
                        SiteContext.Current.Shops.SingleOrDefault(
                            s => s.StoreId.Equals(storeId, StringComparison.OrdinalIgnoreCase));
                }
            }

            store.Checkout.GuestLogin = true;

            return store;
        }

        protected virtual bool IsResourceFile()
        {
            var path = HttpContext.Current.Request.Url.PathAndQuery.ToLower();
            var appPath = HttpContext.Current.Request.ApplicationPath;

            if (appPath != null && appPath.Length > 1)
            {
                appPath = appPath + "/";
            }

            if (path.StartsWith(appPath + "themes/") || path.StartsWith(appPath + "content/")
                || path.StartsWith(appPath + "scripts/") || path.StartsWith(appPath + "favicon")
                || path.StartsWith(appPath + "global/"))
            {
                return true;
            }

            return false;
        }

        private string GetLanguageFromRoute(RouteValueDictionary values)
        {
            if (values != null && values.ContainsKey(Constants.Language))
            {
                return values[Constants.Language].ToString();
            }
            return null;
        }

        private string GetLanguageFromUrl(IEnumerable<string> urlSegments)
        {
            foreach (var urlSegment in urlSegments)
            {
                var languageCandidate = HttpUtility.UrlDecode(urlSegment.Replace("/", ""));
                if (string.IsNullOrEmpty(languageCandidate))
                {
                    continue;
                }

                var constraintsRegEx = string.Format("^({0})$", Constants.LanguageRegex);

                if (Regex.IsMatch(
                    languageCandidate,
                    constraintsRegEx,
                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled))
                {
                    return languageCandidate.ToLowerInvariant();
                }
            }

            return null;
        }

        private ICollection<LoginProvider> GetExternalLoginProviders(IOwinContext context)
        {
            var providersModel = new List<LoginProvider>();

            var providers = context.Authentication.GetExternalAuthenticationTypes();

            foreach (var provider in providers)
            {
                providersModel.Add(new LoginProvider
                {
                    AuthenticationType = provider.AuthenticationType,
                    Caption = provider.Caption,
                    Properties = provider.Properties
                });
            }

            return providersModel;
        }

        private string GetStoreIdFromRoute(RouteValueDictionary values)
        {
            if (values != null && values.ContainsKey(Constants.Store))
            {
                return values[Constants.Store].ToString();
            }
            return null;
        }

        private string GetStoreIdFromUrl(IEnumerable<string> urlSegments)
        {
            foreach (var urlSegment in urlSegments)
            {
                var storeCandidate = HttpUtility.UrlDecode(urlSegment.Replace("/", ""));

                if (string.IsNullOrEmpty(storeCandidate))
                {
                    continue;
                }

                var foundStore = SiteContext.Current.GetShopBySlug(storeCandidate);

                if (foundStore != null)
                {
                    return foundStore.StoreId;
                }
            }

            return null;
        }

        private string ResolveTheme(Shop shop, IOwinContext context)
        {
            var theme = ConfigurationManager.AppSettings["Theme"];
            var shopMetaFields = shop.Metafields["global"];
            if (shopMetaFields != null)
            {
                object themeObject;
                if (shop.Metafields["global"].TryGetValue("defaultThemeName", out themeObject))
                {
                    return themeObject.ToString();
                }
            }

            return theme;
        }
        #endregion
    }
}