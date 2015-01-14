using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient
{
    public class ReviewsClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string Reviews = "products/{0}/reviews";
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public ReviewsClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public ReviewsClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        /// <summary>
        /// List items matching the given query
        /// </summary>
        public Task<ResponseCollection<Review>> GetReviewsAsync(string productId)
        {
            return GetAsync<ResponseCollection<Review>>(CreateRequestUri(string.Format(RelativePaths.Reviews, productId)));
        }
    }
}
