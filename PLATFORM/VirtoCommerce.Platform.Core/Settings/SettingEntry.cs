using System;

namespace VirtoCommerce.Platform.Core.Settings
{
    public class SettingEntry : ICloneable
    {
        public string ModuleId { get; set; }
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

        public object Clone()
        {
            var retVal = new SettingEntry()
            {
                ModuleId = ModuleId,
                Name = Name,
                ObjectId = ObjectId,
                ObjectType = ObjectType,
                GroupName = GroupName,
                Value = Value,
                ValueType = ValueType,
                AllowedValues = AllowedValues,
                DefaultValue = DefaultValue,
                IsArray = IsArray,
                ArrayValues = ArrayValues,
                Title = Title,
                Description = Description
            };
            return retVal;
        }
    }
}
