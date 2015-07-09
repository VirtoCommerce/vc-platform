using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Web.Model.DynamicProperties
{
    public class Property
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public bool IsArray { get; set; }
        public bool IsDictionary { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PropertyValueType ValueType { get; set; }

        public DisplayName[] DisplayNames { get; set; }
        public DictionaryItem[] DictionaryItems { get; set; }
        public ObjectValue[] ObjectValues { get; set; }
    }
}
