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

            var propertyStringValid = !string.IsNullOrEmpty(property);
            var effectiveParameterType = typeof(T);
            var props = property.Split('.');

            if (propertyStringValid && effectiveParameterType.GetProperty(props[0], BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public) == null)
            {
                effectiveParameterType = AbstractTypeFactory<T>.TryCreateInstance()?.GetType() ?? typeof(T);
            }

            var inParameter = Expression.Parameter(typeof(T));
            var expr = (effectiveParameterType == typeof(T)) ? (Expression)inParameter : Expression.Convert(inParameter, effectiveParameterType);

            var propertyResultingType = effectiveParameterType;
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = propertyResultingType.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    propertyResultingType = pi.PropertyType;
                }
                else
                {
                    propertyStringValid = false;
                }
            }
            IOrderedQueryable<T> result;
            if (propertyStringValid)
            {
                var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), propertyResultingType);
                var lambda = Expression.Lambda(delegateType, expr, inParameter);

                result = (effectiveParameterType == typeof(T))
                    ? HandleSingleTypeQueryable(source, methodName, inParameter.Type, propertyResultingType, lambda)
                    : HandleMixedQueryable(source, methodName, inParameter.Type, effectiveParameterType, propertyResultingType, lambda);
            }
            else
            {
                result = source.OrderBy(x => 1);
            }

            return result;
        }

        private static IOrderedQueryable<T> HandleMixedQueryable<T>(
            IQueryable<T> source,
            string methodName,
            Type inParameterType,
            Type effectiveParameterType,
            Type propertyResultingType,
            LambdaExpression sortValueGetter)
        {
            // constructing following logic:
            // var ofType = source.OfType<effectiveType>();
            // var except = source.Except(ofType);
            // var result = ofType.SortMethod(sortValueGetter).Concat(except);

            IOrderedQueryable<T> result;

            var ofType = (IQueryable<T>)typeof(Queryable).GetMethods()
                .Single(method => method.Name.EqualsInvariant("OfType")
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 1
                        && method.GetParameters().Length == 1)
                .MakeGenericMethod(effectiveParameterType)
                .Invoke(null, new object[] { source });

            var orderedOfType = HandleSingleTypeQueryable(ofType, methodName, inParameterType, propertyResultingType, sortValueGetter);

            var others = (IQueryable<T>)typeof(Queryable).GetMethods()
                .Single(method => method.Name.EqualsInvariant("Except")
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 1
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(inParameterType)
                .Invoke(null, new object[] { source, ofType });
            result = ((IQueryable<T>)typeof(Queryable).GetMethods()
                .Single(method => method.Name.EqualsInvariant("Concat")
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 1
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(inParameterType)
                .Invoke(null, new object[] { orderedOfType, others }))
                .OrderBy(x => 1);
            return result;
        }

        private static IOrderedQueryable<T> HandleSingleTypeQueryable<T>(
            IQueryable<T> source,
            string methodName,
            Type inParameterType,
            Type propertyResultingType,
            LambdaExpression sortValueGetter)
        {
            return (IOrderedQueryable<T>)typeof(Queryable).GetMethods()
                .Single(method => method.Name.EqualsInvariant(methodName)
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(inParameterType, propertyResultingType)
                .Invoke(null, new object[] { source, sortValueGetter });
        }
    }
}
