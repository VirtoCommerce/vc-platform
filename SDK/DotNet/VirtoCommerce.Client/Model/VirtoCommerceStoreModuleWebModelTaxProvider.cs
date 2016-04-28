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
    public partial class VirtoCommerceStoreModuleWebModelTaxProvider :  IEquatable<VirtoCommerceStoreModuleWebModelTaxProvider>
    {
        /// <summary>
        /// Inner unique method code
        /// </summary>
        /// <value>Inner unique method code</value>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Display name of shipping method
        /// </summary>
        /// <value>Display name of shipping method</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Absolute logo url of shipping method, can be used in UI
        /// </summary>
        /// <value>Absolute logo url of shipping method, can be used in UI</value>
        [DataMember(Name="logoUrl", EmitDefaultValue=false)]
        public string LogoUrl { get; set; }

        /// <summary>
        /// If true - method can be available on storefront
        /// </summary>
        /// <value>If true - method can be available on storefront</value>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or Sets Priority
        /// </summary>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or Sets Settings
        /// </summary>
        [DataMember(Name="settings", EmitDefaultValue=false)]
        public List<VirtoCommerceStoreModuleWebModelSetting> Settings { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceStoreModuleWebModelTaxProvider {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  LogoUrl: ").Append(LogoUrl).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  Settings: ").Append(Settings).Append("\n");
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
            return this.Equals(obj as VirtoCommerceStoreModuleWebModelTaxProvider);
        }

        /// <summary>
        /// Returns true if VirtoCommerceStoreModuleWebModelTaxProvider instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceStoreModuleWebModelTaxProvider to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceStoreModuleWebModelTaxProvider other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.LogoUrl == other.LogoUrl ||
                    this.LogoUrl != null &&
                    this.LogoUrl.Equals(other.LogoUrl)
                ) && 
                (
                    this.IsActive == other.IsActive ||
                    this.IsActive != null &&
                    this.IsActive.Equals(other.IsActive)
                ) && 
                (
                    this.Priority == other.Priority ||
                    this.Priority != null &&
                    this.Priority.Equals(other.Priority)
                ) && 
                (
                    this.Settings == other.Settings ||
                    this.Settings != null &&
                    this.Settings.SequenceEqual(other.Settings)
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

                if (this.Code != null)
                    hash = hash * 59 + this.Code.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.LogoUrl != null)
                    hash = hash * 59 + this.LogoUrl.GetHashCode();

                if (this.IsActive != null)
                    hash = hash * 59 + this.IsActive.GetHashCode();

                if (this.Priority != null)
                    hash = hash * 59 + this.Priority.GetHashCode();

                if (this.Settings != null)
                    hash = hash * 59 + this.Settings.GetHashCode();

                return hash;
            }
        }

    }
}
