using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Represent move operation detail
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelMoveInfo {
    
    /// <summary>
    /// Gets or Sets Catalog
    /// </summary>
    [DataMember(Name="catalog", EmitDefaultValue=false)]
    public string Catalog { get; set; }

    
    /// <summary>
    /// Gets or Sets Category
    /// </summary>
    [DataMember(Name="category", EmitDefaultValue=false)]
    public string Category { get; set; }

    
    /// <summary>
    /// Gets or Sets ListEntries
    /// </summary>
    [DataMember(Name="listEntries", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelListEntry> ListEntries { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelMoveInfo {\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  Category: ").Append(Category).Append("\n");
      
      sb.Append("  ListEntries: ").Append(ListEntries).Append("\n");
      
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
