using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Data.Caching
{
	public class InMemoryCachingProvider : ICacheProvider
	{
		private ConcurrentDictionary<string, CachedObject> _cache = new ConcurrentDictionary<string, CachedObject>();
		private ICacheKeyGenerator _keyGenerator = new ExpressionCacheKeyGenerator();
		public int MissCount { get; set; }
		public int TotalCount { get; set; }

		#region ICacheProvider Members

		public void Add(string key, object value)
		{
			_cache[key] = value as CachedObject;
		}

		public void Add(string key, object value, TimeSpan timeSpan)
		{
			Add(key, value);
		}

		public object Get(string key)
		{
			CachedObject retVal;
			TotalCount++;
			if (!_cache.TryGetValue(key, out retVal))
			{
				MissCount++;
			}
			return retVal;
		}

		public object GetAndLock(string key, TimeSpan timeSpan, out object lockHandle)
		{
			throw new NotImplementedException();
		}

		public object PutAndUnlock(string key, object value, object lockHandle)
		{
			throw new NotImplementedException();
		}

		public void Unlock(string key, object lockHandle)
		{
			throw new NotImplementedException();
		}

		public object this[string key]
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool Remove(string key)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public System.Collections.IDictionaryEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
