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
    public partial class VirtoCommercePlatformWebModelNotificationsNotificationTemplate :  IEquatable<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Body
        /// </summary>
        [DataMember(Name="body", EmitDefaultValue=false)]
        public string Body { get; set; }

        /// <summary>
        /// Gets or Sets Subject
        /// </summary>
        [DataMember(Name="subject", EmitDefaultValue=false)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or Sets NotificationTypeId
        /// </summary>
        [DataMember(Name="notificationTypeId", EmitDefaultValue=false)]
        public string NotificationTypeId { get; set; }

        /// <summary>
        /// Id of object, that used this template for sending notification
        /// </summary>
        /// <value>Id of object, that used this template for sending notification</value>
        [DataMember(Name="objectId", EmitDefaultValue=false)]
        public string ObjectId { get; set; }

        /// <summary>
        /// Type id of object, that used this template for sending notification
        /// </summary>
        /// <value>Type id of object, that used this template for sending notification</value>
        [DataMember(Name="objectTypeId", EmitDefaultValue=false)]
        public string ObjectTypeId { get; set; }

        /// <summary>
        /// Locale of template
        /// </summary>
        /// <value>Locale of template</value>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public string Language { get; set; }

        /// <summary>
        /// Flag, that shows if this template is default dor notification type
        /// </summary>
        /// <value>Flag, that shows if this template is default dor notification type</value>
        [DataMember(Name="isDefault", EmitDefaultValue=false)]
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformWebModelNotificationsNotificationTemplate {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Body: ").Append(Body).Append("\n");
            sb.Append("  Subject: ").Append(Subject).Append("\n");
            sb.Append("  NotificationTypeId: ").Append(NotificationTypeId).Append("\n");
            sb.Append("  ObjectId: ").Append(ObjectId).Append("\n");
            sb.Append("  ObjectTypeId: ").Append(ObjectTypeId).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  IsDefault: ").Append(IsDefault).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformWebModelNotificationsNotificationTemplate);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformWebModelNotificationsNotificationTemplate instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformWebModelNotificationsNotificationTemplate to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformWebModelNotificationsNotificationTemplate other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.Body == other.Body ||
                    this.Body != null &&
                    this.Body.Equals(other.Body)
                ) && 
                (
                    this.Subject == other.Subject ||
                    this.Subject != null &&
                    this.Subject.Equals(other.Subject)
                ) && 
                (
                    this.NotificationTypeId == other.NotificationTypeId ||
                    this.NotificationTypeId != null &&
                    this.NotificationTypeId.Equals(other.NotificationTypeId)
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
                    this.IsDefault == other.IsDefault ||
                    this.IsDefault != null &&
                    this.IsDefault.Equals(other.IsDefault)
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

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                if (this.Body != null)
                    hash = hash * 59 + this.Body.GetHashCode();

                if (this.Subject != null)
                    hash = hash * 59 + this.Subject.GetHashCode();

                if (this.NotificationTypeId != null)
                    hash = hash * 59 + this.NotificationTypeId.GetHashCode();

                if (this.ObjectId != null)
                    hash = hash * 59 + this.ObjectId.GetHashCode();

                if (this.ObjectTypeId != null)
                    hash = hash * 59 + this.ObjectTypeId.GetHashCode();

                if (this.Language != null)
                    hash = hash * 59 + this.Language.GetHashCode();

                if (this.IsDefault != null)
                    hash = hash * 59 + this.IsDefault.GetHashCode();

                return hash;
            }
        }

    }
}
