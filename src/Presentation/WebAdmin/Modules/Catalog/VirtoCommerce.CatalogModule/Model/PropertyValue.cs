namespace VirtoCommerce.CatalogModule.Model
{
	public class PropertyValue : ILanguageSupport
	{
		public string Id { get; set; }
		public string PropertyId { get; set; }
		public string PropertyName { get; set; }
		public string ValueId { get; set; }
		public object Value { get; set; }
		public PropertyValueType ValueType { get; set; }
		public string LanguageCode { get; set; }
	}
}