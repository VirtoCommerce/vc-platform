using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicProperty
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public bool IsArray { get; set; }
        public bool IsDictionary { get; set; }
        public bool IsMultilingual { get; set; }
        public bool IsRequired { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DynamicPropertyValueType ValueType { get; set; }

        public DynamicPropertyName[] DisplayNames { get; set; }
    }
}
