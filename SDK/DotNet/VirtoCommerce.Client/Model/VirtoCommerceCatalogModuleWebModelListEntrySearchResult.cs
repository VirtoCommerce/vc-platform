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
    /// Class representing the result of ListEntries search.
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelListEntrySearchResult :  IEquatable<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>
    {
        /// <summary>
        /// Gets or sets the total entries count matching the search criteria.
        /// </summary>
        /// <value>Gets or sets the total entries count matching the search criteria.</value>
        [DataMember(Name="totalCount", EmitDefaultValue=false)]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the list entries.
        /// </summary>
        /// <value>Gets or sets the list entries.</value>
        [DataMember(Name="listEntries", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelListEntry> ListEntries { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelListEntrySearchResult {\n");
            sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelListEntrySearchResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelListEntrySearchResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelListEntrySearchResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelListEntrySearchResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.TotalCount == other.TotalCount ||
                    this.TotalCount != null &&
                    this.TotalCount.Equals(other.TotalCount)
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

                if (this.TotalCount != null)
                    hash = hash * 59 + this.TotalCount.GetHashCode();

                if (this.ListEntries != null)
                    hash = hash * 59 + this.ListEntries.GetHashCode();

                return hash;
            }
        }

    }
}
