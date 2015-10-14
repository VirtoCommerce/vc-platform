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
  public class VirtoCommerceMerchandisingModuleWebModelDynamicContentItem {
    
    /// <summary>
    /// Gets or sets the value of dynamic content item type
    /// </summary>
    /// <value>Gets or sets the value of dynamic content item type</value>
    [DataMember(Name="contentType", EmitDefaultValue=false)]
    public string ContentType { get; set; }

    
    /// <summary>
    /// Gets or sets the value of dynamic content item description
    /// </summary>
    /// <value>Gets or sets the value of dynamic content item description</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or sets the value of dynamic content item id
    /// </summary>
    /// <value>Gets or sets the value of dynamic content item id</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets the flag of supporting multilanguage for dynamic content item
    /// </summary>
    /// <value>Gets or sets the flag of supporting multilanguage for dynamic content item</value>
    [DataMember(Name="isMultilingual", EmitDefaultValue=false)]
    public bool? IsMultilingual { get; set; }

    
    /// <summary>
    /// Gets or sets the value of dynamic content item name
    /// </summary>
    /// <value>Gets or sets the value of dynamic content item name</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the dictionary of dynamic content item propertines
    /// </summary>
    /// <value>Gets or sets the dictionary of dynamic content item propertines</value>
    [DataMember(Name="properties", EmitDefaultValue=false)]
    public Dictionary<string, Object> Properties { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelDynamicContentItem {\n");
      
      sb.Append("  ContentType: ").Append(ContentType).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  IsMultilingual: ").Append(IsMultilingual).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Properties: ").Append(Properties).Append("\n");
      
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
