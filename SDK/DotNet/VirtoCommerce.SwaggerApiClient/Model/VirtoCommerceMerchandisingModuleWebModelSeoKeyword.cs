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
  public class VirtoCommerceMerchandisingModuleWebModelSeoKeyword {
    
    /// <summary>
    /// Gets or sets the value of image alternative description
    /// </summary>
    /// <value>Gets or sets the value of image alternative description</value>
    [DataMember(Name="imageAltDescription", EmitDefaultValue=false)]
    public string ImageAltDescription { get; set; }

    
    /// <summary>
    /// Gets or sets the value of SEO keyword
    /// </summary>
    /// <value>Gets or sets the value of SEO keyword</value>
    [DataMember(Name="keyword", EmitDefaultValue=false)]
    public string Keyword { get; set; }

    
    /// <summary>
    /// Gets or sets the value of SEO language
    /// </summary>
    /// <value>Gets or sets the value of SEO language</value>
    [DataMember(Name="language", EmitDefaultValue=false)]
    public string Language { get; set; }

    
    /// <summary>
    /// Gets or sets the value of webpage meta description
    /// </summary>
    /// <value>Gets or sets the value of webpage meta description</value>
    [DataMember(Name="metaDescription", EmitDefaultValue=false)]
    public string MetaDescription { get; set; }

    
    /// <summary>
    /// Gets or sets the value of webpage meta keywords
    /// </summary>
    /// <value>Gets or sets the value of webpage meta keywords</value>
    [DataMember(Name="metaKeywords", EmitDefaultValue=false)]
    public string MetaKeywords { get; set; }

    
    /// <summary>
    /// Gets or sets the value of webpage title
    /// </summary>
    /// <value>Gets or sets the value of webpage title</value>
    [DataMember(Name="title", EmitDefaultValue=false)]
    public string Title { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelSeoKeyword {\n");
      
      sb.Append("  ImageAltDescription: ").Append(ImageAltDescription).Append("\n");
      
      sb.Append("  Keyword: ").Append(Keyword).Append("\n");
      
      sb.Append("  Language: ").Append(Language).Append("\n");
      
      sb.Append("  MetaDescription: ").Append(MetaDescription).Append("\n");
      
      sb.Append("  MetaKeywords: ").Append(MetaKeywords).Append("\n");
      
      sb.Append("  Title: ").Append(Title).Append("\n");
      
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
