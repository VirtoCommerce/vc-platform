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
    public partial class VirtoCommerceCatalogModuleWebModelCatalogSearchResult :  IEquatable<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>
    {
        /// <summary>
        /// Gets or Sets ProductsTotalCount
        /// </summary>
        [DataMember(Name="productsTotalCount", EmitDefaultValue=false)]
        public int? ProductsTotalCount { get; set; }

        /// <summary>
        /// Gets or Sets Products
        /// </summary>
        [DataMember(Name="products", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelProduct> Products { get; set; }

        /// <summary>
        /// Gets or Sets Categories
        /// </summary>
        [DataMember(Name="categories", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelCategory> Categories { get; set; }

        /// <summary>
        /// Gets or Sets Catalogs
        /// </summary>
        [DataMember(Name="catalogs", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelCatalog> Catalogs { get; set; }

        /// <summary>
        /// Gets or Sets Aggregations
        /// </summary>
        [DataMember(Name="aggregations", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelAggregation> Aggregations { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelCatalogSearchResult {\n");
            sb.Append("  ProductsTotalCount: ").Append(ProductsTotalCount).Append("\n");
            sb.Append("  Products: ").Append(Products).Append("\n");
            sb.Append("  Categories: ").Append(Categories).Append("\n");
            sb.Append("  Catalogs: ").Append(Catalogs).Append("\n");
            sb.Append("  Aggregations: ").Append(Aggregations).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelCatalogSearchResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelCatalogSearchResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelCatalogSearchResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelCatalogSearchResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ProductsTotalCount == other.ProductsTotalCount ||
                    this.ProductsTotalCount != null &&
                    this.ProductsTotalCount.Equals(other.ProductsTotalCount)
                ) && 
                (
                    this.Products == other.Products ||
                    this.Products != null &&
                    this.Products.SequenceEqual(other.Products)
                ) && 
                (
                    this.Categories == other.Categories ||
                    this.Categories != null &&
                    this.Categories.SequenceEqual(other.Categories)
                ) && 
                (
                    this.Catalogs == other.Catalogs ||
                    this.Catalogs != null &&
                    this.Catalogs.SequenceEqual(other.Catalogs)
                ) && 
                (
                    this.Aggregations == other.Aggregations ||
                    this.Aggregations != null &&
                    this.Aggregations.SequenceEqual(other.Aggregations)
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

                if (this.ProductsTotalCount != null)
                    hash = hash * 59 + this.ProductsTotalCount.GetHashCode();

                if (this.Products != null)
                    hash = hash * 59 + this.Products.GetHashCode();

                if (this.Categories != null)
                    hash = hash * 59 + this.Categories.GetHashCode();

                if (this.Catalogs != null)
                    hash = hash * 59 + this.Catalogs.GetHashCode();

                if (this.Aggregations != null)
                    hash = hash * 59 + this.Aggregations.GetHashCode();

                return hash;
            }
        }

    }
}
