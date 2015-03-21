#region

using System;
using System.Threading;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class ReviewsClientExtnesion
    {
        #region Public Methods and Operators

        public static ReviewsClient CreateReviewClientWithUri(this CommerceClients source, string serviceUrl)
        {
            var client = new ReviewsClient(
                new Uri(serviceUrl),
                source.CreateMessageProcessingHandler());
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
