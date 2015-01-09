using System;
using VirtoCommerce.ApiClient.Utilities;
using System.Configuration;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class SeoClientExtensions
    {
        public static SeoClient CreateSeoClient(this CommerceClients source)
        {
            var connectionString = ConnectionHelper.GetConnectionString("vc-commerce-api");
            return CreateSeoClient(source, connectionString);
        }

        public static SeoClient CreateSeoClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new SeoClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }
    }
}
