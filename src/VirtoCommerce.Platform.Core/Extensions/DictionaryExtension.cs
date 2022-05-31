using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class DictionaryExtension
    {
        public static TValue GetValueOrThrow<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, string exceptionMessage)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                throw new KeyNotFoundException(exceptionMessage);
            }
            return value;
        }

        /// <summary>
        /// Doesn't throw an exception when the key is null or does not exist
        /// </summary>
        public static TValue GetValueSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValueSafe(key, default);
        }

        /// <summary>
        /// Doesn't throw an exception when the key is null or does not exist
        /// </summary>
        public static TValue GetValueSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (key != null && dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Doesn't throw an exception when the key is null
        /// </summary>
        public static bool TryGetValueSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
        {
            if (key != null)
            {
                return dictionary.TryGetValue(key, out value);
            }

            value = default;

            return false;
        }
    }
}
