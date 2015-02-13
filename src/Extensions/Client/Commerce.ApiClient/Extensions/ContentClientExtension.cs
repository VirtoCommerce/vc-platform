namespace VirtoCommerce.ApiClient.Extensions
{
    #region

    using System;
    using System.Configuration;
    using System.Threading;

    using VirtoCommerce.ApiClient.Utilities;

    #endregion

    public static class ContentClientExtension
    {
        #region Public Methods and Operators

        public static ContentClient CreateContentClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new ContentClient(
                new Uri(connectionString),
                new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }

        public static ContentClient CreateDefaultContentClient(this CommerceClients source)
        {
            var language = Thread.CurrentThread.CurrentUICulture.ToString();

            var connectionString = String.Format(
                "{0}{1}/{2}/",
                ClientContext.Configuration.ConnectionString,
                "mp",
                language);
            return CreateContentClient(source, connectionString);
        }

        #endregion
    }
}