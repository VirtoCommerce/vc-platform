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
    /// Represent move operation detail
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelMoveInfo :  IEquatable<VirtoCommerceCatalogModuleWebModelMoveInfo>
    {
        /// <summary>
        /// Gets or Sets Catalog
        /// </summary>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or Sets Category
        /// </summary>
        [DataMember(Name="category", EmitDefaultValue=false)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or Sets ListEntries
        /// </summary>
        [DataMember(Name="listEntries", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelListEntry> ListEntries { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelMoveInfo {\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  Category: ").Append(Category).Append("\n");
            sb.Append("  ListEntries: ").Append(ListEntries).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelMoveInfo);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelMoveInfo instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelMoveInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelMoveInfo other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Catalog == other.Catalog ||
                    this.Catalog != null &&
                    this.Catalog.Equals(other.Catalog)
                ) && 
                (
                    this.Category == other.Category ||
                    this.Category != null &&
                    this.Category.Equals(other.Category)
                ) && 
                (
                    this.ListEntries == other.ListEntries ||
                    this.ListEntries != null &&
                    this.ListEntries.SequenceEqual(other.ListEntries)
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

                if (this.Catalog != null)
                    hash = hash * 59 + this.Catalog.GetHashCode();

                if (this.Category != null)
                    hash = hash * 59 + this.Category.GetHashCode();

                if (this.ListEntries != null)
                    hash = hash * 59 + this.ListEntries.GetHashCode();

                return hash;
            }
        }

    }
}
