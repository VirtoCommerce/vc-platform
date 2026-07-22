using System;
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
