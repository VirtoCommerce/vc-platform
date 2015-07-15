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
                    .Where(v => v.Property != null && v.Property.Name == name && v.Values != null)
                    .SelectMany(v => v.Values)
                    .FirstOrDefault();
            }

            return result;
        }
    }
}
