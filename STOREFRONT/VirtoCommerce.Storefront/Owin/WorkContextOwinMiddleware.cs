using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using CacheManager.Core;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Owin
{
    /// <summary>
    /// Populate main work context with such commerce data as store, user profile, cart, etc.
    /// </summary>
    public class WorkContextOwinMiddleware : OwinMiddleware
    {
        private static readonly Country[] _allCountries = GetAllCounries();

        private readonly IStoreModuleApi _storeApi;
        private readonly IVirtoCommercePlatformApi _platformApi;
        private readonly ICommerceCoreModuleApi _commerceApi;
        private readonly ICustomerManagementModuleApi _customerApi;
        private readonly IPricingModuleApi _pricingModuleApi;
        private readonly ICartBuilder _cartBuilder;
        private readonly ICMSContentModuleApi _cmsApi;
        private readonly ICacheManager<object> _cacheManager;

        private readonly UnityContainer _container;

        public WorkContextOwinMiddleware(OwinMiddleware next, UnityContainer container)
            : base(next)
        {
            _storeApi = container.Resolve<IStoreModuleApi>();
            _platformApi = container.Resolve<IVirtoCommercePlatformApi>();
            _customerApi = container.Resolve<ICustomerManagementModuleApi>();
            _cartBuilder = container.Resolve<ICartBuilder>();
            _cmsApi = container.Resolve<ICMSContentModuleApi>();
            _pricingModuleApi = container.Resolve<IPricingModuleApi>();
            _commerceApi = container.Resolve<ICommerceCoreModuleApi>();
            _cacheManager = container.Resolve<ICacheManager<object>>();
            _container = container;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (IsStorefrontRequest(context.Request))
            {
                var workContext = _container.Resolve<WorkContext>();
                var urlBuilder = _container.Resolve<IStorefrontUrlBuilder>();

                // Initialize common properties
                workContext.RequestUrl = context.Request.Uri;
                workContext.AllCountries = _allCountries;
                workContext.AllCurrencies = await _cacheManager.GetAsync("GetAllCurrencies", "ApiRegion", async () => { return (await _commerceApi.CommerceGetAllCurrenciesAsync()).Select(x => x.ToWebModel()).ToArray(); });
                workContext.AllStores = await _cacheManager.GetAsync("GetAllStores", "ApiRegion", async () => { return await GetAllStoresAsync(workContext.AllCurrencies); });
                if (workContext.AllStores != null && workContext.AllStores.Any())
                {
                    var currentCustomerId = GetCurrentCustomerId(context);
                    workContext.CurrentCustomer = await _cacheManager.GetAsync("GetCustomer-" + currentCustomerId, "ApiRegion", async () => { return await GetCustomerAsync(context); });
                    MaintainAnonymousCustomerCookie(context, workContext);

                    // Initialize request specific properties
                    workContext.CurrentStore = GetStore(context, workContext.AllStores);
                    workContext.CurrentLanguage = GetLanguage(context, workContext.AllStores, workContext.CurrentStore);
                    workContext.CurrentCurrency = GetCurrency(context, workContext.CurrentStore);

                    //Do not load shopping cart and other for resource requests
                    if (!IsAssetRequest(context.Request.Uri))
                    {
                        //Shopping cart
                        await _cartBuilder.GetOrCreateNewTransientCartAsync(workContext.CurrentStore, workContext.CurrentCustomer, workContext.CurrentLanguage, workContext.CurrentCurrency);
                        workContext.CurrentCart = _cartBuilder.Cart;

                        var linkLists = await _cacheManager.GetAsync("GetLinkLists-" + workContext.CurrentStore.Id, "ApiRegion", async () => { return await _cmsApi.MenuGetListsAsync(workContext.CurrentStore.Id); });
                        workContext.CurrentLinkLists = linkLists != null ? linkLists.Select(ll => ll.ToWebModel(urlBuilder)).ToList() : null;

                        //Initialize catalog search criteria
                        workContext.CurrentCatalogSearchCriteria = GetSearchCriteria(workContext);

                        //Initialize blogs search criteria 
                        //TODO: read from query string
                        workContext.CurrentBlogSearchCritera = new Model.StaticContent.BlogSearchCriteria();
                        //Pricelists
                        var priceListCachey = String.Join("-", "EvaluatePriceLists", workContext.CurrentStore.Id, workContext.CurrentCustomer.Id);
                        workContext.CurrentPriceListIds = await _cacheManager.GetAsync(priceListCachey, "ApiRegion", async () =>
                            {
                                var pricingResult = await _pricingModuleApi.PricingModuleEvaluatePriceListsAsync(
                                    evalContextStoreId: workContext.CurrentStore.Id,
                                    evalContextCatalogId: workContext.CurrentStore.Catalog,
                                    evalContextCustomerId: workContext.CurrentCustomer.Id,
                                    evalContextQuantity: 1);
                                return pricingResult.Select(p => p.Id).ToList();
                            });
                    }
                }
            }

            await Next.Invoke(context);
        }


        protected virtual bool IsStorefrontRequest(IOwinRequest request)
        {
            return !request.Path.StartsWithSegments(new PathString("/admin"))
                && !request.Path.StartsWithSegments(new PathString("/areas/admin"))
                && !request.Path.StartsWithSegments(new PathString("/api"))
                && !request.Path.StartsWithSegments(new PathString("/favicon.ico"));
        }

        protected virtual async Task<Store[]> GetAllStoresAsync(IEnumerable<Currency> availCurrencies)
        {
            var stores = await _storeApi.StoreModuleGetStoresAsync();
            var result = stores.Select(s => s.ToWebModel(availCurrencies)).ToArray();
            return result;
        }

        protected virtual CatalogSearchCriteria GetSearchCriteria(WorkContext workContext)
        {
            var qs = HttpUtility.ParseQueryString(workContext.RequestUrl.Query);
            var retVal = CatalogSearchCriteria.Parse(qs);
            retVal.CatalogId = workContext.CurrentStore.Catalog;
            retVal.Currency = workContext.CurrentCurrency;
            retVal.Language = workContext.CurrentLanguage;
            return retVal;
        }

        private bool IsAssetRequest(Uri uri)
        {
            return uri.AbsolutePath.Contains("themes/assets");
        }

        private string GetCurrentCustomerId(IOwinContext context)
        {
            var retVal = context.Authentication.User.Identity.Name;
            if (!context.Authentication.User.Identity.IsAuthenticated)
            {
                retVal = context.Request.Cookies[StorefrontConstants.AnonymousCustomerIdCookie];
            }
            return retVal;
        }

        protected virtual async Task<Customer> GetCustomerAsync(IOwinContext context)
        {
            var customer = new Customer();

            if (context.Authentication.User.Identity.IsAuthenticated)
            {
                var user = await _platformApi.SecurityGetUserByNameAsync(context.Authentication.User.Identity.Name);
                if (user != null)
                {
                    var contact = await _customerApi.CustomerModuleGetContactByIdAsync(user.Id);
                    if (contact != null)
                    {
                        customer = contact.ToWebModel(user.UserName);
                        customer.HasAccount = true;
                    }
                }
            }

            if (!customer.HasAccount)
            {
                customer.Id = context.Request.Cookies[StorefrontConstants.AnonymousCustomerIdCookie];
                customer.UserName = StorefrontConstants.AnonymousUsername;
                customer.Name = StorefrontConstants.AnonymousUsername;
            }

            return customer;
        }

        protected virtual void MaintainAnonymousCustomerCookie(IOwinContext context, WorkContext workContext)
        {
            string anonymousCustomerId = context.Request.Cookies[StorefrontConstants.AnonymousCustomerIdCookie];

            if (workContext.CurrentCustomer.HasAccount)
            {
                if (!string.IsNullOrEmpty(anonymousCustomerId))
                {
                    // Remove anonymous customer cookie for registered customer
                    context.Response.Cookies.Append(StorefrontConstants.AnonymousCustomerIdCookie, string.Empty, new CookieOptions { Expires = DateTime.UtcNow.AddDays(-30) });
                }
            }
            else
            {
                if (string.IsNullOrEmpty(anonymousCustomerId))
                {
                    // Add anonymous customer cookie for nonregistered customer
                    anonymousCustomerId = Guid.NewGuid().ToString();
                    workContext.CurrentCustomer.Id = anonymousCustomerId;
                    context.Response.Cookies.Append(StorefrontConstants.AnonymousCustomerIdCookie, anonymousCustomerId, new CookieOptions { Expires = DateTime.UtcNow.AddDays(30) });
                }
            }
        }

        protected virtual Store GetStore(IOwinContext context, ICollection<Store> stores)
        {
            //Remove store name from url need to prevent writing store in routing
            var storeId = GetStoreIdFromUrl(context, stores);

            if (string.IsNullOrEmpty(storeId))
            {
                storeId = context.Request.Cookies[StorefrontConstants.StoreCookie];
            }

            if (string.IsNullOrEmpty(storeId))
            {
                storeId = ConfigurationManager.AppSettings["DefaultStore"];
            }

            var store = stores.FirstOrDefault(s => string.Equals(s.Id, storeId, StringComparison.OrdinalIgnoreCase));

            if (store == null)
            {
                store = stores.FirstOrDefault();
            }

            return store;
        }

        protected virtual string GetStoreIdFromUrl(IOwinContext context, ICollection<Store> stores)
        {
            //Try first find by store url (if it defined)
            var retVal = stores.Where(x => x.IsStoreUri(context.Request.Uri)).Select(x => x.Id).FirstOrDefault();
            if (retVal == null)
            {
                foreach (var store in stores)
                {
                    var pathString = new PathString("/" + store.Id);
                    PathString remainingPath;
                    if (context.Request.Path.StartsWithSegments(pathString, out remainingPath))
                    {
                        retVal = store.Id;
                        break;
                    }
                }
            }
            return retVal;
        }

        protected virtual Language GetLanguage(IOwinContext context, ICollection<Store> stores, Store store)
        {
            var languages = stores.SelectMany(s => s.Languages)
                .Union(stores.Select(s => s.DefaultLanguage))
                .Select(x => x.CultureName)
                .Distinct()
                .ToArray();

            //Get language from request url and remove it from from url need to prevent writing language in routing
            var languageCode = GetLanguageFromUrl(context, languages);

            //Get language from Cookies
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = context.Request.Cookies[StorefrontConstants.LanguageCookie];
            }
            var retVal = store.DefaultLanguage;
            //Get store default language if language not in the supported by stores list
            if (!string.IsNullOrEmpty(languageCode))
            {
                var language = new Language(languageCode);
                retVal = store.Languages.Contains(language) ? language : retVal;
            }
            return retVal;
        }

        protected virtual string GetLanguageFromUrl(IOwinContext context, string[] languages)
        {
            var requestPath = context.Request.Path.ToString();
            var retVal = languages.FirstOrDefault(x => requestPath.Contains(String.Format("/{0}/", x)));
            return retVal;
        }

        protected virtual Currency GetCurrency(IOwinContext context, Store store)
        {
            //Get currency from request url
            var currencyCode = context.Request.Query.Get("currency");
            //Next try get from Cookies
            if (string.IsNullOrEmpty(currencyCode))
            {
                currencyCode = context.Request.Cookies[StorefrontConstants.CurrencyCookie];
            }

            var retVal = store.DefaultCurrency;
            //Get store default currency if currency not in the supported by stores list
            if (!string.IsNullOrEmpty(currencyCode))
            {
                retVal = store.Currencies.FirstOrDefault(x => x.IsHasSameCode(currencyCode)) ?? retVal;
            }
            return retVal;
        }

        private static Country[] GetAllCounries()
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(GetRegionInfo)
                .Where(r => r != null)
                .ToList();

            var countriesJson = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/countries.json"));
            var countriesDict = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(countriesJson);

            var countries = countriesDict
                .Select(kvp => ParseCountry(kvp, regions))
                .Where(c => c.Code3 != null)
                .ToArray();

            return countries;
        }

        private static RegionInfo GetRegionInfo(CultureInfo culture)
        {
            RegionInfo result = null;

            try
            {
                result = new RegionInfo(culture.LCID);
            }
            catch
            {
                // ignored
            }

            return result;
        }

        private static Country ParseCountry(KeyValuePair<string, JObject> pair, List<RegionInfo> regions)
        {
            var region = regions.FirstOrDefault(r => string.Equals(r.EnglishName, pair.Key, StringComparison.OrdinalIgnoreCase));

            var country = new Country
            {
                Name = pair.Key,
                Code2 = region != null ? region.TwoLetterISORegionName : String.Empty,
                Code3 = region != null ? region.ThreeLetterISORegionName : String.Empty,
            };

            var provinceCodes = pair.Value["province_codes"].ToObject<Dictionary<string, string>>();
            if (provinceCodes != null && provinceCodes.Any())
            {
                country.Regions = provinceCodes
                    .Select(kvp => new CountryRegion { Name = kvp.Key, Code = kvp.Value })
                    .ToArray();
            }

            return country;
        }
    }
}
