using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VirtoCommerce.Platform.Core.Common;

public static class ReflectionUtility
{
    private static readonly ObjectReferenceComparer _objectReferenceComparer = new();

    private static readonly ConcurrentDictionary<(Type Source, Type Target), FlattenEntry[]> _flattenPlans = new();
    private static readonly ConcurrentDictionary<Type, bool> _assignableFromGenericListCache = new();
    private static readonly ConcurrentDictionary<Type, bool> _isDictionaryCache = new();
    private static readonly ConcurrentDictionary<Type, Type[]> _typeInheritanceChains = new();
    private static readonly ConcurrentDictionary<(Type Type, Type ToBaseType), Type[]> _typeInheritanceChainsTo = new();
    private static readonly ConcurrentDictionary<(Type Type, Type Attribute, bool Inherit), PropertyInfo[]> _propertiesWithAttribute = new();

    public static IEnumerable<string> GetPropertyNames<T>(params Expression<Func<T, object>>[] propertyExpressions)
    {
        var retVal = new List<string>();
        foreach (var propertyExpression in propertyExpressions)
        {
            retVal.Add(GetPropertyName(propertyExpression));
        }

        return retVal;
    }

    public static string GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
    {
        if (propertyExpression is null)
        {
            return null;
        }

        LambdaExpression lambda = propertyExpression;
        MemberExpression memberExpression;
        if (lambda.Body is UnaryExpression unaryExpression)
        {
            memberExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            memberExpression = (MemberExpression)lambda.Body;
        }
        var retVal = memberExpression.Member.Name;

        return retVal;
    }

    public static bool IsHaveAttribute(this PropertyInfo propertyInfo, Type attribute)
    {
        return propertyInfo.GetCustomAttributes(attribute, true).Length > 0;
    }


    /// <summary>
    /// Search all type properties (ancestors properties included) that have specific attribute
    /// </summary>
    /// <param name="attribute">The attribute to search</param>
    /// <param name="type">The type which properties need to be sought</param>
    /// <returns></returns>
    public static PropertyInfo[] FindPropertiesWithAttribute(this Type type, Type attribute)
    {
        return type.FindPropertiesWithAttribute(attribute, true);
    }

    /// <summary>
    /// Search all type properties that have specific attribute
    /// </summary>
    /// <param name="attribute">The attribute to search</param>
    /// <param name="inherit">Should search or not thru type ancestors properties</param>
    /// <param name="type">The type which properties need to be sought</param>
    /// <returns></returns>
    public static PropertyInfo[] FindPropertiesWithAttribute(this Type type, Type attribute, bool inherit)
    {
        return _propertiesWithAttribute.GetOrAdd((type, attribute, inherit), static key =>
            key.Type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttributes(key.Attribute, key.Inherit).Length > 0)
                .ToArray());
    }

    public static Type[] GetTypeInheritanceChain(this Type type)
    {
        return _typeInheritanceChains.GetOrAdd(type, static inputType =>
        {
            var retVal = new List<Type> { inputType };

            var baseType = inputType.BaseType;
            while (baseType is not null && baseType != typeof(Entity) && baseType != typeof(object))
            {
                retVal.Add(baseType);
                baseType = baseType.BaseType;
            }

            return retVal.ToArray();
        });
    }

    public static Type[] GetTypeInheritanceChainTo(this Type type, Type toBaseType)
    {
        return _typeInheritanceChainsTo.GetOrAdd((type, toBaseType), static key =>
        {
            var retVal = new List<Type> { key.Type };

            var baseType = key.Type.BaseType;
            while (baseType is not null && baseType != key.ToBaseType && baseType != typeof(object))
            {
                retVal.Add(baseType);
                baseType = baseType.BaseType;
            }

            return retVal.ToArray();
        });
    }

    public static bool IsDerivativeOf(this Type type, Type typeToCompare)
    {
        ArgumentNullException.ThrowIfNull(type);

        return typeToCompare is not null && type.IsSubclassOf(typeToCompare);
    }

    public static bool IsDictionary(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return _isDictionaryCache.GetOrAdd(type, static inputType =>
        {
            var retVal = typeof(IDictionary).IsAssignableFrom(inputType);
            if (!retVal && inputType is not null)
            {
                retVal = inputType.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(inputType.GetGenericTypeDefinition());
            }

            return retVal;
        });
    }

    public static bool IsAssignableFromGenericList(this Type type)
    {
        // Cached per type: GetInterfaces() allocates a Type[] on every call, and this runs on the
        // ValueObject equality/hash hot path (once per list-typed component).
        return _assignableFromGenericListCache.GetOrAdd(type, static inputType =>
        {
            foreach (var intType in inputType.GetInterfaces())
            {
                if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    return true;
                }
            }

            return false;
        });
    }

    public static T[] GetFlatObjectsListWithInterface<T>(this object obj, List<T> resultList = null)
    {
        resultList ??= [];
        var allObjects = new HashSet<object>(_objectReferenceComparer);
        GetFlatObjectsListWithInterface(obj, resultList, allObjects);

        return resultList.ToArray();
    }

    private static void GetFlatObjectsListWithInterface<T>(object obj, List<T> resultList, HashSet<object> allObjects)
    {
        // Prevent loops
        if (!allObjects.Add(obj))
        {
            return;
        }

        if (obj is T t)
        {
            resultList.Add(t);
        }

        foreach (var entry in GetFlattenPlan(obj.GetType(), typeof(T)))
        {
            var value = entry.Property.GetValue(obj);

            if (entry.IsSingle && value is not null)
            {
                // Handle single objects
                GetFlatObjectsListWithInterface(value, resultList, allObjects);
            }
            else if (value is not string && value is IEnumerable enumerable)
            {
                // Handle collections and arrays
                FlattenEnumerable(enumerable, resultList, allObjects);
            }
        }
    }

    private static void FlattenEnumerable<T>(IEnumerable enumerable, List<T> resultList, HashSet<object> allObjects)
    {
        foreach (var item in enumerable)
        {
            if (item is T)
            {
                GetFlatObjectsListWithInterface(item, resultList, allObjects);
            }
        }
    }

    // Builds the visit plan once per source-runtime-type and target-type pair, in declared property order
    // so the flattened result matches the original implementation. Indexer properties are skipped. A
    // property counts as a single object when its declared type is assignable to the target type.
    // Otherwise it counts as a collection candidate when its runtime value is a non-string IEnumerable,
    // meaning any reference type other than string, or a value type that implements IEnumerable.
    private static FlattenEntry[] GetFlattenPlan(Type sourceType, Type targetType)
    {
        return _flattenPlans.GetOrAdd((sourceType, targetType), static key => BuildFlattenPlan(key.Source, key.Target));
    }

    private static FlattenEntry[] BuildFlattenPlan(Type sourceType, Type targetType)
    {
        var entries = new List<FlattenEntry>();

        foreach (var property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            var propertyType = property.PropertyType;
            if (targetType.IsAssignableFrom(propertyType))
            {
                entries.Add(new FlattenEntry(property, IsSingle: true));
            }
            else if (propertyType != typeof(string) && (!propertyType.IsValueType || typeof(IEnumerable).IsAssignableFrom(propertyType)))
            {
                entries.Add(new FlattenEntry(property, IsSingle: false));
            }
        }

        return entries.ToArray();
    }

    private sealed record FlattenEntry(PropertyInfo Property, bool IsSingle);


    private class ObjectReferenceComparer : IEqualityComparer<object>
    {
        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            return obj.GetHashCode();
        }
    }
}
