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
  public class VirtoCommerceContentWebModelsPage {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Page element name, contains the path relative to the root pages folder
    /// </summary>
    /// <value>Page element name, contains the path relative to the root pages folder</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Page element text content (text page element), based on content type
    /// </summary>
    /// <value>Page element text content (text page element), based on content type</value>
    [DataMember(Name="content", EmitDefaultValue=false)]
    public string Content { get; set; }

    
    /// <summary>
    /// Page element byte content (non-text page element), bases on content type
    /// </summary>
    /// <value>Page element byte content (non-text page element), bases on content type</value>
    [DataMember(Name="byteContent", EmitDefaultValue=false)]
    public string ByteContent { get; set; }

    
    /// <summary>
    /// Gets or Sets ContentType
    /// </summary>
    [DataMember(Name="contentType", EmitDefaultValue=false)]
    public string ContentType { get; set; }

    
    /// <summary>
    /// Locale
    /// </summary>
    /// <value>Locale</value>
    [DataMember(Name="language", EmitDefaultValue=false)]
    public string Language { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedDate
    /// </summary>
    [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
    public DateTime? ModifiedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets FileUrl
    /// </summary>
    [DataMember(Name="fileUrl", EmitDefaultValue=false)]
    public string FileUrl { get; set; }

    
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
      sb.Append("class VirtoCommerceContentWebModelsPage {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Content: ").Append(Content).Append("\n");
      
      sb.Append("  ByteContent: ").Append(ByteContent).Append("\n");
      
      sb.Append("  ContentType: ").Append(ContentType).Append("\n");
      
      sb.Append("  Language: ").Append(Language).Append("\n");
      
      sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
      
      sb.Append("  FileUrl: ").Append(FileUrl).Append("\n");
      
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
