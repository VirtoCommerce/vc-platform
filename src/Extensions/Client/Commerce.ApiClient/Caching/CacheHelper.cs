#region
using System;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace VirtoCommerce.ApiClient.Caching
{
    #region
    
    #endregion

    public class CacheHelper
    {
        #region Constants
        public const string GlobalCachePrefix = "_vcc@che";
        #endregion

        #region Fields
        private readonly ICacheRepository _cacheRepository;
        #endregion

        #region Constructors and Destructors
        public CacheHelper(ICacheRepository repository)
        {
            this._cacheRepository = repository;
        }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        ///     Creates the cache key.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public static string CreateCacheKey(string prefix, params string[] keys)
        {
            var returnKey = new StringBuilder(string.Concat(GlobalCachePrefix, prefix));

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    returnKey.Append("-");
                    returnKey.Append(key);
                }
            }

            return returnKey.ToString();
        }

        public static string CreateCacheKey(params string[] keys)
        {
            var returnKey = new StringBuilder();

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    returnKey.Append("-");
                    returnKey.Append(key);
                }
            }

            return returnKey.ToString();
        }

        public void Add(string key, object value)
        {
            this._cacheRepository.Add(key, value);
        }

        public void Add(string key, object value, TimeSpan timeout)
        {
            this._cacheRepository.Add(key, value, timeout);
        }

        public void Clear()
        {
            this._cacheRepository.Clear();
        }

        public T Get<T>(string cacheKey, Func<T> fallbackFunction, TimeSpan timeSpan, bool useCache = true)
            where T : class
        {
            if (this._cacheRepository == null || !useCache)
            {
                return fallbackFunction();
            }

            var data = this._cacheRepository.Get(cacheKey);

            if (data != null)
            {
                if (data == DBNull.Value)
                {
                    return null;
                }
                return data as T;
            }

            var data2 = fallbackFunction();
            this._cacheRepository.Add(cacheKey, data2 ?? (object)DBNull.Value, timeSpan);

            return data2;
        }

        public object Get(string key)
        {
            return this._cacheRepository[key];
        }

        public async Task<T> GetAsync<T>(
            string cacheKey,
            Func<Task<T>> fallbackFunction,
            TimeSpan timeSpan,
            bool useCache = true) where T : class
        {
            if (this._cacheRepository == null || !useCache)
            {
                return await fallbackFunction();
            }

            var data = this._cacheRepository.Get(cacheKey);

            if (data != null)
            {
                if (data == DBNull.Value)
                {
                    return null;
                }
                return data as T;
            }

            var data2 = await fallbackFunction();
            this._cacheRepository.Add(cacheKey, data2 ?? (object)DBNull.Value, timeSpan);

            return data2;
        }

        public bool Remove(string key)
        {
            return this._cacheRepository.Remove(key);
        }
        #endregion
    }
}