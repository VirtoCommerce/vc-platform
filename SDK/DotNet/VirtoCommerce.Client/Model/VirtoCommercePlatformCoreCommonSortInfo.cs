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
    public partial class VirtoCommercePlatformCoreCommonSortInfo :  IEquatable<VirtoCommercePlatformCoreCommonSortInfo>
    {
        /// <summary>
        /// Gets or Sets SortColumn
        /// </summary>
        [DataMember(Name="sortColumn", EmitDefaultValue=false)]
        public string SortColumn { get; set; }

        /// <summary>
        /// Gets or Sets SortDirection
        /// </summary>
        [DataMember(Name="sortDirection", EmitDefaultValue=false)]
        public string SortDirection { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCoreCommonSortInfo {\n");
            sb.Append("  SortColumn: ").Append(SortColumn).Append("\n");
            sb.Append("  SortDirection: ").Append(SortDirection).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformCoreCommonSortInfo);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreCommonSortInfo instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCoreCommonSortInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreCommonSortInfo other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.SortColumn == other.SortColumn ||
                    this.SortColumn != null &&
                    this.SortColumn.Equals(other.SortColumn)
                ) && 
                (
                    this.SortDirection == other.SortDirection ||
                    this.SortDirection != null &&
                    this.SortDirection.Equals(other.SortDirection)
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

                if (this.SortColumn != null)
                    hash = hash * 59 + this.SortColumn.GetHashCode();

                if (this.SortDirection != null)
                    hash = hash * 59 + this.SortDirection.GetHashCode();

                return hash;
            }
        }

    }
}
