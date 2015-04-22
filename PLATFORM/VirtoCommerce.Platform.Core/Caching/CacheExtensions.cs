using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Platform.Core.Caching
{
	public static class CacheExtensions 
	{
		public static T GetPutFromCache<T>(this ICacheProvider provider, CacheKey cacheKey, Func<T> getValueFunction)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (cacheKey == null)
			{
				throw new ArgumentNullException("cacheKey");
			}
			if (getValueFunction == null)
			{
				throw new ArgumentNullException("getValueFunction");
			}

			T retVal = default(T);

			var cachedObj = provider.Get(cacheKey);
			if (cachedObj == null)
			{
				cachedObj = new CachedObject(getValueFunction());

				var expirationTimeout = provider.GetExpirationTimeout(cacheKey);
				provider.Put(cacheKey, cachedObj, expirationTimeout);
			}

			retVal = (T)cachedObj.Data;

			return retVal;
		}

		public static IQueryable<T> AsCacheable<T>(this IQueryable<T> query, ICacheProvider cacheProvider)
		{
			return (IQueryable<T>)CacheQueryProvider.CreateCachedQuery(query, cacheProvider);
		}

    
	}
}
