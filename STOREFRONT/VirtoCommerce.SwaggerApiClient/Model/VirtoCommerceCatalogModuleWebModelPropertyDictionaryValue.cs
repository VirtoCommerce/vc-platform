using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Individual dictionary value record for dictionary supporting property.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelPropertyDictionaryValue {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets the property id that this dictionary value belongs to.
    /// </summary>
    /// <value>Gets or sets the property id that this dictionary value belongs to.</value>
    [DataMember(Name="propertyId", EmitDefaultValue=false)]
    public string PropertyId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of this dictionary value in default language.
    /// </summary>
    /// <value>Gets or sets the value of this dictionary value in default language.</value>
    [DataMember(Name="alias", EmitDefaultValue=false)]
    public string Alias { get; set; }

    
    /// <summary>
    /// Gets or sets the language code.
    /// </summary>
    /// <value>Gets or sets the language code.</value>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>Gets or sets the value.</value>
    [DataMember(Name="value", EmitDefaultValue=false)]
    public string Value { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelPropertyDictionaryValue {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  PropertyId: ").Append(PropertyId).Append("\n");
      
      sb.Append("  Alias: ").Append(Alias).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
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
