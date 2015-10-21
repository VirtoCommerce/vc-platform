using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Represent presentation placeholders for dynamic content publication
  /// </summary>
  [DataContract]
  public class VirtoCommerceMarketingModuleWebModelDynamicContentPlace {
    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or Sets FolderId
    /// </summary>
    [DataMember(Name="folderId", EmitDefaultValue=false)]
    public string FolderId { get; set; }

    
    /// <summary>
    /// all parent folders ids concatenated (1;21;344)
    /// </summary>
    /// <value>all parent folders ids concatenated (1;21;344)</value>
    [DataMember(Name="outline", EmitDefaultValue=false)]
    public string Outline { get; set; }

    
    /// <summary>
    /// all parent folders names concatenated (Root\Child\Child2)
    /// </summary>
    /// <value>all parent folders names concatenated (Root\Child\Child2)</value>
    [DataMember(Name="path", EmitDefaultValue=false)]
    public string Path { get; set; }

    
    /// <summary>
    /// Gets or Sets ImageUrl
    /// </summary>
    [DataMember(Name="imageUrl", EmitDefaultValue=false)]
    public string ImageUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets CreatedDate
    /// </summary>
    [DataMember(Name="createdDate", EmitDefaultValue=false)]
    public DateTime? CreatedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedDate
    /// </summary>
    [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
    public DateTime? ModifiedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets CreatedBy
    /// </summary>
    [DataMember(Name="createdBy", EmitDefaultValue=false)]
    public string CreatedBy { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedBy
    /// </summary>
    [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
    public string ModifiedBy { get; set; }

    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMarketingModuleWebModelDynamicContentPlace {\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  FolderId: ").Append(FolderId).Append("\n");
      
      sb.Append("  Outline: ").Append(Outline).Append("\n");
      
      sb.Append("  Path: ").Append(Path).Append("\n");
      
      sb.Append("  ImageUrl: ").Append(ImageUrl).Append("\n");
      
      sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
      
      sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
      
      sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
      
      sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
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
