using System;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
	public class CacheHelper
	{
		private readonly ICacheRepository CacheRepository;
		private string RegionName = "default";

		public CacheHelper(ICacheRepository repository)
		{
			CacheRepository = repository;
		}

		public CacheHelper(ICacheRepository repository, string regionName)
		{
			CacheRepository = repository;
			RegionName = regionName;
		}

		public T Get<T>(string cacheKey, Func<T> fallbackFunction, TimeSpan timeSpan, bool useCache = true) where T : class
		{
			if (useCache && this.CacheRepository != null)
			{
				var data = this.CacheRepository.Get(cacheKey/*, this.RegionName*/) as T;
				if (data != null)
				{
					return data;
				}
			}

			var data2 = fallbackFunction();
			if (data2 != null && this.CacheRepository != null)
			{
				this.CacheRepository.Add(cacheKey, data2, timeSpan/*, this.regionName*/);
			}

			return data2;
		}


		public void Add(string key, object value)
		{
			CacheRepository.Add(key, value);
		}

		public void Add(string key, object value, TimeSpan timeout)
		{
			CacheRepository.Add(key, value, timeout);
		}

		public object Get(string key)
		{
			return CacheRepository[key];
		}

		public bool Remove(string key)
		{
			return CacheRepository.Remove(key);
		}

		public void Clear()
		{
			CacheRepository.Clear();
		}

		/*
		public string CreateCacheKey(string prefix, params string[] keys)
		{
			return CacheHelper.CreateCacheKey(prefix, keys);
		}
		 * */

		/// <summary>
		/// Creates the cache key.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <param name="keys">The keys.</param>
		/// <returns></returns>
		public static string CreateCacheKey(string prefix, params string[] keys)
		{
			var returnKey = new StringBuilder(prefix);

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
