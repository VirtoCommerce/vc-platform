using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
	internal class CacheLinqQuery<T> : IOrderedQueryable<T>
	{
		private Expression queryExpression;
		private CacheQueryProvider queryProvider;

		internal CacheLinqQuery(CacheQueryProvider queryProvider, Expression queryExpression)
		{
			this.queryProvider = queryProvider;
			this.queryExpression = queryExpression;
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return this.queryProvider.ExecuteQuery<T>(this.queryExpression);
		}

		#endregion

		#region IEnumerable Members
	
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.queryProvider.ExecuteQuery<T>(this.queryExpression);
		}

		#endregion

		#region IQueryable Members

		public Type ElementType
		{
			get { return typeof(T); }
		}

		public Expression Expression
		{
			get { return this.queryExpression; }
		}

		public IQueryProvider Provider
		{
			get { return this.queryProvider; }
		}

		#endregion
	}
}
