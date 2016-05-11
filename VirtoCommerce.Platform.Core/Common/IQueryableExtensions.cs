using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace VirtoCommerce.Platform.Core.Common
{
	public static class IQueryableExtensions
	{
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "OrderBy");
		}
		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "OrderByDescending");
		}
		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "ThenBy");
		}
		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "ThenByDescending");
		}

      
        public static IOrderedQueryable<T> OrderBySortInfos<T>(this IQueryable<T> source, SortInfo[] sortInfos)
        {
            if(sortInfos.IsNullOrEmpty())
            {
                throw new ArgumentNullException("sortInfos");
            }
            IOrderedQueryable<T> retVal = null;
            var firstSortInfo = sortInfos.First();
            if (firstSortInfo.SortDirection == SortDirection.Descending)
            {
                retVal = source.OrderByDescending(firstSortInfo.SortColumn);
            }
            else
            {
                retVal = source.OrderBy(firstSortInfo.SortColumn);
            }
            return retVal.ThenBySortInfos(sortInfos.Skip(1).ToArray());
        }

        public static IOrderedQueryable<T> ThenBySortInfos<T>(this IOrderedQueryable<T> source, SortInfo[] sortInfos)
        {
            var retVal = source;
            if (sortInfos.IsNullOrEmpty())
            {
                foreach (var sortInfo in sortInfos)
                {
                    if (sortInfo.SortDirection == SortDirection.Descending)
                    {
                        retVal = retVal.ThenByDescending(sortInfo.SortColumn);
                    }
                    else
                    {
                        retVal = retVal.ThenBy(sortInfo.SortColumn);
                    }
                }
            }
            return retVal;
        }

        public static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
		{
			if (property == null)
				throw new ArgumentNullException("property");

			string[] props = property.Split('.');
			Type type = typeof(T);
			ParameterExpression arg = Expression.Parameter(type, "x");
			Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }
                else
                {
                    return source.OrderBy(x => 1);
                }
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
			LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

			object result = typeof(Queryable).GetMethods().Single(
					method => method.Name == methodName
							&& method.IsGenericMethodDefinition
							&& method.GetGenericArguments().Length == 2
							&& method.GetParameters().Length == 2)
					.MakeGenericMethod(typeof(T), type)
					.Invoke(null, new object[] { source, lambda });
			return (IOrderedQueryable<T>)result;
		}

	}
}
