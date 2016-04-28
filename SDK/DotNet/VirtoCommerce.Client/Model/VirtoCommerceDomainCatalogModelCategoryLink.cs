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
    public partial class VirtoCommerceDomainCatalogModelCategoryLink :  IEquatable<VirtoCommerceDomainCatalogModelCategoryLink>
    {
        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets Catalog
        /// </summary>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCatalog Catalog { get; set; }

        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets Category
        /// </summary>
        [DataMember(Name="category", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCategory Category { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainCatalogModelCategoryLink {\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  Category: ").Append(Category).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCatalogModelCategoryLink);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCatalogModelCategoryLink instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCatalogModelCategoryLink to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCatalogModelCategoryLink other)
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
                    this.Catalog == other.Catalog ||
                    this.Catalog != null &&
                    this.Catalog.Equals(other.Catalog)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.Category == other.Category ||
                    this.Category != null &&
                    this.Category.Equals(other.Category)
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

                if (this.Catalog != null)
                    hash = hash * 59 + this.Catalog.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                if (this.Category != null)
                    hash = hash * 59 + this.Category.GetHashCode();

                return hash;
            }
        }

    }
}
