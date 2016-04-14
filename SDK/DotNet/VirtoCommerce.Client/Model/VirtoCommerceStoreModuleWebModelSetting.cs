using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceStoreModuleWebModelSetting :  IEquatable<VirtoCommerceStoreModuleWebModelSetting>
    {
        /// <summary>
        /// Gets or Sets GroupName
        /// </summary>
        [DataMember(Name="groupName", EmitDefaultValue=false)]
        public string GroupName { get; set; }

        /// <summary>
        /// System name (ID) of the setting
        /// </summary>
        /// <value>System name (ID) of the setting</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Current value for non-array setting
        /// </summary>
        /// <value>Current value for non-array setting</value>
        [DataMember(Name="value", EmitDefaultValue=false)]
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets ValueType
        /// </summary>
        [DataMember(Name="valueType", EmitDefaultValue=false)]
        public string ValueType { get; set; }

        /// <summary>
        /// Predefined set of allowed values for this setting
        /// </summary>
        /// <value>Predefined set of allowed values for this setting</value>
        [DataMember(Name="allowedValues", EmitDefaultValue=false)]
        public List<string> AllowedValues { get; set; }

        /// <summary>
        /// Gets or Sets DefaultValue
        /// </summary>
        [DataMember(Name="defaultValue", EmitDefaultValue=false)]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Defines whether the setting can have multiple values
        /// </summary>
        /// <value>Defines whether the setting can have multiple values</value>
        [DataMember(Name="isArray", EmitDefaultValue=false)]
        public bool? IsArray { get; set; }

        /// <summary>
        /// Current values for array setting
        /// </summary>
        /// <value>Current values for array setting</value>
        [DataMember(Name="arrayValues", EmitDefaultValue=false)]
        public List<string> ArrayValues { get; set; }

        /// <summary>
        /// User-friendly name of the setting
        /// </summary>
        /// <value>User-friendly name of the setting</value>
        [DataMember(Name="title", EmitDefaultValue=false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceStoreModuleWebModelSetting {\n");
            sb.Append("  GroupName: ").Append(GroupName).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  ValueType: ").Append(ValueType).Append("\n");
            sb.Append("  AllowedValues: ").Append(AllowedValues).Append("\n");
            sb.Append("  DefaultValue: ").Append(DefaultValue).Append("\n");
            sb.Append("  IsArray: ").Append(IsArray).Append("\n");
            sb.Append("  ArrayValues: ").Append(ArrayValues).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommerceStoreModuleWebModelSetting);
        }

        /// <summary>
        /// Returns true if VirtoCommerceStoreModuleWebModelSetting instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceStoreModuleWebModelSetting to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceStoreModuleWebModelSetting other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.GroupName == other.GroupName ||
                    this.GroupName != null &&
                    this.GroupName.Equals(other.GroupName)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Value == other.Value ||
                    this.Value != null &&
                    this.Value.Equals(other.Value)
                ) && 
                (
                    this.ValueType == other.ValueType ||
                    this.ValueType != null &&
                    this.ValueType.Equals(other.ValueType)
                ) && 
                (
                    this.AllowedValues == other.AllowedValues ||
                    this.AllowedValues != null &&
                    this.AllowedValues.SequenceEqual(other.AllowedValues)
                ) && 
                (
                    this.DefaultValue == other.DefaultValue ||
                    this.DefaultValue != null &&
                    this.DefaultValue.Equals(other.DefaultValue)
                ) && 
                (
                    this.IsArray == other.IsArray ||
                    this.IsArray != null &&
                    this.IsArray.Equals(other.IsArray)
                ) && 
                (
                    this.ArrayValues == other.ArrayValues ||
                    this.ArrayValues != null &&
                    this.ArrayValues.SequenceEqual(other.ArrayValues)
                ) && 
                (
                    this.Title == other.Title ||
                    this.Title != null &&
                    this.Title.Equals(other.Title)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)

                if (this.GroupName != null)
                    hash = hash * 59 + this.GroupName.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Value != null)
                    hash = hash * 59 + this.Value.GetHashCode();

                if (this.ValueType != null)
                    hash = hash * 59 + this.ValueType.GetHashCode();

                if (this.AllowedValues != null)
                    hash = hash * 59 + this.AllowedValues.GetHashCode();

                if (this.DefaultValue != null)
                    hash = hash * 59 + this.DefaultValue.GetHashCode();

                if (this.IsArray != null)
                    hash = hash * 59 + this.IsArray.GetHashCode();

                if (this.ArrayValues != null)
                    hash = hash * 59 + this.ArrayValues.GetHashCode();

                if (this.Title != null)
                    hash = hash * 59 + this.Title.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                return hash;
            }
        }

    }
}
