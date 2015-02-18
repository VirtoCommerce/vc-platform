#region

using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

#endregion

namespace VirtoCommerce.ApiClient.Caching
{

    #region

    #endregion

    public class HttpCacheRepository : ICacheRepository
    {
        #region Fields

        private readonly object _lock = new object();

        private object _cache;

        #endregion

        #region Public Indexers

        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Add(key, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Add(string key, object value)
        {
            var cache = GetCache();
            cache.Insert(key, value);
        }

        public void Add(string key, object value, TimeSpan timeout)
        {
            CacheItemRemovedCallback callback = null;

            // only need call back if item is in the locking states
            if (CacheEntries.ContainsKey(key))
            {
                callback = ItemRemovedCallback;
            }

            Insert(key, value, null, timeout, CacheItemPriority.Normal, callback);
        }

        public void Clear()
        {
            var cache = GetCache();
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }

        public object Get(string key)
        {
            var cache = GetCache();
            return cache.Get(key);
        }

        public object GetAndLock(string key, TimeSpan timespan, out object lockHandle)
        {
            lockHandle = CacheEntries.GetLock(key);
            return Get(key);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            var cache = GetCache();
            return cache.GetEnumerator();
        }

        public object PutAndUnlock(string key, object value, object lockHandle)
        {
            //var cache = GetCache();
            //return cache.PutAndUnlock(key, Serialize(value), (DataCacheLockHandle)lockHandle);
            return null;
        }

        public bool Remove(string key)
        {
            var cache = GetCache();
            cache.Remove(key);
            return true;
        }

        public void Unlock(string key, object lockHandle)
        {
            //var cache = GetCache();
            //cache.Unlock(key, (DataCacheLockHandle)lockHandle);
        }

        #endregion

        /*
		private void Insert(string key, object obj, CacheDependency dep, DateTimeOffset timeframeOffset, CacheItemPriority priority, CacheItemRemovedCallback callback)
		{
			if (obj != null)
			{
				var cache = GetCache();
				cache.Insert(key, obj, dep, timeframeOffset.UtcDateTime, Cache.NoSlidingExpiration, priority, callback);
			}
		}
*/

        #region Methods

        /// <summary>
        ///     Items the removed callback.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="reason">The reason.</param>
        internal static void ItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Expired)
            {
                var cacheEntry = CacheEntries.Get(key);

                if (cacheEntry != null)
                {
                    lock (cacheEntry.Lock)
                    {
                        CacheEntries.Remove(key);
                    }
                }
            }
        }

        private Cache GetCache()
        {
            if (_cache != null)
            {
                return _cache as Cache;
            }

            lock (_lock)
            {
                if (_cache != null)
                {
                    return _cache as Cache;
                }

                var context = HttpContext.Current;

                _cache = context != null ? context.Cache : HttpRuntime.Cache;
            }

            return _cache as Cache;
        }

        private void Insert(
            string key,
            object obj,
            CacheDependency dep,
            TimeSpan timeframe,
            CacheItemPriority priority,
            CacheItemRemovedCallback callback)
        {
            if (obj != null)
            {
                var cache = GetCache();
                cache.Insert(
                    key,
                    obj,
                    dep,
                    DateTime.UtcNow.Add(timeframe),
                    Cache.NoSlidingExpiration,
                    priority,
                    callback);
            }
        }

        #endregion
    }
}
