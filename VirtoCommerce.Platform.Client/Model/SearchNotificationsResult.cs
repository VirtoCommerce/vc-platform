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

namespace VirtoCommerce.Platform.Client.Model
{
    /// <summary>
    /// SearchNotificationsResult
    /// </summary>
    [DataContract]
    public partial class SearchNotificationsResult :  IEquatable<SearchNotificationsResult>
    {
        /// <summary>
        /// Gets or Sets Notifications
        /// </summary>
        [DataMember(Name="notifications", EmitDefaultValue=false)]
        public List<Notification> Notifications { get; set; }

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
            sb.Append("class SearchNotificationsResult {\n");
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
            return this.Equals(obj as SearchNotificationsResult);
        }

        /// <summary>
        /// Returns true if SearchNotificationsResult instances are equal
        /// </summary>
        /// <param name="other">Instance of SearchNotificationsResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SearchNotificationsResult other)
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
