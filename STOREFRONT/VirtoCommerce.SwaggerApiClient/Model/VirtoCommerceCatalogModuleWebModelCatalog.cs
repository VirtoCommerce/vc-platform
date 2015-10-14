using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Merchandising Catalog.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelCatalog {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>Gets or sets the name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Catalog} is virtual or common.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Catalog} is virtual or common.</value>
    [DataMember(Name="virtual", EmitDefaultValue=false)]
    public bool? Virtual { get; set; }

    
    /// <summary>
    /// Gets the language from languages list marked as default.
    /// </summary>
    /// <value>Gets the language from languages list marked as default.</value>
    [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
    public VirtoCommerceCatalogModuleWebModelCatalogLanguage DefaultLanguage { get; set; }

    
    /// <summary>
    /// Gets or sets the catalog languages.
    /// </summary>
    /// <value>Gets or sets the catalog languages.</value>
    [DataMember(Name="languages", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelCatalogLanguage> Languages { get; set; }

    
    /// <summary>
    /// Gets or sets the catalog properties.
    /// </summary>
    /// <value>Gets or sets the catalog properties.</value>
    [DataMember(Name="properties", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelProperty> Properties { get; set; }

    
    /// <summary>
    /// Gets or Sets SecurityScopes
    /// </summary>
    [DataMember(Name="securityScopes", EmitDefaultValue=false)]
    public List<string> SecurityScopes { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelCatalog {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Virtual: ").Append(Virtual).Append("\n");
      
      sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
      
      sb.Append("  Languages: ").Append(Languages).Append("\n");
      
      sb.Append("  Properties: ").Append(Properties).Append("\n");
      
      sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
      
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
