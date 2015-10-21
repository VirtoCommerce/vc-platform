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
  public class VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult {
    
    /// <summary>
    /// Subject
    /// </summary>
    /// <value>Subject</value>
    [DataMember(Name="subject", EmitDefaultValue=false)]
    public string Subject { get; set; }

    
    /// <summary>
    /// Gets or Sets Body
    /// </summary>
    [DataMember(Name="body", EmitDefaultValue=false)]
    public string Body { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult {\n");
      
      sb.Append("  Subject: ").Append(Subject).Append("\n");
      
      sb.Append("  Body: ").Append(Body).Append("\n");
      
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
