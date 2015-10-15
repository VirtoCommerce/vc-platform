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
  public class VirtoCommerceCatalogModuleWebExportImportCsvExportInfo {
    
    /// <summary>
    /// Gets or Sets CatalogId
    /// </summary>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or Sets ProductIds
    /// </summary>
    [DataMember(Name="productIds", EmitDefaultValue=false)]
    public List<string> ProductIds { get; set; }

    
    /// <summary>
    /// Gets or Sets CategoryIds
    /// </summary>
    [DataMember(Name="categoryIds", EmitDefaultValue=false)]
    public List<string> CategoryIds { get; set; }

    
    /// <summary>
    /// Gets or Sets PriceListId
    /// </summary>
    [DataMember(Name="priceListId", EmitDefaultValue=false)]
    public string PriceListId { get; set; }

    
    /// <summary>
    /// Gets or Sets FulfilmentCenterId
    /// </summary>
    [DataMember(Name="fulfilmentCenterId", EmitDefaultValue=false)]
    public string FulfilmentCenterId { get; set; }

    
    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or Sets Configuration
    /// </summary>
    [DataMember(Name="configuration", EmitDefaultValue=false)]
    public VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration Configuration { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebExportImportCsvExportInfo {\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  ProductIds: ").Append(ProductIds).Append("\n");
      
      sb.Append("  CategoryIds: ").Append(CategoryIds).Append("\n");
      
      sb.Append("  PriceListId: ").Append(PriceListId).Append("\n");
      
      sb.Append("  FulfilmentCenterId: ").Append(FulfilmentCenterId).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  Configuration: ").Append(Configuration).Append("\n");
      
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
