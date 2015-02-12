namespace VirtoCommerce.ApiClient.Extensions
{
    #region

    using System;
    using System.Configuration;

    using VirtoCommerce.ApiClient.Utilities;

    #endregion

    public static class CartClientExtension
    {
        #region Public Methods and Operators

        public static CartClient CreateCartClient(this CommerceClients source)
        {
            var connectionString = ClientContext.Configuration.ConnectionString;
            return CreateCartClient(source, connectionString);
        }

        public static CartClient CreateCartClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new CartClient(
                new Uri(connectionString),
                new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }

        #endregion
    }
}