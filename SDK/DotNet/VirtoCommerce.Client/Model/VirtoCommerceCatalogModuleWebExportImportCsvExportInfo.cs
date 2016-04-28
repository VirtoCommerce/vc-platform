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
    public partial class VirtoCommerceCatalogModuleWebExportImportCsvExportInfo :  IEquatable<VirtoCommerceCatalogModuleWebExportImportCsvExportInfo>
    {
        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets ProductIds
        /// </summary>
        [DataMember(Name="productIds", EmitDefaultValue=false)]
        public List<string> ProductIds { get; set; }

        /// <summary>
        /// Gets or Sets CategoryIds
        /// </summary>
        [DataMember(Name="categoryIds", EmitDefaultValue=false)]
        public List<string> CategoryIds { get; set; }

        /// <summary>
        /// Gets or Sets PriceListId
        /// </summary>
        [DataMember(Name="priceListId", EmitDefaultValue=false)]
        public string PriceListId { get; set; }

        /// <summary>
        /// Gets or Sets FulfilmentCenterId
        /// </summary>
        [DataMember(Name="fulfilmentCenterId", EmitDefaultValue=false)]
        public string FulfilmentCenterId { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

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
            sb.Append("class VirtoCommerceCatalogModuleWebExportImportCsvExportInfo {\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  ProductIds: ").Append(ProductIds).Append("\n");
            sb.Append("  CategoryIds: ").Append(CategoryIds).Append("\n");
            sb.Append("  PriceListId: ").Append(PriceListId).Append("\n");
            sb.Append("  FulfilmentCenterId: ").Append(FulfilmentCenterId).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebExportImportCsvExportInfo);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebExportImportCsvExportInfo instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebExportImportCsvExportInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebExportImportCsvExportInfo other)
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
                    this.ProductIds == other.ProductIds ||
                    this.ProductIds != null &&
                    this.ProductIds.SequenceEqual(other.ProductIds)
                ) && 
                (
                    this.CategoryIds == other.CategoryIds ||
                    this.CategoryIds != null &&
                    this.CategoryIds.SequenceEqual(other.CategoryIds)
                ) && 
                (
                    this.PriceListId == other.PriceListId ||
                    this.PriceListId != null &&
                    this.PriceListId.Equals(other.PriceListId)
                ) && 
                (
                    this.FulfilmentCenterId == other.FulfilmentCenterId ||
                    this.FulfilmentCenterId != null &&
                    this.FulfilmentCenterId.Equals(other.FulfilmentCenterId)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
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

                if (this.ProductIds != null)
                    hash = hash * 59 + this.ProductIds.GetHashCode();

                if (this.CategoryIds != null)
                    hash = hash * 59 + this.CategoryIds.GetHashCode();

                if (this.PriceListId != null)
                    hash = hash * 59 + this.PriceListId.GetHashCode();

                if (this.FulfilmentCenterId != null)
                    hash = hash * 59 + this.FulfilmentCenterId.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.Configuration != null)
                    hash = hash * 59 + this.Configuration.GetHashCode();

                return hash;
            }
        }

    }
}
