using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Linq.Expressions;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
	public class ExpressionCacheKeyGenerator : ICacheKeyGenerator
	{
		#region ICacheKeyGenerator Members

		public CacheKey GetCacheKey(object keyData)
		{
			if (keyData == null)
				throw new ArgumentNullException("keyData");

			CacheKey result = null;

			var expression = keyData as Expression;
			if (expression != null)
				result = GetCacheKey(expression);

			return result;
		}

		#endregion

		public CacheKey GetCacheKey(Expression expression)
		{
			// use the string representation of the expression for the cache key
			string key = expression.ToString();

			// the key is potentially very long, so use an md5 fingerprint
			// (fine if the query result data isn't critically sensitive)
			key = ToMd5Fingerprint(key);

			return CacheKey.Create(null, key); ;
		}

		public string ToMd5Fingerprint(string value)
		{
			var bytes = Encoding.Unicode.GetBytes(value.ToCharArray());
			var hash = new MD5CryptoServiceProvider().ComputeHash(bytes);

			// concat the hash bytes into one long string
			return hash.Aggregate(new StringBuilder(32),
				(sb, b) => sb.Append(b.ToString("X2")))
				.ToString();
		}
	}
}
