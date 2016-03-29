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
    public partial class VirtoCommercePlatformCoreSecurityUserSearchRequest :  IEquatable<VirtoCommercePlatformCoreSecurityUserSearchRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommercePlatformCoreSecurityUserSearchRequest" /> class.
        /// Initializes a new instance of the <see cref="VirtoCommercePlatformCoreSecurityUserSearchRequest" />class.
        /// </summary>
        /// <param name="AccountTypes">AccountTypes.</param>
        /// <param name="Keyword">Keyword.</param>
        /// <param name="MemberId">MemberId.</param>
        /// <param name="SkipCount">SkipCount.</param>
        /// <param name="TakeCount">TakeCount.</param>

        public VirtoCommercePlatformCoreSecurityUserSearchRequest(List<string> AccountTypes = null, string Keyword = null, string MemberId = null, int? SkipCount = null, int? TakeCount = null)
        {
            this.AccountTypes = AccountTypes;
            this.Keyword = Keyword;
            this.MemberId = MemberId;
            this.SkipCount = SkipCount;
            this.TakeCount = TakeCount;
            
        }

        /// <summary>
        /// Gets or Sets AccountTypes
        /// </summary>
        [DataMember(Name="accountTypes", EmitDefaultValue=false)]
        public List<string> AccountTypes { get; set; }

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
        /// Gets or Sets SkipCount
        /// </summary>
        [DataMember(Name="skipCount", EmitDefaultValue=false)]
        public int? SkipCount { get; set; }

        /// <summary>
        /// Gets or Sets TakeCount
        /// </summary>
        [DataMember(Name="takeCount", EmitDefaultValue=false)]
        public int? TakeCount { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCoreSecurityUserSearchRequest {\n");
            sb.Append("  AccountTypes: ").Append(AccountTypes).Append("\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  MemberId: ").Append(MemberId).Append("\n");
            sb.Append("  SkipCount: ").Append(SkipCount).Append("\n");
            sb.Append("  TakeCount: ").Append(TakeCount).Append("\n");
            
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
            return this.Equals(obj as VirtoCommercePlatformCoreSecurityUserSearchRequest);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreSecurityUserSearchRequest instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCoreSecurityUserSearchRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreSecurityUserSearchRequest other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.AccountTypes == other.AccountTypes ||
                    this.AccountTypes != null &&
                    this.AccountTypes.SequenceEqual(other.AccountTypes)
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
                    this.SkipCount == other.SkipCount ||
                    this.SkipCount != null &&
                    this.SkipCount.Equals(other.SkipCount)
                ) && 
                (
                    this.TakeCount == other.TakeCount ||
                    this.TakeCount != null &&
                    this.TakeCount.Equals(other.TakeCount)
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
                
                if (this.AccountTypes != null)
                    hash = hash * 59 + this.AccountTypes.GetHashCode();
                
                if (this.Keyword != null)
                    hash = hash * 59 + this.Keyword.GetHashCode();
                
                if (this.MemberId != null)
                    hash = hash * 59 + this.MemberId.GetHashCode();
                
                if (this.SkipCount != null)
                    hash = hash * 59 + this.SkipCount.GetHashCode();
                
                if (this.TakeCount != null)
                    hash = hash * 59 + this.TakeCount.GetHashCode();
                
                return hash;
            }
        }

    }


}
