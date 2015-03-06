using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class PropertyValue
    {
        public string Id { get; set; }
		public string PropertyName { get; set; }
		public string LanguageCode { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public PropertyValueType ValueType { get; set; }
		public string ValueId { get; set; }
        public object Value { get; set; }
    }
}