using System;
using System.Configuration;
using System.Linq;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Platform.Client.Security;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Test
{
    public class StorefrontTestBase
    {
        protected WorkContext GetTestWorkContext()
        {
            var apiClient = GetApiClient();
            var storeApi = new StoreModuleApi(apiClient);
            var commerceApi = new CommerceCoreModuleApi(apiClient);
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
            var apiAppId = ConfigurationManager.AppSettings["vc-public-ApiAppId"];
            var apiSecretKey = ConfigurationManager.AppSettings["vc-public-ApiSecretKey"];
            var hmacHandler = new HmacRestRequestHandler(apiAppId, apiSecretKey);
            var apiClient = new ApiClient(baseUrl, new VirtoCommerce.Client.Client.Configuration(), hmacHandler.PrepareRequest);
            return apiClient;
        }
    }
}
