using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Caching;

namespace SwashbuckleModule.Web
{
	/// <summary>
	/// Thats wrapper created with some Swachbuckle source code changes (that reason why we do not use swachbuckle as nuget package)
	/// writing request to author https://github.com/domaindrivendev/Swashbuckle/issues/456
	/// </summary>
	public class CachedSwaggerProviderWrapper : ISwaggerProvider
	{
		private readonly CacheManager _cacheManager;
		private readonly ISwaggerProvider _swaggerProvider;
		public CachedSwaggerProviderWrapper(ISwaggerProvider swaggerProvider, CacheManager cacheManager)
		{
			_cacheManager = cacheManager;
			_swaggerProvider = swaggerProvider;
		}
		#region ISwaggerProvider Members

		public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
		{
			var cacheKey = CacheKey.Create("Swashbuckle", apiVersion);
			return _cacheManager.Get(cacheKey, () => _swaggerProvider.GetSwagger(rootUrl, apiVersion));
		}

		#endregion
	}
}