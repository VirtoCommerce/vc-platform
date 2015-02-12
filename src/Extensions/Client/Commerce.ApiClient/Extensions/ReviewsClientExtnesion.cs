namespace VirtoCommerce.ApiClient.Extensions
{
    #region

    using System;
    using System.Configuration;
    using System.Threading;

    using VirtoCommerce.ApiClient.Utilities;

    #endregion

    public static class ReviewsClientExtnesion
    {
        #region Public Methods and Operators

        public static ReviewsClient CreateReviewClientWithUri(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new ReviewsClient(
                new Uri(connectionString),
                new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }

        public static ReviewsClient CreateReviewsClient(this CommerceClients source)
        {
            return source.CreateReviewsClient(Thread.CurrentThread.CurrentUICulture.ToString());
        }

        public static ReviewsClient CreateReviewsClient(this CommerceClients source, string language)
        {
            // http://localhost/admin/api/mp/{0}/{1}/
            var connectionString = String.Format(
                "{0}{1}/{2}/",
                ClientContext.Configuration.ConnectionString,
                "mp",
                language);
            return CreateReviewClientWithUri(source, connectionString);
        }

        #endregion
    }
}