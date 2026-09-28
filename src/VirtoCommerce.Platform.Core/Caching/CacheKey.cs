using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class CacheKey
    {
        // Only type-owned prefixes are registered, never individual keys or request data.
        private static readonly ConcurrentDictionary<string, string> _cacheNames = new(StringComparer.OrdinalIgnoreCase);

        public static string With(params string[] keys)
        {
            return string.Join("-", keys);
        }

        public static string With(params ReadOnlySpan<string> keys)
        {
            return string.Join("-", keys);
        }

        public static string With(Type ownerType, params string[] keys)
        {
            return $"{GetOwnerPrefix(ownerType)}:{string.Join("-", keys)}";
        }

        public static string With(Type ownerType, params ReadOnlySpan<string> keys)
        {
            return $"{GetOwnerPrefix(ownerType)}:{string.Join("-", keys)}";
        }

        /// <summary>
        /// Associates an owner's cache keys with a logical model type for telemetry.
        /// Does not change cache keys, lookup behavior, or invalidation.
        /// </summary>
        public static void RegisterCacheName(Type ownerType, Type modelType)
        {
            _cacheNames[ownerType.GetCacheKey()] = modelType.GetCacheKey();
        }

        /// <summary>
        /// Returns the registered logical group for a type-owned key, or null for an unclassified key.
        /// Arbitrary string prefixes and the variable part of a key are never used as metric names.
        /// </summary>
        public static string GetCacheName(object key)
        {
            if (key is string stringKey)
            {
                var separator = stringKey.IndexOf(':');
                if (separator > 0 && _cacheNames.TryGetValue(stringKey[..separator], out var name))
                {
                    return name;
                }
            }

            return null;
        }

        private static string GetOwnerPrefix(Type ownerType)
        {
            var prefix = ownerType.GetCacheKey();
            _cacheNames.TryAdd(prefix, prefix);
            return prefix;
        }

        public static object Normalize(object key)
        {
            return key is string stringKey
                ? Normalize(stringKey)
                : key;
        }

        [SuppressMessage("Major Code Smell", "S3267:Loops should be simplified using the \"Where\" LINQ method",
            Justification = "Perf-critical cache-key normalization: a LINQ Any/Where over EnumerateRunes() would box the StringRuneEnumerator and allocate a delegate on every call — the explicit alloc-free scan is the whole point of this hot path.")]
        public static string Normalize(string key)
        {
            foreach (var rune in key.EnumerateRunes())
            {
                if (Rune.ToLowerInvariant(rune) != rune)
                {
                    return key.ToLowerInvariant();
                }
            }

            return key;
        }
    }
}
