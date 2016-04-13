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
    public partial class VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap :  IEquatable<VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap>
    {
        /// <summary>
        /// Gets or Sets EntityColumnName
        /// </summary>
        [DataMember(Name="entityColumnName", EmitDefaultValue=false)]
        public string EntityColumnName { get; set; }

        /// <summary>
        /// Gets or Sets CsvColumnName
        /// </summary>
        [DataMember(Name="csvColumnName", EmitDefaultValue=false)]
        public string CsvColumnName { get; set; }

        /// <summary>
        /// Gets or Sets IsSystemProperty
        /// </summary>
        [DataMember(Name="isSystemProperty", EmitDefaultValue=false)]
        public bool? IsSystemProperty { get; set; }

        /// <summary>
        /// Gets or Sets IsRequired
        /// </summary>
        [DataMember(Name="isRequired", EmitDefaultValue=false)]
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Gets or Sets CustomValue
        /// </summary>
        [DataMember(Name="customValue", EmitDefaultValue=false)]
        public string CustomValue { get; set; }

        /// <summary>
        /// Gets or Sets StringFormat
        /// </summary>
        [DataMember(Name="stringFormat", EmitDefaultValue=false)]
        public string StringFormat { get; set; }

        /// <summary>
        /// Gets or Sets Locale
        /// </summary>
        [DataMember(Name="locale", EmitDefaultValue=false)]
        public string Locale { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap {\n");
            sb.Append("  EntityColumnName: ").Append(EntityColumnName).Append("\n");
            sb.Append("  CsvColumnName: ").Append(CsvColumnName).Append("\n");
            sb.Append("  IsSystemProperty: ").Append(IsSystemProperty).Append("\n");
            sb.Append("  IsRequired: ").Append(IsRequired).Append("\n");
            sb.Append("  CustomValue: ").Append(CustomValue).Append("\n");
            sb.Append("  StringFormat: ").Append(StringFormat).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.EntityColumnName == other.EntityColumnName ||
                    this.EntityColumnName != null &&
                    this.EntityColumnName.Equals(other.EntityColumnName)
                ) && 
                (
                    this.CsvColumnName == other.CsvColumnName ||
                    this.CsvColumnName != null &&
                    this.CsvColumnName.Equals(other.CsvColumnName)
                ) && 
                (
                    this.IsSystemProperty == other.IsSystemProperty ||
                    this.IsSystemProperty != null &&
                    this.IsSystemProperty.Equals(other.IsSystemProperty)
                ) && 
                (
                    this.IsRequired == other.IsRequired ||
                    this.IsRequired != null &&
                    this.IsRequired.Equals(other.IsRequired)
                ) && 
                (
                    this.CustomValue == other.CustomValue ||
                    this.CustomValue != null &&
                    this.CustomValue.Equals(other.CustomValue)
                ) && 
                (
                    this.StringFormat == other.StringFormat ||
                    this.StringFormat != null &&
                    this.StringFormat.Equals(other.StringFormat)
                ) && 
                (
                    this.Locale == other.Locale ||
                    this.Locale != null &&
                    this.Locale.Equals(other.Locale)
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

                if (this.EntityColumnName != null)
                    hash = hash * 59 + this.EntityColumnName.GetHashCode();

                if (this.CsvColumnName != null)
                    hash = hash * 59 + this.CsvColumnName.GetHashCode();

                if (this.IsSystemProperty != null)
                    hash = hash * 59 + this.IsSystemProperty.GetHashCode();

                if (this.IsRequired != null)
                    hash = hash * 59 + this.IsRequired.GetHashCode();

                if (this.CustomValue != null)
                    hash = hash * 59 + this.CustomValue.GetHashCode();

                if (this.StringFormat != null)
                    hash = hash * 59 + this.StringFormat.GetHashCode();

                if (this.Locale != null)
                    hash = hash * 59 + this.Locale.GetHashCode();

                return hash;
            }
        }

    }
}
