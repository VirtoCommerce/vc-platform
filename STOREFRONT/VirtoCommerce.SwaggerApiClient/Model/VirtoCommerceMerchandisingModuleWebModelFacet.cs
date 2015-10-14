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
  public class VirtoCommerceMerchandisingModuleWebModelFacet {
    
    /// <summary>
    /// Gets or sets the value of facet type
    /// </summary>
    /// <value>Gets or sets the value of facet type</value>
    [DataMember(Name="facetType", EmitDefaultValue=false)]
    public string FacetType { get; set; }

    
    /// <summary>
    /// Gets or sets the value of facet field
    /// </summary>
    /// <value>Gets or sets the value of facet field</value>
    [DataMember(Name="field", EmitDefaultValue=false)]
    public string Field { get; set; }

    
    /// <summary>
    /// Gets or sets the value of facet label
    /// </summary>
    /// <value>Gets or sets the value of facet label</value>
    [DataMember(Name="label", EmitDefaultValue=false)]
    public string Label { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of facet values
    /// </summary>
    /// <value>Gets or sets the collection of facet values</value>
    [DataMember(Name="values", EmitDefaultValue=false)]
    public List<VirtoCommerceMerchandisingModuleWebModelFacetValue> Values { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelFacet {\n");
      
      sb.Append("  FacetType: ").Append(FacetType).Append("\n");
      
      sb.Append("  Field: ").Append(Field).Append("\n");
      
      sb.Append("  Label: ").Append(Label).Append("\n");
      
      sb.Append("  Values: ").Append(Values).Append("\n");
      
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
