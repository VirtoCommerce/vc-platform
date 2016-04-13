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
    public partial class VirtoCommerceDomainCustomerModelMembersSearchCriteria :  IEquatable<VirtoCommerceDomainCustomerModelMembersSearchCriteria>
    {
        /// <summary>
        /// Gets or Sets MemberType
        /// </summary>
        [DataMember(Name="memberType", EmitDefaultValue=false)]
        public string MemberType { get; set; }

        /// <summary>
        /// Gets or Sets MemberTypes
        /// </summary>
        [DataMember(Name="memberTypes", EmitDefaultValue=false)]
        public List<string> MemberTypes { get; set; }

        /// <summary>
        /// Gets or Sets Keyword
        /// </summary>
        [DataMember(Name="keyword", EmitDefaultValue=false)]
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or Sets MemberId
        /// </summary>
        [DataMember(Name="memberId", EmitDefaultValue=false)]
        public string MemberId { get; set; }

        /// <summary>
        /// Gets or Sets DeepSearch
        /// </summary>
        [DataMember(Name="deepSearch", EmitDefaultValue=false)]
        public bool? DeepSearch { get; set; }

        /// <summary>
        /// Gets or Sets Sort
        /// </summary>
        [DataMember(Name="sort", EmitDefaultValue=false)]
        public string Sort { get; set; }

        /// <summary>
        /// Gets or Sets SortInfos
        /// </summary>
        [DataMember(Name="sortInfos", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreCommonSortInfo> SortInfos { get; set; }

        /// <summary>
        /// Gets or Sets Skip
        /// </summary>
        [DataMember(Name="skip", EmitDefaultValue=false)]
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or Sets Take
        /// </summary>
        [DataMember(Name="take", EmitDefaultValue=false)]
        public int? Take { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainCustomerModelMembersSearchCriteria {\n");
            sb.Append("  MemberType: ").Append(MemberType).Append("\n");
            sb.Append("  MemberTypes: ").Append(MemberTypes).Append("\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  MemberId: ").Append(MemberId).Append("\n");
            sb.Append("  DeepSearch: ").Append(DeepSearch).Append("\n");
            sb.Append("  Sort: ").Append(Sort).Append("\n");
            sb.Append("  SortInfos: ").Append(SortInfos).Append("\n");
            sb.Append("  Skip: ").Append(Skip).Append("\n");
            sb.Append("  Take: ").Append(Take).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCustomerModelMembersSearchCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCustomerModelMembersSearchCriteria instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCustomerModelMembersSearchCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCustomerModelMembersSearchCriteria other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.MemberType == other.MemberType ||
                    this.MemberType != null &&
                    this.MemberType.Equals(other.MemberType)
                ) && 
                (
                    this.MemberTypes == other.MemberTypes ||
                    this.MemberTypes != null &&
                    this.MemberTypes.SequenceEqual(other.MemberTypes)
                ) && 
                (
                    this.Keyword == other.Keyword ||
                    this.Keyword != null &&
                    this.Keyword.Equals(other.Keyword)
                ) && 
                (
                    this.MemberId == other.MemberId ||
                    this.MemberId != null &&
                    this.MemberId.Equals(other.MemberId)
                ) && 
                (
                    this.DeepSearch == other.DeepSearch ||
                    this.DeepSearch != null &&
                    this.DeepSearch.Equals(other.DeepSearch)
                ) && 
                (
                    this.Sort == other.Sort ||
                    this.Sort != null &&
                    this.Sort.Equals(other.Sort)
                ) && 
                (
                    this.SortInfos == other.SortInfos ||
                    this.SortInfos != null &&
                    this.SortInfos.SequenceEqual(other.SortInfos)
                ) && 
                (
                    this.Skip == other.Skip ||
                    this.Skip != null &&
                    this.Skip.Equals(other.Skip)
                ) && 
                (
                    this.Take == other.Take ||
                    this.Take != null &&
                    this.Take.Equals(other.Take)
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

                if (this.MemberType != null)
                    hash = hash * 59 + this.MemberType.GetHashCode();

                if (this.MemberTypes != null)
                    hash = hash * 59 + this.MemberTypes.GetHashCode();

                if (this.Keyword != null)
                    hash = hash * 59 + this.Keyword.GetHashCode();

                if (this.MemberId != null)
                    hash = hash * 59 + this.MemberId.GetHashCode();

                if (this.DeepSearch != null)
                    hash = hash * 59 + this.DeepSearch.GetHashCode();

                if (this.Sort != null)
                    hash = hash * 59 + this.Sort.GetHashCode();

                if (this.SortInfos != null)
                    hash = hash * 59 + this.SortInfos.GetHashCode();

                if (this.Skip != null)
                    hash = hash * 59 + this.Skip.GetHashCode();

                if (this.Take != null)
                    hash = hash * 59 + this.Take.GetHashCode();

                return hash;
            }
        }

    }
}
