namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class PropertyValue
    {
        public string Id { get; set; }
		public string PropertyName { get; set; }
		public string LanguageCode { get; set; }
		public PropertyValueType ValueType { get; set; }
		public string ValueId { get; set; }
        public object Value { get; set; }
    }
}