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
  public class VirtoCommerceMerchandisingModuleWebModelAsset {
    
    /// <summary>
    /// Gets or sets the value of asset absolute URL
    /// </summary>
    /// <value>Gets or sets the value of asset absolute URL</value>
    [DataMember(Name="url", EmitDefaultValue=false)]
    public string Url { get; set; }

    
    /// <summary>
    /// Gets or sets the value of asset group name
    /// </summary>
    /// <value>Gets or sets the value of asset group name</value>
    [DataMember(Name="group", EmitDefaultValue=false)]
    public string Group { get; set; }

    
    /// <summary>
    /// Gets or sets the value of asset name
    /// </summary>
    /// <value>Gets or sets the value of asset name</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the value of asset file size in bytes
    /// </summary>
    /// <value>Gets or sets the value of asset file size in bytes</value>
    [DataMember(Name="size", EmitDefaultValue=false)]
    public long? Size { get; set; }

    
    /// <summary>
    /// Gets or sets the value of asset MIME type
    /// </summary>
    /// <value>Gets or sets the value of asset MIME type</value>
    [DataMember(Name="mimeType", EmitDefaultValue=false)]
    public string MimeType { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelAsset {\n");
      
      sb.Append("  Url: ").Append(Url).Append("\n");
      
      sb.Append("  Group: ").Append(Group).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Size: ").Append(Size).Append("\n");
      
      sb.Append("  MimeType: ").Append(MimeType).Append("\n");
      
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
