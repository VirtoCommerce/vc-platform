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
    public partial class VirtoCommerceDomainCatalogModelCatalog :  IEquatable<VirtoCommerceDomainCatalogModelCatalog>
    {
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets IsVirtual
        /// </summary>
        [DataMember(Name="isVirtual", EmitDefaultValue=false)]
        public bool? IsVirtual { get; set; }

        /// <summary>
        /// Gets or Sets DefaultLanguage
        /// </summary>
        [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCatalogLanguage DefaultLanguage { get; set; }

        /// <summary>
        /// Gets or Sets Languages
        /// </summary>
        [DataMember(Name="languages", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelCatalogLanguage> Languages { get; set; }

        /// <summary>
        /// Gets or Sets Properties
        /// </summary>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelProperty> Properties { get; set; }

        /// <summary>
        /// Gets or Sets PropertyValues
        /// </summary>
        [DataMember(Name="propertyValues", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelPropertyValue> PropertyValues { get; set; }

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
            sb.Append("class VirtoCommerceDomainCatalogModelCatalog {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  IsVirtual: ").Append(IsVirtual).Append("\n");
            sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
            sb.Append("  Languages: ").Append(Languages).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            sb.Append("  PropertyValues: ").Append(PropertyValues).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCatalogModelCatalog);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCatalogModelCatalog instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCatalogModelCatalog to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCatalogModelCatalog other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.IsVirtual == other.IsVirtual ||
                    this.IsVirtual != null &&
                    this.IsVirtual.Equals(other.IsVirtual)
                ) && 
                (
                    this.DefaultLanguage == other.DefaultLanguage ||
                    this.DefaultLanguage != null &&
                    this.DefaultLanguage.Equals(other.DefaultLanguage)
                ) && 
                (
                    this.Languages == other.Languages ||
                    this.Languages != null &&
                    this.Languages.SequenceEqual(other.Languages)
                ) && 
                (
                    this.Properties == other.Properties ||
                    this.Properties != null &&
                    this.Properties.SequenceEqual(other.Properties)
                ) && 
                (
                    this.PropertyValues == other.PropertyValues ||
                    this.PropertyValues != null &&
                    this.PropertyValues.SequenceEqual(other.PropertyValues)
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

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.IsVirtual != null)
                    hash = hash * 59 + this.IsVirtual.GetHashCode();

                if (this.DefaultLanguage != null)
                    hash = hash * 59 + this.DefaultLanguage.GetHashCode();

                if (this.Languages != null)
                    hash = hash * 59 + this.Languages.GetHashCode();

                if (this.Properties != null)
                    hash = hash * 59 + this.Properties.GetHashCode();

                if (this.PropertyValues != null)
                    hash = hash * 59 + this.PropertyValues.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
