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
    public class VirtoCommercePlatformCoreSecurityRoleSearchResponse : IEquatable<VirtoCommercePlatformCoreSecurityRoleSearchResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommercePlatformCoreSecurityRoleSearchResponse" /> class.
        /// </summary>
        public VirtoCommercePlatformCoreSecurityRoleSearchResponse()
        {
            
        }

        
        /// <summary>
        /// Gets or Sets Roles
        /// </summary>
        [DataMember(Name="roles", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreSecurityRole> Roles { get; set; }
  
        
        /// <summary>
        /// Gets or Sets TotalCount
        /// </summary>
        [DataMember(Name="totalCount", EmitDefaultValue=false)]
        public int? TotalCount { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCoreSecurityRoleSearchResponse {\n");
            sb.Append("  Roles: ").Append(Roles).Append("\n");
            sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
            
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
            return this.Equals(obj as VirtoCommercePlatformCoreSecurityRoleSearchResponse);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreSecurityRoleSearchResponse instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommercePlatformCoreSecurityRoleSearchResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreSecurityRoleSearchResponse other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Roles == other.Roles ||
                    this.Roles != null &&
                    this.Roles.SequenceEqual(other.Roles)
                ) && 
                (
                    this.TotalCount == other.TotalCount ||
                    this.TotalCount != null &&
                    this.TotalCount.Equals(other.TotalCount)
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
                
                if (this.Roles != null)
                    hash = hash * 57 + this.Roles.GetHashCode();
                
                if (this.TotalCount != null)
                    hash = hash * 57 + this.TotalCount.GetHashCode();
                
                return hash;
            }
        }

    }


}
