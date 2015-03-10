using System;
using System.Net.Http;

namespace VirtoCommerce.ApiClient.Caching
{
    public class PrimaryCacheKey
    {
        #region Fields

        private readonly HttpMethod _method;
        private readonly Uri _uri;

        #endregion

        #region Constructors and Destructors

        public PrimaryCacheKey(Uri uri, HttpMethod method)
        {
            _uri = uri;
            _method = method;
            if (_method == HttpMethod.Post) // A response to a POST can be returned to a GET method
            {
                _method = HttpMethod.Get;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            var key2 = (PrimaryCacheKey)obj;
            return key2._uri == _uri && key2._method == _method;
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + _uri.GetHashCode();
            hash = (hash * 7) + _method.GetHashCode();
            return hash;
        }

        #endregion
    }
}
