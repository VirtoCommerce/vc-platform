using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Represent dynamic content publication and link content and places together\r\n            may contain conditional expressions applicability
  /// </summary>
  [DataContract]
  public class VirtoCommerceMarketingModuleWebModelDynamicContentPublication {
    
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
    /// Priority used for chose publication in combination
    /// </summary>
    /// <value>Priority used for chose publication in combination</value>
    [DataMember(Name="priority", EmitDefaultValue=false)]
    public int? Priority { get; set; }

    
    /// <summary>
    /// Gets or Sets IsActive
    /// </summary>
    [DataMember(Name="isActive", EmitDefaultValue=false)]
    public bool? IsActive { get; set; }

    
    /// <summary>
    /// Store where the publication is active
    /// </summary>
    /// <value>Store where the publication is active</value>
    [DataMember(Name="storeId", EmitDefaultValue=false)]
    public string StoreId { get; set; }

    
    /// <summary>
    /// Gets or Sets StartDate
    /// </summary>
    [DataMember(Name="startDate", EmitDefaultValue=false)]
    public DateTime? StartDate { get; set; }

    
    /// <summary>
    /// Gets or Sets EndDate
    /// </summary>
    [DataMember(Name="endDate", EmitDefaultValue=false)]
    public DateTime? EndDate { get; set; }

    
    /// <summary>
    /// Gets or Sets ContentItems
    /// </summary>
    [DataMember(Name="contentItems", EmitDefaultValue=false)]
    public List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> ContentItems { get; set; }

    
    /// <summary>
    /// Gets or Sets ContentPlaces
    /// </summary>
    [DataMember(Name="contentPlaces", EmitDefaultValue=false)]
    public List<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> ContentPlaces { get; set; }

    
    /// <summary>
    /// Dynamic conditions tree determine the applicability of this publication
    /// </summary>
    /// <value>Dynamic conditions tree determine the applicability of this publication</value>
    [DataMember(Name="dynamicExpression", EmitDefaultValue=false)]
    public VirtoCommerceDomainCommonConditionExpressionTree DynamicExpression { get; set; }

    
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
      sb.Append("class VirtoCommerceMarketingModuleWebModelDynamicContentPublication {\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  Priority: ").Append(Priority).Append("\n");
      
      sb.Append("  IsActive: ").Append(IsActive).Append("\n");
      
      sb.Append("  StoreId: ").Append(StoreId).Append("\n");
      
      sb.Append("  StartDate: ").Append(StartDate).Append("\n");
      
      sb.Append("  EndDate: ").Append(EndDate).Append("\n");
      
      sb.Append("  ContentItems: ").Append(ContentItems).Append("\n");
      
      sb.Append("  ContentPlaces: ").Append(ContentPlaces).Append("\n");
      
      sb.Append("  DynamicExpression: ").Append(DynamicExpression).Append("\n");
      
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
