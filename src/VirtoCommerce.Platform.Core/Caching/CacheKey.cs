using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class CacheKey
    {
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
            return $"{ownerType.GetCacheKey()}:{string.Join("-", keys)}";
        }

        public static string With(Type ownerType, params ReadOnlySpan<string> keys)
        {
            return $"{ownerType.GetCacheKey()}:{string.Join("-", keys)}";
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
