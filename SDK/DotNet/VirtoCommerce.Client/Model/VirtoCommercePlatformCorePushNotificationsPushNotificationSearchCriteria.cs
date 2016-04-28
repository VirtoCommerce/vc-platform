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
    public partial class VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria :  IEquatable<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria>
    {
        /// <summary>
        /// Gets or Sets Ids
        /// </summary>
        [DataMember(Name="ids", EmitDefaultValue=false)]
        public List<string> Ids { get; set; }

        /// <summary>
        /// Gets or Sets OnlyNew
        /// </summary>
        [DataMember(Name="onlyNew", EmitDefaultValue=false)]
        public bool? OnlyNew { get; set; }

        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        [DataMember(Name="startDate", EmitDefaultValue=false)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        [DataMember(Name="endDate", EmitDefaultValue=false)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or Sets Start
        /// </summary>
        [DataMember(Name="start", EmitDefaultValue=false)]
        public int? Start { get; set; }

        /// <summary>
        /// Gets or Sets Count
        /// </summary>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }

        /// <summary>
        /// Gets or Sets OrderBy
        /// </summary>
        [DataMember(Name="orderBy", EmitDefaultValue=false)]
        public string OrderBy { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria {\n");
            sb.Append("  Ids: ").Append(Ids).Append("\n");
            sb.Append("  OnlyNew: ").Append(OnlyNew).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  Start: ").Append(Start).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  OrderBy: ").Append(OrderBy).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Ids == other.Ids ||
                    this.Ids != null &&
                    this.Ids.SequenceEqual(other.Ids)
                ) && 
                (
                    this.OnlyNew == other.OnlyNew ||
                    this.OnlyNew != null &&
                    this.OnlyNew.Equals(other.OnlyNew)
                ) && 
                (
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) && 
                (
                    this.EndDate == other.EndDate ||
                    this.EndDate != null &&
                    this.EndDate.Equals(other.EndDate)
                ) && 
                (
                    this.Start == other.Start ||
                    this.Start != null &&
                    this.Start.Equals(other.Start)
                ) && 
                (
                    this.Count == other.Count ||
                    this.Count != null &&
                    this.Count.Equals(other.Count)
                ) && 
                (
                    this.OrderBy == other.OrderBy ||
                    this.OrderBy != null &&
                    this.OrderBy.Equals(other.OrderBy)
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

                if (this.Ids != null)
                    hash = hash * 59 + this.Ids.GetHashCode();

                if (this.OnlyNew != null)
                    hash = hash * 59 + this.OnlyNew.GetHashCode();

                if (this.StartDate != null)
                    hash = hash * 59 + this.StartDate.GetHashCode();

                if (this.EndDate != null)
                    hash = hash * 59 + this.EndDate.GetHashCode();

                if (this.Start != null)
                    hash = hash * 59 + this.Start.GetHashCode();

                if (this.Count != null)
                    hash = hash * 59 + this.Count.GetHashCode();

                if (this.OrderBy != null)
                    hash = hash * 59 + this.OrderBy.GetHashCode();

                return hash;
            }
        }

    }
}
