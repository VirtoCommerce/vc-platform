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
    public class VirtoCommerceContentWebModelsGetPagesCriteria : IEquatable<VirtoCommerceContentWebModelsGetPagesCriteria>
    {
        
        /// <summary>
        /// Max value of last updated date, if it's null returns all pages for store
        /// </summary>
        /// <value>Max value of last updated date, if it's null returns all pages for store</value>
        [DataMember(Name="lastUpdateDate", EmitDefaultValue=false)]
        public DateTime? LastUpdateDate { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceContentWebModelsGetPagesCriteria {\n");
            sb.Append("  LastUpdateDate: ").Append(LastUpdateDate).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceContentWebModelsGetPagesCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsGetPagesCriteria instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceContentWebModelsGetPagesCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsGetPagesCriteria other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.LastUpdateDate == other.LastUpdateDate ||
                    this.LastUpdateDate != null &&
                    this.LastUpdateDate.Equals(other.LastUpdateDate)
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
                
                if (this.LastUpdateDate != null)
                    hash = hash * 57 + this.LastUpdateDate.GetHashCode();
                
                return hash;
            }
        }

    }


}
