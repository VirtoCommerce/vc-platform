namespace VirtoCommerce.ApiClient.Utilities
{
    #region

    using System;
    using System.Net.Http;
    using System.Threading;

    #endregion

    /// <summary>
    ///     Handler to add tokens to the request header
    /// </summary>
    public class AzureSubscriptionMessageProcessingHandler : TokenMessageProcessingHandler
    {
        #region Fields

        private readonly string subscriptionKey;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenMessageProcessingHandler" /> class.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key.</param>
        /// <param name="token">The token.</param>
        /// <exception cref="System.ArgumentException">Security token</exception>
        public AzureSubscriptionMessageProcessingHandler(string subscriptionKey, string token)
            : base(token)
        {
            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new ArgumentException("subscriptionKey");
            }

            this.subscriptionKey = subscriptionKey;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Processes an HTTP request message.
        /// </summary>
        /// <param name="request">The HTTP request message to process.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>
        ///     Returns <see cref="T:System.Net.Http.HttpRequestMessage" />.The HTTP request message that was processed.
        /// </returns>
        protected override HttpRequestMessage ProcessRequest(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Add("ocp-apim-subscription-key", this.subscriptionKey);
            base.ProcessRequest(request, cancellationToken);
            return request;
        }

        #endregion
    }
}