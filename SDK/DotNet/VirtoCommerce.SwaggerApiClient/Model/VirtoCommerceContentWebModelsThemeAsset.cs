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
  public class VirtoCommerceContentWebModelsThemeAsset {
    
    /// <summary>
    /// Id, contains full path relative to theme root folder
    /// </summary>
    /// <value>Id, contains full path relative to theme root folder</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Theme asset text content (text files - html, css, js &amp; etc), based on content type
    /// </summary>
    /// <value>Theme asset text content (text files - html, css, js &amp; etc), based on content type</value>
    [DataMember(Name="content", EmitDefaultValue=false)]
    public string Content { get; set; }

    
    /// <summary>
    /// Theme asset byte content (non-text files - images, fonts, zips &amp; etc), based on content type
    /// </summary>
    /// <value>Theme asset byte content (non-text files - images, fonts, zips &amp; etc), based on content type</value>
    [DataMember(Name="byteContent", EmitDefaultValue=false)]
    public string ByteContent { get; set; }

    
    /// <summary>
    /// Gets or Sets AssetUrl
    /// </summary>
    [DataMember(Name="assetUrl", EmitDefaultValue=false)]
    public string AssetUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets ContentType
    /// </summary>
    [DataMember(Name="contentType", EmitDefaultValue=false)]
    public string ContentType { get; set; }

    
    /// <summary>
    /// Theme asset last update date
    /// </summary>
    /// <value>Theme asset last update date</value>
    [DataMember(Name="updated", EmitDefaultValue=false)]
    public DateTime? Updated { get; set; }

    
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
      sb.Append("class VirtoCommerceContentWebModelsThemeAsset {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Content: ").Append(Content).Append("\n");
      
      sb.Append("  ByteContent: ").Append(ByteContent).Append("\n");
      
      sb.Append("  AssetUrl: ").Append(AssetUrl).Append("\n");
      
      sb.Append("  ContentType: ").Append(ContentType).Append("\n");
      
      sb.Append("  Updated: ").Append(Updated).Append("\n");
      
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
