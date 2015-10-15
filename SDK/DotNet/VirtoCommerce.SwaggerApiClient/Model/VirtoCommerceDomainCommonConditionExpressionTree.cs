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
  public class VirtoCommerceDomainCommonConditionExpressionTree {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or Sets AvailableChildren
    /// </summary>
    [DataMember(Name="availableChildren", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCommonDynamicExpression> AvailableChildren { get; set; }

    
    /// <summary>
    /// Gets or Sets Children
    /// </summary>
    [DataMember(Name="children", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCommonDynamicExpression> Children { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceDomainCommonConditionExpressionTree {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  AvailableChildren: ").Append(AvailableChildren).Append("\n");
      
      sb.Append("  Children: ").Append(Children).Append("\n");
      
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
