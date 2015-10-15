using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult {
    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult {\n");
      
      sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
      
      sb.Append("  NewCount: ").Append(NewCount).Append("\n");
      
      sb.Append("  NotifyEvents: ").Append(NotifyEvents).Append("\n");
      
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
