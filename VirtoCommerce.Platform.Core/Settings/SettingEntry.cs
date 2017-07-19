using System;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core.Settings
{
    /// <summary>
    /// Represent setting record
    /// </summary>
    public class SettingEntry : ICloneable
    {
        public string ModuleId { get; set; }
        /// <summary>
        /// Setting may belong to any object in system
        /// </summary>
        public string ObjectId { get; set; }
        public string ObjectType { get; set; }
        /// <summary>
        /// Setting group name
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Setting name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Setting string value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Setting raw value
        /// </summary>
        public object RawValue { get; set; }
        public SettingValueType ValueType { get; set; }
        public string[] AllowedValues { get; set; }
        public string DefaultValue { get; set; }
        public object RawDefaultValue { get; set; }
        public bool IsArray { get; set; }
        public string[] ArrayValues { get; set; }
        public object[] RawArrayValues { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Flag for runtime registered settings 
        /// </summary>
        public bool IsRuntime { get; set; }

        public object Clone()
        {
            return MemberwiseClone() as SettingEntry;
        }
    }
}
