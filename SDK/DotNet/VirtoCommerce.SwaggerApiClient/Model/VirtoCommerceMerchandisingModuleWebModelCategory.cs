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
  public class VirtoCommerceMerchandisingModuleWebModelCategory {
    
    /// <summary>
    /// Gets or sets the value of category code
    /// </summary>
    /// <value>Gets or sets the value of category code</value>
    [DataMember(Name="code", EmitDefaultValue=false)]
    public string Code { get; set; }

    
    /// <summary>
    /// Gets or sets the value of category id
    /// </summary>
    /// <value>Gets or sets the value of category id</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets the value of category name
    /// </summary>
    /// <value>Gets or sets the value of category name</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of category parents categories
    /// </summary>
    /// <value>Gets or sets the collection of category parents categories</value>
    [DataMember(Name="parents", EmitDefaultValue=false)]
    public List<VirtoCommerceMerchandisingModuleWebModelCategory> Parents { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of category SEO parameters
    /// </summary>
    /// <value>Gets or sets the collection of category SEO parameters</value>
    [DataMember(Name="seo", EmitDefaultValue=false)]
    public List<VirtoCommerceMerchandisingModuleWebModelSeoKeyword> Seo { get; set; }

    
    /// <summary>
    /// Gets or sets the category image
    /// </summary>
    /// <value>Gets or sets the category image</value>
    [DataMember(Name="image", EmitDefaultValue=false)]
    public VirtoCommerceMerchandisingModuleWebModelImage Image { get; set; }

    
    /// <summary>
    /// Gets or sets the flag of virtual category
    /// </summary>
    /// <value>Gets or sets the flag of virtual category</value>
    [DataMember(Name="virtual", EmitDefaultValue=false)]
    public bool? Virtual { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelCategory {\n");
      
      sb.Append("  Code: ").Append(Code).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Parents: ").Append(Parents).Append("\n");
      
      sb.Append("  Seo: ").Append(Seo).Append("\n");
      
      sb.Append("  Image: ").Append(Image).Append("\n");
      
      sb.Append("  Virtual: ").Append(Virtual).Append("\n");
      
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
