namespace VirtoCommerce.Platform.Core.Settings
{
    public class SettingEntry
    {
		/// <summary>
		/// Setting may belong to any object in system
		/// </summary>
		public string ObjectId { get; set; }
		public string ObjectType { get; set; }

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
