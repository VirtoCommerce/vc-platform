using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.Caching
{
    public class PrivateCacheHandler : DelegatingHandler
    {
        #region Fields

        private readonly HttpCache _httpCache;

        #endregion

        #region Constructors and Destructors

        public PrivateCacheHandler(HttpMessageHandler innerHandler, HttpCache httpCache)
        {
            this._httpCache = httpCache;
            this.InnerHandler = innerHandler;
        }

        #endregion

        // Process Request and Response

        #region Methods

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var queryResult = await this._httpCache.QueryCacheAsync(request);

            if (queryResult.Status == CacheStatus.ReturnStored)
            {
                return queryResult.GetHttpResponseMessage(request);
            }

            if (request.Headers.CacheControl != null && request.Headers.CacheControl.OnlyIfCached)
            {
                return this.CreateGatewayTimeoutResponse(request);
            }

            if (queryResult.Status == CacheStatus.Revalidate)
            {
                queryResult.ApplyConditionalHeaders(request);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                await this._httpCache.UpdateContentAsync(response, queryResult.SelectedVariant);
                response.Dispose();
                return queryResult.GetHttpResponseMessage(request);
            }

            if (this._httpCache.CanStore(response))
            {
                if (response.Content != null)
                {
                    await response.Content.LoadIntoBufferAsync();
                }
                await this._httpCache.StoreResponseAsync(response);
            }

            return response;
        }

        private HttpResponseMessage CreateGatewayTimeoutResponse(HttpRequestMessage request)
        {
            return new HttpResponseMessage(HttpStatusCode.GatewayTimeout)
                   {
                       RequestMessage = request
                   };
        }

        #endregion
    }
}
