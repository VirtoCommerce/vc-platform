using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class EnumerableExtension
    {
        #region Public Methods and Operators

        /// <summary>
        ///     To the dictionary.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns>IDictionary{``0``1}.</returns>
        public static IDictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> list)
        {
            return list.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        #endregion
    }
}
