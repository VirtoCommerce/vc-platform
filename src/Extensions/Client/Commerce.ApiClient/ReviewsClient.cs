namespace VirtoCommerce.ApiClient
{
    #region

    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using VirtoCommerce.ApiClient.Utilities;
    using VirtoCommerce.Web.Core.DataContracts;

    #endregion

    public class ReviewsClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public ReviewsClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public ReviewsClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     List items matching the given query
        /// </summary>
        public Task<ResponseCollection<Review>> GetReviewsAsync(string productId)
        {
            return
                this.GetAsync<ResponseCollection<Review>>(
                    this.CreateRequestUri(string.Format(RelativePaths.Reviews, productId)));
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Reviews = "products/{0}/reviews";

            #endregion
        }
    }
}