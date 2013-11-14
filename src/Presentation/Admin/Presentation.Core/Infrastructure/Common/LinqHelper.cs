using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public static class LinqHelper
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
        public static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = linq.Expression.Parameter(type, "x");
            linq.Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = linq.Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = linq.Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        public static IOrderedQueryable<T> ApplySortOrder<T>(this IQueryable<T> query, SortDescriptionCollection sortDescriptions)
        {
            IOrderedQueryable<T> orderedItems;
            if (sortDescriptions[0].Direction == ListSortDirection.Ascending)
                orderedItems = query.OrderBy(sortDescriptions[0].PropertyName);
            else
                orderedItems = query.OrderByDescending(sortDescriptions[0].PropertyName);
            return orderedItems;
        }
    }
}
