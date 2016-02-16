using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using CacheManager.Core;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.LinkList.Services;
using VirtoCommerce.Storefront.Model.Quote.Services;

namespace VirtoCommerce.Storefront.Owin
{
    /// <summary>
    /// Populate main work context with such commerce data as store, user profile, cart, etc.
    /// </summary>
    public sealed class WorkContextOwinMiddleware : OwinMiddleware
    {
        private static readonly Country[] _allCountries = GetAllCounries();

        private readonly IStoreModuleApi _storeApi;
        private readonly IVirtoCommercePlatformApi _platformApi;
        private readonly ICommerceCoreModuleApi _commerceApi;
        private readonly IPricingModuleApi _pricingModuleApi;
        private readonly ICartBuilder _cartBuilder;
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;
        private readonly ICMSContentModuleApi _cmsApi;
        private readonly ICustomerService _customerService;
        private readonly IMenuLinkListService _linkListService;
        private readonly ICacheManager<object> _cacheManager;

        private readonly UnityContainer _container;

        public WorkContextOwinMiddleware(OwinMiddleware next, UnityContainer container)
            : base(next)
        {
            _storeApi = container.Resolve<IStoreModuleApi>();
            _platformApi = container.Resolve<IVirtoCommercePlatformApi>();
            _cartBuilder = container.Resolve<ICartBuilder>();
            _quoteRequestBuilder = container.Resolve<IQuoteRequestBuilder>();
            _cmsApi = container.Resolve<ICMSContentModuleApi>();
            _pricingModuleApi = container.Resolve<IPricingModuleApi>();
            _commerceApi = container.Resolve<ICommerceCoreModuleApi>();
            _cacheManager = container.Resolve<ICacheManager<object>>();
            _customerService = container.Resolve<ICustomerService>();
            _linkListService = container.Resolve<IMenuLinkListService>();
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
                workContext.AllStores = await _cacheManager.GetAsync("GetAllStores", "ApiRegion", async () => { return await GetAllStoresAsync(); });
                if (workContext.AllStores != null && workContext.AllStores.Any())
                {
                    // Initialize request specific properties
                    workContext.CurrentStore = GetStore(context, workContext.AllStores);
                    workContext.CurrentLanguage = GetLanguage(context, workContext.AllStores, workContext.CurrentStore);
                    workContext.AllCurrencies = await _cacheManager.GetAsync("GetAllCurrencies-" + workContext.CurrentLanguage.CultureName, "ApiRegion", async () => { return (await _commerceApi.CommerceGetAllCurrenciesAsync()).Select(x => x.ToWebModel(workContext.CurrentLanguage)).ToArray(); });
                    //Sync store currencies with avail in system
                    foreach (var store in workContext.AllStores)
                    {
                        store.SyncCurrencies(workContext.AllCurrencies, workContext.CurrentLanguage);
                        store.CurrentSeoInfo = store.SeoInfos.FirstOrDefault(x => x.Language == workContext.CurrentLanguage);
                    }

                    //Set current currency
                    workContext.CurrentCurrency = GetCurrency(context, workContext.CurrentStore);

                    var qs = HttpUtility.ParseQueryString(workContext.RequestUrl.Query);
                    //Initialize catalog search criteria
                    workContext.CurrentCatalogSearchCriteria = new CatalogSearchCriteria(qs);
                    workContext.CurrentCatalogSearchCriteria.CatalogId = workContext.CurrentStore.Catalog;
                    workContext.CurrentCatalogSearchCriteria.Currency = workContext.CurrentCurrency;
                    workContext.CurrentCatalogSearchCriteria.Language = workContext.CurrentLanguage;

                    workContext.CurrentOrderSearchCriteria = new Model.Order.OrderSearchCriteria(qs);
                    workContext.CurrentQuoteSearchCriteria = new Model.Quote.QuoteSearchCriteria(qs);

                    //Current customer
                    workContext.CurrentCustomer = await GetCustomerAsync(context);
                    MaintainAnonymousCustomerCookie(context, workContext);

                    //Do not load shopping cart and other for resource requests
                    if (!IsAssetRequest(context.Request.Uri))
                    {
                        //Shopping cart
                        await _cartBuilder.GetOrCreateNewTransientCartAsync(workContext.CurrentStore, workContext.CurrentCustomer, workContext.CurrentLanguage, workContext.CurrentCurrency);
                        workContext.CurrentCart = _cartBuilder.Cart;

                        if (workContext.CurrentStore.QuotesEnabled)
                        {
                            await _quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, workContext.CurrentCustomer, workContext.CurrentLanguage, workContext.CurrentCurrency);
                            workContext.CurrentQuoteRequest = _quoteRequestBuilder.QuoteRequest;
                        }

                        var linkLists = await _cacheManager.GetAsync("GetAllStoreLinkLists-" + workContext.CurrentStore.Id, "ApiRegion", async () => await _linkListService.LoadAllStoreLinkListsAsync(workContext.CurrentStore.Id) );
                        workContext.CurrentLinkLists = linkLists.Where(x => x.Language == workContext.CurrentLanguage).ToList();


                        //Initialize blogs search criteria 
                        //TODO: read from query string
                        workContext.CurrentBlogSearchCritera = new Model.StaticContent.BlogSearchCriteria(qs);

                        //Pricelists
                        var pricelistCacheKey = string.Join("-", "EvaluatePriceLists", workContext.CurrentStore.Id, workContext.CurrentCustomer.Id);
                        workContext.CurrentPricelists = await _cacheManager.GetAsync(pricelistCacheKey, "ApiRegion", async () =>
                        {
                            var evalContext = new VirtoCommerceDomainPricingModelPriceEvaluationContext
                            {
                                StoreId = workContext.CurrentStore.Id,
                                CatalogId = workContext.CurrentStore.Catalog,
                                CustomerId = workContext.CurrentCustomer.Id,
                                Quantity = 1
                            };
                            var pricingResult = await _pricingModuleApi.PricingModuleEvaluatePriceListsAsync(evalContext);
                            return pricingResult.Select(p => p.ToWebModel()).ToList();
                        });
                    }
                }
            }

