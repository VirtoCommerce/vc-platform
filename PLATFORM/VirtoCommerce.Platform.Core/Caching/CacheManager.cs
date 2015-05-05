using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Caching
{
    public class CacheManager
    {
        private readonly Dictionary<string, ICacheProvider> _cacheProviders;
        private readonly List<CacheSettings> _cacheSettings;

        public CacheManager(ICacheProvider defaultCacheProvider, CacheSettings[] cacheSettings)
        {
			_cacheSettings = new List<CacheSettings>();
			if(cacheSettings != null)
			{
				_cacheSettings.AddRange(cacheSettings);
			}
			_cacheProviders = new Dictionary<string, ICacheProvider>();
			AddCacheProvider("default", defaultCacheProvider);
        }

		public void AddCacheSettings(CacheSettings[] cacheSettings)
		{
			if(cacheSettings == null)
			{
				throw new ArgumentNullException("cacheSettings");
			}
			_cacheSettings.AddRange(cacheSettings.Where(x => !_cacheSettings.Contains(x)));
		}

		public void AddCacheProvider(string providerName, ICacheProvider cacheProvider)
		{
			if (!_cacheProviders.ContainsKey(providerName))
			{
				_cacheProviders.Add(providerName, cacheProvider);
			}
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

            var repository = GetProvider(cacheKey.CacheGroup);

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

            T retVal;

            var repository = GetProvider(cacheKey.CacheGroup);

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

        public bool Remove(CacheKey cacheKey)
        {
            if (cacheKey == null)
            {
                throw new ArgumentNullException("cacheKey");
            }

            var result = false;
            var repository = GetProvider(cacheKey.CacheGroup);

            if (repository != null)
            {
                result = repository.Remove(cacheKey.Key);
            }

            return result;
        }


        private ICacheProvider GetProvider(string group)
        {
            ICacheProvider retVal = null;

            var settings = GetSettings(group);
            if (settings != null && settings.IsEnabled)
            {
				if (!_cacheProviders.TryGetValue(settings.ProviderName, out retVal))
				{
					if(String.IsNullOrEmpty(settings.ProviderName))
					{
						retVal = _cacheProviders["default"];
					}
					else
					{
						throw new NullReferenceException("Cache provider " + settings.ProviderName + " not registered");
					}
				}
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
			return _cacheSettings.FirstOrDefault(x => String.Equals(cacheGroup, x.Group));
        }
    }
}
