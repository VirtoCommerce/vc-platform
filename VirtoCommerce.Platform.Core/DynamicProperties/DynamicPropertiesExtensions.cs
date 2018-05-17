using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public static class DynamicPropertiesExtensions
    {
        public static T GetDynamicPropertyValue<T>(this IHasDynamicProperties owner, string propertyName, T defaultValue)
        {
            var result = defaultValue;

            var propValue = owner?.DynamicProperties?.Where(p => p.Name == propertyName && p.Values != null)
                .SelectMany(p => p.Values)
                .FirstOrDefault();

            if (propValue?.Value != null)
            {
                var jObject = propValue.Value as JObject;
                var dictItem = propValue.Value as DynamicPropertyDictionaryItem;

                if (jObject != null)
                {
                    dictItem = jObject.ToObject<DynamicPropertyDictionaryItem>();
                }

                var value = dictItem != null ? dictItem.Name : propValue.Value;
                result = (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }

            return result;
        }

        /// <summary>
        /// Copies property values from one object to another using property name and type for comparison
        /// </summary>
        /// <param name="sourceOwner"></param>
        /// <param name="targetOwner"></param>
        public static void CopyPropertyValuesFrom(this IHasDynamicProperties targetOwner, IHasDynamicProperties sourceOwner)
        {
            if (sourceOwner == null)
            {
                throw new ArgumentNullException(nameof(sourceOwner));
            }
            if (targetOwner == null)
            {
                throw new ArgumentNullException(nameof(targetOwner));
            }

            var comparer = AnonymousComparer.Create((DynamicProperty x) => x.Name.ToLowerInvariant() + ":" + x.ValueType);
            // Copy property values for same properties from one object to other 
            sourceOwner.DynamicProperties.CompareTo(targetOwner.DynamicProperties, comparer, (state, sourceProp, targetProp) =>
            {
                if (state == EntryState.Modified)
                {
                    targetProp.Values = sourceProp.Values;
                }
            });
        }
    }
}
