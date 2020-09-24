using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, nameof(OrderBy));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, nameof(OrderByDescending));
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            // We are intentionally skipping casting to effective type as we are already working with IOrderedQueryable, and OfType produces IQueryable, for which e.g. ThenBy is not applied
            return ApplyOrder<T, IOrderedQueryable<T>>(source, property, nameof(ThenBy), false);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T, IOrderedQueryable<T>>(source, property, nameof(ThenByDescending), false);
        }


        public static IOrderedQueryable<T> OrderBySortInfos<T>(this IQueryable<T> source, IEnumerable<SortInfo> sortInfos)
        {
            if (sortInfos.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(sortInfos));
            }

            var elementType = typeof(T);
            var firstSortInfo = sortInfos.First();
            var methodName = (firstSortInfo.SortDirection == SortDirection.Descending) ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);

            // source.OrderBy/OrderByDescending<T>(firstSortInfo.SortColumn)
            var firstSortResult = InvokeGenericMethod(typeof(IQueryableExtensions), methodName, new[] { elementType }, new object[] { source, firstSortInfo.SortColumn });
            var remainingSortInfos = sortInfos.Skip(1).ToArray();
            // firstSortResult.ThenBySortInfos<T>(remainingSortInfos)
            var result = InvokeGenericMethod(typeof(IQueryableExtensions), nameof(IQueryableExtensions.ThenBySortInfos), new[] { elementType }, new object[] { firstSortResult, remainingSortInfos });

            return (IOrderedQueryable<T>)result;
        }

        public static IOrderedQueryable<T> ThenBySortInfos<T>(this IOrderedQueryable<T> source, SortInfo[] sortInfos)
        {
            var result = source;
            if (!sortInfos.IsNullOrEmpty())
            {
                foreach (var sortInfo in sortInfos)
                {
                    if (sortInfo.SortDirection == SortDirection.Descending)
                    {
                        result = result.ThenByDescending(sortInfo.SortColumn);
                    }
                    else
                    {
                        result = result.ThenBy(sortInfo.SortColumn);
                    }
                }
            }
            return result;
        }

        public static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            return ApplyOrder<T, IQueryable<T>>(source, property, methodName, true);
        }

        /// <summary>
        /// Applies ordering to the IQueryable, based on property string
        /// </summary>
        /// <typeparam name="TElement">IQueryable element type</typeparam>
        /// <typeparam name="TQueryable">IQueryable specific type (e.g. could be <see cref="IOrderedQueryable{T}"/>)</typeparam>
        /// <param name="source">IQueryable to sort</param>
        /// <param name="property">Property string, e.g. Student.Address.City</param>
        /// <param name="methodName">Sort method name from <see cref="Queryable"/> class - e.g. OrderBy, ThenBy, etc.</param>
        /// <param name="castToEffectiveType">Set to true to cast collection to type registered in <see cref="AbstractTypeFactory{BaseType}"/>.
        /// NOTE! After casting with <see cref="Queryable.OfType{TResult}(IQueryable)"/> <typeparamref name="TQueryable"/> (e.g. <see cref="IOrderedQueryable{T}"/>) becomes <see cref="IQueryable{T}"/>,
        /// so then it cannot be passed to methods that accept e.g. <see cref="IOrderedQueryable{T}"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TElement> ApplyOrder<TElement, TQueryable>(TQueryable source, string property, string methodName, bool castToEffectiveType) where TQueryable : IQueryable<TElement>
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            IOrderedQueryable<TElement> result = null;

            var effectiveType = GetEffectiveType<TElement>();

            var sourceOfEffectiveType = source;

            if (castToEffectiveType)
            {
                sourceOfEffectiveType = (TQueryable)ConvertCollectionToType(source, effectiveType);
            }

            var expressionArgument = Expression.Parameter(typeof(TElement));
            var expr = (effectiveType == typeof(TElement)) ? (Expression)expressionArgument : Expression.Convert(expressionArgument, effectiveType);
            var propertyExpression = GetPropertyExpression(property, expr);

            if (propertyExpression == null)
            {
                return source.OrderBy(x => 1);
            }

            var propertyType = propertyExpression.Type;
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TElement), propertyType);
            // It is expression for getting property value on each object from collection - e.g. for Student: x => x.Address.City
            var lambda = Expression.Lambda(delegateType, propertyExpression, expressionArgument);

            // Calling existing System.Linq.Queryable sorting method, e.g. for methodName = OrderBy: sourceOfEffectiveType.OrderBy(x => x.Address.City)
            result = (IOrderedQueryable<TElement>)InvokeGenericMethod(typeof(Queryable), methodName, new[] { typeof(TElement), propertyType }, new object[] { sourceOfEffectiveType, lambda });

            return result;
        }

        /// <summary>
        /// Converts <see cref="IQueryable{T}"/> of one type to <see cref="IQueryable{EffectiveType}"/> of another type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="effectiveType"></param>
        /// <returns></returns>
        private static IQueryable<T> ConvertCollectionToType<T>(IQueryable<T> source, Type effectiveType)
        {
            var result = source;

            // If registered type is not T - need to cast collection to allow to use registered type own properties
            if (effectiveType != typeof(T))
            {
                // sourceOfEffectiveType = source.OfType<"effectiveType">()
                result = (IQueryable<T>)InvokeGenericMethod(typeof(Queryable), nameof(Queryable.OfType), new[] { effectiveType }, new object[] { source });
            }
            else
            {
                // no cast needed - use T
            }

            return result;
        }

        /// <summary>
        /// Invokes generic method with the given params
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="methodName"></param>
        /// <param name="genericTypeArguments"></param>
        /// <param name="methodArguments"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static object InvokeGenericMethod(Type methodType, string methodName, Type[] genericTypeArguments, object[] methodArguments, object instance = null)
        {
            try
            {
                return methodType.GetMethods()
                    .Single(method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == genericTypeArguments.Length
                        && method.GetParameters().Length == methodArguments.Length)
                    .MakeGenericMethod(genericTypeArguments)
                    .Invoke(instance, methodArguments);
            }
            // This catch is needed to get unwrapped exception, like calling method without reflection. Otherwise it would be TargetInvocationException with meaningful inner exception
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        /// <summary>
        /// Gets type registered in <see cref="AbstractTypeFactory{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Returns single registered type, or <typeparamref name="T"/> if no types registered, or <code>null</code> if more than one registered</returns>
        private static Type GetEffectiveType<T>()
        {
            Type result;
            var registeredTypes = AbstractTypeFactory<T>.AllTypeInfos.ToList();

            // If only one registered type - return it
            if (registeredTypes.Count == 1)
            {
                result = registeredTypes[0].Type;
            }
            else
            {
                result = typeof(T);
            }

            return result;
        }

        /// <summary>
        /// Gets property expression of the last property in chain for the object expression of given type 
        /// </summary>
        /// <param name="propertyString">Property chain, e.g. "Date.Year" for the DateTime type</param>
        /// <param name="objectExpression">Linq expression with the object variable</param>
        /// <returns></returns>
        private static Expression GetPropertyExpression(string propertyString, Expression objectExpression)
        {
            var result = objectExpression;
            var propertyType = objectExpression.Type;
            var props = propertyString.Split('.');
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = propertyType.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    result = Expression.Property(result, pi);
                    propertyType = pi.PropertyType;
                }
                else
                {
                    result = null;
                    break;
                }
            }

            return result;
        }
    }
}
