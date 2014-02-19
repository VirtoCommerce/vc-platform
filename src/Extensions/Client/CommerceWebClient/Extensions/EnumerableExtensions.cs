using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions
{
	/// <summary>
	/// Class EnumerableExtensions.
	/// </summary>
    public static class EnumerableExtensions
    {
		/// <summary>
		/// To the dictionary.
		/// </summary>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns>IDictionary{``0``1}.</returns>
        public static IDictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> list)
        {
            return list.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static IEnumerable<ItemPropertyValue> LocalizedProperties(this IEnumerable<ItemPropertyValue> props)
        {
            return props.GroupBy(x => x.Name).Select(propGroup =>
                propGroup.FirstOrDefault(x => !string.IsNullOrEmpty(x.Locale) //Try to find property with current locale
                    && string.Equals(x.Locale, StoreHelper.CustomerSession.Language, StringComparison.InvariantCultureIgnoreCase))
                ?? (propGroup.FirstOrDefault(x => !string.IsNullOrEmpty(x.Locale) //Try to find property with defaut locale
                    && string.Equals(x.Locale, StoreHelper.GetDefaultLanguageCode(), StringComparison.InvariantCultureIgnoreCase))
                ?? (propGroup.FirstOrDefault(x => string.IsNullOrEmpty(x.Locale)) //Try to find property with no locale
                ?? propGroup.First()))).ToList();//Finally return any other property
        }
    }
}