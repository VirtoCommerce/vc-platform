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
  public class VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap {
    
    /// <summary>
    /// Gets or Sets EntityColumnName
    /// </summary>
    [DataMember(Name="entityColumnName", EmitDefaultValue=false)]
    public string EntityColumnName { get; set; }

    
    /// <summary>
    /// Gets or Sets CsvColumnName
    /// </summary>
    [DataMember(Name="csvColumnName", EmitDefaultValue=false)]
    public string CsvColumnName { get; set; }

    
    /// <summary>
    /// Gets or Sets IsSystemProperty
    /// </summary>
    [DataMember(Name="isSystemProperty", EmitDefaultValue=false)]
    public bool? IsSystemProperty { get; set; }

    
    /// <summary>
    /// Gets or Sets IsRequired
    /// </summary>
    [DataMember(Name="isRequired", EmitDefaultValue=false)]
    public bool? IsRequired { get; set; }

    
    /// <summary>
    /// Gets or Sets CustomValue
    /// </summary>
    [DataMember(Name="customValue", EmitDefaultValue=false)]
    public string CustomValue { get; set; }

    
    /// <summary>
    /// Gets or Sets StringFormat
    /// </summary>
    [DataMember(Name="stringFormat", EmitDefaultValue=false)]
    public string StringFormat { get; set; }

    
    /// <summary>
    /// Gets or Sets Locale
    /// </summary>
    [DataMember(Name="locale", EmitDefaultValue=false)]
    public string Locale { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebExportImportCsvProductPropertyMap {\n");
      
      sb.Append("  EntityColumnName: ").Append(EntityColumnName).Append("\n");
      
      sb.Append("  CsvColumnName: ").Append(CsvColumnName).Append("\n");
      
      sb.Append("  IsSystemProperty: ").Append(IsSystemProperty).Append("\n");
      
      sb.Append("  IsRequired: ").Append(IsRequired).Append("\n");
      
      sb.Append("  CustomValue: ").Append(CustomValue).Append("\n");
      
      sb.Append("  StringFormat: ").Append(StringFormat).Append("\n");
      
      sb.Append("  Locale: ").Append(Locale).Append("\n");
      
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
