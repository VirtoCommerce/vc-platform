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
    public partial class VirtoCommercePlatformWebModelNotificationsNotification :  IEquatable<VirtoCommercePlatformWebModelNotificationsNotification>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets DisplayName
        /// </summary>
        [DataMember(Name="displayName", EmitDefaultValue=false)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets IsEmail
        /// </summary>
        [DataMember(Name="isEmail", EmitDefaultValue=false)]
        public bool? IsEmail { get; set; }

        /// <summary>
        /// Gets or Sets IsSms
        /// </summary>
        [DataMember(Name="isSms", EmitDefaultValue=false)]
        public bool? IsSms { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets IsActive
        /// </summary>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or Sets IsSuccessSend
        /// </summary>
        [DataMember(Name="isSuccessSend", EmitDefaultValue=false)]
        public bool? IsSuccessSend { get; set; }

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
        /// Type of notificaiton sending gateway
        /// </summary>
        /// <value>Type of notificaiton sending gateway</value>
        [DataMember(Name="sendingGateway", EmitDefaultValue=false)]
        public string SendingGateway { get; set; }

        /// <summary>
        /// Gets or Sets Subject
        /// </summary>
        [DataMember(Name="subject", EmitDefaultValue=false)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or Sets Body
        /// </summary>
        [DataMember(Name="body", EmitDefaultValue=false)]
        public string Body { get; set; }

        /// <summary>
        /// Gets or Sets Sender
        /// </summary>
        [DataMember(Name="sender", EmitDefaultValue=false)]
        public string Sender { get; set; }

        /// <summary>
        /// Gets or Sets Recipient
        /// </summary>
        [DataMember(Name="recipient", EmitDefaultValue=false)]
        public string Recipient { get; set; }

        /// <summary>
        /// Sending attempts count
        /// </summary>
        /// <value>Sending attempts count</value>
        [DataMember(Name="attemptCount", EmitDefaultValue=false)]
        public int? AttemptCount { get; set; }

        /// <summary>
        /// Max sending attempt count, if MaxAttemptCount less or equal AttemptCount IsActive &#x3D; false and IsSent &#x3D; false, notification stop sending
        /// </summary>
        /// <value>Max sending attempt count, if MaxAttemptCount less or equal AttemptCount IsActive &#x3D; false and IsSent &#x3D; false, notification stop sending</value>
        [DataMember(Name="maxAttemptCount", EmitDefaultValue=false)]
        public int? MaxAttemptCount { get; set; }

        /// <summary>
        /// Last fail sending attempt error message
        /// </summary>
        /// <value>Last fail sending attempt error message</value>
        [DataMember(Name="lastFailAttemptMessage", EmitDefaultValue=false)]
        public string LastFailAttemptMessage { get; set; }

        /// <summary>
        /// Last fail sending attempt date
        /// </summary>
        /// <value>Last fail sending attempt date</value>
        [DataMember(Name="lastFailAttemptDate", EmitDefaultValue=false)]
        public DateTime? LastFailAttemptDate { get; set; }

        /// <summary>
        /// Start sending date, if not null notification will be sending after that date
        /// </summary>
        /// <value>Start sending date, if not null notification will be sending after that date</value>
        [DataMember(Name="startSendingDate", EmitDefaultValue=false)]
        public DateTime? StartSendingDate { get; set; }

        /// <summary>
        /// Gets or Sets SentDate
        /// </summary>
        [DataMember(Name="sentDate", EmitDefaultValue=false)]
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformWebModelNotificationsNotification {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  IsEmail: ").Append(IsEmail).Append("\n");
            sb.Append("  IsSms: ").Append(IsSms).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  IsSuccessSend: ").Append(IsSuccessSend).Append("\n");
            sb.Append("  ObjectId: ").Append(ObjectId).Append("\n");
            sb.Append("  ObjectTypeId: ").Append(ObjectTypeId).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  SendingGateway: ").Append(SendingGateway).Append("\n");
            sb.Append("  Subject: ").Append(Subject).Append("\n");
            sb.Append("  Body: ").Append(Body).Append("\n");
            sb.Append("  Sender: ").Append(Sender).Append("\n");
            sb.Append("  Recipient: ").Append(Recipient).Append("\n");
            sb.Append("  AttemptCount: ").Append(AttemptCount).Append("\n");
            sb.Append("  MaxAttemptCount: ").Append(MaxAttemptCount).Append("\n");
            sb.Append("  LastFailAttemptMessage: ").Append(LastFailAttemptMessage).Append("\n");
            sb.Append("  LastFailAttemptDate: ").Append(LastFailAttemptDate).Append("\n");
            sb.Append("  StartSendingDate: ").Append(StartSendingDate).Append("\n");
            sb.Append("  SentDate: ").Append(SentDate).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformWebModelNotificationsNotification);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformWebModelNotificationsNotification instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformWebModelNotificationsNotification to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformWebModelNotificationsNotification other)
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
                    this.DisplayName == other.DisplayName ||
                    this.DisplayName != null &&
                    this.DisplayName.Equals(other.DisplayName)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.IsEmail == other.IsEmail ||
                    this.IsEmail != null &&
                    this.IsEmail.Equals(other.IsEmail)
                ) && 
                (
                    this.IsSms == other.IsSms ||
                    this.IsSms != null &&
                    this.IsSms.Equals(other.IsSms)
                ) && 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.IsActive == other.IsActive ||
                    this.IsActive != null &&
                    this.IsActive.Equals(other.IsActive)
                ) && 
                (
                    this.IsSuccessSend == other.IsSuccessSend ||
                    this.IsSuccessSend != null &&
                    this.IsSuccessSend.Equals(other.IsSuccessSend)
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
                    this.SendingGateway == other.SendingGateway ||
                    this.SendingGateway != null &&
                    this.SendingGateway.Equals(other.SendingGateway)
                ) && 
                (
                    this.Subject == other.Subject ||
                    this.Subject != null &&
                    this.Subject.Equals(other.Subject)
                ) && 
                (
                    this.Body == other.Body ||
                    this.Body != null &&
                    this.Body.Equals(other.Body)
                ) && 
                (
                    this.Sender == other.Sender ||
                    this.Sender != null &&
                    this.Sender.Equals(other.Sender)
                ) && 
                (
                    this.Recipient == other.Recipient ||
                    this.Recipient != null &&
                    this.Recipient.Equals(other.Recipient)
                ) && 
                (
                    this.AttemptCount == other.AttemptCount ||
                    this.AttemptCount != null &&
                    this.AttemptCount.Equals(other.AttemptCount)
                ) && 
                (
                    this.MaxAttemptCount == other.MaxAttemptCount ||
                    this.MaxAttemptCount != null &&
                    this.MaxAttemptCount.Equals(other.MaxAttemptCount)
                ) && 
                (
                    this.LastFailAttemptMessage == other.LastFailAttemptMessage ||
                    this.LastFailAttemptMessage != null &&
                    this.LastFailAttemptMessage.Equals(other.LastFailAttemptMessage)
                ) && 
                (
                    this.LastFailAttemptDate == other.LastFailAttemptDate ||
                    this.LastFailAttemptDate != null &&
                    this.LastFailAttemptDate.Equals(other.LastFailAttemptDate)
                ) && 
                (
                    this.StartSendingDate == other.StartSendingDate ||
                    this.StartSendingDate != null &&
                    this.StartSendingDate.Equals(other.StartSendingDate)
                ) && 
                (
                    this.SentDate == other.SentDate ||
                    this.SentDate != null &&
                    this.SentDate.Equals(other.SentDate)
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

                if (this.DisplayName != null)
                    hash = hash * 59 + this.DisplayName.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.IsEmail != null)
                    hash = hash * 59 + this.IsEmail.GetHashCode();

                if (this.IsSms != null)
                    hash = hash * 59 + this.IsSms.GetHashCode();

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.IsActive != null)
                    hash = hash * 59 + this.IsActive.GetHashCode();

                if (this.IsSuccessSend != null)
                    hash = hash * 59 + this.IsSuccessSend.GetHashCode();

                if (this.ObjectId != null)
                    hash = hash * 59 + this.ObjectId.GetHashCode();

                if (this.ObjectTypeId != null)
                    hash = hash * 59 + this.ObjectTypeId.GetHashCode();

                if (this.Language != null)
                    hash = hash * 59 + this.Language.GetHashCode();

                if (this.SendingGateway != null)
                    hash = hash * 59 + this.SendingGateway.GetHashCode();

                if (this.Subject != null)
                    hash = hash * 59 + this.Subject.GetHashCode();

                if (this.Body != null)
                    hash = hash * 59 + this.Body.GetHashCode();

                if (this.Sender != null)
                    hash = hash * 59 + this.Sender.GetHashCode();

                if (this.Recipient != null)
                    hash = hash * 59 + this.Recipient.GetHashCode();

                if (this.AttemptCount != null)
                    hash = hash * 59 + this.AttemptCount.GetHashCode();

                if (this.MaxAttemptCount != null)
                    hash = hash * 59 + this.MaxAttemptCount.GetHashCode();

                if (this.LastFailAttemptMessage != null)
                    hash = hash * 59 + this.LastFailAttemptMessage.GetHashCode();

                if (this.LastFailAttemptDate != null)
                    hash = hash * 59 + this.LastFailAttemptDate.GetHashCode();

                if (this.StartSendingDate != null)
                    hash = hash * 59 + this.StartSendingDate.GetHashCode();

                if (this.SentDate != null)
                    hash = hash * 59 + this.SentDate.GetHashCode();

                return hash;
            }
        }

    }
}
