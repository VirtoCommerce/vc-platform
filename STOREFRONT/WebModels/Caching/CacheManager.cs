using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtoCommerce.Web.Caching
{
    public class CacheManager
    {
        private readonly  ICacheProvider _cacheProvider;

        public CacheManager(ICacheProvider defaultCacheProvider)
        {

            _cacheProvider = defaultCacheProvider;
        }


		public void Put(CacheKey cacheKey, TimeSpan expiration, object cacheValue)
		{
			if (cacheKey == null)
				throw new ArgumentNullException("cacheKey");

			var repository = GetProvider(cacheKey.CacheGroup);
			if (repository == null)
			{
				throw new NullReferenceException("repository");
			}

		
			var cachedObj = new CachedObject(cacheValue);
			repository.Add(cacheKey.Key, cachedObj, expiration);

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
        public async Task<T> GetAsync<T>(CacheKey cacheKey, TimeSpan expiration, Func< Task<T> > getValueFunction)
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
                retVal = await getValueFunction();
            }
            else
            {
                var cachedObj = repository.Get(cacheKey.Key) as CachedObject;
                if (cachedObj == null)
                {
                    cachedObj = new CachedObject(await getValueFunction());
                    repository.Add(cacheKey.Key, cachedObj, expiration);

                }

                retVal = (T)cachedObj.Data;
            }

            return retVal;
        }

		public void Clear()
		{
            _cacheProvider.Clear();
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
            return _cacheProvider;
        }

   
    }
}
