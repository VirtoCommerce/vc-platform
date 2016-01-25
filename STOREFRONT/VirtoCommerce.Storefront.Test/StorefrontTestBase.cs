using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Test
{
    public class StorefrontTestBase
    {
        protected WorkContext GetTestWorkContext()
        {
            var apiClientCfg = new Client.Client.Configuration(GetApiClient());
            var storeApi = new StoreModuleApi(apiClientCfg);
            var commerceApi = new CommerceCoreModuleApi(apiClientCfg);
            var allStores = storeApi.StoreModuleGetStores().Select(x=>x.ToWebModel());
            var defautStore = allStores.FirstOrDefault(x => string.Equals(x.Id, "Electronics", StringComparison.InvariantCultureIgnoreCase));
            var currencies = commerceApi.CommerceGetAllCurrencies().Select(x => x.ToWebModel(defautStore.DefaultLanguage));
            defautStore.SyncCurrencies(currencies, defautStore.DefaultLanguage);

            var retVal = new WorkContext
            {
                AllCurrencies = defautStore.Currencies,
                CurrentLanguage = defautStore.DefaultLanguage,
                CurrentCurrency = defautStore.DefaultCurrency,
                CurrentStore = defautStore
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
