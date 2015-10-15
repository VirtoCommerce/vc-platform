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
  public class VirtoCommercePlatformWebModelNotificationsNotificationTemplate {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or Sets Body
    /// </summary>
    [DataMember(Name="body", EmitDefaultValue=false)]
    public string Body { get; set; }

    
    /// <summary>
    /// Gets or Sets Subject
    /// </summary>
    [DataMember(Name="subject", EmitDefaultValue=false)]
    public string Subject { get; set; }

    
    /// <summary>
    /// Gets or Sets NotificationTypeId
    /// </summary>
    [DataMember(Name="notificationTypeId", EmitDefaultValue=false)]
    public string NotificationTypeId { get; set; }

    
    /// <summary>
    /// Id of object, that used this template for sending notification
    /// </summary>
    /// <value>Id of object, that used this template for sending notification</value>
    [DataMember(Name="objectId", EmitDefaultValue=false)]
    public string ObjectId { get; set; }

    
    /// <summary>
    /// Type id of object, that used this template for sending notification
    /// </summary>
    /// <value>Type id of object, that used this template for sending notification</value>
    [DataMember(Name="objectTypeId", EmitDefaultValue=false)]
    public string ObjectTypeId { get; set; }

    
    /// <summary>
    /// Locale of template
    /// </summary>
    /// <value>Locale of template</value>
    [DataMember(Name="language", EmitDefaultValue=false)]
    public string Language { get; set; }

    
    /// <summary>
    /// Flag, that shows if this template is default dor notification type
    /// </summary>
    /// <value>Flag, that shows if this template is default dor notification type</value>
    [DataMember(Name="isDefault", EmitDefaultValue=false)]
    public bool? IsDefault { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformWebModelNotificationsNotificationTemplate {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Body: ").Append(Body).Append("\n");
      
      sb.Append("  Subject: ").Append(Subject).Append("\n");
      
      sb.Append("  NotificationTypeId: ").Append(NotificationTypeId).Append("\n");
      
      sb.Append("  ObjectId: ").Append(ObjectId).Append("\n");
      
      sb.Append("  ObjectTypeId: ").Append(ObjectTypeId).Append("\n");
      
      sb.Append("  Language: ").Append(Language).Append("\n");
      
      sb.Append("  IsDefault: ").Append(IsDefault).Append("\n");
      
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
