using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// The actual property value assigned to concrete merchandising entity.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelPropertyValue {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets the name of the property that this value belongs to.
    /// </summary>
    /// <value>Gets or sets the name of the property that this value belongs to.</value>
    [DataMember(Name="propertyName", EmitDefaultValue=false)]
    public string PropertyName { get; set; }

    
    /// <summary>
    /// Gets or sets the language of this property value.
    /// </summary>
    /// <value>Gets or sets the language of this property value.</value>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    
    /// <summary>
    /// Gets or sets the value of this dictionary value in default language.
    /// </summary>
    /// <value>Gets or sets the value of this dictionary value in default language.</value>
    [DataMember(Name="alias", EmitDefaultValue=false)]
    public string Alias { get; set; }

    
    /// <summary>
    /// Gets or sets the type of the value.
    /// </summary>
    /// <value>Gets or sets the type of the value.</value>
    [DataMember(Name="valueType", EmitDefaultValue=false)]
    public string ValueType { get; set; }

    
    /// <summary>
    /// Gets or sets the value id in case this value is for property which supports dictionary values.
    /// </summary>
    /// <value>Gets or sets the value id in case this value is for property which supports dictionary values.</value>
    [DataMember(Name="valueId", EmitDefaultValue=false)]
    public string ValueId { get; set; }

    
    /// <summary>
    /// Gets or sets the actual value.
    /// </summary>
    /// <value>Gets or sets the actual value.</value>
    [DataMember(Name="value", EmitDefaultValue=false)]
    public Object Value { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelPropertyValue {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  PropertyName: ").Append(PropertyName).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
      sb.Append("  Alias: ").Append(Alias).Append("\n");
      
      sb.Append("  ValueType: ").Append(ValueType).Append("\n");
      
      sb.Append("  ValueId: ").Append(ValueId).Append("\n");
      
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
