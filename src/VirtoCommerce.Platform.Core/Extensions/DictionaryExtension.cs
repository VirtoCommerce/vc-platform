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
    }
}
