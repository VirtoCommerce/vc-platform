using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Common
{
    public class NullCacheItem
    {
    }

    public static class CacheManagerExtension
    {
        private static readonly ConcurrentDictionary<string, object> _locks = new ConcurrentDictionary<string, object>();

        public static T Get<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, Func<T> getValueFunction)
        {
            return cacheManager.Get(cacheKey, region, getValueFunction, true);
        }
        public static T Get<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, Func<T> getValueFunction, bool cacheNullValue)
        {
            var result = cacheManager.Get(cacheKey, region);
            if (result == null)
            {
                //http://stackoverflow.com/questions/12804879/is-it-ok-to-use-a-string-as-a-lock-object
                lock (_locks.GetOrAdd(cacheKey, x => new object()))
                {
                    result = cacheManager.Get(cacheKey, region);
                    if (result == null)
                    {
                        result = getValueFunction();
                        if (result != null || cacheNullValue)
                        {
                            cacheManager.Add(cacheKey, result ?? new NullCacheItem(), region);
                        }
                    }
                }
            }
            if (result is NullCacheItem)
            {
                result = default(T);
            }
            return (T)result;
        }
        public static T Get<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, TimeSpan expiration, Func<T> getValueFunction)
        {
            return cacheManager.Get(cacheKey, region, expiration, getValueFunction, true);
        }
        public static T Get<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, TimeSpan expiration, Func<T> getValueFunction, bool cacheNullValue)
        {
            return cacheManager.Get(cacheKey, region, expiration, ExpirationMode.Sliding, getValueFunction, true);
        }
        public static T Get<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, TimeSpan expiration, ExpirationMode expirationMode, Func<T> getValueFunction, bool cacheNullValue)
        {
            var result = cacheManager.Get(cacheKey, region);
            if (result == null)
            {
                //http://stackoverflow.com/questions/12804879/is-it-ok-to-use-a-string-as-a-lock-object
                lock (_locks.GetOrAdd(cacheKey, x => new object()))
                {
                    result = cacheManager.Get(cacheKey, region);
                    if (result == null)
                    {
                        result = getValueFunction();
                        if (result != null || cacheNullValue)
                        {
                            var cacheItem = new CacheItem<object>(cacheKey, region, result ?? new NullCacheItem(), expirationMode, expiration);
                            cacheManager.Add(cacheItem);
                        }
                    }
                }
            }
            if (result is NullCacheItem)
            {
                result = default(T);
            }
            return (T)result;
        }

        public static async Task<T> GetAsync<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, Func<Task<T>> getValueFunction)
        {
            return await cacheManager.GetAsync(cacheKey, region, getValueFunction, true);
        }

        public static async Task<T> GetAsync<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, Func<Task<T>> getValueFunction, bool cacheNullValue)
        {
            //http://sanjeev.dwivedi.net/?p=292
            var asyncLockObject = AsyncLock.GetLockByKey(cacheKey);
            var result = cacheManager.Get(cacheKey, region);
            if (result == null)
            {
                using (await asyncLockObject.LockAsync())
                {
                    result = cacheManager.Get(cacheKey, region);
                    if (result == null)
                    {
                        result = await getValueFunction();
                        if (result != null || cacheNullValue)
                        {
                            cacheManager.Add(cacheKey, result ?? new NullCacheItem(), region);
                        }
                    }
                }
            }
            if (result is NullCacheItem)
            {
                result = default(T);
            }
            return (T)result;
        }

        public static async Task<T> GetAsync<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, TimeSpan expiration, Func<Task<T>> getValueFunction)
        {
            return await cacheManager.GetAsync(cacheKey, region, expiration, getValueFunction, true);
        }
        public static async Task<T> GetAsync<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, TimeSpan expiration, Func<Task<T>> getValueFunction, bool cacheNullValue)
        {
            return await cacheManager.GetAsync(cacheKey, region, expiration, ExpirationMode.Sliding, getValueFunction, true);
        }
        public static async Task<T> GetAsync<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, TimeSpan expiration, ExpirationMode expirationMode, Func<Task<T>> getValueFunction, bool cacheNullValue)
        {
            var result = cacheManager.Get(cacheKey, region);
            if (result == null)
            {
                //http://sanjeev.dwivedi.net/?p=292
                var asyncLockObject = AsyncLock.GetLockByKey(cacheKey);
                using (await asyncLockObject.LockAsync())
                {
                    result = cacheManager.Get(cacheKey, region);
                    if (result == null)
                    {
                        result = await getValueFunction();
                        if (result != null || cacheNullValue)
                        {
                            var cacheItem = new CacheItem<object>(cacheKey, region, result ?? new NullCacheItem(), expirationMode, expiration);
                            cacheManager.Add(cacheItem);
                        }
                    }
                }
            }
            if (result is NullCacheItem)
            {
                result = default(T);
            }
            return (T)result;
        }
    }
}
