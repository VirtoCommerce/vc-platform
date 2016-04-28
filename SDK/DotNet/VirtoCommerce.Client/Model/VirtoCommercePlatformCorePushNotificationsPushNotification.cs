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
    public partial class VirtoCommercePlatformCorePushNotificationsPushNotification :  IEquatable<VirtoCommercePlatformCorePushNotificationsPushNotification>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Creator
        /// </summary>
        [DataMember(Name="creator", EmitDefaultValue=false)]
        public string Creator { get; set; }

        /// <summary>
        /// Gets or Sets Created
        /// </summary>
        [DataMember(Name="created", EmitDefaultValue=false)]
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or Sets IsNew
        /// </summary>
        [DataMember(Name="isNew", EmitDefaultValue=false)]
        public bool? IsNew { get; set; }

        /// <summary>
        /// Gets or Sets NotifyType
        /// </summary>
        [DataMember(Name="notifyType", EmitDefaultValue=false)]
        public string NotifyType { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [DataMember(Name="title", EmitDefaultValue=false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets RepeatCount
        /// </summary>
        [DataMember(Name="repeatCount", EmitDefaultValue=false)]
        public int? RepeatCount { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCorePushNotificationsPushNotification {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Creator: ").Append(Creator).Append("\n");
            sb.Append("  Created: ").Append(Created).Append("\n");
            sb.Append("  IsNew: ").Append(IsNew).Append("\n");
            sb.Append("  NotifyType: ").Append(NotifyType).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  RepeatCount: ").Append(RepeatCount).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformCorePushNotificationsPushNotification);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCorePushNotificationsPushNotification instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCorePushNotificationsPushNotification to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCorePushNotificationsPushNotification other)
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
                    this.Creator == other.Creator ||
                    this.Creator != null &&
                    this.Creator.Equals(other.Creator)
                ) && 
                (
                    this.Created == other.Created ||
                    this.Created != null &&
                    this.Created.Equals(other.Created)
                ) && 
                (
                    this.IsNew == other.IsNew ||
                    this.IsNew != null &&
                    this.IsNew.Equals(other.IsNew)
                ) && 
                (
                    this.NotifyType == other.NotifyType ||
                    this.NotifyType != null &&
                    this.NotifyType.Equals(other.NotifyType)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.Title == other.Title ||
                    this.Title != null &&
                    this.Title.Equals(other.Title)
                ) && 
                (
                    this.RepeatCount == other.RepeatCount ||
                    this.RepeatCount != null &&
                    this.RepeatCount.Equals(other.RepeatCount)
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

                if (this.Creator != null)
                    hash = hash * 59 + this.Creator.GetHashCode();

                if (this.Created != null)
                    hash = hash * 59 + this.Created.GetHashCode();

                if (this.IsNew != null)
                    hash = hash * 59 + this.IsNew.GetHashCode();

                if (this.NotifyType != null)
                    hash = hash * 59 + this.NotifyType.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.Title != null)
                    hash = hash * 59 + this.Title.GetHashCode();

                if (this.RepeatCount != null)
                    hash = hash * 59 + this.RepeatCount.GetHashCode();

                return hash;
            }
        }

    }
}
