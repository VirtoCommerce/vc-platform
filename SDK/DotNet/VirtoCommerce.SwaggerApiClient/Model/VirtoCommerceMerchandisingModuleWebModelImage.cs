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
  public class VirtoCommerceMerchandisingModuleWebModelImage {
    
    /// <summary>
    /// Gets or sets the image file content as bytes array
    /// </summary>
    /// <value>Gets or sets the image file content as bytes array</value>
    [DataMember(Name="attachement", EmitDefaultValue=false)]
    public string Attachement { get; set; }

    
    /// <summary>
    /// Gets or sets the value for image name
    /// </summary>
    /// <value>Gets or sets the value for image name</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the value of image gorup name
    /// </summary>
    /// <value>Gets or sets the value of image gorup name</value>
    [DataMember(Name="group", EmitDefaultValue=false)]
    public string Group { get; set; }

    
    /// <summary>
    /// Gets or sets the value of image absolute URL
    /// </summary>
    /// <value>Gets or sets the value of image absolute URL</value>
    [DataMember(Name="src", EmitDefaultValue=false)]
    public string Src { get; set; }

    
    /// <summary>
    /// Gets or sets the value of thumbnail image absolute URL
    /// </summary>
    /// <value>Gets or sets the value of thumbnail image absolute URL</value>
    [DataMember(Name="thumbSrc", EmitDefaultValue=false)]
    public string ThumbSrc { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelImage {\n");
      
      sb.Append("  Attachement: ").Append(Attachement).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Group: ").Append(Group).Append("\n");
      
      sb.Append("  Src: ").Append(Src).Append("\n");
      
      sb.Append("  ThumbSrc: ").Append(ThumbSrc).Append("\n");
      
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
