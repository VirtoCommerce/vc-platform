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
    /// Notification for catalog data export job.
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification :  IEquatable<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>
    {
        /// <summary>
        /// Gets or sets the URL for downloading exported data.
        /// </summary>
        /// <value>Gets or sets the URL for downloading exported data.</value>
        [DataMember(Name="downloadUrl", EmitDefaultValue=false)]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Gets or sets the job finish date and time.
        /// </summary>
        /// <value>Gets or sets the job finish date and time.</value>
        [DataMember(Name="finished", EmitDefaultValue=false)]
        public DateTime? Finished { get; set; }

        /// <summary>
        /// Gets or sets the total count of objects to process.
        /// </summary>
        /// <value>Gets or sets the total count of objects to process.</value>
        [DataMember(Name="totalCount", EmitDefaultValue=false)]
        public long? TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the count of processed objects.
        /// </summary>
        /// <value>Gets or sets the count of processed objects.</value>
        [DataMember(Name="processedCount", EmitDefaultValue=false)]
        public long? ProcessedCount { get; set; }

        /// <summary>
        /// Gets or sets the count of errors during processing.
        /// </summary>
        /// <value>Gets or sets the count of errors during processing.</value>
        [DataMember(Name="errorCount", EmitDefaultValue=false)]
        public long? ErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the errors that has occurred during processing.
        /// </summary>
        /// <value>Gets or sets the errors that has occurred during processing.</value>
        [DataMember(Name="errors", EmitDefaultValue=false)]
        public List<string> Errors { get; set; }

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
            sb.Append("class VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification {\n");
            sb.Append("  DownloadUrl: ").Append(DownloadUrl).Append("\n");
            sb.Append("  Finished: ").Append(Finished).Append("\n");
            sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
            sb.Append("  ProcessedCount: ").Append(ProcessedCount).Append("\n");
            sb.Append("  ErrorCount: ").Append(ErrorCount).Append("\n");
            sb.Append("  Errors: ").Append(Errors).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.DownloadUrl == other.DownloadUrl ||
                    this.DownloadUrl != null &&
                    this.DownloadUrl.Equals(other.DownloadUrl)
                ) && 
                (
                    this.Finished == other.Finished ||
                    this.Finished != null &&
                    this.Finished.Equals(other.Finished)
                ) && 
                (
                    this.TotalCount == other.TotalCount ||
                    this.TotalCount != null &&
                    this.TotalCount.Equals(other.TotalCount)
                ) && 
                (
                    this.ProcessedCount == other.ProcessedCount ||
                    this.ProcessedCount != null &&
                    this.ProcessedCount.Equals(other.ProcessedCount)
                ) && 
                (
                    this.ErrorCount == other.ErrorCount ||
                    this.ErrorCount != null &&
                    this.ErrorCount.Equals(other.ErrorCount)
                ) && 
                (
                    this.Errors == other.Errors ||
                    this.Errors != null &&
                    this.Errors.SequenceEqual(other.Errors)
                ) && 
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

                if (this.DownloadUrl != null)
                    hash = hash * 59 + this.DownloadUrl.GetHashCode();

                if (this.Finished != null)
                    hash = hash * 59 + this.Finished.GetHashCode();

                if (this.TotalCount != null)
                    hash = hash * 59 + this.TotalCount.GetHashCode();

                if (this.ProcessedCount != null)
                    hash = hash * 59 + this.ProcessedCount.GetHashCode();

                if (this.ErrorCount != null)
                    hash = hash * 59 + this.ErrorCount.GetHashCode();

                if (this.Errors != null)
                    hash = hash * 59 + this.Errors.GetHashCode();

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
