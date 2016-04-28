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
    /// Catalog Language information.
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelCatalogLanguage :  IEquatable<VirtoCommerceCatalogModuleWebModelCatalogLanguage>
    {
        /// <summary>
        /// Gets or sets the catalog identifier.
        /// </summary>
        /// <value>Gets or sets the catalog identifier.</value>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this catalog language is default.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this catalog language is default.</value>
        [DataMember(Name="isDefault", EmitDefaultValue=false)]
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        /// <value>Gets or sets the language code.</value>
        [DataMember(Name="languageCode", EmitDefaultValue=false)]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets the human-readable language name.
        /// </summary>
        /// <value>Gets the human-readable language name.</value>
        [DataMember(Name="displayName", EmitDefaultValue=false)]
        public string DisplayName { get; private set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelCatalogLanguage {\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  IsDefault: ").Append(IsDefault).Append("\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelCatalogLanguage);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelCatalogLanguage instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelCatalogLanguage to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelCatalogLanguage other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.IsDefault == other.IsDefault ||
                    this.IsDefault != null &&
                    this.IsDefault.Equals(other.IsDefault)
                ) && 
                (
                    this.LanguageCode == other.LanguageCode ||
                    this.LanguageCode != null &&
                    this.LanguageCode.Equals(other.LanguageCode)
                ) && 
                (
                    this.DisplayName == other.DisplayName ||
                    this.DisplayName != null &&
                    this.DisplayName.Equals(other.DisplayName)
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

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.IsDefault != null)
                    hash = hash * 59 + this.IsDefault.GetHashCode();

                if (this.LanguageCode != null)
                    hash = hash * 59 + this.LanguageCode.GetHashCode();

                if (this.DisplayName != null)
                    hash = hash * 59 + this.DisplayName.GetHashCode();

                return hash;
            }
        }

    }
}
