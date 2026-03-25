namespace VirtoCommerce.Platform.Core.Settings
{
    /// <summary>
    /// Schema-only DTO for the Settings V2 API.
    /// Contains metadata about a setting without its current value.
    /// </summary>
    public class SettingPropertySchema
    {
        /// <summary>
        /// Unique setting identifier (dot-notation), e.g. "Catalog.ImageCategories"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Human-readable display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Pipe-delimited group path for tree building, e.g. "Catalog|General"
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Owning module ID
        /// </summary>
        public string ModuleId { get; set; }

        public SettingValueType ValueType { get; set; }

        public object DefaultValue { get; set; }

        public object[] AllowedValues { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// True when value is forced by configuration override and cannot be changed via UI
        /// </summary>
        public bool IsReadOnly { get; set; }

        public bool IsDictionary { get; set; }

        public bool IsLocalizable { get; set; }

        /// <summary>
        /// Application restart is needed after changing this setting
        /// </summary>
        public bool RestartRequired { get; set; }
    }
}
