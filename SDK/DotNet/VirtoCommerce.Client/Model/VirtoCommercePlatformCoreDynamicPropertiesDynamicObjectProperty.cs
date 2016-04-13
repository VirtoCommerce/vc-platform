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
    public partial class VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty :  IEquatable<VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty>
    {
        /// <summary>
        /// Gets or Sets ObjectId
        /// </summary>
        [DataMember(Name="objectId", EmitDefaultValue=false)]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or Sets Values
        /// </summary>
        [DataMember(Name="values", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue> Values { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets ObjectType
        /// </summary>
        [DataMember(Name="objectType", EmitDefaultValue=false)]
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or Sets IsArray
        /// </summary>
        [DataMember(Name="isArray", EmitDefaultValue=false)]
        public bool? IsArray { get; set; }

        /// <summary>
        /// Gets or Sets IsDictionary
        /// </summary>
        [DataMember(Name="isDictionary", EmitDefaultValue=false)]
        public bool? IsDictionary { get; set; }

        /// <summary>
        /// Gets or Sets IsMultilingual
        /// </summary>
        [DataMember(Name="isMultilingual", EmitDefaultValue=false)]
        public bool? IsMultilingual { get; set; }

        /// <summary>
        /// Gets or Sets IsRequired
        /// </summary>
        [DataMember(Name="isRequired", EmitDefaultValue=false)]
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Gets or Sets ValueType
        /// </summary>
        [DataMember(Name="valueType", EmitDefaultValue=false)]
        public string ValueType { get; set; }

        /// <summary>
        /// Gets or Sets DisplayNames
        /// </summary>
        [DataMember(Name="displayNames", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyName> DisplayNames { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty {\n");
            sb.Append("  ObjectId: ").Append(ObjectId).Append("\n");
            sb.Append("  Values: ").Append(Values).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  ObjectType: ").Append(ObjectType).Append("\n");
            sb.Append("  IsArray: ").Append(IsArray).Append("\n");
            sb.Append("  IsDictionary: ").Append(IsDictionary).Append("\n");
            sb.Append("  IsMultilingual: ").Append(IsMultilingual).Append("\n");
            sb.Append("  IsRequired: ").Append(IsRequired).Append("\n");
            sb.Append("  ValueType: ").Append(ValueType).Append("\n");
            sb.Append("  DisplayNames: ").Append(DisplayNames).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ObjectId == other.ObjectId ||
                    this.ObjectId != null &&
                    this.ObjectId.Equals(other.ObjectId)
                ) && 
                (
                    this.Values == other.Values ||
                    this.Values != null &&
                    this.Values.SequenceEqual(other.Values)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.ObjectType == other.ObjectType ||
                    this.ObjectType != null &&
                    this.ObjectType.Equals(other.ObjectType)
                ) && 
                (
                    this.IsArray == other.IsArray ||
                    this.IsArray != null &&
                    this.IsArray.Equals(other.IsArray)
                ) && 
                (
                    this.IsDictionary == other.IsDictionary ||
                    this.IsDictionary != null &&
                    this.IsDictionary.Equals(other.IsDictionary)
                ) && 
                (
                    this.IsMultilingual == other.IsMultilingual ||
                    this.IsMultilingual != null &&
                    this.IsMultilingual.Equals(other.IsMultilingual)
                ) && 
                (
                    this.IsRequired == other.IsRequired ||
                    this.IsRequired != null &&
                    this.IsRequired.Equals(other.IsRequired)
                ) && 
                (
                    this.ValueType == other.ValueType ||
                    this.ValueType != null &&
                    this.ValueType.Equals(other.ValueType)
                ) && 
                (
                    this.DisplayNames == other.DisplayNames ||
                    this.DisplayNames != null &&
                    this.DisplayNames.SequenceEqual(other.DisplayNames)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
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

                if (this.ObjectId != null)
                    hash = hash * 59 + this.ObjectId.GetHashCode();

                if (this.Values != null)
                    hash = hash * 59 + this.Values.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.ObjectType != null)
                    hash = hash * 59 + this.ObjectType.GetHashCode();

                if (this.IsArray != null)
                    hash = hash * 59 + this.IsArray.GetHashCode();

                if (this.IsDictionary != null)
                    hash = hash * 59 + this.IsDictionary.GetHashCode();

                if (this.IsMultilingual != null)
                    hash = hash * 59 + this.IsMultilingual.GetHashCode();

                if (this.IsRequired != null)
                    hash = hash * 59 + this.IsRequired.GetHashCode();

                if (this.ValueType != null)
                    hash = hash * 59 + this.ValueType.GetHashCode();

                if (this.DisplayNames != null)
                    hash = hash * 59 + this.DisplayNames.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
