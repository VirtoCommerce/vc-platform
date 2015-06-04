using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace VirtoCommerce.ApiClient.Caching
{
    public class CacheEntry
    {
        #region Constructors and Destructors

        internal CacheEntry(PrimaryCacheKey key, HttpHeaderValueCollection<string> varyHeaders)
        {
            Key = key;
            VaryHeaders = varyHeaders;
        }

        #endregion

        #region Public Properties

        public PrimaryCacheKey Key { get; private set; }
        public HttpHeaderValueCollection<string> VaryHeaders { get; private set; }

        #endregion

        #region Public Methods and Operators

        public CacheContent CreateContent(HttpResponseMessage response)
        {
            return new CacheContent()
            {
                CacheEntry = this,
                Key = CreateSecondaryKey(response.RequestMessage),
                HasValidator =
                    response.Headers.ETag != null
                        || (response.Content != null && response.Content.Headers.LastModified != null),
                Expires = HttpCache.GetExpireDate(response),
                CacheControl = response.Headers.CacheControl ?? new CacheControlHeaderValue(),
                Response = response,
            };
        }

        public string CreateSecondaryKey(HttpRequestMessage request)
        {
            var key = new StringBuilder();
            foreach (var h in VaryHeaders.OrderBy(v => v))
                // Sort the vary headers so that ordering doesn't generate different stored variants
            {
                if (h != "*")
                {
                    key.Append(h).Append(':');
                    var addedOne = false;

                    IEnumerable<string> values;
                    if (request.Headers.TryGetValues(h, out values))
                    {
                        foreach (var val in values)
                        {
                            key.Append(val).Append(',');
                            addedOne = true;
                        }
                    }

                    if (addedOne)
                    {
                        key.Length--; // truncate trailing comma.
                    }
                }
                else
                {
                    key.Append('*');
                }
            }
            return key.ToString().ToLowerInvariant();
        }

        #endregion
    }
}
