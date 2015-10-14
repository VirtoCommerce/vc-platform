using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Base class for all entries used in catalog categories browsing.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelListEntry {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets the type. E.g. \"product\", \"category\"
    /// </summary>
    /// <value>Gets or sets the type. E.g. \"product\", \"category\"</value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    public string Type { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this entry is active.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this entry is active.</value>
    [DataMember(Name="isActive", EmitDefaultValue=false)]
    public bool? IsActive { get; set; }

    
    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    /// <value>Gets or sets the image URL.</value>
    [DataMember(Name="imageUrl", EmitDefaultValue=false)]
    public string ImageUrl { get; set; }

    
    /// <summary>
    /// Gets or sets the entry code.
    /// </summary>
    /// <value>Gets or sets the entry code.</value>
    [DataMember(Name="code", EmitDefaultValue=false)]
    public string Code { get; set; }

    
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>Gets or sets the name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the links.
    /// </summary>
    /// <value>Gets or sets the links.</value>
    [DataMember(Name="links", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelListEntryLink> Links { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelListEntry {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Type: ").Append(Type).Append("\n");
      
      sb.Append("  IsActive: ").Append(IsActive).Append("\n");
      
      sb.Append("  ImageUrl: ").Append(ImageUrl).Append("\n");
      
      sb.Append("  Code: ").Append(Code).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Links: ").Append(Links).Append("\n");
      
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
