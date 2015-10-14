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
  public class VirtoCommerceContentWebModelsThemeAssetFolder {
    
    /// <summary>
    /// Theme asset folder name, one of the predefined values - 'assets', 'templates', 'snippets', 'layout', 'config', 'locales'
    /// </summary>
    /// <value>Theme asset folder name, one of the predefined values - 'assets', 'templates', 'snippets', 'layout', 'config', 'locales'</value>
    [DataMember(Name="folderName", EmitDefaultValue=false)]
    public string FolderName { get; set; }

    
    /// <summary>
    /// Gets or Sets Assets
    /// </summary>
    [DataMember(Name="assets", EmitDefaultValue=false)]
    public List<VirtoCommerceContentWebModelsThemeAsset> Assets { get; set; }

    
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
      sb.Append("class VirtoCommerceContentWebModelsThemeAssetFolder {\n");
      
      sb.Append("  FolderName: ").Append(FolderName).Append("\n");
      
      sb.Append("  Assets: ").Append(Assets).Append("\n");
      
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
