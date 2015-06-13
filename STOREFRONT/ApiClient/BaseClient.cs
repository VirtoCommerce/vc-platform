#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.ApiClient.Caching;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;

#endregion

namespace VirtoCommerce.ApiClient
{
    public class BaseClient
    {
        #region Constants

        private const string UnknownErrorCode = "UnknownError";

        #endregion

        #region Static Fields

        private static readonly IContentStore _store = new InMemoryContentStore();

        #endregion

        #region Fields

        private readonly HttpClient _httpClient;
        private readonly HttpCache _httpCache;
        private bool _disposed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseClient" /> class.
        /// </summary>
        /// <param name="baseEndpoint">The base endpoint.</param>
        /// <param name="handler">Message processing handler</param>
        public BaseClient(Uri baseEndpoint, HttpMessageHandler handler = null)
        {
            BaseAddress = baseEndpoint;
            this._httpCache = new HttpCache(_store);
            var cachingHandler = new PrivateCacheHandler(handler ?? new HttpClientHandler(), this._httpCache);

            //httpClient = HttpClientFactory.Create(caheHandler);
            this._httpClient = new HttpClient(cachingHandler) { Timeout = TimeSpan.FromSeconds(90) };
            this._disposed = false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the base address.
        /// </summary>
        /// <value>
        ///     The base address.
        /// </value>
        protected Uri BaseAddress { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the request URI.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryStringParameters">The query string parameters.</param>
        /// <returns>Request URI</returns>
        protected Uri CreateRequestUri(string relativePath, params KeyValuePair<string, string>[] queryStringParameters)
        {
            var queryString = string.Empty;

            if (queryStringParameters != null && queryStringParameters.Length > 0)
            {
                var queryStringProperties = HttpUtility.ParseQueryString(BaseAddress.Query);
                foreach (var queryStringParameter in queryStringParameters)
                {
                    queryStringProperties[queryStringParameter.Key] = queryStringParameter.Value;
                }

                queryString = queryStringProperties.ToString();
            }

            return CreateRequestUri(relativePath, queryString);
        }

        protected Uri CreateRequestUri(
            string relativePath,
            object queryStringParameters)
        {
            var parameters =
                queryStringParameters.ToPropertyDictionary()
                    .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToNullOrString()))
                    .ToArray();
            return CreateRequestUri(relativePath, parameters);
        }

        /// <summary>
        ///     Creates the request URI.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>Request URI</returns>
        protected Uri CreateRequestUri(string relativePath, string queryString)
        {
            var endpoint = new Uri(BaseAddress, relativePath);
            var uriBuilder = new UriBuilder(endpoint) { Query = queryString };
            return uriBuilder.Uri;
        }

        /// <summary>
        ///     http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this._disposed)
                {
                    this._httpClient.Dispose();
                }
            }

            this._disposed = true;
        }
        
        /// <summary>
        ///     Sends a GET request.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        /// <returns>Response object.</returns>
        protected virtual async Task<T> GetAsync<T>(Uri requestUri, bool useCache = true)
            where T : class
        {
            var message = new HttpRequestMessage(HttpMethod.Get, requestUri);

            if (!useCache)
            {
                message.Headers.Add("Cache-Control", "no-cache, no-, must-revalidate"); 
            }

            using (var response = await this._httpClient.SendAsync(message))
            {
                await ThrowIfResponseNotSuccessfulAsync(response);

                //return await response.Content.ReadAsAsync<T>();

                var taskObject = await response.Content.ReadAsAsync<T>();

                // the following will reduce number of serializations
                if (!(response.Content is ObjectContent) && taskObject != null)
                {
                    response.Content = new ObjectContent(typeof(T), taskObject, new JsonMediaTypeFormatter());
                    await this._httpCache.StoreResponseAsync(response);
                }

                return await Task.FromResult((T)taskObject);
            }
        }

        /// <summary>
        ///     Sends an http request.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        protected Task SendAsync(Uri requestUri, HttpMethod httpMethod)
        {
            return SendAsync<object>(requestUri, httpMethod);
        }

        /// <summary>
        ///     Sends an http request.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        protected Task<TOutput> SendAsync<TOutput>(Uri requestUri, HttpMethod httpMethod)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri);
            return SendAsync<TOutput>(message, true);
        }

        /// <summary>
        ///     Sends an http request.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="body">The body.</param>
        protected Task SendAsync<TInput>(Uri requestUri, HttpMethod httpMethod, TInput body)
        {
            return SendAsync<TInput, object>(requestUri, httpMethod, body);
        }

        /// <summary>
        ///     Sends an http request.
        /// </summary>
        /// <typeparam name="TInput">Input type.</typeparam>
        /// <typeparam name="TOutput">Output type.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="body">The body.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        protected Task<TOutput> SendAsync<TInput, TOutput>(
            Uri requestUri,
            HttpMethod httpMethod,
            TInput body)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content =
                    new ObjectContent<TInput>(
                        body,
                        CreateMediaTypeFormatter())
            };

            return SendAsync<TOutput>(message, true);
        }

        private MediaTypeFormatter CreateMediaTypeFormatter()
        {
            //MediaTypeFormatter formatter;
            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    DefaultValueHandling =
                        DefaultValueHandling.Ignore,
                    NullValueHandling =
                        NullValueHandling.Ignore
                }
            };

            return formatter;
        }

        private async Task<TOutput> SendAsync<TOutput>(HttpRequestMessage message, bool hasResult)
        {
            using (var response = await this._httpClient.SendAsync(message).ConfigureAwait(false))
            {
                await ThrowIfResponseNotSuccessfulAsync(response);

                if (!hasResult)
                {
                    return default(TOutput);
                }

                return await response.Content.ReadAsAsync<TOutput>().ConfigureAwait(false);
            }
        }

        private async Task ThrowIfResponseNotSuccessfulAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                //Do not treat not found as exception, but rather as null result
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }

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
                catch (UnsupportedMediaTypeException) // can't parse the message, create it manually
                {
                    
                }

                var errorCode = UnknownErrorCode;
                var errorMessage = "An unknown error has occurred during this operation";
                List<ErrorDetail> errorDetails = null;

                if (managementServiceError != null)
                {
                    errorCode = managementServiceError.Code;
                    errorMessage = managementServiceError.Message + " " + managementServiceError.ExceptionMessage;
                    errorDetails = managementServiceError.Details;
                }
                else
                {
                    errorMessage = response.Content.ReadAsStringAsync().Result ?? errorMessage;
                }

                throw new ManagementClientException(response.StatusCode, errorCode, errorMessage, errorDetails);
            }
        }

        #endregion
    }
}
