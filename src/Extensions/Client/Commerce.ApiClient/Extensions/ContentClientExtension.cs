using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.Configuration.DynamicContent;

namespace VirtoCommerce.ApiClient.Extensions
{
    using System.Configuration;

    public static class ContentClientExtension
    {
        public static ContentClient CreateDefaultContentClient(this CommerceClients source, string language = "")
        {
            var session = ClientContext.Session;
            language = String.IsNullOrEmpty(language) ? session.Language : language;

            var connectionString = String.Format("{0}{1}/{2}/", ClientContext.Configuration.ConnectionString, "mp", language);
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
