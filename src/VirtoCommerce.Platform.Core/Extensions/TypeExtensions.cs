using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VirtoCommerce.Platform.Core.Extensions
{
    public static class TypeExtensions
    {
        public static Type[] GetTypeInheritanceChainTo(this Type type, Type toBaseType)
        {
            var retVal = new List<Type> { type };

            var baseType = type.BaseType;

            while (baseType != toBaseType && baseType != typeof(object))
            {
                retVal.Add(baseType);
                baseType = baseType.BaseType;
            }

            return retVal.ToArray();
        }

        private static readonly ConcurrentDictionary<Type, string> PrettyPrintCache = new ConcurrentDictionary<Type, string>();

        public static string PrettyPrint(this Type type)
        {
            return PrettyPrintCache.GetOrAdd(
                type,
                t =>
                {
                    try
                    {
                        return PrettyPrintRecursive(t, 0);
                    }
                    catch (Exception)
                    {
                        return t.Name;
                    }
                });
        }

        private static readonly ConcurrentDictionary<Type, string> TypeCacheKeys = new ConcurrentDictionary<Type, string>();
        public static string GetCacheKey(this Type type)
        {
            return TypeCacheKeys.GetOrAdd(type, t => $"{t.PrettyPrint()}");
        }

        private static string PrettyPrintRecursive(Type type, int depth)
        {
            if (depth > 3)
            {
                return type.Name;
            }

            var nameParts = type.Name.Split('`');
            if (nameParts.Length == 1)
            {
                return nameParts[0];
            }

            var genericArguments = type.GetTypeInfo().GetGenericArguments();
            return !type.IsConstructedGenericType
                ? $"{nameParts[0]}<{new string(',', genericArguments.Length - 1)}>"
                : $"{nameParts[0]}<{string.Join(",", genericArguments.Select(t => PrettyPrintRecursive(t, depth + 1)))}>";
        }


        /// <summary>
        /// Iterates recursively over each public property of object to gather member values.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> TraverseObjectGraph(this object original)
        {
            foreach (var result in original.TraverseObjectGraphRecursively(new List<object>(), original.GetType().Name))
            {
                yield return result;
            }
        }

        private static IEnumerable<KeyValuePair<string, object>> TraverseObjectGraphRecursively(this object obj, List<object> visited, string memberPath)
        {
            yield return new KeyValuePair<string, object>(memberPath, obj);
            if (obj != null)
            {
                var typeOfOriginal = obj.GetType();
                if (!IsPrimitive(typeOfOriginal) && !visited.Any(x => ReferenceEquals(obj, x))) // ReferenceEquals is a mandatory approach
                {
                    visited.Add(obj);
                    if (obj is IEnumerable objEnum)
                    {
                        var originalEnumerator = objEnum.GetEnumerator();
                        var iIdx = 0;
                        while (originalEnumerator.MoveNext())
                        {
                            foreach (var result in originalEnumerator.Current.TraverseObjectGraphRecursively(visited, $@"{memberPath}[{iIdx++}]"))
                            {
                                yield return result;
                            }
                        }
                    }
                    else
                    {
                        foreach (var propInfo in typeOfOriginal.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            foreach (var result in propInfo.GetValue(obj).TraverseObjectGraphRecursively(visited, $@"{memberPath}.{propInfo.Name}"))
                            {
                                yield return result;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if type is a value-type, primitive type or string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string)) return true;
            return (type.IsValueType || type.IsPrimitive);
        }

        /// <summary>
        /// Check if type is a value-type, primitive type  or string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPrimitive(this object obj)
        {
            return obj == null || obj.GetType().IsPrimitive();
        }

        public static Type UnwrapNullableType(this Type type) => Nullable.GetUnderlyingType(type) ?? type;

        public static bool IsNullableType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return !typeInfo.IsValueType
                   || typeInfo.IsGenericType
                   && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        public static Type UnwrapEnumType(this Type type)
        {
            var isNullable = type.IsNullableType();
            var underlyingNonNullableType = isNullable ? type.UnwrapNullableType() : type;
            if (!underlyingNonNullableType.GetTypeInfo().IsEnum)
            {
                return type;
            }

            var underlyingEnumType = Enum.GetUnderlyingType(underlyingNonNullableType);
            return isNullable ? MakeNullable(underlyingEnumType) : underlyingEnumType;
        }

        public static Type MakeNullable(this Type type, bool nullable = true)
          => type.IsNullableType() == nullable
              ? type
              : nullable
                  ? typeof(Nullable<>).MakeGenericType(type)
                  : type.UnwrapNullableType();
    }
}
