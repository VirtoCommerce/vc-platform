using CacheManager.Core;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Data.Common;

namespace SwashbuckleModule.Web
{
    public class CachingSwaggerProvider : ISwaggerProvider
    {
        private readonly ISwaggerProvider _swaggerProvider;
        private readonly ICacheManager<object> _cacheManager;
        private readonly string _cacheKey;

        public CachingSwaggerProvider(ISwaggerProvider swaggerProvider, ICacheManager<object> cacheManager, string cacheKey)
        {
            _swaggerProvider = swaggerProvider;
            _cacheManager = cacheManager;
            _cacheKey = cacheKey;
        }
        #region ISwaggerProvider Members

        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            var cacheKey = string.Join("-", "Swashbuckle", apiVersion, _cacheKey);
            return _cacheManager.Get(cacheKey, "SwashbuckleRegion", () => _swaggerProvider.GetSwagger(rootUrl, apiVersion));
        }

        #endregion
    }
}
