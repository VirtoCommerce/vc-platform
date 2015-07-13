using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Web.Model.DynamicProperties
{
    public class Property
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public bool IsArray { get; set; }
        public bool IsDictionary { get; set; }
        public bool IsMultilingual { get; set; }
        public bool IsRequired { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PropertyValueType ValueType { get; set; }

        public PropertyName[] DisplayNames { get; set; }
    }
}
