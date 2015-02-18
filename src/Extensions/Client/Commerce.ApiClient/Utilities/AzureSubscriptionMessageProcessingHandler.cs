#region

using System;
using System.Net.Http;
using System.Threading;

#endregion

namespace VirtoCommerce.ApiClient.Utilities
{
    /// <summary>
    ///     Adds Azure subscription key to the request header
    /// </summary>
    public class AzureSubscriptionMessageProcessingHandler : HmacMessageProcessingHandler
    {
        #region Fields

        private readonly string _subscriptionKey;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HmacMessageProcessingHandler" /> class.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key.</param>
        /// <param name="appId"></param>
        /// <param name="secretKey"></param>
        /// <exception cref="System.ArgumentException">Security token</exception>
        public AzureSubscriptionMessageProcessingHandler(string subscriptionKey, string appId, string secretKey)
            : base(appId, secretKey)
        {
            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new ArgumentException("subscriptionKey");
            }

            _subscriptionKey = subscriptionKey;
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
            request.Headers.Add("ocp-apim-subscription-key", _subscriptionKey);
            base.ProcessRequest(request, cancellationToken);
            return request;
        }

        #endregion
    }
}
