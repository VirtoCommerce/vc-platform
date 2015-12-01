using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VirtoCommerceCustomerModuleWebModelSearchCriteria : IEquatable<VirtoCommerceCustomerModuleWebModelSearchCriteria>
    {
        
        /// <summary>
        /// Word, part of word or phrase to search
        /// </summary>
        /// <value>Word, part of word or phrase to search</value>
        [DataMember(Name="keyword", EmitDefaultValue=false)]
        public string Keyword { get; set; }
  
        
        /// <summary>
        /// It used to limit search within an organization
        /// </summary>
        /// <value>It used to limit search within an organization</value>
        [DataMember(Name="organizationId", EmitDefaultValue=false)]
        public string OrganizationId { get; set; }
  
        
        /// <summary>
        /// It used to skip some first search results
        /// </summary>
        /// <value>It used to skip some first search results</value>
        [DataMember(Name="start", EmitDefaultValue=false)]
        public int? Start { get; set; }
  
        
        /// <summary>
        /// It used to limit the number of search results
        /// </summary>
        /// <value>It used to limit the number of search results</value>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCustomerModuleWebModelSearchCriteria {\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCustomerModuleWebModelSearchCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCustomerModuleWebModelSearchCriteria instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceCustomerModuleWebModelSearchCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCustomerModuleWebModelSearchCriteria other)
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
                    this.OrganizationId == other.OrganizationId ||
                    this.OrganizationId != null &&
                    this.OrganizationId.Equals(other.OrganizationId)
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
                    hash = hash * 57 + this.Keyword.GetHashCode();
                
                if (this.OrganizationId != null)
                    hash = hash * 57 + this.OrganizationId.GetHashCode();
                
                if (this.Start != null)
                    hash = hash * 57 + this.Start.GetHashCode();
                
                if (this.Count != null)
                    hash = hash * 57 + this.Count.GetHashCode();
                
                return hash;
            }
        }

    }


}