            await Next.Invoke(context);
        }


        private bool IsStorefrontRequest(IOwinRequest request)
        {
            return !request.Path.StartsWithSegments(new PathString("/admin"))
                && !request.Path.StartsWithSegments(new PathString("/areas/admin"))
                && !request.Path.StartsWithSegments(new PathString("/api"))
                && !request.Path.StartsWithSegments(new PathString("/favicon.ico"));
        }

        private async Task<Store[]> GetAllStoresAsync()
        {
            var stores = await _storeApi.StoreModuleGetStoresAsync();
            var result = stores.Select(s => s.ToWebModel()).ToArray();
            return result.Any() ? result : null;
        }

     
        private bool IsAssetRequest(Uri uri)
        {
            return uri.AbsolutePath.Contains("themes/assets") || !string.IsNullOrEmpty(Path.GetExtension(uri.ToString()));
        }

        private async Task<CustomerInfo> GetCustomerAsync(IOwinContext context)
        {
            CustomerInfo retVal = new CustomerInfo();

            if (context.Authentication.User.Identity.IsAuthenticated)
            {
                var sidClaim = context.Authentication.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
                var userId = sidClaim != null ? sidClaim.Value : null;
                if (userId == null)
                {
                    //If somehow claim not found in user cookies need load user by name from API
                    var user = await _platformApi.SecurityGetUserByNameAsync(context.Authentication.User.Identity.Name);
                    if (user != null)
                    {
                        userId = user.Id;
                    }
                }

                if (userId != null)
                {
                    retVal = await _customerService.GetCustomerByIdAsync(userId) ?? retVal;
                    retVal.Id = userId;
                    retVal.UserName = context.Authentication.User.Identity.Name;
                    retVal.IsRegisteredUser = true;
                }
            }

            if (!retVal.IsRegisteredUser)
            {
                retVal.Id = context.Request.Cookies[StorefrontConstants.AnonymousCustomerIdCookie];
                retVal.UserName = StorefrontConstants.AnonymousUsername;
                retVal.FullName = StorefrontConstants.AnonymousUsername;
            }

            return retVal;
        }

        private void MaintainAnonymousCustomerCookie(IOwinContext context, WorkContext workContext)
        {
            string anonymousCustomerId = context.Request.Cookies[StorefrontConstants.AnonymousCustomerIdCookie];

            if (workContext.CurrentCustomer.IsRegisteredUser)
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

        private Store GetStore(IOwinContext context, ICollection<Store> stores)
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

        private string GetStoreIdFromUrl(IOwinContext context, ICollection<Store> stores)
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

        private Language GetLanguage(IOwinContext context, ICollection<Store> stores, Store store)
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

        private string GetLanguageFromUrl(IOwinContext context, string[] languages)
        {
            var requestPath = context.Request.Path.ToString();
            var retVal = languages.FirstOrDefault(x => requestPath.Contains(string.Format("/{0}/", x)));
            return retVal;
        }

        private static Currency GetCurrency(IOwinContext context, Store store)
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
                retVal = store.Currencies.FirstOrDefault(x => x.Equals(currencyCode)) ?? retVal;
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
                Code2 = region != null ? region.TwoLetterISORegionName : string.Empty,
                Code3 = region != null ? region.ThreeLetterISORegionName : string.Empty,
                RegionType = pair.Value["label"] != null ? pair.Value["label"].ToString() : null
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
