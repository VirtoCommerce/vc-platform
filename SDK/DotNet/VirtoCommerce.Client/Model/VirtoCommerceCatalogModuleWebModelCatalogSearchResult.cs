using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelCatalogSearchResult {
    
    /// <summary>
    /// Gets or Sets TotalCount
    /// </summary>
    [DataMember(Name="totalCount", EmitDefaultValue=false)]
    public int? TotalCount { get; set; }

    
    /// <summary>
    /// Gets or Sets Products
    /// </summary>
    [DataMember(Name="products", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelProduct> Products { get; set; }

    
    /// <summary>
    /// Gets or Sets Categories
    /// </summary>
    [DataMember(Name="categories", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelCategory> Categories { get; set; }

    
    /// <summary>
    /// Gets or Sets Catalogs
    /// </summary>
    [DataMember(Name="catalogs", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelCatalog> Catalogs { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelCatalogSearchResult {\n");
      
      sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
      
      sb.Append("  Products: ").Append(Products).Append("\n");
      
      sb.Append("  Categories: ").Append(Categories).Append("\n");
      
      sb.Append("  Catalogs: ").Append(Catalogs).Append("\n");
      
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
