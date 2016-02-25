using System;
using System.Configuration;
using System.Linq;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Test
{
    public class StorefrontTestBase
    {
        protected WorkContext GetTestWorkContext()
        {
            var apiClientCfg = new Client.Client.Configuration(GetApiClient());
            var storeApi = new StoreModuleApi(apiClientCfg);
            var commerceApi = new CommerceCoreModuleApi(apiClientCfg);
            var allStores = storeApi.StoreModuleGetStores().Select(x => x.ToWebModel());
            var defaultStore = allStores.FirstOrDefault(x => string.Equals(x.Id, "Electronics", StringComparison.InvariantCultureIgnoreCase));
            var currencies = commerceApi.CommerceGetAllCurrencies().Select(x => x.ToWebModel(defaultStore.DefaultLanguage));
            defaultStore.SyncCurrencies(currencies, defaultStore.DefaultLanguage);

            var retVal = new WorkContext
            {
                AllCurrencies = defaultStore.Currencies,
                CurrentLanguage = defaultStore.DefaultLanguage,
                CurrentCurrency = defaultStore.DefaultCurrency,
                CurrentStore = defaultStore,
                CurrentCart = new Model.Cart.ShoppingCart(defaultStore.DefaultCurrency, defaultStore.DefaultLanguage),
                CurrentCustomer = new CustomerInfo
                {
                    Id = Guid.NewGuid().ToString()
                }
            };
            return retVal;
        }

        protected ApiClient GetApiClient()
        {
            var baseUrl = ConfigurationManager.ConnectionStrings["VirtoCommerceBaseUrl"].ConnectionString;
            var apiClient = new HmacApiClient(baseUrl, ConfigurationManager.AppSettings["vc-public-ApiAppId"], ConfigurationManager.AppSettings["vc-public-ApiSecretKey"]);
            return apiClient;
        }
    }
}
