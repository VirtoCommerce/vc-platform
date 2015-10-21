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
  public class VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration {
    
    /// <summary>
    /// Gets or Sets ETag
    /// </summary>
    [DataMember(Name="eTag", EmitDefaultValue=false)]
    public string ETag { get; set; }

    
    /// <summary>
    /// Gets or Sets Delimiter
    /// </summary>
    [DataMember(Name="delimiter", EmitDefaultValue=false)]
    public string Delimiter { get; set; }

    
    /// <summary>
    /// Gets or Sets CsvColumns
    /// </summary>
    [DataMember(Name="csvColumns", EmitDefaultValue=false)]
    public List<string> CsvColumns { get; set; }

    
    /// <summary>
    /// Gets or Sets PropertyMaps
    /// </summary>
    [DataMember(Name="propertyMaps", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap> PropertyMaps { get; set; }

    
    /// <summary>
    /// Gets or Sets PropertyCsvColumns
    /// </summary>
    [DataMember(Name="propertyCsvColumns", EmitDefaultValue=false)]
    public List<string> PropertyCsvColumns { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration {\n");
      
      sb.Append("  ETag: ").Append(ETag).Append("\n");
      
      sb.Append("  Delimiter: ").Append(Delimiter).Append("\n");
      
      sb.Append("  CsvColumns: ").Append(CsvColumns).Append("\n");
      
      sb.Append("  PropertyMaps: ").Append(PropertyMaps).Append("\n");
      
      sb.Append("  PropertyCsvColumns: ").Append(PropertyCsvColumns).Append("\n");
      
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
