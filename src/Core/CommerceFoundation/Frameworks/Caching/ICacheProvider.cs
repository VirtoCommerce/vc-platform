using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
	public interface ICacheProvider
	{
		ICacheKeyGenerator KeyGenerator { get; }
		TimeSpan GetExpirationTimeout(CacheKey key);
		void Put(CacheKey key, CachedObject cachedObj, TimeSpan expirationTimeout);
		CachedObject Get(CacheKey key);

	}
}
