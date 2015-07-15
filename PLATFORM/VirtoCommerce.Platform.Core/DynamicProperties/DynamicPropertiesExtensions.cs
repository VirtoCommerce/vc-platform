using System.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public static class DynamicPropertiesExtensions
    {
        public static T GetDynamicPropertyValue<T>(this IHasDynamicProperties owner, string name, T defaultValue)
        {
            var result = defaultValue;

            if (owner != null && owner.DynamicPropertyValues != null)
            {
                var value = owner.DynamicPropertyValues
                    .Where(v => v.Property != null && v.Property.Name == name && v.Values != null)
                    .SelectMany(v => v.Values)
                    .FirstOrDefault();

                var dictionaryItem = value as DynamicPropertyDictionaryItem;
                if (dictionaryItem != null)
                    value = dictionaryItem.Name;

                if (value != null)
                    result = (T)value;
            }

            return result;
        }
    }
}
