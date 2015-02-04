using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.Configuration.Security;

namespace VirtoCommerce.ApiClient.Extensions
{
    using System.Configuration;

    public static class SecurityClientExtension
    {
        public static SecurityClient CreateSecurityClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString + "security/";
            return CreateSecurityClient(source, connectionString);
        }

        public static SecurityClient CreateSecurityClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new SecurityClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }
    }
}
