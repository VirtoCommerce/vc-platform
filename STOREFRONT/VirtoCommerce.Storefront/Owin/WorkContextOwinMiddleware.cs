using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using VirtoCommerce.Client.Api;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Owin
{
    /// <summary>
    /// Populate main work context with such commerce data as store, user profile, cart, etc.
    /// </summary>
    public class WorkContextOwinMiddleware : OwinMiddleware
    {
        private readonly IStoreModuleApi _storeApi;
        private readonly IVirtoCommercePlatformApi _platformApi;
        private readonly ICustomerManagementModuleApi _customerApi;
        private readonly UnityContainer _container;


        public WorkContextOwinMiddleware(OwinMiddleware next, UnityContainer container)
            : base(next)
        {
            _storeApi = container.Resolve<IStoreModuleApi>();
            _platformApi = container.Resolve<IVirtoCommercePlatformApi>();
            _customerApi = container.Resolve<ICustomerManagementModuleApi>();
            _container = container;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var workContext = _container.Resolve<WorkContext>();
            // Initialize common properties: stores, user profile, cart
            workContext.AllStores = await GetAllStoresAsync();
            workContext.Customer = await GetCustomerAsync(context.Authentication.User.Identity);

            // Initialize request specific properties: store, language, currency
            workContext.CurrentStore = GetStore(context, workContext.AllStores);
            workContext.CurrentLanguage = GetLanguage(context, workContext.AllStores, workContext.CurrentStore);
            workContext.CurrentCurrency = GetCurrency(context, workContext.CurrentStore);

            workContext.CurrentPage = 1;

            await Next.Invoke(context);
        }


        protected virtual async Task<Store[]> GetAllStoresAsync()
        {
            var stores = await _storeApi.StoreModuleGetStoresAsync();
            var result = stores.Select(s => s.ToWebModel()).ToArray();
            return result;
        }

        protected virtual async Task<Customer> GetCustomerAsync(IIdentity identity)
        {
            Customer result = null;

            if (identity.IsAuthenticated)
            {
                var user = await _platformApi.SecurityGetUserByNameAsync(identity.Name);
                if (user != null)
                {
                    var contact = await _customerApi.CustomerModuleGetContactByIdAsync(user.Id);
                    if (contact != null)
                    {
                        result = contact.ToWebModel();
                    }
                }
            }

            return result;
        }

        protected virtual Store GetStore(IOwinContext context, ICollection<Store> stores)
        {
            //Remove store name from url need to prevent writing store in routing
            var storeName = RemoveStoreNameFromUrl(context, stores);

            if (string.IsNullOrEmpty(storeName))
            {
                storeName = context.Request.Cookies[StorefrontConstants.StoreCookie];
            }

            if (string.IsNullOrEmpty(storeName))
            {
                storeName = ConfigurationManager.AppSettings["DefaultStore"];
            }

            var store = stores.FirstOrDefault(s => string.Equals(s.Name, storeName, StringComparison.OrdinalIgnoreCase));

            if (store == null)
            {
                store = stores.FirstOrDefault();
            }

            return store;
        }

        protected virtual string RemoveStoreNameFromUrl(IOwinContext context, ICollection<Store> stores)
        {
            string removedStoreName = null;

            foreach (var store in stores)
            {
                var pathString = new PathString("/" + store.Name);
                PathString remainingPath;
                if (context.Request.Path.StartsWithSegments(pathString, out remainingPath))
                {
                    removedStoreName = store.Name;
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
            if (!String.IsNullOrEmpty(languageCode))
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
            if (String.IsNullOrEmpty(currencyCode))
            {
                currencyCode = context.Request.Cookies[StorefrontConstants.CurrencyCookie];
            }

            var retVal = store.DefaultCurrency;
            //Get store default currency if currency not in the supported by stores list
            if (!String.IsNullOrEmpty(currencyCode))
            {
                var currency = new Currency(EnumUtility.SafeParse<CurrencyCodes>(currencyCode, store.DefaultCurrency.CurrencyCode));
                retVal = store.Currencies.Contains(currency) ? currency : retVal;
            }
            return retVal;
        }

        protected virtual void RewritePath(IOwinContext context, PathString newPath)
        {
            context.Request.Path = newPath;
            HttpContext.Current.RewritePath("~" + newPath.Value);
        }
    }
}
