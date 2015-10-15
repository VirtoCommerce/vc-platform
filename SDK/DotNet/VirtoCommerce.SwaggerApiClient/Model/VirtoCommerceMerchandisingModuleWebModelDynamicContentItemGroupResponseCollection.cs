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
  public class VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection {
    
    /// <summary>
    /// Gets or sets the collection of reposponse items
    /// </summary>
    /// <value>Gets or sets the collection of reposponse items</value>
    [DataMember(Name="items", EmitDefaultValue=false)]
    public List<VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup> Items { get; set; }

    
    /// <summary>
    /// Gets or sets the value of response items total count
    /// </summary>
    /// <value>Gets or sets the value of response items total count</value>
    [DataMember(Name="total", EmitDefaultValue=false)]
    public int? Total { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection {\n");
      
      sb.Append("  Items: ").Append(Items).Append("\n");
      
      sb.Append("  Total: ").Append(Total).Append("\n");
      
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
