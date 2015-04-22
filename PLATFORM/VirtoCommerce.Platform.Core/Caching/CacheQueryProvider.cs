using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Linq.Expressions;
using System.Reflection;

namespace VirtoCommerce.Platform.Core.Caching
{
	public class CacheQueryProvider : IQueryProvider
	{
		private IQueryable _underlyingQuery;
		private ICacheProvider _cacheProvider;

		private CacheQueryProvider(IQueryable underlyingQuery, ICacheProvider cacheProvider)
		{
			this._underlyingQuery = underlyingQuery;
			this._cacheProvider = cacheProvider;
		}

		public static IQueryable CreateCachedQuery(IQueryable underlyingQuery, ICacheProvider cacheProvider)
		{
			var provider = new CacheQueryProvider(underlyingQuery, cacheProvider);
			return provider.CreateQuery(underlyingQuery.Expression);
		}

		#region IQueryProvider Members

		public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
		{
			return new CacheLinqQuery<TElement>(this, expression);
		}

		public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
		{
			Type et = TypeSystem.GetIEnumerableElementType(expression.Type);
			Type qt = typeof(CacheLinqQuery<>).MakeGenericType(et);
			object[] args = new object[] { this, expression };

			ConstructorInfo ci = qt.GetConstructor(
				BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				new Type[] { typeof(CacheQueryProvider), typeof(Expression) },
				null);

			return (IQueryable)ci.Invoke(args);
		}

		public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
		{
			var key = _cacheProvider.KeyGenerator.GetCacheKey(expression);
			Func<TResult> getter = () => { return this._underlyingQuery.Provider.Execute<TResult>(expression); };
			var retVal = _cacheProvider.GetPutFromCache(key, getter);
			return retVal;
		}

		public object Execute(System.Linq.Expressions.Expression expression)
		{
			var key = _cacheProvider.KeyGenerator.GetCacheKey(expression);
			Func<object> getter = () => { return this._underlyingQuery.Provider.Execute(expression); };
			var retVal = _cacheProvider.GetPutFromCache(key, getter);
			return retVal;
		}

		#endregion

		internal IEnumerator<TElement> ExecuteQuery<TElement>(Expression expression)
		{
			var key = _cacheProvider.KeyGenerator.GetCacheKey(expression);
			Func<IEnumerable<TElement>> getter = () => { return this._underlyingQuery.Provider.CreateQuery<TElement>(expression).ToArray(); };
			var retVal = _cacheProvider.GetPutFromCache(key, getter);
			return retVal.GetEnumerator();
		}

	}
}
