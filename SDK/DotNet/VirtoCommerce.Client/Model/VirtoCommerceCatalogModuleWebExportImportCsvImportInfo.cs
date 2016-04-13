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
    public partial class VirtoCommerceCatalogModuleWebExportImportCsvImportInfo :  IEquatable<VirtoCommerceCatalogModuleWebExportImportCsvImportInfo>
    {
        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets FileUrl
        /// </summary>
        [DataMember(Name="fileUrl", EmitDefaultValue=false)]
        public string FileUrl { get; set; }

        /// <summary>
        /// Gets or Sets Configuration
        /// </summary>
        [DataMember(Name="configuration", EmitDefaultValue=false)]
        public VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration Configuration { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebExportImportCsvImportInfo {\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  FileUrl: ").Append(FileUrl).Append("\n");
            sb.Append("  Configuration: ").Append(Configuration).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebExportImportCsvImportInfo);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebExportImportCsvImportInfo instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebExportImportCsvImportInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebExportImportCsvImportInfo other)
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
                    this.FileUrl == other.FileUrl ||
                    this.FileUrl != null &&
                    this.FileUrl.Equals(other.FileUrl)
                ) && 
                (
                    this.Configuration == other.Configuration ||
                    this.Configuration != null &&
                    this.Configuration.Equals(other.Configuration)
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

                if (this.FileUrl != null)
                    hash = hash * 59 + this.FileUrl.GetHashCode();

                if (this.Configuration != null)
                    hash = hash * 59 + this.Configuration.GetHashCode();

                return hash;
            }
        }

    }
}
