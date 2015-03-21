using System;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
    public class CacheHelper
    {
        private readonly ICacheRepository _cacheRepository;
        public const string GlobalCachePrefix = "_vcc@che";

        public string KeyPrefix { get; private set; }

        public CacheHelper(ICacheRepository repository)
            : this(repository, null)
        {
        }

        public CacheHelper(ICacheRepository repository, string keyPrefix)
        {
            _cacheRepository = repository;
            KeyPrefix = keyPrefix ?? string.Empty;
        }

        public T Get<T>(string cacheKey, Func<T> fallbackFunction, TimeSpan timeSpan, bool useCache = true) where T : class
        {
            if (_cacheRepository == null || !useCache)
            {
                return fallbackFunction();
            }

            var data = _cacheRepository.Get(cacheKey);

            if (data != null)
            {
                if (data == DBNull.Value)
                {
                    return null;
                }
                return data as T;
            }

            var data2 = fallbackFunction();
            _cacheRepository.Add(cacheKey, data2 ?? (object)DBNull.Value, timeSpan);

            return data2;
        }


        public void Add(string key, object value)
        {
            _cacheRepository.Add(key, value);
        }

        public void Add(string key, object value, TimeSpan timeout)
        {
            _cacheRepository.Add(key, value, timeout);
        }

        public object Get(string key)
        {
            return _cacheRepository[key];
        }

        public bool Remove(string key)
        {
            return _cacheRepository.Remove(key);
        }

        public void Clear()
        {
            _cacheRepository.Clear();
        }

        public string CreateKey(params string[] keyParts)
        {
            return CreateCacheKey(KeyPrefix, keyParts);
        }

        /// <summary>
        /// Creates the cache key.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public static string CreateCacheKey(string prefix, params string[] keys)
        {
            var returnKey = new StringBuilder(string.Concat(GlobalCachePrefix, prefix));

            if (keys != null)
                foreach (var key in keys)
                {
                    returnKey.Append("-");
                    returnKey.Append(key);
                }

            return returnKey.ToString();
        }

        public static string CreateCacheKey(params string[] keys)
        {
            var returnKey = new StringBuilder();

            if (keys != null)
                foreach (var key in keys)
                {
                    returnKey.Append("-");
                    returnKey.Append(key);
                }

            return returnKey.ToString();
        }
    }
}
