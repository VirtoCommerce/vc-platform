namespace VirtoCommerce.ApiClient.Extensions
{
    #region

    using System;
    using System.Configuration;

    using VirtoCommerce.ApiClient.Utilities;

    #endregion

    public static class SecurityClientExtension
    {
        #region Public Methods and Operators

        public static SecurityClient CreateSecurityClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString + "security/";
            return CreateSecurityClient(source, connectionString);
        }

        public static SecurityClient CreateSecurityClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new SecurityClient(
                new Uri(connectionString),
                new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }

        #endregion
    }
}