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
  public class VirtoCommerceMerchandisingModuleWebModelReviewComment {
    
    /// <summary>
    /// Gets or sets the value of review comment abuse reports count
    /// </summary>
    /// <value>Gets or sets the value of review comment abuse reports count</value>
    [DataMember(Name="abuseCount", EmitDefaultValue=false)]
    public int? AbuseCount { get; set; }

    
    /// <summary>
    /// Gets or sets the value of review comment author name
    /// </summary>
    /// <value>Gets or sets the value of review comment author name</value>
    [DataMember(Name="author", EmitDefaultValue=false)]
    public string Author { get; set; }

    
    /// <summary>
    /// Gets or sets the value of review comment text content
    /// </summary>
    /// <value>Gets or sets the value of review comment text content</value>
    [DataMember(Name="comment", EmitDefaultValue=false)]
    public string Comment { get; set; }

    
    /// <summary>
    /// Gets or sets the value of review comment created date/time
    /// </summary>
    /// <value>Gets or sets the value of review comment created date/time</value>
    [DataMember(Name="createdDateTime", EmitDefaultValue=false)]
    public DateTime? CreatedDateTime { get; set; }

    
    /// <summary>
    /// Gets or sets the value of review comment id
    /// </summary>
    /// <value>Gets or sets the value of review comment id</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets the value of review comment negative feedback count
    /// </summary>
    /// <value>Gets or sets the value of review comment negative feedback count</value>
    [DataMember(Name="negativeFeedbackCount", EmitDefaultValue=false)]
    public int? NegativeFeedbackCount { get; set; }

    
    /// <summary>
    /// Gets or sets the value of review comment positive feedback count
    /// </summary>
    /// <value>Gets or sets the value of review comment positive feedback count</value>
    [DataMember(Name="positiveFeedbackCount", EmitDefaultValue=false)]
    public int? PositiveFeedbackCount { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelReviewComment {\n");
      
      sb.Append("  AbuseCount: ").Append(AbuseCount).Append("\n");
      
      sb.Append("  Author: ").Append(Author).Append("\n");
      
      sb.Append("  Comment: ").Append(Comment).Append("\n");
      
      sb.Append("  CreatedDateTime: ").Append(CreatedDateTime).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  NegativeFeedbackCount: ").Append(NegativeFeedbackCount).Append("\n");
      
      sb.Append("  PositiveFeedbackCount: ").Append(PositiveFeedbackCount).Append("\n");
      
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
