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
    /// Information to define linking information from item or category to category.
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelListEntryLink :  IEquatable<VirtoCommerceCatalogModuleWebModelListEntryLink>
    {
        /// <summary>
        /// Gets or sets the list entry identifier.
        /// </summary>
        /// <value>Gets or sets the list entry identifier.</value>
        [DataMember(Name="listEntryId", EmitDefaultValue=false)]
        public string ListEntryId { get; set; }

        /// <summary>
        /// Gets or sets the type of the list entry. E.g. \&quot;product\&quot;, \&quot;category\&quot;
        /// </summary>
        /// <value>Gets or sets the type of the list entry. E.g. \&quot;product\&quot;, \&quot;category\&quot;</value>
        [DataMember(Name="listEntryType", EmitDefaultValue=false)]
        public string ListEntryType { get; set; }

        /// <summary>
        /// Gets or sets the target catalog identifier.
        /// </summary>
        /// <value>Gets or sets the target catalog identifier.</value>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the target category identifier.
        /// </summary>
        /// <value>Gets or sets the target category identifier.</value>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelListEntryLink {\n");
            sb.Append("  ListEntryId: ").Append(ListEntryId).Append("\n");
            sb.Append("  ListEntryType: ").Append(ListEntryType).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelListEntryLink);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelListEntryLink instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelListEntryLink to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelListEntryLink other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ListEntryId == other.ListEntryId ||
                    this.ListEntryId != null &&
                    this.ListEntryId.Equals(other.ListEntryId)
                ) && 
                (
                    this.ListEntryType == other.ListEntryType ||
                    this.ListEntryType != null &&
                    this.ListEntryType.Equals(other.ListEntryType)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
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

                if (this.ListEntryId != null)
                    hash = hash * 59 + this.ListEntryId.GetHashCode();

                if (this.ListEntryType != null)
                    hash = hash * 59 + this.ListEntryType.GetHashCode();

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                return hash;
            }
        }

    }
}
