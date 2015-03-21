using System.Web;

namespace VirtoCommerce.Web.Client.Caching.Interfaces
{
    public interface ICacheHeadersHelper
    {
        /// <summary>
        /// Implementations should set the cache headers for the HTTP response given <see cref="settings"/>.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="settings">The cache settings.</param>
        void SetCacheHeaders(HttpResponseBase response, CacheSettings settings);
    }
}
