using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Information to define linking information from item or category to category.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelCategoryLink {
    
    /// <summary>
    /// Gets or sets the source item id.
    /// </summary>
    /// <value>Gets or sets the source item id.</value>
    [DataMember(Name="sourceItemId", EmitDefaultValue=false)]
    public string SourceItemId { get; set; }

    
    /// <summary>
    /// Gets or sets the source category identifier.
    /// </summary>
    /// <value>Gets or sets the source category identifier.</value>
    [DataMember(Name="sourceCategoryId", EmitDefaultValue=false)]
    public string SourceCategoryId { get; set; }

    
    /// <summary>
    /// Gets or sets the target catalog identifier.
    /// </summary>
    /// <value>Gets or sets the target catalog identifier.</value>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or sets the target category identifier.
    /// </summary>
    /// <value>Gets or sets the target category identifier.</value>
    [DataMember(Name="categoryId", EmitDefaultValue=false)]
    public string CategoryId { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelCategoryLink {\n");
      
      sb.Append("  SourceItemId: ").Append(SourceItemId).Append("\n");
      
      sb.Append("  SourceCategoryId: ").Append(SourceCategoryId).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
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
