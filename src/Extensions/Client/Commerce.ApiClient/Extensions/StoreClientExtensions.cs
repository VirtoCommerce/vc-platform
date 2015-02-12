namespace VirtoCommerce.ApiClient.Extensions
{
    #region

    using System;
    using System.Configuration;

    using VirtoCommerce.ApiClient.Utilities;

    #endregion

    public static class StoreClientExtensions
    {
        #region Public Methods and Operators

        public static StoreClient CreateStoreClient(this CommerceClients source)
        {
            var connectionString = String.Format("{0}{1}/", ClientContext.Configuration.ConnectionString, "mp");
            return CreateStoreClient(source, connectionString);
        }

        public static StoreClient CreateStoreClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new StoreClient(
                new Uri(connectionString),
                new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }

        #endregion
    }
}