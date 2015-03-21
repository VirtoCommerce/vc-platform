using System;
using System.Collections.Generic;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
	/// <summary>
	/// Представляет методы работы с кэшем
	/// </summary>
	public class CacheManager
	{
		private Func<string, ICacheRepository> CacheRepositoryFactory;
		private Func<string, CacheSettings> CacheSettingFactory;
		private static CacheManager _noCacheManager= new CacheManager(x => null , x => null);

		public CacheManager(Func<string, ICacheRepository> cacheRepositoryFactory, Func<string, CacheSettings> settingFactory)
		{
			CacheRepositoryFactory = cacheRepositoryFactory;
			CacheSettingFactory = settingFactory;
		}
	
		public static CacheManager NoCache
		{
			get
			{
				return _noCacheManager;
			}
		}

		public void Put(CacheKey cacheKey, object cacheValue)
		{
			if (cacheKey == null)
				throw new ArgumentNullException("cacheKey");

			var repository = GetRepository(cacheKey.CacheGroup);
			if (repository != null)
			{
				var expirationTimeout = GetExpirationTimeoutByCacheGroup(cacheKey.CacheGroup);
				var cachedObj = new CachedObject(cacheValue);
				repository.Add(cacheKey.Key, cachedObj, expirationTimeout);
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

			var repository = GetRepository(cacheKey.CacheGroup);

			if (repository != null)
			{
				var cachedObj = repository.Get(cacheKey.Key) as CachedObject;
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

			var repository = GetRepository(cacheKey.CacheGroup);

			if (repository == null)
			{
				retVal = getValueFunction();
			}
			else
			{
				var cachedObj = repository.Get(cacheKey.Key) as CachedObject;
				if (cachedObj == null)
				{
					cachedObj = new CachedObject(getValueFunction());

					var expirationTimeout = GetExpirationTimeoutByCacheGroup(cacheKey.CacheGroup);
					if (expirationTimeout != TimeSpan.Zero)
					{
						repository.Add(cacheKey.Key, cachedObj, expirationTimeout);
					}
				}

				retVal = (T)cachedObj.Data;
			}

			return retVal;
		}


		private ICacheRepository GetRepository(string group)
		{
			ICacheRepository retVal = null;

			var settings = GetSettings(group);
			if (settings != null && settings.IsEnabled)
			{
				retVal = CacheRepositoryFactory(settings.ProviderName);
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
