using System;
using System.Configuration;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class OrderClientExtension
    {
        public static OrderClient CreateOrderClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreateOrderClient(source, connectionString);
        }

        public static OrderClient CreateOrderClient(this CommerceClients source, string serviceUrl)
        {
            var client = new OrderClient(new Uri(serviceUrl), source.CreateAzureSubscriptionMessageProcessingHandler());
            return client;
        }
    }
}