using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace VirtoCommerce.Platform.Core.Common
{
	public static class PredicateBuilder
	{
		public static Expression<Func<T, bool>> True<T>() { return f => true; }
		public static Expression<Func<T, bool>> False<T>() { return f => false; }

		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
		{
			var invokedExpr = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expression1.Body, invokedExpr), expression1.Parameters);
		}

		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
		{
			var invokedExpr = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expression1.Body, invokedExpr), expression1.Parameters);
		}
	}
}
