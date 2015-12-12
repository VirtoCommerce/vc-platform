using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
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
        private readonly ICustomerManagementModuleApi _customerApi;
        private readonly ICartBuilder _cartBuilder;
        private readonly UnityContainer _container;

        public WorkContextOwinMiddleware(OwinMiddleware next, UnityContainer container)
            : base(next)
        {
            _storeApi = container.Resolve<IStoreModuleApi>();
            _platformApi = container.Resolve<IVirtoCommercePlatformApi>();
            _customerApi = container.Resolve<ICustomerManagementModuleApi>();
            _cartBuilder = container.Resolve<ICartBuilder>();
            _container = container;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var workContext = _container.Resolve<WorkContext>();

            // Initialize common properties
            workContext.RequestUrl = context.Request.Uri;
            workContext.AllCountries = _allCountries;
            workContext.AllStores = await GetAllStoresAsync();
            workContext.CurrentCustomer = await GetCustomerAsync(context);
            MaintainAnonymousCustomerCookie(context, workContext);

            // Initialize request specific properties
            workContext.CurrentStore = GetStore(context, workContext.AllStores);
            workContext.CurrentLanguage = GetLanguage(context, workContext.AllStores, workContext.CurrentStore);
            workContext.CurrentCurrency = GetCurrency(context, workContext.CurrentStore);

            await _cartBuilder.GetOrCreateNewTransientCartAsync(workContext.CurrentStore, workContext.CurrentCustomer, workContext.CurrentLanguage, workContext.CurrentCurrency);
            workContext.CurrentCart = _cartBuilder.Cart;

            //Initialize catalog search context
            workContext.CurrentCatalogSearchCriteria = GetSearchCriteria(workContext);

            await Next.Invoke(context);
        }

        protected virtual async Task<Store[]> GetAllStoresAsync()
        {
            var stores = await _storeApi.StoreModuleGetStoresAsync();
            var result = stores.Select(s => s.ToWebModel()).ToArray();
            return result;
        }

        protected virtual CatalogSearchCriteria GetSearchCriteria(WorkContext workContext)
        {
            var result = new CatalogSearchCriteria
            {
                CatalogId = workContext.CurrentStore.Catalog,
                Currency = workContext.CurrentCurrency,
                Language = workContext.CurrentLanguage
            };

            var qs = HttpUtility.ParseQueryString(workContext.RequestUrl.Query);

            result.Keyword = qs.Get("keyword");
            result.PageNumber = Convert.ToInt32(qs.Get("page") ?? "1");

            // tags=name1:value1,value2,value3;name2:value1,value2,value3
            result.Terms = (qs.GetValues("terms") ?? new string[0])
                .SelectMany(s => s.Split(';'))
                .Select(s => s.Split(':'))
                .Where(a => a.Length == 2)
                .SelectMany(a => a[1].Split(',').Select(v => new Term { Name = a[0], Value = v }))
                .ToArray();

            //TODO: get other parameters from query sting
            return result;
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
                customer.UserName = "Anonymous";
                customer.Name = "Anonymous";
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
            var storeId = RemoveStoreIdFromUrl(context, stores);

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

        protected virtual string RemoveStoreIdFromUrl(IOwinContext context, ICollection<Store> stores)
        {
            string removedStoreName = null;

            foreach (var store in stores)
            {
                var pathString = new PathString("/" + store.Id);
                PathString remainingPath;
                if (context.Request.Path.StartsWithSegments(pathString, out remainingPath))
                {
                    removedStoreName = store.Id;
                    RewritePath(context, remainingPath);
                    break;
                }
            }

            return removedStoreName;
        }

        protected virtual Language GetLanguage(IOwinContext context, ICollection<Store> stores, Store store)
        {
            var languages = stores.SelectMany(s => s.Languages)
                .Union(stores.Select(s => s.DefaultLanguage))
                .Select(x => x.CultureName)
                .Distinct()
                .ToArray();

            //Get language from request url and remove it from from url need to prevent writing language in routing
            var languageCode = RemoveLanguageFromUrl(context, languages);

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

        protected virtual string RemoveLanguageFromUrl(IOwinContext context, string[] languages)
        {
            string removedLanguage = null;

            foreach (var language in languages)
            {
                var pathString = new PathString("/" + language);
                PathString remainingPath;
                if (context.Request.Path.StartsWithSegments(pathString, out remainingPath))
                {
                    removedLanguage = language;
                    RewritePath(context, remainingPath);
                    break;
                }
            }

            return removedLanguage;
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
                var currency = new Currency(EnumUtility.SafeParse(currencyCode, store.DefaultCurrency.CurrencyCode));
                retVal = store.Currencies.Contains(currency) ? currency : retVal;
            }
            return retVal;
        }

        protected virtual void RewritePath(IOwinContext context, PathString newPath)
        {
            context.Request.Path = newPath;
            HttpContext.Current.RewritePath("~" + newPath.Value);
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
                Code2 = region?.TwoLetterISORegionName,
                Code3 = region?.ThreeLetterISORegionName,
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
