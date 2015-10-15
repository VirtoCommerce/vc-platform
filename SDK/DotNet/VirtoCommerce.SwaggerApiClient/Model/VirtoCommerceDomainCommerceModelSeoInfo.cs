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
  public class VirtoCommerceDomainCommerceModelSeoInfo {
    
    /// <summary>
    /// Gets or Sets SemanticUrl
    /// </summary>
    [DataMember(Name="semanticUrl", EmitDefaultValue=false)]
    public string SemanticUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets PageTitle
    /// </summary>
    [DataMember(Name="pageTitle", EmitDefaultValue=false)]
    public string PageTitle { get; set; }

    
    /// <summary>
    /// Gets or Sets MetaDescription
    /// </summary>
    [DataMember(Name="metaDescription", EmitDefaultValue=false)]
    public string MetaDescription { get; set; }

    
    /// <summary>
    /// Gets or Sets ImageAltDescription
    /// </summary>
    [DataMember(Name="imageAltDescription", EmitDefaultValue=false)]
    public string ImageAltDescription { get; set; }

    
    /// <summary>
    /// Gets or Sets MetaKeywords
    /// </summary>
    [DataMember(Name="metaKeywords", EmitDefaultValue=false)]
    public string MetaKeywords { get; set; }

    
    /// <summary>
    /// Gets or Sets ObjectId
    /// </summary>
    [DataMember(Name="objectId", EmitDefaultValue=false)]
    public string ObjectId { get; set; }

    
    /// <summary>
    /// Gets or Sets ObjectType
    /// </summary>
    [DataMember(Name="objectType", EmitDefaultValue=false)]
    public string ObjectType { get; set; }

    
    /// <summary>
    /// Gets or Sets LanguageCode
    /// </summary>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    
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
      sb.Append("class VirtoCommerceDomainCommerceModelSeoInfo {\n");
      
      sb.Append("  SemanticUrl: ").Append(SemanticUrl).Append("\n");
      
      sb.Append("  PageTitle: ").Append(PageTitle).Append("\n");
      
      sb.Append("  MetaDescription: ").Append(MetaDescription).Append("\n");
      
      sb.Append("  ImageAltDescription: ").Append(ImageAltDescription).Append("\n");
      
      sb.Append("  MetaKeywords: ").Append(MetaKeywords).Append("\n");
      
      sb.Append("  ObjectId: ").Append(ObjectId).Append("\n");
      
      sb.Append("  ObjectType: ").Append(ObjectType).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
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
