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
    public class TokenMessageProcessingHandler : MessageProcessingHandler
    {
        private string token;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenMessageProcessingHandler"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <exception cref="System.ArgumentException">Security token</exception>
        public TokenMessageProcessingHandler(string token)
            : base(new WebRequestHandler())
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("token");
            }

            this.token = token;
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
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
            return request;
        }

        /// <summary>
        /// Processes an HTTP response message.
        /// </summary>
        /// <param name="response">The HTTP response message to process.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Net.Http.HttpResponseMessage" />.The HTTP response message that was processed.
        /// </returns>
        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return response;
        }
    }
}
