using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
	public class CacheKey
	{
		public string CacheGroup { get; private set; }
		public string Key { get; private set; }

		private CacheKey(string cacheGroup, string key)
		{
			CacheGroup = cacheGroup;
			Key = key;
		}

		public static CacheKey Create(string cacheGroup, params string[] keyItems)
		{
			StringBuilder keyBuilder = new StringBuilder(cacheGroup);

			foreach (string item in keyItems)
			{
				keyBuilder.Append("-");
				keyBuilder.Append(item);
			}

			return new CacheKey(cacheGroup, keyBuilder.ToString());
		}

		public override string ToString()
		{
			return Key;
		}
	}
}
