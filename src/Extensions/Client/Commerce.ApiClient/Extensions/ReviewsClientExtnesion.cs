using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient.Extensions
{
    using System.Configuration;

    public static class ReviewsClientExtnesion
    {
        public static ReviewsClient CreateReviewsClient(this CommerceClients source)
        {
            var connectionString = ConnectionHelper.GetConnectionString("vc-commerce-api");
            return CreateReviewsClient(source, connectionString);
        }

        public static ReviewsClient CreateReviewsClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new ReviewsClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }
    }
}
