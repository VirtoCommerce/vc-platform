using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Domain.Catalog.Model
{
	public class PropertyValue : AuditableEntity, ILanguageSupport
	{
		public string PropertyId { get; set; }
		public string PropertyName { get; set; }
		public string Alias { get; set; }
		public string ValueId { get; set; }
		public object Value { get; set; }
		public PropertyValueType ValueType { get; set; }
		public string LanguageCode { get; set; }

		public override string ToString()
		{
			return PropertyId ?? "unknow" + ":" + ValueId ?? "undef";
		}
	}
}