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
  public class VirtoCommerceMerchandisingModuleWebModelFacetValue {
    
    /// <summary>
    /// Gets or sets the facet value count
    /// </summary>
    /// <value>Gets or sets the facet value count</value>
    [DataMember(Name="count", EmitDefaultValue=false)]
    public int? Count { get; set; }

    
    /// <summary>
    /// Gets or sets the flag for facet value is applied
    /// </summary>
    /// <value>Gets or sets the flag for facet value is applied</value>
    [DataMember(Name="isApplied", EmitDefaultValue=false)]
    public bool? IsApplied { get; set; }

    
    /// <summary>
    /// Gets or sets the facet value label
    /// </summary>
    /// <value>Gets or sets the facet value label</value>
    [DataMember(Name="label", EmitDefaultValue=false)]
    public string Label { get; set; }

    
    /// <summary>
    /// Gets or sets the facet value
    /// </summary>
    /// <value>Gets or sets the facet value</value>
    [DataMember(Name="value", EmitDefaultValue=false)]
    public Object Value { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelFacetValue {\n");
      
      sb.Append("  Count: ").Append(Count).Append("\n");
      
      sb.Append("  IsApplied: ").Append(IsApplied).Append("\n");
      
      sb.Append("  Label: ").Append(Label).Append("\n");
      
      sb.Append("  Value: ").Append(Value).Append("\n");
      
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
