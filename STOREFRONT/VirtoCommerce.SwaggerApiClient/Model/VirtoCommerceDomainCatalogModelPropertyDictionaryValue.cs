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
  public class VirtoCommerceDomainCatalogModelPropertyDictionaryValue {
    
    /// <summary>
    /// Gets or Sets PropertyId
    /// </summary>
    [DataMember(Name="propertyId", EmitDefaultValue=false)]
    public string PropertyId { get; set; }

    
    /// <summary>
    /// Gets or Sets Property
    /// </summary>
    [DataMember(Name="property", EmitDefaultValue=false)]
    public VirtoCommerceDomainCatalogModelProperty Property { get; set; }

    
    /// <summary>
    /// Gets or Sets Alias
    /// </summary>
    [DataMember(Name="alias", EmitDefaultValue=false)]
    public string Alias { get; set; }

    
    /// <summary>
    /// Gets or Sets LanguageCode
    /// </summary>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    
    /// <summary>
    /// Gets or Sets Value
    /// </summary>
    [DataMember(Name="value", EmitDefaultValue=false)]
    public string Value { get; set; }

    
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
      sb.Append("class VirtoCommerceDomainCatalogModelPropertyDictionaryValue {\n");
      
      sb.Append("  PropertyId: ").Append(PropertyId).Append("\n");
      
      sb.Append("  Property: ").Append(Property).Append("\n");
      
      sb.Append("  Alias: ").Append(Alias).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
      sb.Append("  Value: ").Append(Value).Append("\n");
      
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
