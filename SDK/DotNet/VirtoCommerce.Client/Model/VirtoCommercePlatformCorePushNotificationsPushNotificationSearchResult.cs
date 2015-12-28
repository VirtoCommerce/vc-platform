using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult : IEquatable<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult" /> class.
        /// </summary>
        public VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult()
        {
            
        }

        
        /// <summary>
        /// Gets or Sets TotalCount
        /// </summary>
        [DataMember(Name="totalCount", EmitDefaultValue=false)]
        public int? TotalCount { get; set; }
  
        
        /// <summary>
        /// Gets or Sets NewCount
        /// </summary>
        [DataMember(Name="newCount", EmitDefaultValue=false)]
        public int? NewCount { get; set; }
  
        
        /// <summary>
        /// Gets or Sets NotifyEvents
        /// </summary>
        [DataMember(Name="notifyEvents", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCorePushNotificationsPushNotification> NotifyEvents { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult {\n");
            sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
            sb.Append("  NewCount: ").Append(NewCount).Append("\n");
            sb.Append("  NotifyEvents: ").Append(NotifyEvents).Append("\n");
            
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
            return this.Equals(obj as VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.TotalCount == other.TotalCount ||
                    this.TotalCount != null &&
                    this.TotalCount.Equals(other.TotalCount)
                ) && 
                (
                    this.NewCount == other.NewCount ||
                    this.NewCount != null &&
                    this.NewCount.Equals(other.NewCount)
                ) && 
                (
                    this.NotifyEvents == other.NotifyEvents ||
                    this.NotifyEvents != null &&
                    this.NotifyEvents.SequenceEqual(other.NotifyEvents)
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
                
                if (this.TotalCount != null)
                    hash = hash * 57 + this.TotalCount.GetHashCode();
                
                if (this.NewCount != null)
                    hash = hash * 57 + this.NewCount.GetHashCode();
                
                if (this.NotifyEvents != null)
                    hash = hash * 57 + this.NotifyEvents.GetHashCode();
                
                return hash;
            }
        }

    }


}
