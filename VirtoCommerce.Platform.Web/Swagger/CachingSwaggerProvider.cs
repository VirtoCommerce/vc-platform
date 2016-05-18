using System;
using CacheManager.Core;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    [CLSCompliant(false)]
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

        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            const string cacheRegion = "Swagger";
            var cacheKey = string.Join("-", cacheRegion, apiVersion, _cacheKey);
            return _cacheManager.Get(cacheKey, cacheRegion, () => _swaggerProvider.GetSwagger(rootUrl, apiVersion));
        }
    }
}
