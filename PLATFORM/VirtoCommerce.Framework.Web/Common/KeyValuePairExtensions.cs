using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Framework.Web.Common
{
    public static class KeyValuePairExtensions
    {
        /// <summary>
        /// Withes the key.
        /// </summary>
        /// <typeparam name="TK">The type of the tk.</typeparam>
        /// <typeparam name="TV">The type of the tv.</typeparam>
        /// <param name="kv">The kv.</param>
        /// <param name="newKey">The new key.</param>
        /// <returns>KeyValuePair{``0``1}.</returns>
        public static KeyValuePair<TK, TV> WithKey<TK, TV>(this KeyValuePair<TK, TV> kv, TK newKey)
        {
            return new KeyValuePair<TK, TV>(newKey, kv.Value);
        }

        /// <summary>
        /// Withes the value.
        /// </summary>
        /// <typeparam name="TK">The type of the tk.</typeparam>
        /// <typeparam name="TV">The type of the tv.</typeparam>
        /// <param name="kv">The kv.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>KeyValuePair{``0``1}.</returns>
        public static KeyValuePair<TK, TV> WithValue<TK, TV>(this KeyValuePair<TK, TV> kv, TV newValue)
        {
            return new KeyValuePair<TK, TV>(kv.Key, newValue);
        }
    }
}
