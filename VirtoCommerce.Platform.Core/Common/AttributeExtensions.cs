using System;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class AttributeExtensions
    {
        private static readonly string[] _emptyArray = new string[0];

        public static string[] SplitString(this Attribute attribute, string original, char separator)
        {
            var result = _emptyArray;

            if (!string.IsNullOrEmpty(original))
            {
                result = original.Split(separator)
                    .Select(part => part.Trim())
                    .Where(part => !string.IsNullOrEmpty(part))
                    .ToArray();
            }

            return result;
        }

        /// <summary>
        /// Gets type attribute value https://stackoverflow.com/a/2656211/5907312
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="type">Type for getting attribute</param>
        /// <param name="valueSelector">Attribute value selector</param>
        /// <returns>Attribute value</returns>
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return (att != null) ? valueSelector(att) : default(TValue);
        }
    }
}
