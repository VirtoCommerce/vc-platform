using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Exceptions;

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
        /// Check object clone immutability and independency:
        /// Ensures the data in object clone equial to original.
        /// Ensures no shared references between original and cloned objects (each object is a fully independent).
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static async Task AssertCloneIndependency(this ICloneable original)
        {
            await Task.Run(() =>
            {
                var clone = original.Clone();
                var sOriginal = JsonSerializer.Serialize((object)original, new JsonSerializerOptions() { WriteIndented = true });
                var sClone = JsonSerializer.Serialize(clone, new JsonSerializerOptions() { WriteIndented = true });
                if (!sOriginal.Equals(sClone)) // Ensure data in objects is equal
                {
                    throw new PlatformException(@$"Clone check failed: object and clone not equal.");
                }
                original.AssertNoSharedRefsWith(clone); // Ensure no shared references between objects (each object is a fully independent)
            });
        }

        /// <summary>
        /// Ensures no shared references with expected object (original and expected objects are fully independent from each other).
        /// </summary>
        /// <param name="original"></param>
        /// <param name="expected"></param>
        public static void AssertNoSharedRefsWith(this object original, object expected)
        {
            AssertNoSharedRefsWith(original, expected, new List<object>(), original.GetType().Name);
        }

        private static void AssertNoSharedRefsWith(object original, object expected, List<object> visited, string memberPath)
        {
            if (original != null && expected != null)
            {
                var typeOfOriginal = original.GetType();
                if (!IsPrimitive(typeOfOriginal) && !visited.Contains(original))
                {
                    visited.Add(original);
                    if (ReferenceEquals(original, expected))
                    {
                        throw new MemberAccessException(@$"Deep clone check failed: objects at path {memberPath} are reference equal.");
                    }
                    if (original is IEnumerable)
                    {
                        var originalEnumerator = ((IEnumerable)original).GetEnumerator();
                        var expectedEnumerator = ((IEnumerable)expected).GetEnumerator();
                        var iIdx = 0;
                        while (originalEnumerator.MoveNext())
                        {
                            expectedEnumerator.MoveNext();
                            AssertNoSharedRefsWith(originalEnumerator.Current, expectedEnumerator.Current, visited, $@"{memberPath}[{iIdx++}]");
                        }
                    }
                    else
                    {
                        foreach (var propInfo in typeOfOriginal.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            AssertNoSharedRefsWith(propInfo.GetValue(original), propInfo.GetValue(expected), visited, $@"{memberPath}.{propInfo.Name}");
                        }
                    }
                }
            }
            else if ((original == null && expected != null) || (original != null && expected == null))
            {
                throw new MemberAccessException(@$"Deep clone check failed: one of objects at path {memberPath} is null.");
            }
        }

        /// <summary>
        /// Check if type is a value-type or string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string)) return true;
            return (type.IsValueType & type.IsPrimitive);
        }
    }
}
