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
  public class VirtoCommerceDomainCatalogModelCatalogLanguage {
    
    /// <summary>
    /// Gets or Sets CatalogId
    /// </summary>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or Sets Catalog
    /// </summary>
    [DataMember(Name="catalog", EmitDefaultValue=false)]
    public VirtoCommerceDomainCatalogModelCatalog Catalog { get; set; }

    
    /// <summary>
    /// Gets or Sets IsDefault
    /// </summary>
    [DataMember(Name="isDefault", EmitDefaultValue=false)]
    public bool? IsDefault { get; set; }

    
    /// <summary>
    /// Gets or Sets LanguageCode
    /// </summary>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    
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
      sb.Append("class VirtoCommerceDomainCatalogModelCatalogLanguage {\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  IsDefault: ").Append(IsDefault).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
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
