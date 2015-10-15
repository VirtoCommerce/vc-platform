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
  public class VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup {
    
    /// <summary>
    /// Gets or sets the value of dynamic content item group name
    /// </summary>
    /// <value>Gets or sets the value of dynamic content item group name</value>
    [DataMember(Name="groupName", EmitDefaultValue=false)]
    public string GroupName { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of dynamic content items for dynamic content item group
    /// </summary>
    /// <value>Gets or sets the collection of dynamic content items for dynamic content item group</value>
    [DataMember(Name="items", EmitDefaultValue=false)]
    public List<VirtoCommerceMerchandisingModuleWebModelDynamicContentItem> Items { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup {\n");
      
      sb.Append("  GroupName: ").Append(GroupName).Append("\n");
      
      sb.Append("  Items: ").Append(Items).Append("\n");
      
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
