using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;

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

        protected virtual string StoreCookie { get { return "vcf.store"; } }
        protected virtual string LanguageCookie { get { return "vcf.language"; } }
        protected virtual string CurrencyCookie { get { return "vcf.currency"; } }

        public WorkContextOwinMiddleware(OwinMiddleware next, IStoreModuleApi storeApi, IVirtoCommercePlatformApi platformApi, ICustomerManagementModuleApi customerApi)
            : base(next)
        {
            _storeApi = storeApi;
            _platformApi = platformApi;
            _customerApi = customerApi;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var workContext = context.Get<WorkContext>();

            // Initialize common properties: stores, user profile, cart
            workContext.AllStores = await GetAllStoresAsync();
            workContext.Customer = await GetCustomerAsync(context.Authentication.User.Identity);

            // Initialize request specific properties: store, language, currency
            workContext.CurrentStore = GetStore(context, workContext.AllStores);
            workContext.CurrentLanguage = GetLanguage(context, workContext.AllStores, workContext.CurrentStore);
            workContext.CurrentCurrency = GetCurrency(context, workContext.CurrentStore);

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
            var storeName = RemoveStoreNameFromUrl(context, stores);

            if (string.IsNullOrEmpty(storeName))
            {
                storeName = context.Request.Cookies[StoreCookie];
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

        protected virtual string GetLanguage(IOwinContext context, ICollection<Store> stores, Store store)
        {
            var languages = stores.SelectMany(s => s.Languages)
                .Union(stores.Select(s => s.DefaultLanguage))
                .Distinct()
                .ToArray();

            var language = RemoveLanguageFromUrl(context, languages);

            if (string.IsNullOrEmpty(language))
            {
                language = context.Request.Cookies[LanguageCookie];
            }

            if (string.IsNullOrEmpty(language))
            {
                language = "en-US";
            }

            if (string.IsNullOrEmpty(language) || !store.Languages.Any(l => l.Equals(language, StringComparison.OrdinalIgnoreCase)))
            {
                language = store.DefaultLanguage;
            }

            return language;
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

        protected virtual string GetCurrency(IOwinContext context, Store store)
        {
            var currency = context.Request.Query.Get("currency");

            if (string.IsNullOrEmpty(currency))
            {
                currency = context.Request.Cookies[CurrencyCookie];
            }

            if (string.IsNullOrEmpty(currency))
            {
                currency = "USD";
            }

            if (string.IsNullOrEmpty(currency) || !store.Currencies.Any(c => c.Equals(currency, StringComparison.OrdinalIgnoreCase)))
            {
                currency = store.DefaultCurrency;
            }

            return currency;
        }

        protected virtual void RewritePath(IOwinContext context, PathString newPath)
        {
            context.Request.Path = newPath;
            HttpContext.Current.RewritePath("~" + newPath.Value);
        }
    }
}
