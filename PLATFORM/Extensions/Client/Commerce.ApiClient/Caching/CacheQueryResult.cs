using System.Diagnostics;
using System.Net.Http;

namespace VirtoCommerce.ApiClient.Caching
{
    public enum CacheStatus
    {
        CannotUseCache,
        Revalidate,
        ReturnStored
    }

    public class CacheQueryResult
    {
        #region Fields

        public CacheContent SelectedVariant;

        #endregion

        #region Public Properties

        public CacheStatus Status { get; set; }

        #endregion

        #region Public Methods and Operators

        public static CacheQueryResult CannotUseCache()
        {
            return new CacheQueryResult()
            {
                Status = CacheStatus.CannotUseCache
            };
        }

        public static CacheQueryResult ReturnStored(CacheContent cacheContent)
        {
            return new CacheQueryResult()
            {
                Status = CacheStatus.ReturnStored,
                SelectedVariant = cacheContent
            };
        }

        public static CacheQueryResult Revalidate(CacheContent cacheContent)
        {
            return new CacheQueryResult()
            {
                Status = CacheStatus.Revalidate,
                SelectedVariant = cacheContent
            };
        }

        #endregion

        #region Methods

        internal void ApplyConditionalHeaders(HttpRequestMessage request)
        {
            Debug.Assert(SelectedVariant != null);
            if (SelectedVariant == null || !SelectedVariant.HasValidator)
            {
                return;
            }

            var httpResponseMessage = SelectedVariant.Response;

            if (httpResponseMessage.Headers.ETag != null)
            {
                request.Headers.IfNoneMatch.Add(httpResponseMessage.Headers.ETag);
            }
            else
            {
                if (httpResponseMessage.Content != null && httpResponseMessage.Content.Headers.LastModified != null)
                {
                    request.Headers.IfModifiedSince = httpResponseMessage.Content.Headers.LastModified;
                }
            }
        }

        internal HttpResponseMessage GetHttpResponseMessage(HttpRequestMessage request)
        {
            var response = SelectedVariant.Response;
            response.RequestMessage = request;
            HttpCache.UpdateAgeHeader(response);
            return response;
        }

        public void SetContent(ObjectContent content)
        {
            SelectedVariant.Response.Content = content;
        }

        #endregion
    }
}
