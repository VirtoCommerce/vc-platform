using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiWebClient.Clients.Extensions
{
    using System.Configuration;

    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Utilities;
    using VirtoCommerce.ApiWebClient.Helpers;

    public static class CachedClientExtensions
    {
        public static BrowseCachedClient CreateBrowseCachedClient(this CommerceClients source, string language = "")
        {
            var session = ClientContext.Session;
            language = String.IsNullOrEmpty(language) ? session.Language : language;
            // http://localhost/admin/api/mp/{0}/{1}/
            var connectionString = ConnectionHelper.GetConnectionString("VirtoCommerce") + String.Format("{0}/{1}/", session.CatalogId, language);
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new BrowseCachedClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }    
    }
}
