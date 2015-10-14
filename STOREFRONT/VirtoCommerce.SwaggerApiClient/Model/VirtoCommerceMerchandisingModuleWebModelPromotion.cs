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
  public class VirtoCommerceMerchandisingModuleWebModelPromotion {
    
    /// <summary>
    /// Gets or sets the value of promotion type
    /// </summary>
    /// <value>Gets or sets the value of promotion type</value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    public string Type { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion name
    /// </summary>
    /// <value>Gets or sets the value of promotion name</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store id
    /// </summary>
    /// <value>Gets or sets the value of store id</value>
    [DataMember(Name="store", EmitDefaultValue=false)]
    public string Store { get; set; }

    
    /// <summary>
    /// Gets or sets the value of catalog id
    /// </summary>
    /// <value>Gets or sets the value of catalog id</value>
    [DataMember(Name="catalog", EmitDefaultValue=false)]
    public string Catalog { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion description
    /// </summary>
    /// <value>Gets or sets the value of promotion description</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or sets the activity flag for promotion
    /// </summary>
    /// <value>Gets or sets the activity flag for promotion</value>
    [DataMember(Name="isActive", EmitDefaultValue=false)]
    public bool? IsActive { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion maximum common usage limit
    /// </summary>
    /// <value>Gets or sets the value of promotion maximum common usage limit</value>
    [DataMember(Name="maxUsageCount", EmitDefaultValue=false)]
    public int? MaxUsageCount { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion maximum personal usage limit
    /// </summary>
    /// <value>Gets or sets the value of promotion maximum personal usage limit</value>
    [DataMember(Name="maxPersonalUsageCount", EmitDefaultValue=false)]
    public int? MaxPersonalUsageCount { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of coupon codes
    /// </summary>
    /// <value>Gets or sets the collection of coupon codes</value>
    [DataMember(Name="coupons", EmitDefaultValue=false)]
    public List<string> Coupons { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion start date/time
    /// </summary>
    /// <value>Gets or sets the value of promotion start date/time</value>
    [DataMember(Name="startDate", EmitDefaultValue=false)]
    public DateTime? StartDate { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion end date/time
    /// </summary>
    /// <value>Gets or sets the value of promotion end date/time</value>
    [DataMember(Name="endDate", EmitDefaultValue=false)]
    public DateTime? EndDate { get; set; }

    
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
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelPromotion {\n");
      
      sb.Append("  Type: ").Append(Type).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Store: ").Append(Store).Append("\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  IsActive: ").Append(IsActive).Append("\n");
      
      sb.Append("  MaxUsageCount: ").Append(MaxUsageCount).Append("\n");
      
      sb.Append("  MaxPersonalUsageCount: ").Append(MaxPersonalUsageCount).Append("\n");
      
      sb.Append("  Coupons: ").Append(Coupons).Append("\n");
      
      sb.Append("  StartDate: ").Append(StartDate).Append("\n");
      
      sb.Append("  EndDate: ").Append(EndDate).Append("\n");
      
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
