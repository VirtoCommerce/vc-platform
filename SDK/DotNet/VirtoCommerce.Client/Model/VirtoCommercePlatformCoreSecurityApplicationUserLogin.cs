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
    public partial class VirtoCommercePlatformCoreSecurityApplicationUserLogin :  IEquatable<VirtoCommercePlatformCoreSecurityApplicationUserLogin>
    {
        /// <summary>
        /// Gets or Sets LoginProvider
        /// </summary>
        [DataMember(Name="loginProvider", EmitDefaultValue=false)]
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or Sets ProviderKey
        /// </summary>
        [DataMember(Name="providerKey", EmitDefaultValue=false)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCoreSecurityApplicationUserLogin {\n");
            sb.Append("  LoginProvider: ").Append(LoginProvider).Append("\n");
            sb.Append("  ProviderKey: ").Append(ProviderKey).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformCoreSecurityApplicationUserLogin);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreSecurityApplicationUserLogin instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCoreSecurityApplicationUserLogin to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreSecurityApplicationUserLogin other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.LoginProvider == other.LoginProvider ||
                    this.LoginProvider != null &&
                    this.LoginProvider.Equals(other.LoginProvider)
                ) && 
                (
                    this.ProviderKey == other.ProviderKey ||
                    this.ProviderKey != null &&
                    this.ProviderKey.Equals(other.ProviderKey)
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

                if (this.LoginProvider != null)
                    hash = hash * 59 + this.LoginProvider.GetHashCode();

                if (this.ProviderKey != null)
                    hash = hash * 59 + this.ProviderKey.GetHashCode();

                return hash;
            }
        }

    }
}
