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
    public partial class VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration :  IEquatable<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>
    {
        /// <summary>
        /// Gets or Sets ETag
        /// </summary>
        [DataMember(Name="eTag", EmitDefaultValue=false)]
        public string ETag { get; set; }

        /// <summary>
        /// Gets or Sets Delimiter
        /// </summary>
        [DataMember(Name="delimiter", EmitDefaultValue=false)]
        public string Delimiter { get; set; }

        /// <summary>
        /// Gets or Sets CsvColumns
        /// </summary>
        [DataMember(Name="csvColumns", EmitDefaultValue=false)]
        public List<string> CsvColumns { get; set; }

        /// <summary>
        /// Gets or Sets PropertyMaps
        /// </summary>
        [DataMember(Name="propertyMaps", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap> PropertyMaps { get; set; }

        /// <summary>
        /// Gets or Sets PropertyCsvColumns
        /// </summary>
        [DataMember(Name="propertyCsvColumns", EmitDefaultValue=false)]
        public List<string> PropertyCsvColumns { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration {\n");
            sb.Append("  ETag: ").Append(ETag).Append("\n");
            sb.Append("  Delimiter: ").Append(Delimiter).Append("\n");
            sb.Append("  CsvColumns: ").Append(CsvColumns).Append("\n");
            sb.Append("  PropertyMaps: ").Append(PropertyMaps).Append("\n");
            sb.Append("  PropertyCsvColumns: ").Append(PropertyCsvColumns).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ETag == other.ETag ||
                    this.ETag != null &&
                    this.ETag.Equals(other.ETag)
                ) && 
                (
                    this.Delimiter == other.Delimiter ||
                    this.Delimiter != null &&
                    this.Delimiter.Equals(other.Delimiter)
                ) && 
                (
                    this.CsvColumns == other.CsvColumns ||
                    this.CsvColumns != null &&
                    this.CsvColumns.SequenceEqual(other.CsvColumns)
                ) && 
                (
                    this.PropertyMaps == other.PropertyMaps ||
                    this.PropertyMaps != null &&
                    this.PropertyMaps.SequenceEqual(other.PropertyMaps)
                ) && 
                (
                    this.PropertyCsvColumns == other.PropertyCsvColumns ||
                    this.PropertyCsvColumns != null &&
                    this.PropertyCsvColumns.SequenceEqual(other.PropertyCsvColumns)
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

                if (this.ETag != null)
                    hash = hash * 59 + this.ETag.GetHashCode();

                if (this.Delimiter != null)
                    hash = hash * 59 + this.Delimiter.GetHashCode();

                if (this.CsvColumns != null)
                    hash = hash * 59 + this.CsvColumns.GetHashCode();

                if (this.PropertyMaps != null)
                    hash = hash * 59 + this.PropertyMaps.GetHashCode();

                if (this.PropertyCsvColumns != null)
                    hash = hash * 59 + this.PropertyCsvColumns.GetHashCode();

                return hash;
            }
        }

    }
}
