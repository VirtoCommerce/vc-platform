using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings
{
    public class ObjectSettingEntry : SettingDescriptor
    {
        public ObjectSettingEntry()
        {
        }
        public ObjectSettingEntry(SettingDescriptor descriptor)
        {
            RestartRequired = descriptor.RestartRequired;
            ModuleId = descriptor.ModuleId;
            GroupName = descriptor.GroupName;
            Name = descriptor.Name;
            DisplayName = descriptor.DisplayName;
            IsRequired = descriptor.IsRequired;
            ValueType = descriptor.ValueType;
            AllowedValues = descriptor.AllowedValues;
            DefaultValue = descriptor.DefaultValue;
            IsDictionary = descriptor.IsDictionary;
            IsHidden = descriptor.IsHidden;
        }
        public bool ItHasValues => Value != null || !AllowedValues.IsNullOrEmpty();
        /// <summary>
        /// Setting may belong to any object in system
        /// </summary>
        public string ObjectId { get; set; }
        public string ObjectType { get; set; }
        /// <summary>
        /// Flag indicates the this setting is read only and can't be changed
        /// </summary>
        public bool IsReadOnly { get; set; }

        public object Value { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return ObjectId;
            yield return ObjectType;
        }
    }
}
