using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.ApiClient
{
    public class BaseClient
    {
        private const string UnknownErrorCode = "UnknownError";

        private readonly HttpClient httpClient;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementClientBase"/> class.
        /// </summary>
        /// <param name="baseEndpoint">The base endpoint.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="handler">Message processing handler</param>
        public BaseClient(Uri baseEndpoint, MessageProcessingHandler handler = null)
        {
            this.BaseAddress = baseEndpoint;
            this.httpClient = new HttpClient(handler);
            this.disposed = false;
        }

        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        /// <value>
        /// The base address.
        /// </value>
        protected Uri BaseAddress { get; set; }

        /// <summary>
        /// Creates the request URI.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryStringParameters">The query string parameters.</param>
        /// <returns>Request URI</returns>
        protected Uri CreateRequestUri(string relativePath, params KeyValuePair<string, string>[] queryStringParameters)
        {
            string queryString = string.Empty;

            if (queryStringParameters != null && queryStringParameters.Length > 0)
            {
                NameValueCollection queryStringProperties = HttpUtility.ParseQueryString(this.BaseAddress.Query);
                foreach (KeyValuePair<string, string> queryStringParameter in queryStringParameters)
                {
                    queryStringProperties[queryStringParameter.Key] = queryStringParameter.Value;
                }

                queryString = queryStringProperties.ToString();
            }

            return this.CreateRequestUri(relativePath, queryString);
        }

        /// <summary>
        /// Creates the request URI.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>Request URI</returns>
        protected Uri CreateRequestUri(string relativePath, string queryString)
        {
            var endpoint = new Uri(this.BaseAddress, relativePath);
            var uriBuilder = new UriBuilder(endpoint) { Query = queryString };
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Sends a GET request.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        /// <returns>Response object.</returns>
        protected async Task<T> GetAsync<T>(Uri requestUri, string userId = null)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, requestUri);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                message.Headers.Add(Constants.Headers.PrincipalId, HttpUtility.UrlEncode(userId));
            }

            using (HttpResponseMessage response = await this.httpClient.SendAsync(message))
            {
                await this.ThrowIfResponseNotSuccessfulAsync(response);

                return await response.Content.ReadAsAsync<T>();
            }
        }

        /// <summary>
        /// Sends an http request.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        protected Task SendAsync(Uri requestUri, HttpMethod httpMethod, string userId = null)
        {
            return this.SendAsync<object>(requestUri, httpMethod, userId);
        }

        /// <summary>
        /// Sends an http request.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="userId">The user id.</param>
        protected Task<TOutput> SendAsync<TOutput>(Uri requestUri, HttpMethod httpMethod, string userId = null)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri);
            return this.SendAsync<TOutput>(requestUri, httpMethod, message, true, userId);
        }

        /// <summary>
        /// Sends an http request.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="body">The body.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        protected Task SendAsync<TInput>(Uri requestUri, HttpMethod httpMethod, TInput body, string userId = null)
        {
            return this.SendAsync<TInput, object>(requestUri, httpMethod, body, userId);
        }

        /// <summary>
        /// Sends an http request.
        /// </summary>
        /// <typeparam name="TInput">Input type.</typeparam>
        /// <typeparam name="TOutput">Output type.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="body">The body.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        protected Task<TOutput> SendAsync<TInput, TOutput>(Uri requestUri, HttpMethod httpMethod, TInput body, string userId = null)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = new ObjectContent<TInput>(body, this.CreateMediaTypeFormatter())
            };

            return this.SendAsync<TOutput>(requestUri, httpMethod, message, true, userId);
        }

        private async Task<TOutput> SendAsync<TOutput>(Uri requestUri, HttpMethod httpMethod, HttpRequestMessage message, bool hasResult, string userId = null)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                message.Headers.Add(Constants.Headers.PrincipalId, userId);
            }

            using (HttpResponseMessage response = await this.httpClient.SendAsync(message))
            {
                await this.ThrowIfResponseNotSuccessfulAsync(response);

                if (!hasResult)
                {
                    return default(TOutput);
                }

                return await response.Content.ReadAsAsync<TOutput>();
            }
        }

        private async Task ThrowIfResponseNotSuccessfulAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                ManagementServiceError managementServiceError = null;

                try
                {
                    managementServiceError = await response.Content.ReadAsAsync<ManagementServiceError>();
                }
                catch (InvalidOperationException)
                {
                    // ReadAsAsync will synchronously throw InvalidOperationException when there is no default formatter for the ContentType.
                    // We will treat these cases as an unknown error
                }

                string errorCode = UnknownErrorCode;
                string errorMessage = "An unknown error has occurred during this operation";
                List<ErrorDetail> errorDetails = null;

                if (managementServiceError != null)
                {
                    errorCode = managementServiceError.Code;
                    errorMessage = managementServiceError.Message;
                    errorDetails = managementServiceError.Details;
                }
                else
                {
                    errorMessage = response.Content.ReadAsStringAsync().Result ?? errorMessage;
                }

                throw new ManagementClientException(response.StatusCode, errorCode, errorMessage, errorDetails);
            }
        }

        private MediaTypeFormatter CreateMediaTypeFormatter()
        {
            //MediaTypeFormatter formatter;
            var formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
            formatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            return formatter;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    this.httpClient.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
