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
    public partial class VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult :  IEquatable<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>
    {
        /// <summary>
        /// Gets or Sets Notifications
        /// </summary>
        [DataMember(Name="notifications", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformWebModelNotificationsNotification> Notifications { get; set; }

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
            sb.Append("class VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult {\n");
            sb.Append("  Notifications: ").Append(Notifications).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Notifications == other.Notifications ||
                    this.Notifications != null &&
                    this.Notifications.SequenceEqual(other.Notifications)
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

                if (this.Notifications != null)
                    hash = hash * 59 + this.Notifications.GetHashCode();

                if (this.TotalCount != null)
                    hash = hash * 59 + this.TotalCount.GetHashCode();

                return hash;
            }
        }

    }
}
