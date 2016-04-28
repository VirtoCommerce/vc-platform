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
    public partial class VirtoCommercePlatformCoreSecuritySecurityResult :  IEquatable<VirtoCommercePlatformCoreSecuritySecurityResult>
    {
        /// <summary>
        /// Gets or Sets Succeeded
        /// </summary>
        [DataMember(Name="succeeded", EmitDefaultValue=false)]
        public bool? Succeeded { get; set; }

        /// <summary>
        /// Gets or Sets Errors
        /// </summary>
        [DataMember(Name="errors", EmitDefaultValue=false)]
        public List<string> Errors { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCoreSecuritySecurityResult {\n");
            sb.Append("  Succeeded: ").Append(Succeeded).Append("\n");
            sb.Append("  Errors: ").Append(Errors).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformCoreSecuritySecurityResult);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreSecuritySecurityResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCoreSecuritySecurityResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreSecuritySecurityResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Succeeded == other.Succeeded ||
                    this.Succeeded != null &&
                    this.Succeeded.Equals(other.Succeeded)
                ) && 
                (
                    this.Errors == other.Errors ||
                    this.Errors != null &&
                    this.Errors.SequenceEqual(other.Errors)
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

                if (this.Succeeded != null)
                    hash = hash * 59 + this.Succeeded.GetHashCode();

                if (this.Errors != null)
                    hash = hash * 59 + this.Errors.GetHashCode();

                return hash;
            }
        }

    }
}
