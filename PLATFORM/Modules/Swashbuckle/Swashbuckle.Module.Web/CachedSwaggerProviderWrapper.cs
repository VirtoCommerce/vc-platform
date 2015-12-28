using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CacheManager.Core;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Data.Common;

namespace SwashbuckleModule.Web
{
	/// <summary>
	/// Thats wrapper created with some Swachbuckle source code changes (that reason why we do not use swachbuckle as nuget package)
	/// writing request to author https://github.com/domaindrivendev/Swashbuckle/issues/456
	/// </summary>
	public class CachedSwaggerProviderWrapper : ISwaggerProvider
	{
		private readonly ICacheManager<object> _cacheManager;
		private readonly ISwaggerProvider _swaggerProvider;
		public CachedSwaggerProviderWrapper(ISwaggerProvider swaggerProvider, ICacheManager<object> cacheManager)
		{
			_cacheManager = cacheManager;
			_swaggerProvider = swaggerProvider;
		}
		#region ISwaggerProvider Members

		public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
		{
			return _cacheManager.Get("Swashbuckle" + apiVersion, "SwashbuckleRegion",  () => _swaggerProvider.GetSwagger(rootUrl, apiVersion));
        }

		#endregion
	}
}