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
  public class VirtoCommerceDomainCatalogModelCategoryLink {
    
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
    /// Gets or Sets CategoryId
    /// </summary>
    [DataMember(Name="categoryId", EmitDefaultValue=false)]
    public string CategoryId { get; set; }

    
    /// <summary>
    /// Gets or Sets Category
    /// </summary>
    [DataMember(Name="category", EmitDefaultValue=false)]
    public VirtoCommerceDomainCatalogModelCategory Category { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceDomainCatalogModelCategoryLink {\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
      sb.Append("  Category: ").Append(Category).Append("\n");
      
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
