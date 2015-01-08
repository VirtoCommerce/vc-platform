using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient.Extensions
{
    using System.Configuration;

    public static class ContentClientExtension
    {
        public static ContentClient CreateContentClient(this CommerceClients source)
        {
            var connectionString = ConnectionHelper.GetConnectionString("vc-commerce-api");
            return CreateContentClient(source, connectionString);
        }

        public static ContentClient CreateContentClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new ContentClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }
    }
}
