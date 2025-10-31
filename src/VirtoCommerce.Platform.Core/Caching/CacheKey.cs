using System;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class CacheKey
    {
        public static string With(params string[] keys)
        {
            return string.Join("-", keys);
        }

        public static string With(Type ownerType, params string[] keys)
        {
            return Normalize($"{ownerType.GetCacheKey()}:{string.Join("-", keys)}");
        }

        public static object Normalize(object key)
        {
            return key is string stringKey
                ? Normalize(stringKey)
                : key;
        }

        public static string Normalize(string key)
        {
            return key.ToLowerInvariant();
        }
    }
}
