using System.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyObjectValue
    {
        public DynamicProperty Property { get; set; }
        public string ObjectId { get; set; }
        public string Locale { get; set; }
        public string[] Values { get; set; }

        public DynamicPropertyObjectValue Clone()
        {
            return new DynamicPropertyObjectValue
            {
                Property = Property == null ? null : Property.Clone(),
                ObjectId = ObjectId,
                Locale = Locale,
                Values = Values == null ? null : Values.ToArray(),
            };
        }
    }
}
