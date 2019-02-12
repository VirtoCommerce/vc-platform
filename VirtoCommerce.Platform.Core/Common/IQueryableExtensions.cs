using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        public static IOrderedQueryable<T> OrderBySortInfos<T>(this IQueryable<T> source, SortInfo[] sortInfos)
        {
            if (sortInfos.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(sortInfos));
            }
            IOrderedQueryable<T> retVal;
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
            if (!sortInfos.IsNullOrEmpty())
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
                throw new ArgumentNullException(nameof(property));

            var props = property.Split('.');
            var registeredType = AbstractTypeFactory<T>.TryCreateInstance()?.GetType();
            var effectiveType = registeredType ?? typeof(T);
            var arg = Expression.Parameter(typeof(T), "x");
            Expression expr = Expression.Convert(arg, effectiveType);

            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = effectiveType.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    effectiveType = pi.PropertyType;
                }
                else
                {
                    return source.OrderBy(x => 1);
                }
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), effectiveType);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(arg.Type, effectiveType)
                    .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result;
        }
    }
}
