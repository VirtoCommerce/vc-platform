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
    public partial class VirtoCommercePlatformWebModelPackagingProgressMessage :  IEquatable<VirtoCommercePlatformWebModelPackagingProgressMessage>
    {
        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name="message", EmitDefaultValue=false)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or Sets Level
        /// </summary>
        [DataMember(Name="level", EmitDefaultValue=false)]
        public string Level { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformWebModelPackagingProgressMessage {\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  Level: ").Append(Level).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformWebModelPackagingProgressMessage);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformWebModelPackagingProgressMessage instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformWebModelPackagingProgressMessage to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformWebModelPackagingProgressMessage other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Message == other.Message ||
                    this.Message != null &&
                    this.Message.Equals(other.Message)
                ) && 
                (
                    this.Level == other.Level ||
                    this.Level != null &&
                    this.Level.Equals(other.Level)
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

                if (this.Message != null)
                    hash = hash * 59 + this.Message.GetHashCode();

                if (this.Level != null)
                    hash = hash * 59 + this.Level.GetHashCode();

                return hash;
            }
        }

    }
}
