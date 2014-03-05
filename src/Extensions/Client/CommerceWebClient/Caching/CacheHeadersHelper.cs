using System;
using System.Web;
using System.Web.UI;
using ICacheHeadersHelper = VirtoCommerce.Web.Client.Caching.Interfaces.ICacheHeadersHelper;

namespace VirtoCommerce.Web.Client.Caching
{
    public class CacheHeadersHelper : ICacheHeadersHelper
    {
        /// <summary>
        /// Sets the cache headers for the HTTP response given <see cref="settings" />.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="settings">The cache settings.</param>
        public void SetCacheHeaders(HttpResponseBase response, CacheSettings settings)
        {
            var cacheability = HttpCacheability.NoCache;

            switch (settings.Location)
            {
                case OutputCacheLocation.Any:
                case OutputCacheLocation.Downstream:
                    cacheability = HttpCacheability.Public;
                    break;
                case OutputCacheLocation.Client:
                case OutputCacheLocation.ServerAndClient:
                    cacheability = HttpCacheability.Private;
                    break;                    
            }

            response.Cache.SetCacheability(cacheability);

            if (cacheability != HttpCacheability.NoCache)
            {
                response.Cache.SetExpires(DateTime.Now.AddSeconds(settings.Duration));
                response.Cache.SetMaxAge(new TimeSpan(0, 0, settings.Duration));
            }

            if (settings.NoStore)
            {
                response.Cache.SetNoStore();
            }
        }
    }
}
