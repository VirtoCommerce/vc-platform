using System.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public static class DynamicPropertiesExtensions
    {
        public static string GetDynamicPropertyValue(this IHasDynamicProperties owner, string name)
        {
            string result = null;

            if (owner != null && owner.DynamicPropertyValues != null)
            {
                result = owner.DynamicPropertyValues
                    .Where(v => v.Property != null && v.Property.Name == "Tax exempt" && v.Values != null)
                    .SelectMany(v => v.Values)
                    .FirstOrDefault();
            }

            return result;
        }

        public static DynamicProperty Clone(this DynamicProperty source)
        {
            return new DynamicProperty
            {
                Id = source.Id,
                Name = source.Name,
                ObjectType = source.ObjectType,
                IsArray = source.IsArray,
                IsDictionary = source.IsDictionary,
                IsMultilingual = source.IsMultilingual,
                IsRequired = source.IsRequired,
                ValueType = source.ValueType,
                DisplayNames = source.DisplayNames == null ? null : source.DisplayNames.Select(n => n.Clone()).ToArray(),
            };
        }

        public static DynamicPropertyName Clone(this DynamicPropertyName source)
        {
            return new DynamicPropertyName
            {
                Locale = source.Locale,
                Name = source.Name,
            };
        }

        public static DynamicPropertyObjectValue Clone(this DynamicPropertyObjectValue source)
        {
            return new DynamicPropertyObjectValue
            {
                Property = source.Property == null ? null : source.Property.Clone(),
                ObjectId = source.ObjectId,
                Locale = source.Locale,
                Values = source.Values == null ? null : source.Values.ToArray(),
            };
        }
    }
}
