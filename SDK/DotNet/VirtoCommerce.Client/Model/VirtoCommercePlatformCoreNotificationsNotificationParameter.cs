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
    public partial class VirtoCommercePlatformCoreNotificationsNotificationParameter :  IEquatable<VirtoCommercePlatformCoreNotificationsNotificationParameter>
    {
        /// <summary>
        /// Gets or Sets ParameterName
        /// </summary>
        [DataMember(Name="parameterName", EmitDefaultValue=false)]
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or Sets ParameterDescription
        /// </summary>
        [DataMember(Name="parameterDescription", EmitDefaultValue=false)]
        public string ParameterDescription { get; set; }

        /// <summary>
        /// Gets or Sets ParameterCodeInView
        /// </summary>
        [DataMember(Name="parameterCodeInView", EmitDefaultValue=false)]
        public string ParameterCodeInView { get; set; }

        /// <summary>
        /// Gets or Sets IsDictionary
        /// </summary>
        [DataMember(Name="isDictionary", EmitDefaultValue=false)]
        public bool? IsDictionary { get; set; }

        /// <summary>
        /// Gets or Sets IsArray
        /// </summary>
        [DataMember(Name="isArray", EmitDefaultValue=false)]
        public bool? IsArray { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        [DataMember(Name="value", EmitDefaultValue=false)]
        public Object Value { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCoreNotificationsNotificationParameter {\n");
            sb.Append("  ParameterName: ").Append(ParameterName).Append("\n");
            sb.Append("  ParameterDescription: ").Append(ParameterDescription).Append("\n");
            sb.Append("  ParameterCodeInView: ").Append(ParameterCodeInView).Append("\n");
            sb.Append("  IsDictionary: ").Append(IsDictionary).Append("\n");
            sb.Append("  IsArray: ").Append(IsArray).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformCoreNotificationsNotificationParameter);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreNotificationsNotificationParameter instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCoreNotificationsNotificationParameter to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreNotificationsNotificationParameter other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ParameterName == other.ParameterName ||
                    this.ParameterName != null &&
                    this.ParameterName.Equals(other.ParameterName)
                ) && 
                (
                    this.ParameterDescription == other.ParameterDescription ||
                    this.ParameterDescription != null &&
                    this.ParameterDescription.Equals(other.ParameterDescription)
                ) && 
                (
                    this.ParameterCodeInView == other.ParameterCodeInView ||
                    this.ParameterCodeInView != null &&
                    this.ParameterCodeInView.Equals(other.ParameterCodeInView)
                ) && 
                (
                    this.IsDictionary == other.IsDictionary ||
                    this.IsDictionary != null &&
                    this.IsDictionary.Equals(other.IsDictionary)
                ) && 
                (
                    this.IsArray == other.IsArray ||
                    this.IsArray != null &&
                    this.IsArray.Equals(other.IsArray)
                ) && 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.Value == other.Value ||
                    this.Value != null &&
                    this.Value.Equals(other.Value)
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

                if (this.ParameterName != null)
                    hash = hash * 59 + this.ParameterName.GetHashCode();

                if (this.ParameterDescription != null)
                    hash = hash * 59 + this.ParameterDescription.GetHashCode();

                if (this.ParameterCodeInView != null)
                    hash = hash * 59 + this.ParameterCodeInView.GetHashCode();

                if (this.IsDictionary != null)
                    hash = hash * 59 + this.IsDictionary.GetHashCode();

                if (this.IsArray != null)
                    hash = hash * 59 + this.IsArray.GetHashCode();

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.Value != null)
                    hash = hash * 59 + this.Value.GetHashCode();

                return hash;
            }
        }

    }
}
