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
  public class VirtoCommercePlatformCoreSecurityRoleSearchRequest {
    
    /// <summary>
    /// Gets or Sets Keyword
    /// </summary>
    [DataMember(Name="keyword", EmitDefaultValue=false)]
    public string Keyword { get; set; }

    
    /// <summary>
    /// Gets or Sets SkipCount
    /// </summary>
    [DataMember(Name="skipCount", EmitDefaultValue=false)]
    public int? SkipCount { get; set; }

    
    /// <summary>
    /// Gets or Sets TakeCount
    /// </summary>
    [DataMember(Name="takeCount", EmitDefaultValue=false)]
    public int? TakeCount { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformCoreSecurityRoleSearchRequest {\n");
      
      sb.Append("  Keyword: ").Append(Keyword).Append("\n");
      
      sb.Append("  SkipCount: ").Append(SkipCount).Append("\n");
      
      sb.Append("  TakeCount: ").Append(TakeCount).Append("\n");
      
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
