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
    /// Merchandising Catalog.
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelCatalog :  IEquatable<VirtoCommerceCatalogModuleWebModelCatalog>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>Gets or sets the name.</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Catalog} is virtual or common.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Catalog} is virtual or common.</value>
        [DataMember(Name="isVirtual", EmitDefaultValue=false)]
        public bool? IsVirtual { get; set; }

        /// <summary>
        /// Gets the language from languages list marked as default.
        /// </summary>
        /// <value>Gets the language from languages list marked as default.</value>
        [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
        public VirtoCommerceCatalogModuleWebModelCatalogLanguage DefaultLanguage { get; set; }

        /// <summary>
        /// Gets or sets the catalog languages.
        /// </summary>
        /// <value>Gets or sets the catalog languages.</value>
        [DataMember(Name="languages", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelCatalogLanguage> Languages { get; set; }

        /// <summary>
        /// Gets or sets the catalog properties.
        /// </summary>
        /// <value>Gets or sets the catalog properties.</value>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelProperty> Properties { get; set; }

        /// <summary>
        /// Gets or Sets SecurityScopes
        /// </summary>
        [DataMember(Name="securityScopes", EmitDefaultValue=false)]
        public List<string> SecurityScopes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelCatalog {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  IsVirtual: ").Append(IsVirtual).Append("\n");
            sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
            sb.Append("  Languages: ").Append(Languages).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelCatalog);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelCatalog instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelCatalog to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelCatalog other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
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
                    this.SecurityScopes == other.SecurityScopes ||
                    this.SecurityScopes != null &&
                    this.SecurityScopes.SequenceEqual(other.SecurityScopes)
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

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

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

                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();

                return hash;
            }
        }

    }
}
