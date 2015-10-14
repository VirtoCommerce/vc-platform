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
  public class VirtoCommerceContentWebModelsGetPagesResult {
    
    /// <summary>
    /// Collection of pages folders (by default - 'pages', 'includes'), that contains page elements
    /// </summary>
    /// <value>Collection of pages folders (by default - 'pages', 'includes'), that contains page elements</value>
    [DataMember(Name="folders", EmitDefaultValue=false)]
    public List<VirtoCommerceContentWebModelsPageFolder> Folders { get; set; }

    
    /// <summary>
    /// Collection of page elements (used in pages rendering (page html, images, etc.))
    /// </summary>
    /// <value>Collection of page elements (used in pages rendering (page html, images, etc.))</value>
    [DataMember(Name="pages", EmitDefaultValue=false)]
    public List<VirtoCommerceContentWebModelsPage> Pages { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceContentWebModelsGetPagesResult {\n");
      
      sb.Append("  Folders: ").Append(Folders).Append("\n");
      
      sb.Append("  Pages: ").Append(Pages).Append("\n");
      
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
