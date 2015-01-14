using System;
using VirtoCommerce.ApiClient.Utilities;
using System.Configuration;
using VirtoCommerce.Web.Core.Configuration.Application;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class SeoClientExtensions
    {
        public static SeoClient CreateDefaultSeoClient(this CommerceClients source, params object[] options)
        {
            var connectionString = string.Format(AppConfigConfiguration.Instance.Connection.DataServiceUri, options);
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
