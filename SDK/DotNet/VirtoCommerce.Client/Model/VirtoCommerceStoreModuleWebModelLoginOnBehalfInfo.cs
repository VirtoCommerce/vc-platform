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
    /// Represent result for checking of possibility login on behalf request
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo :  IEquatable<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo>
    {
        /// <summary>
        /// Gets or Sets UserName
        /// </summary>
        [DataMember(Name="userName", EmitDefaultValue=false)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or Sets CanLoginOnBehalf
        /// </summary>
        [DataMember(Name="canLoginOnBehalf", EmitDefaultValue=false)]
        public bool? CanLoginOnBehalf { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo {\n");
            sb.Append("  UserName: ").Append(UserName).Append("\n");
            sb.Append("  CanLoginOnBehalf: ").Append(CanLoginOnBehalf).Append("\n");
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
            return this.Equals(obj as VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo);
        }

        /// <summary>
        /// Returns true if VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.UserName == other.UserName ||
                    this.UserName != null &&
                    this.UserName.Equals(other.UserName)
                ) && 
                (
                    this.CanLoginOnBehalf == other.CanLoginOnBehalf ||
                    this.CanLoginOnBehalf != null &&
                    this.CanLoginOnBehalf.Equals(other.CanLoginOnBehalf)
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

                if (this.UserName != null)
                    hash = hash * 59 + this.UserName.GetHashCode();

                if (this.CanLoginOnBehalf != null)
                    hash = hash * 59 + this.CanLoginOnBehalf.GetHashCode();

                return hash;
            }
        }

    }
}
