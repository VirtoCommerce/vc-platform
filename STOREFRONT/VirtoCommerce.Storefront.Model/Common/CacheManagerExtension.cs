using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CacheManager.Core;

namespace VirtoCommerce.Storefront.Model.Common
{
    public static class CacheManagerExtension
    {
        private static object _lockObject = new object();


        public static T Get<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, Func<T> getValueFunction)
        {
            var result = cacheManager.Get<T>(cacheKey, region);
            if (result == null)
            {
                lock (_lockObject)
                {
                    result = cacheManager.Get<T>(cacheKey, region);
                    if (result == null)
                    {
                        result = getValueFunction();
                        if (result != null)
                        {
                            cacheManager.Put(cacheKey, result, region);
                        }
                    }
                }
            }
            return result;
        }

        public static async Task<T> GetAsync<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, Func<Task<T>> getValueFunction)
        {
            //http://sanjeev.dwivedi.net/?p=292
            var asyncLockObject = new AsyncLock();
            var result = cacheManager.Get<T>(cacheKey, region);
            if (result == null)
            {
                using (var releaser = await asyncLockObject.LockAsync())
                {
                    result = cacheManager.Get<T>(cacheKey, region);
                    if (result == null)
                    {
                        result = await getValueFunction();
                        if (result != null)
                        {
                            cacheManager.Put(cacheKey, result, region);
                        }
                    }
                }
            }
            return result;
        }

        public static async Task<T> GetAsync<T>(this ICacheManager<object> cacheManager, string cacheKey, string region, TimeSpan expiration, Func<Task<T>> getValueFunction)
        {

            var result = cacheManager.Get<T>(cacheKey, region);
            if (result == null)
            {
                //http://sanjeev.dwivedi.net/?p=292
                var asyncLockObject = new AsyncLock();
                using (var releaser = await asyncLockObject.LockAsync())
                {
                    result = cacheManager.Get<T>(cacheKey, region);
                    if (result == null)
                    {
                        result = await getValueFunction();
                        if (result != null)
                        {
                            var cacheItem = new CacheItem<object>(cacheKey, region, result, ExpirationMode.Absolute, expiration);
                            cacheManager.Add(cacheItem);
                        }
                    }
                }
            }
            return result;
        }
    }
}