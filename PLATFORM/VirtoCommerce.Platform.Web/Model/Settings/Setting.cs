using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Model.Settings
{
    public class Setting
    {
        public string GroupName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
        public SettingValueType ValueType { get; set; }
        public string[] AllowedValues { get; set; }
        public string DefaultValue { get; set; }
        public bool IsArray { get; set; }
        public string[] ArrayValues { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
