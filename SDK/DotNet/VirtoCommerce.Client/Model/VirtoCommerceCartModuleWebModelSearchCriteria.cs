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
    public partial class VirtoCommerceCartModuleWebModelSearchCriteria :  IEquatable<VirtoCommerceCartModuleWebModelSearchCriteria>
    {
        /// <summary>
        /// Gets or sets the value of search criteria keyword
        /// </summary>
        /// <value>Gets or sets the value of search criteria keyword</value>
        [DataMember(Name="keyword", EmitDefaultValue=false)]
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the value of search criteria customer id
        /// </summary>
        /// <value>Gets or sets the value of search criteria customer id</value>
        [DataMember(Name="customerId", EmitDefaultValue=false)]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the value of search criteria store id
        /// </summary>
        /// <value>Gets or sets the value of search criteria store id</value>
        [DataMember(Name="storeId", EmitDefaultValue=false)]
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the value of search criteria skip records count
        /// </summary>
        /// <value>Gets or sets the value of search criteria skip records count</value>
        [DataMember(Name="start", EmitDefaultValue=false)]
        public int? Start { get; set; }

        /// <summary>
        /// Gets or sets the value of search criteria page size
        /// </summary>
        /// <value>Gets or sets the value of search criteria page size</value>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCartModuleWebModelSearchCriteria {\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
            sb.Append("  StoreId: ").Append(StoreId).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCartModuleWebModelSearchCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCartModuleWebModelSearchCriteria instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCartModuleWebModelSearchCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCartModuleWebModelSearchCriteria other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Keyword == other.Keyword ||
                    this.Keyword != null &&
                    this.Keyword.Equals(other.Keyword)
                ) && 
                (
                    this.CustomerId == other.CustomerId ||
                    this.CustomerId != null &&
                    this.CustomerId.Equals(other.CustomerId)
                ) && 
                (
                    this.StoreId == other.StoreId ||
                    this.StoreId != null &&
                    this.StoreId.Equals(other.StoreId)
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

                if (this.Keyword != null)
                    hash = hash * 59 + this.Keyword.GetHashCode();

                if (this.CustomerId != null)
                    hash = hash * 59 + this.CustomerId.GetHashCode();

                if (this.StoreId != null)
                    hash = hash * 59 + this.StoreId.GetHashCode();

                if (this.Start != null)
                    hash = hash * 59 + this.Start.GetHashCode();

                if (this.Count != null)
                    hash = hash * 59 + this.Count.GetHashCode();

                return hash;
            }
        }

    }
}
