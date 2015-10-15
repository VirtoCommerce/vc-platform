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
  public class VirtoCommercePlatformWebModelNotificationsNotification {
    
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
    /// Max sending attempt count, if MaxAttemptCount less or equal AttemptCount IsActive = false and IsSent = false, notification stop sending
    /// </summary>
    /// <value>Max sending attempt count, if MaxAttemptCount less or equal AttemptCount IsActive = false and IsSent = false, notification stop sending</value>
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
