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
    public partial class VirtoCommerceDomainMarketingModelMarketingSearchCriteria :  IEquatable<VirtoCommerceDomainMarketingModelMarketingSearchCriteria>
    {
        /// <summary>
        /// Gets or Sets FolderId
        /// </summary>
        [DataMember(Name="folderId", EmitDefaultValue=false)]
        public string FolderId { get; set; }

        /// <summary>
        /// Gets or Sets ResponseGroup
        /// </summary>
        [DataMember(Name="responseGroup", EmitDefaultValue=false)]
        public string ResponseGroup { get; set; }

        /// <summary>
        /// Gets or Sets Keyword
        /// </summary>
        [DataMember(Name="keyword", EmitDefaultValue=false)]
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or Sets Start
        /// </summary>
        [DataMember(Name="start", EmitDefaultValue=false)]
        public int? Start { get; set; }

        /// <summary>
        /// Gets or Sets Count
        /// </summary>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainMarketingModelMarketingSearchCriteria {\n");
            sb.Append("  FolderId: ").Append(FolderId).Append("\n");
            sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  Start: ").Append(Start).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainMarketingModelMarketingSearchCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainMarketingModelMarketingSearchCriteria instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainMarketingModelMarketingSearchCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainMarketingModelMarketingSearchCriteria other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.FolderId == other.FolderId ||
                    this.FolderId != null &&
                    this.FolderId.Equals(other.FolderId)
                ) && 
                (
                    this.ResponseGroup == other.ResponseGroup ||
                    this.ResponseGroup != null &&
                    this.ResponseGroup.Equals(other.ResponseGroup)
                ) && 
                (
                    this.Keyword == other.Keyword ||
                    this.Keyword != null &&
                    this.Keyword.Equals(other.Keyword)
                ) && 
                (
                    this.Start == other.Start ||
                    this.Start != null &&
                    this.Start.Equals(other.Start)
                ) && 
                (
                    this.Count == other.Count ||
                    this.Count != null &&
                    this.Count.Equals(other.Count)
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

                if (this.FolderId != null)
                    hash = hash * 59 + this.FolderId.GetHashCode();

                if (this.ResponseGroup != null)
                    hash = hash * 59 + this.ResponseGroup.GetHashCode();

                if (this.Keyword != null)
                    hash = hash * 59 + this.Keyword.GetHashCode();

                if (this.Start != null)
                    hash = hash * 59 + this.Start.GetHashCode();

                if (this.Count != null)
                    hash = hash * 59 + this.Count.GetHashCode();

                return hash;
            }
        }

    }
}
