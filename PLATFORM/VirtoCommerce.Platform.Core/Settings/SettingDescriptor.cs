namespace VirtoCommerce.Platform.Core.Settings
{
    public class SettingDescriptor
    {
        public string GroupName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public SettingValueType ValueType { get; set; }
        public string[] AllowedValues { get; set; }
        public string DefaultValue { get; set; }
        public bool IsArray { get; set; }
        public string[] ArrayValues { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
