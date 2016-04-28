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
    public partial class VirtoCommercePlatformWebModelNotificationsTestNotificationRequest :  IEquatable<VirtoCommercePlatformWebModelNotificationsTestNotificationRequest>
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets ObjectId
        /// </summary>
        [DataMember(Name="objectId", EmitDefaultValue=false)]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or Sets ObjectTypeId
        /// </summary>
        [DataMember(Name="objectTypeId", EmitDefaultValue=false)]
        public string ObjectTypeId { get; set; }

        /// <summary>
        /// Gets or Sets Language
        /// </summary>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public string Language { get; set; }

        /// <summary>
        /// Gets or Sets NotificationParameters
        /// </summary>
        [DataMember(Name="notificationParameters", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformWebModelNotificationsNotificationParameter> NotificationParameters { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformWebModelNotificationsTestNotificationRequest {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  ObjectId: ").Append(ObjectId).Append("\n");
            sb.Append("  ObjectTypeId: ").Append(ObjectTypeId).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  NotificationParameters: ").Append(NotificationParameters).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformWebModelNotificationsTestNotificationRequest);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformWebModelNotificationsTestNotificationRequest instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformWebModelNotificationsTestNotificationRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformWebModelNotificationsTestNotificationRequest other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.ObjectId == other.ObjectId ||
                    this.ObjectId != null &&
                    this.ObjectId.Equals(other.ObjectId)
                ) && 
                (
                    this.ObjectTypeId == other.ObjectTypeId ||
                    this.ObjectTypeId != null &&
                    this.ObjectTypeId.Equals(other.ObjectTypeId)
                ) && 
                (
                    this.Language == other.Language ||
                    this.Language != null &&
                    this.Language.Equals(other.Language)
                ) && 
                (
                    this.NotificationParameters == other.NotificationParameters ||
                    this.NotificationParameters != null &&
                    this.NotificationParameters.SequenceEqual(other.NotificationParameters)
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

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.ObjectId != null)
                    hash = hash * 59 + this.ObjectId.GetHashCode();

                if (this.ObjectTypeId != null)
                    hash = hash * 59 + this.ObjectTypeId.GetHashCode();

                if (this.Language != null)
                    hash = hash * 59 + this.Language.GetHashCode();

                if (this.NotificationParameters != null)
                    hash = hash * 59 + this.NotificationParameters.GetHashCode();

                return hash;
            }
        }

    }
}
