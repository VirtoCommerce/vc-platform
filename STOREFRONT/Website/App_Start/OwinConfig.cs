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
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Owin;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Stores;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Cms;
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
            app.UseStageMarker(PipelineStage.ResolveCache);

            return app;
        }
        #endregion
    }

    public class SiteContextDataOwinMiddleware : OwinMiddleware
    {
        /// <summary>
        /// The store cookie
        /// </summary>
        protected virtual string StoreCookie { get { return "vcf.store"; } }
        
        protected virtual string PreviewThemeCookie { get { return "vcf.previewtheme"; } }

        /// <summary>
        /// The currency cookie
        /// </summary>
        protected virtual string CurrencyCookie { get { return "vcf.currency"; } }

        protected virtual string LanguageCookie { get { return "vcf.Language"; } }

        protected virtual string AnonymousCookie
        {
            get { return "vcf.AnonymousId"; }
        }

        #region Constructors and Destructors
        public SiteContextDataOwinMiddleware(OwinMiddleware next)
            : base(next)
        {
        }
        #endregion


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
            ctx.Language = language;

            var shop = this.GetStore(context, language);

            if (shop == null)
            {
                throw new HttpException(404, "Store Not Found");
            }

            var currency = GetStoreCurrency(context, shop);
            shop.Currency = currency;
            ctx.Shop = shop;
            ctx.Themes = await commerceService.GetThemesAsync(SiteContext.Current);

            // if language is not set, set it to default shop language
            if (String.IsNullOrEmpty(ctx.Language))
            {
                language = shop.DefaultLanguage;
                if (String.IsNullOrEmpty(language))
                {
                    throw new HttpException(404, "Store Language Not Designed");
                }
                
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(language);
                ctx.Language = language;
            }

            if (!this.IsResourceFile()) // only load settings for resource files, no need for other contents
            {
                // save info to the cookies
                context.Response.Cookies.Append(StoreCookie, shop.StoreId, new CookieOptions { Expires = DateTime.UtcNow.AddDays(30) });
                context.Response.Cookies.Append(LanguageCookie, ctx.Language, new CookieOptions { Expires = DateTime.UtcNow.AddDays(30) });
                context.Response.Cookies.Append(CurrencyCookie, shop.Currency, new CookieOptions { Expires = DateTime.UtcNow.AddDays(30) });

                if (context.Authentication.User != null && context.Authentication.User.Identity.IsAuthenticated)
                {
                    ctx.Customer = await customerService.GetCustomerAsync(
                        context.Authentication.User.Identity.Name, shop.StoreId);

                    if (ctx.Customer == null)
                    {
                        context.Authentication.SignOut();
                    }
                    else
                    {
                        ctx.CustomerId = ctx.Customer.Id;    
                    }
                }

                if (ctx.Customer == null)
                {
                    var cookie = context.Request.Cookies[AnonymousCookie];

                    if (string.IsNullOrEmpty(cookie))
                    {
                        cookie = Guid.NewGuid().ToString();

                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTime.UtcNow.AddDays(30)
                        };

                        context.Response.Cookies.Append(AnonymousCookie, cookie, cookieOptions);
                    }

                    ctx.CustomerId = cookie;
                }

                // TODO: detect if shop exists, user has access
                // TODO: store anonymous customer id in cookie and update and merge cart once customer is logged in

                ctx.Linklists = await commerceService.GetListsAsync(SiteContext.Current);
                ctx.PageTitle = ctx.Shop.Name;
                ctx.Collections = await commerceService.GetCollectionsAsync(SiteContext.Current);
                ctx.Pages = new PageCollection();
                ctx.Forms = commerceService.GetForms();
               

                var cart = await commerceService.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);
                if (cart == null)
                {
                    var dtoCart = new ApiClient.DataContracts.Cart.ShoppingCart
                    {
                        CreatedBy = ctx.CustomerId,
                        CreatedDate = DateTime.UtcNow,
                        Currency = shop.Currency,
                        CustomerId = ctx.CustomerId,
                        CustomerName = ctx.Customer != null ? ctx.Customer.Name : null,
                        LanguageCode = ctx.Language,
                        Name = "default",
                        StoreId = shop.StoreId
                    };

                    await commerceService.CreateCartAsync(dtoCart);
                    cart = await commerceService.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);
                }

                ctx.Cart = cart;

                if (context.Authentication.User.Identity.IsAuthenticated)
                {
                    var anonymousCookie = context.Request.Cookies[AnonymousCookie];

                    if (anonymousCookie != null)
                    {
                        var anonymousCart = await commerceService.GetCartAsync(ctx.StoreId, anonymousCookie);

                        if (anonymousCart != null)
                        {
                            ctx.Cart = await commerceService.MergeCartsAsync(anonymousCart);
                        }
                    }

                    context.Response.Cookies.Delete(AnonymousCookie);
                }

                ctx.PriceLists = await commerceService.GetPriceListsAsync(ctx.Shop.Catalog, shop.Currency, new TagQuery());
                ctx.Theme = commerceService.GetTheme(SiteContext.Current, this.ResolveTheme(shop, context));

                // update theme files
                await commerceService.UpdateThemeCacheAsync(SiteContext.Current);

                ctx.Blogs = commerceService.GetBlogs(SiteContext.Current);
            }
            else
            {
                ctx.Theme = commerceService.GetTheme(SiteContext.Current, this.ResolveTheme(shop, context));
            }

            ctx.Settings = commerceService.GetSettings(
                ctx.Theme.ToString(),
                context.Request.Path.HasValue && context.Request.Path.Value.Contains(".scss") ? "''" : null);

            ctx.CountryOptionTags = commerceService.GetCountryTags();

            if (ctx.Shop.Currency.Equals("GBP", StringComparison.OrdinalIgnoreCase) || ctx.Shop.Currency.Equals("USD", StringComparison.OrdinalIgnoreCase))
            {
                ctx.Shop.MoneyFormat = commerceService.CurrencyDictionary[ctx.Shop.Currency] + "{{ amount }}";
            }
            else
            {
                ctx.Shop.MoneyFormat = "{{ amount }} " + commerceService.CurrencyDictionary[ctx.Shop.Currency];
            }

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
                language = String.Empty;
            }

            //if (string.IsNullOrEmpty(language))
            //{
            //    language = SiteContext.Current.Shop.DefaultLanguage;
            //}

            return language;
        }

        protected virtual string GetStoreCurrency(IOwinContext context, Shop store)
        {
            var currency = context.Request.Query.Get("currency");

            if (String.IsNullOrEmpty(currency))
            {
                currency = context.Request.Cookies[CurrencyCookie];
            }

            if (String.IsNullOrEmpty(currency) ||
                !String.IsNullOrEmpty(currency) && !store.Currencies.Any(c => c.Equals(currency, StringComparison.OrdinalIgnoreCase)))
            {
                currency = store.Currency;
            }

            if (String.IsNullOrEmpty(currency))
            {
                currency = "USD";
            }

            return currency;
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
                var allStores = SiteContext.Current.Shops;

                var url = context.Request.Uri.AbsoluteUri.ToLower();
                var stores = (from s in allStores where 
                                  (!string.IsNullOrEmpty(s.Url) && url.Contains(s.Url)) ||
                                  (!string.IsNullOrEmpty(s.SecureUrl) && url.Contains(s.SecureUrl))
                              select s).ToArray();

                storeId = stores.Length > 0 ? stores[0].StoreId : String.Empty;

                if (String.IsNullOrEmpty(storeId))
                {
                    // try getting store from the cookie
                    storeId = context.Request.Cookies[StoreCookie];

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
                    /*
                else
                {
                    return null;
                }
                     * */
            }

            if (store == null)
            {
                if (loadDefault)
                {

                    context.Response.Cookies.Delete(StoreCookie);
                    storeId = ConfigurationManager.AppSettings["DefaultStore"];
                    store =
                        SiteContext.Current.Shops.SingleOrDefault(
                            s => s.StoreId.Equals(storeId, StringComparison.OrdinalIgnoreCase));
                }
            }

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
            #region Preview theme functionality
            var previewTheme = context.Request.Query["previewtheme"];
            if (!String.IsNullOrEmpty(previewTheme)) // save in cookie and return
            {
                context.Response.Cookies.Append(PreviewThemeCookie, previewTheme);
            }
            else
            {
                previewTheme = context.Request.Cookies[PreviewThemeCookie];
            }

            if (!String.IsNullOrEmpty(previewTheme))
                return previewTheme;
            #endregion

            var theme = ConfigurationManager.AppSettings["Theme"];
            if (shop.Metafields != null)
            {
                var shopMetaFields = shop.Metafields["global"];
                if (shopMetaFields != null)
                {
                    object themeObject;
                    if (shop.Metafields["global"].TryGetValue("defaultThemeName", out themeObject))
                    {
                        return themeObject.ToString();
                    }
                }
            }

            return theme;
        }
        #endregion
    }
}