using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient
{
    using VirtoCommerce.ApiClient.Caching;

    public class BaseClient
    {
        private const string UnknownErrorCode = "UnknownError";

        private readonly HttpClient httpClient;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementClientBase"/> class.
        /// </summary>
        /// <param name="baseEndpoint">The base endpoint.</param>
        /// <param name="handler">Message processing handler</param>
        public BaseClient(Uri baseEndpoint, HttpMessageHandler handler = null)
        {
            BaseAddress = baseEndpoint;
            httpClient = new HttpClient(handler);
            disposed = false;
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
                NameValueCollection queryStringProperties = HttpUtility.ParseQueryString(BaseAddress.Query);
                foreach (KeyValuePair<string, string> queryStringParameter in queryStringParameters)
                {
                    queryStringProperties[queryStringParameter.Key] = queryStringParameter.Value;
                }

                queryString = queryStringProperties.ToString();
            }

            return CreateRequestUri(relativePath, queryString);
        }

        /// <summary>
        /// Creates the request URI.
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
        /// Sends a GET request.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        /// <returns>Response object.</returns>
        protected virtual async Task<T> GetAsyncInternal<T>(Uri requestUri, string userId = null) where T : class 
        {
            var message = new HttpRequestMessage(HttpMethod.Get, requestUri);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                message.Headers.Add(Constants.Headers.PrincipalId, HttpUtility.UrlEncode(userId));
            }

            using (HttpResponseMessage response = await httpClient.SendAsync(message))
            {
                await ThrowIfResponseNotSuccessfulAsync(response);

                return await response.Content.ReadAsAsync<T>();
            }
        }

        protected virtual async Task<T> GetAsync<T>(Uri requestUri, string userId = null, bool useCache = true) where T:class
        {
            return await Helper.GetAsync(requestUri.ToString(),
                () => GetAsyncInternal<T>(requestUri, userId),
                this.GetCacheTimeOut(requestUri.ToString()),
                ClientContext.Configuration.IsCacheEnabled && useCache);
        }

        /// <summary>
        /// Sends an http request.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        protected Task SendAsync(Uri requestUri, HttpMethod httpMethod, string userId = null)
        {
            return SendAsync<object>(requestUri, httpMethod, userId);
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
            return SendAsync<TOutput>(message, true, userId);
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
            return SendAsync<TInput, object>(requestUri, httpMethod, body, userId);
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
                Content = new ObjectContent<TInput>(body, CreateMediaTypeFormatter())
            };

            return SendAsync<TOutput>(message, true, userId);
        }

        private async Task<TOutput> SendAsync<TOutput>(HttpRequestMessage message, bool hasResult, string userId = null)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                message.Headers.Add(Constants.Headers.PrincipalId, userId);
            }

            using (HttpResponseMessage response = await httpClient.SendAsync(message))
            {
                await ThrowIfResponseNotSuccessfulAsync(response);

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

                string errorCode = UnknownErrorCode;
                string errorMessage = "An unknown error has occurred during this operation";
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

        private MediaTypeFormatter CreateMediaTypeFormatter()
        {
            //MediaTypeFormatter formatter;
            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                }
            };

            return formatter;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    httpClient.Dispose();
                }
            }

            disposed = true;
        }

        #region Cache Implementation
        CacheHelper _cacheHelper;
        private CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }

        private ICacheRepository _cacheRepository = new HttpCacheRepository();
        protected virtual ICacheRepository CacheRepository
        {
            get { return _cacheRepository ?? (_cacheRepository = new HttpCacheRepository()); }
        }

        protected virtual TimeSpan GetCacheTimeOut(string requestUrl)
        {
            return new TimeSpan(0,0,0,30);
        }
        #endregion
    }
}
