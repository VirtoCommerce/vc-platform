using System;
using System.Runtime.Caching;
using VirtoCommerce.Framework.Core.Caching;

namespace VirtoCommerce.Framework.Data.Caching
{
	public class InMemoryCacheProvider : ICacheProvider
	{
		private ObjectCache _cache;

		public InMemoryCacheProvider()
		{
			_cache = MemoryCache.Default;
		}

		#region ICacheProvider Members

		public void PutItem(CacheKey key, CachedObject cachedObject, TimeSpan expirationTimeout)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			var expirationDate = DateTime.UtcNow.Add(expirationTimeout);

			var policy = new CacheItemPolicy
			{
				AbsoluteExpiration = expirationDate
			};
			var cachedItem = new CacheItem(key.ToString(), cachedObject);
			_cache.Set(cachedItem, policy);
		}

		public CachedObject GetItem(CacheKey key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			var retVal = _cache.GetCacheItem(key.ToString());
			return retVal != null ? retVal.Value as CachedObject : null;
		}


		public void RemoveItem(CacheKey key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			_cache.Remove(key.ToString());
		}

		#endregion
	}
}
