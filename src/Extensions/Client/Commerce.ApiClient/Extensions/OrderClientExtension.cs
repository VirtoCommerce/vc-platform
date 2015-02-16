using System;
using System.Configuration;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class OrderClientExtension
    {
        public static OrderClient CreateOrderClient(this CommerceClients source)
        {
            return source.CreateOrderClient(ClientContext.Session.StoreId);
        }

        public static OrderClient CreateOrderClient(this CommerceClients source, string storeId)
        {
            var connectionString = String.Format("{0}{1}/{2}/", ClientContext.Configuration.ConnectionString, "mp", storeId);

            return CreateOrderClientWithUri(source, connectionString);
        }

        public static OrderClient CreateOrderClientWithUri(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new OrderClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));

            return client;
        }
    }
}