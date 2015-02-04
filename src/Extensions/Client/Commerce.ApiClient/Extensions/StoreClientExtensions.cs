using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.Configuration.Store;

namespace VirtoCommerce.ApiClient.Extensions
{
    using System.Configuration;

    public static class StoreClientExtensions
    {
        public static StoreClient CreateStoreClient(this CommerceClients source)
        {
            var connectionString = String.Format("{0}{1}/", ClientContext.Configuration.ConnectionString, "mp");
            return CreateStoreClient(source, connectionString);
        }

        public static StoreClient CreateStoreClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new StoreClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }
    }
}
