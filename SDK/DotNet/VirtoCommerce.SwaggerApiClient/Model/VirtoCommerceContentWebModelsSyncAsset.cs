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
  public class VirtoCommerceContentWebModelsSyncAsset {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Asset element name, contains full path of theme asset or page element relative to the root
    /// </summary>
    /// <value>Asset element name, contains full path of theme asset or page element relative to the root</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Asset element text content (text files - html, js, css, txt, liquid &amp; etc), based on content type
    /// </summary>
    /// <value>Asset element text content (text files - html, js, css, txt, liquid &amp; etc), based on content type</value>
    [DataMember(Name="content", EmitDefaultValue=false)]
    public string Content { get; set; }

    
    /// <summary>
    /// Asset element byte content (non-text files - images, fonts, zips, pdfs &amp; etc), based on content type
    /// </summary>
    /// <value>Asset element byte content (non-text files - images, fonts, zips, pdfs &amp; etc), based on content type</value>
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
    /// Last updated date of the asset element
    /// </summary>
    /// <value>Last updated date of the asset element</value>
    [DataMember(Name="updated", EmitDefaultValue=false)]
    public DateTime? Updated { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceContentWebModelsSyncAsset {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Content: ").Append(Content).Append("\n");
      
      sb.Append("  ByteContent: ").Append(ByteContent).Append("\n");
      
      sb.Append("  AssetUrl: ").Append(AssetUrl).Append("\n");
      
      sb.Append("  ContentType: ").Append(ContentType).Append("\n");
      
      sb.Append("  Updated: ").Append(Updated).Append("\n");
      
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
