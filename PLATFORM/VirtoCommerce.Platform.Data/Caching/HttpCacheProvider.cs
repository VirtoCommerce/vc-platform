using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Data.Caching
{
	public class HttpCacheProvider : ICacheProvider
	{
		private readonly object _lock = new object();
		private object _cache;

		private Cache GetCache()
		{
			if (_cache != null)
				return _cache as Cache;

			lock (_lock)
			{
				if (_cache != null)
					return _cache as Cache;

				var context = HttpContext.Current;

				_cache = context != null ? context.Cache : HttpRuntime.Cache;
			}

			return _cache as Cache;
		}

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

		public object Get(string key)
		{
			var cache = GetCache();
			return cache.Get(key);
		}

		public object this[string key]
		{
			get { return Get(key); }
			set
			{
				Add(key, value);
			}
		}

		public bool Remove(string key)
		{
			var cache = GetCache();
			cache.Remove(key);
			return true;
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			var cache = GetCache();
			return cache.GetEnumerator();
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

		public object GetAndLock(string key, TimeSpan timespan, out object lockHandle)
		{
			lockHandle = CacheEntries.GetLock(key);
			return Get(key);
		}

		public object PutAndUnlock(string key, object value, object lockHandle)
		{
			//var cache = GetCache();
			//return cache.PutAndUnlock(key, Serialize(value), (DataCacheLockHandle)lockHandle);
			return null;
		}

		public void Unlock(string key, object lockHandle)
		{
			//var cache = GetCache();
			//cache.Unlock(key, (DataCacheLockHandle)lockHandle);
		}

		private void Insert(string key, object obj, CacheDependency dep, TimeSpan timeframe, CacheItemPriority priority, CacheItemRemovedCallback callback)
		{
			if (obj != null)
			{
				var cache = GetCache();
				cache.Insert(key, obj, dep, DateTime.UtcNow.Add(timeframe), Cache.NoSlidingExpiration, priority, callback);
			}
		}

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

		/// <summary>
		/// Items the removed callback.
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
	}
}
