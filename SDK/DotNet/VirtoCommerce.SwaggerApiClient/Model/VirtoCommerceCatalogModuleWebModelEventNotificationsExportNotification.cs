using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Notification for catalog data export job.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification {
    
    /// <summary>
    /// Gets or sets the URL for downloading exported data.
    /// </summary>
    /// <value>Gets or sets the URL for downloading exported data.</value>
    [DataMember(Name="downloadUrl", EmitDefaultValue=false)]
    public string DownloadUrl { get; set; }

    
    /// <summary>
    /// Gets or sets the job finish date and time.
    /// </summary>
    /// <value>Gets or sets the job finish date and time.</value>
    [DataMember(Name="finished", EmitDefaultValue=false)]
    public DateTime? Finished { get; set; }

    
    /// <summary>
    /// Gets or sets the total count of objects to process.
    /// </summary>
    /// <value>Gets or sets the total count of objects to process.</value>
    [DataMember(Name="totalCount", EmitDefaultValue=false)]
    public long? TotalCount { get; set; }

    
    /// <summary>
    /// Gets or sets the count of processed objects.
    /// </summary>
    /// <value>Gets or sets the count of processed objects.</value>
    [DataMember(Name="processedCount", EmitDefaultValue=false)]
    public long? ProcessedCount { get; set; }

    
    /// <summary>
    /// Gets or sets the count of errors during processing.
    /// </summary>
    /// <value>Gets or sets the count of errors during processing.</value>
    [DataMember(Name="errorCount", EmitDefaultValue=false)]
    public long? ErrorCount { get; set; }

    
    /// <summary>
    /// Gets or sets the errors that has occurred during processing.
    /// </summary>
    /// <value>Gets or sets the errors that has occurred during processing.</value>
    [DataMember(Name="errors", EmitDefaultValue=false)]
    public List<string> Errors { get; set; }

    
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
      sb.Append("class VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification {\n");
      
      sb.Append("  DownloadUrl: ").Append(DownloadUrl).Append("\n");
      
      sb.Append("  Finished: ").Append(Finished).Append("\n");
      
      sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
      
      sb.Append("  ProcessedCount: ").Append(ProcessedCount).Append("\n");
      
      sb.Append("  ErrorCount: ").Append(ErrorCount).Append("\n");
      
      sb.Append("  Errors: ").Append(Errors).Append("\n");
      
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
