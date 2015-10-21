using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Catalog Language information.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelCatalogLanguage {
    
    /// <summary>
    /// Gets or sets the catalog identifier.
    /// </summary>
    /// <value>Gets or sets the catalog identifier.</value>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this catalog language is default.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this catalog language is default.</value>
    [DataMember(Name="isDefault", EmitDefaultValue=false)]
    public bool? IsDefault { get; set; }

    
    /// <summary>
    /// Gets or sets the language code.
    /// </summary>
    /// <value>Gets or sets the language code.</value>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    
    /// <summary>
    /// Gets the human-readable language name.
    /// </summary>
    /// <value>Gets the human-readable language name.</value>
    [DataMember(Name="displayName", EmitDefaultValue=false)]
    public string DisplayName { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelCatalogLanguage {\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  IsDefault: ").Append(IsDefault).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
      sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
      
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
