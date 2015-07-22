using System.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyObjectValue
    {
        public string Locale { get; set; }
        public object Value { get; set; }

        public DynamicPropertyObjectValue Clone()
        {
            return new DynamicPropertyObjectValue
            {
                Locale = Locale,
                Value = Value
            };
        }
    }
}
