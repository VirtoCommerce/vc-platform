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
  public class VirtoCommercePlatformWebModelNotificationsTestNotificationRequest {
    
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
    public Dictionary<string, Object> NotificationParameters { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
