using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace VirtoCommerce.ApiClient.Caching
{
    public class CacheContent
    {
        #region Public Properties

        public CacheControlHeaderValue CacheControl { get; set; }
        public CacheEntry CacheEntry { get; set; }

        public DateTimeOffset Expires { get; set; }
        public bool HasValidator { get; set; }
        public string Key { get; set; }
        public HttpResponseMessage Response { get; set; }

        #endregion

        #region Public Methods and Operators

        public bool IsFresh()
        {
            return Expires > DateTime.UtcNow;
        }

        #endregion
    }
}
