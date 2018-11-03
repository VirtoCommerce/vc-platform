using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var invokedExpr = Expression.Invoke(expression2, expression1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expression1.Body, invokedExpr), expression1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var invokedExpr = Expression.Invoke(expression2, expression1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expression1.Body, invokedExpr), expression1.Parameters);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var invoke = Expression.Invoke(expression, expression.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.Not(invoke), expression.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            var result = False<T>();

            foreach (var expression in expressions)
            {
                result = result.Or(expression);
            }

            return result;
        }

        public static Expression<Func<T, bool>> And<T>(this IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            var result = True<T>();

            foreach (var expression in expressions)
            {
                result = result.And(expression);
            }

            return result;
        }

        public static Expression<Func<T, bool>> AndNot<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            return expression1.And(expression2.Not());
        }

        public static Expression<Func<T, bool>> OrNot<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            return expression1.Or(expression2.Not());
        }
    }
}
