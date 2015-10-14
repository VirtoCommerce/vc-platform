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
  public class VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria {
    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
