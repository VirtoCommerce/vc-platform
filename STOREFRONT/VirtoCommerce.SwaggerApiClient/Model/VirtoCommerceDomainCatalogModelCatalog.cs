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
  public class VirtoCommerceDomainCatalogModelCatalog {
    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets Virtual
    /// </summary>
    [DataMember(Name="virtual", EmitDefaultValue=false)]
    public bool? Virtual { get; set; }

    
    /// <summary>
    /// Gets or Sets DefaultLanguage
    /// </summary>
    [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
    public VirtoCommerceDomainCatalogModelCatalogLanguage DefaultLanguage { get; set; }

    
    /// <summary>
    /// Gets or Sets Languages
    /// </summary>
    [DataMember(Name="languages", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCatalogModelCatalogLanguage> Languages { get; set; }

    
    /// <summary>
    /// Gets or Sets PropertyValues
    /// </summary>
    [DataMember(Name="propertyValues", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCatalogModelPropertyValue> PropertyValues { get; set; }

    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceDomainCatalogModelCatalog {\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Virtual: ").Append(Virtual).Append("\n");
      
      sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
      
      sb.Append("  Languages: ").Append(Languages).Append("\n");
      
      sb.Append("  PropertyValues: ").Append(PropertyValues).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
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
