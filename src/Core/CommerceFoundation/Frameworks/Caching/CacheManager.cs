using System;
using System.Collections.Generic;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
	/// <summary>
	/// Представляет методы работы с кэшем
	/// </summary>
	public class CacheManager
	{
		private Func<string, ICacheProvider> CacheProviderFactory;
		private Func<string, CacheSettings> CacheSettingFactory;

		public CacheManager(Func<string, ICacheProvider> cacheProviderFactory, Func<string, CacheSettings> settingFactory)
		{
			CacheProviderFactory = cacheProviderFactory;
			CacheSettingFactory = settingFactory;
		}
	

		public void Put(CacheKey cacheKey, object cacheValue)
		{
			if (cacheKey == null)
				throw new ArgumentNullException("cacheKey");

			var repository = GetProvider(cacheKey.CacheGroup);
			if (repository != null)
			{
				var expirationTimeout = GetExpirationTimeoutByCacheGroup(cacheKey.CacheGroup);
				var cachedObj = new CachedObject(cacheValue);
				repository.Put(cacheKey, cachedObj, expirationTimeout);
			}
		}

		/// <summary>
		/// Gets the specified cache key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="cacheKey">The cache key.</param>
		/// <returns></returns>
		public T Get<T>(CacheKey cacheKey)
		{
			if (cacheKey == null)
			{
				throw new ArgumentNullException("cacheKey");
			}
			T retVal = default(T);

			var repository = GetProvider(cacheKey.CacheGroup);

			if (repository != null)
			{
				var cachedObj = repository.Get(cacheKey);
				if (cachedObj != null)
				{
					retVal = (T)cachedObj.Data;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Gets the specified cache key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="cacheKey">The cache key.</param>
		/// <param name="getValueFunction">The get value function.</param>
		/// <returns></returns>
		public T Get<T>(CacheKey cacheKey, Func<T> getValueFunction)
		{
			if (cacheKey == null)
			{
				throw new ArgumentNullException("cacheKey");
			}
			if (getValueFunction == null)
			{
				throw new ArgumentNullException("getValueFunction");
			}

			T retVal = default(T);

			var repository = GetProvider(cacheKey.CacheGroup);

			if (repository == null)
			{
				retVal = getValueFunction();
			}
			else
			{
				var cachedObj = repository.Get(cacheKey);
				if (cachedObj == null)
				{
					cachedObj = new CachedObject(getValueFunction());

					var expirationTimeout = GetExpirationTimeoutByCacheGroup(cacheKey.CacheGroup);
					if (expirationTimeout != TimeSpan.Zero)
					{
						repository.Put(cacheKey, cachedObj, expirationTimeout);
					}
				}

				retVal = (T)cachedObj.Data;
			}

			return retVal;
		}

		
		private ICacheProvider GetProvider(string group)
		{
			ICacheProvider retVal = null;

			var settings = GetSettings(group);
			if (settings != null && settings.IsEnabled)
			{
				retVal = CacheProviderFactory(settings.ProviderName);
			}
			return retVal;
		}

		private TimeSpan GetExpirationTimeoutByCacheGroup(string group)
		{
			var retVal = TimeSpan.Zero;
			var settings = GetSettings(group);
			if (settings.IsEnabled)
			{
				retVal = settings.Timeout;
			}
			return retVal;
		}

		private CacheSettings GetSettings(string cacheGroup)
		{
			return CacheSettingFactory(cacheGroup);
		}

	}
}
