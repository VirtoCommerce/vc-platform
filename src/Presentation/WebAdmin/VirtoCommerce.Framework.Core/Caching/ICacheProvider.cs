using System;

namespace VirtoCommerce.Framework.Core.Caching
{
	public interface ICacheProvider
	{
		void PutItem(CacheKey key, CachedObject cachedObject, TimeSpan expirationTimeout);
		
		CachedObject GetItem(CacheKey key);
		
		void RemoveItem(CacheKey key);
	}
}