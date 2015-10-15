using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Asset containing any content.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelAsset {
    
    /// <summary>
    /// Gets or Sets Size
    /// </summary>
    [DataMember(Name="size", EmitDefaultValue=false)]
    public long? Size { get; set; }

    
    /// <summary>
    /// Gets or Sets MimeType
    /// </summary>
    [DataMember(Name="mimeType", EmitDefaultValue=false)]
    public string MimeType { get; set; }

    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or Sets RelativeUrl
    /// </summary>
    [DataMember(Name="relativeUrl", EmitDefaultValue=false)]
    public string RelativeUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets Url
    /// </summary>
    [DataMember(Name="url", EmitDefaultValue=false)]
    public string Url { get; set; }

    
    /// <summary>
    /// Gets or sets the asset type identifier.
    /// </summary>
    /// <value>Gets or sets the asset type identifier.</value>
    [DataMember(Name="typeId", EmitDefaultValue=false)]
    public string TypeId { get; set; }

    
    /// <summary>
    /// Gets or sets the asset group name.
    /// </summary>
    /// <value>Gets or sets the asset group name.</value>
    [DataMember(Name="group", EmitDefaultValue=false)]
    public string Group { get; set; }

    
    /// <summary>
    /// Gets or sets the asset name.
    /// </summary>
    /// <value>Gets or sets the asset name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the asset language.
    /// </summary>
    /// <value>Gets or sets the asset language.</value>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelAsset {\n");
      
      sb.Append("  Size: ").Append(Size).Append("\n");
      
      sb.Append("  MimeType: ").Append(MimeType).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  RelativeUrl: ").Append(RelativeUrl).Append("\n");
      
      sb.Append("  Url: ").Append(Url).Append("\n");
      
      sb.Append("  TypeId: ").Append(TypeId).Append("\n");
      
      sb.Append("  Group: ").Append(Group).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
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
