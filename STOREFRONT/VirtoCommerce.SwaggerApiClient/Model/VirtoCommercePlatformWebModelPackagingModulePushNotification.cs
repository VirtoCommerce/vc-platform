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
  public class VirtoCommercePlatformWebModelPackagingModulePushNotification {
    
    /// <summary>
    /// Gets or Sets ProgressLog
    /// </summary>
    [DataMember(Name="progressLog", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformWebModelPackagingProgressMessage> ProgressLog { get; set; }

    
    /// <summary>
    /// Gets or Sets Started
    /// </summary>
    [DataMember(Name="started", EmitDefaultValue=false)]
    public DateTime? Started { get; set; }

    
    /// <summary>
    /// Gets or Sets Finished
    /// </summary>
    [DataMember(Name="finished", EmitDefaultValue=false)]
    public DateTime? Finished { get; set; }

    
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
    /// Gets or Sets New
    /// </summary>
    [DataMember(Name="new", EmitDefaultValue=false)]
    public bool? New { get; set; }

    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformWebModelPackagingModulePushNotification {\n");
      
      sb.Append("  ProgressLog: ").Append(ProgressLog).Append("\n");
      
      sb.Append("  Started: ").Append(Started).Append("\n");
      
      sb.Append("  Finished: ").Append(Finished).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Creator: ").Append(Creator).Append("\n");
      
      sb.Append("  Created: ").Append(Created).Append("\n");
      
      sb.Append("  New: ").Append(New).Append("\n");
      
      sb.Append("  NotifyType: ").Append(NotifyType).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  Title: ").Append(Title).Append("\n");
      
      sb.Append("  RepeatCount: ").Append(RepeatCount).Append("\n");
      
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
