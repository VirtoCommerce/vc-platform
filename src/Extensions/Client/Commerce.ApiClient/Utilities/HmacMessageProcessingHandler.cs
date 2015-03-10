#region

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;

#endregion

namespace VirtoCommerce.ApiClient.Utilities
{
    /// <summary>
    ///     Adds request signature to the Authorization header.
    /// </summary>
    public class HmacMessageProcessingHandler : MessageProcessingHandler
    {
        #region Fields

        private readonly string _appId;

        private readonly string _secretKey;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HmacMessageProcessingHandler" /> class.
        /// </summary>
        /// <param name="appId">The API application ID.</param>
        /// <param name="secretKey">The API secret key.</param>
        /// <exception cref="System.ArgumentException">Security token</exception>
        public HmacMessageProcessingHandler(string appId, string secretKey)
            : base(new WebRequestHandler())
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException("appId must not be empty.");
            }
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentException("secretKey must not be empty.");
            }

            _appId = appId;
            _secretKey = secretKey;
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
            var signature = new ApiRequestSignature { AppId = _appId };

            var parameters = new[]
            {
                new NameValuePair(null, _appId),
                new NameValuePair(null, signature.TimestampString)
            };

            signature.Hash = HmacUtility.GetHashString(key => new HMACSHA256(key), _secretKey, parameters);

            request.Headers.Authorization = new AuthenticationHeaderValue("HMACSHA256", signature.ToString());
            return request;
        }

        /// <summary>
        ///     Processes an HTTP response message.
        /// </summary>
        /// <param name="response">The HTTP response message to process.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>
        ///     Returns <see cref="T:System.Net.Http.HttpResponseMessage" />.The HTTP response message that was processed.
        /// </returns>
        protected override HttpResponseMessage ProcessResponse(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            return response;
        }

        #endregion
    }
}
