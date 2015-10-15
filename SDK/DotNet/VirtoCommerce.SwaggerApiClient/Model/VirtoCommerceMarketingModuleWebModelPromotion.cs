using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Represent marketing promotion, define applicable rules and rewards amount in marketing system
  /// </summary>
  [DataContract]
  public class VirtoCommerceMarketingModuleWebModelPromotion {
    
    /// <summary>
    /// It contains the name of realizing this type promotion.\r\n            DynamicPromotion is build in implementation allow to construct promotion with dynamic conditions and rewards.\r\n            For complex custom scenarios user may define personal 'hard-coded' promotion types
    /// </summary>
    /// <value>It contains the name of realizing this type promotion.\r\n            DynamicPromotion is build in implementation allow to construct promotion with dynamic conditions and rewards.\r\n            For complex custom scenarios user may define personal 'hard-coded' promotion types</value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    public string Type { get; set; }

    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Store id that is covered by this promotion
    /// </summary>
    /// <value>Store id that is covered by this promotion</value>
    [DataMember(Name="store", EmitDefaultValue=false)]
    public string Store { get; set; }

    
    /// <summary>
    /// Catalog id that is covered by this promotion
    /// </summary>
    /// <value>Catalog id that is covered by this promotion</value>
    [DataMember(Name="catalog", EmitDefaultValue=false)]
    public string Catalog { get; set; }

    
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or Sets IsActive
    /// </summary>
    [DataMember(Name="isActive", EmitDefaultValue=false)]
    public bool? IsActive { get; set; }

    
    /// <summary>
    /// Maximum promotion usage count
    /// </summary>
    /// <value>Maximum promotion usage count</value>
    [DataMember(Name="maxUsageCount", EmitDefaultValue=false)]
    public int? MaxUsageCount { get; set; }

    
    /// <summary>
    /// Gets or Sets MaxPersonalUsageCount
    /// </summary>
    [DataMember(Name="maxPersonalUsageCount", EmitDefaultValue=false)]
    public int? MaxPersonalUsageCount { get; set; }

    
    /// <summary>
    /// List of coupons codes which may be used for activate that promotion
    /// </summary>
    /// <value>List of coupons codes which may be used for activate that promotion</value>
    [DataMember(Name="coupons", EmitDefaultValue=false)]
    public List<string> Coupons { get; set; }

    
    /// <summary>
    /// Used for choosing in combination
    /// </summary>
    /// <value>Used for choosing in combination</value>
    [DataMember(Name="priority", EmitDefaultValue=false)]
    public int? Priority { get; set; }

    
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
    /// Dynamic conditions tree determine the applicability of this promotion and reward definition
    /// </summary>
    /// <value>Dynamic conditions tree determine the applicability of this promotion and reward definition</value>
    [DataMember(Name="dynamicExpression", EmitDefaultValue=false)]
    public VirtoCommerceDomainMarketingModelPromoDynamicExpressionTree DynamicExpression { get; set; }

    
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
      sb.Append("class VirtoCommerceMarketingModuleWebModelPromotion {\n");
      
      sb.Append("  Type: ").Append(Type).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Store: ").Append(Store).Append("\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  IsActive: ").Append(IsActive).Append("\n");
      
      sb.Append("  MaxUsageCount: ").Append(MaxUsageCount).Append("\n");
      
      sb.Append("  MaxPersonalUsageCount: ").Append(MaxPersonalUsageCount).Append("\n");
      
      sb.Append("  Coupons: ").Append(Coupons).Append("\n");
      
      sb.Append("  Priority: ").Append(Priority).Append("\n");
      
      sb.Append("  StartDate: ").Append(StartDate).Append("\n");
      
      sb.Append("  EndDate: ").Append(EndDate).Append("\n");
      
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
