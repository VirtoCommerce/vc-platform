using System.Linq;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public static class DynamicPropertiesExtensions
    {
        public static T GetDynamicPropertyValue<T>(this IHasDynamicProperties owner, string propertyName, T defaultValue)
        {
            var result = defaultValue;

            if (owner != null && owner.DynamicProperties != null)
            {
                var propValue = owner.DynamicProperties.Where(v => v.Name == propertyName && v.Values != null)
                                                       .SelectMany(v => v.Values)
                                                       .FirstOrDefault();

                if (propValue != null && propValue.Value != null)
                {
                    var jObject = propValue.Value as JObject;
                    var dictItem = propValue.Value as DynamicPropertyDictionaryItem;
                    if (jObject != null)
                    {
                        dictItem = jObject.ToObject<DynamicPropertyDictionaryItem>();
                    }
                    if (dictItem != null)
                    {
                        result = (T)(object)dictItem.Name;
                    }
                    else
                    {
                        result = (T)propValue.Value;
                    }
                }
            }

            return result;
        }

        public static void SetObjectId(this IHasDynamicProperties owner, string id)
        {
            if (owner != null && owner.DynamicProperties != null)
            {
                owner.Id = id;

                foreach (var dynamicObjectProperty in owner.DynamicProperties)
                {
                    dynamicObjectProperty.ObjectId = id;
                }
            }
        }

        /// <summary>
        /// Apply property values for those who have the same name and ValueType
        /// </summary>
        public static void ApplyDynamicPropertiesValues(this IHasDynamicProperties owner, IHasDynamicProperties source)
        {
            if (source.DynamicProperties != null && owner.DynamicProperties != null)
            {
                foreach (var sourceProperty in source.DynamicProperties)
                {
                    var ownerProperty = owner.DynamicProperties
                        .FirstOrDefault(x => x.Name.Equals(sourceProperty.Name) && x.ValueType == sourceProperty.ValueType);
                    if (ownerProperty != null && sourceProperty.Values != null)
                    {
                        ownerProperty.Values = sourceProperty.Values.Select(x => x.Clone()).ToArray();
                    }
                }
            }
        }
    }
}
