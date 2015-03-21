using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.Utilities
{
    /// <summary>
    /// Handler to add tokens to the request header
    /// </summary>
    public class AzureSubscriptionMessageProcessingHandler : TokenMessageProcessingHandler
    {
        private string subscriptionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenMessageProcessingHandler"/> class.
        /// </summary>
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

        /// <summary>
        /// Processes an HTTP request message.
        /// </summary>
        /// <param name="request">The HTTP request message to process.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Net.Http.HttpRequestMessage" />.The HTTP request message that was processed.
        /// </returns>
        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("ocp-apim-subscription-key", this.subscriptionKey);
            base.ProcessRequest(request, cancellationToken);
            return request;
        }
    }
}
