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
                return this.Get(key);
            }
            set
            {
                this.Add(key, value);
            }
        }
        #endregion

        #region Public Methods and Operators
        public void Add(string key, object value)
        {
            var cache = this.GetCache();
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

            this.Insert(key, value, null, timeout, CacheItemPriority.Normal, callback);
        }

        public void Clear()
        {
            var cache = this.GetCache();
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }

        public object Get(string key)
        {
            var cache = this.GetCache();
            return cache.Get(key);
        }

        public object GetAndLock(string key, TimeSpan timespan, out object lockHandle)
        {
            lockHandle = CacheEntries.GetLock(key);
            return this.Get(key);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            var cache = this.GetCache();
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
            var cache = this.GetCache();
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
            if (this._cache != null)
            {
                return this._cache as Cache;
            }

            lock (this._lock)
            {
                if (this._cache != null)
                {
                    return this._cache as Cache;
                }

                var context = HttpContext.Current;

                this._cache = context != null ? context.Cache : HttpRuntime.Cache;
            }

            return this._cache as Cache;
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
                var cache = this.GetCache();
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