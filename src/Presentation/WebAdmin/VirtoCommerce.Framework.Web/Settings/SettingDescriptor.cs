using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.Framework.Web.Settings
{
	public class SettingDescriptor
	{
		public string GroupName { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public string ValueType { get; set; }
		public string[] AllowedValues { get; set; }
		public string DefaultValue { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public object RawValue()
		{
			return ModuleSetting.RawValue(ValueType, Value);
		}
	}
}
