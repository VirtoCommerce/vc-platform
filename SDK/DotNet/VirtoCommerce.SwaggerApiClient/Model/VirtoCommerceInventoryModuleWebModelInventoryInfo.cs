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
  public class VirtoCommerceInventoryModuleWebModelInventoryInfo {
    
    /// <summary>
    /// Gets or Sets CreatedDate
    /// </summary>
    [DataMember(Name="createdDate", EmitDefaultValue=false)]
    public DateTime? CreatedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets CreatedBy
    /// </summary>
    [DataMember(Name="createdBy", EmitDefaultValue=false)]
    public string CreatedBy { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedDate
    /// </summary>
    [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
    public DateTime? ModifiedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedBy
    /// </summary>
    [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
    public string ModifiedBy { get; set; }

    
    /// <summary>
    /// Gets or Sets FulfillmentCenter
    /// </summary>
    [DataMember(Name="fulfillmentCenter", EmitDefaultValue=false)]
    public VirtoCommerceInventoryModuleWebModelFulfillmentCenter FulfillmentCenter { get; set; }

    
    /// <summary>
    /// Gets or Sets FulfillmentCenterId
    /// </summary>
    [DataMember(Name="fulfillmentCenterId", EmitDefaultValue=false)]
    public string FulfillmentCenterId { get; set; }

    
    /// <summary>
    /// Gets or Sets ProductId
    /// </summary>
    [DataMember(Name="productId", EmitDefaultValue=false)]
    public string ProductId { get; set; }

    
    /// <summary>
    /// Gets or Sets InStockQuantity
    /// </summary>
    [DataMember(Name="inStockQuantity", EmitDefaultValue=false)]
    public long? InStockQuantity { get; set; }

    
    /// <summary>
    /// Gets or Sets ReservedQuantity
    /// </summary>
    [DataMember(Name="reservedQuantity", EmitDefaultValue=false)]
    public long? ReservedQuantity { get; set; }

    
    /// <summary>
    /// Gets or Sets ReorderMinQuantity
    /// </summary>
    [DataMember(Name="reorderMinQuantity", EmitDefaultValue=false)]
    public long? ReorderMinQuantity { get; set; }

    
    /// <summary>
    /// Gets or Sets PreorderQuantity
    /// </summary>
    [DataMember(Name="preorderQuantity", EmitDefaultValue=false)]
    public long? PreorderQuantity { get; set; }

    
    /// <summary>
    /// Gets or Sets BackorderQuantity
    /// </summary>
    [DataMember(Name="backorderQuantity", EmitDefaultValue=false)]
    public long? BackorderQuantity { get; set; }

    
    /// <summary>
    /// Gets or Sets AllowBackorder
    /// </summary>
    [DataMember(Name="allowBackorder", EmitDefaultValue=false)]
    public bool? AllowBackorder { get; set; }

    
    /// <summary>
    /// Gets or Sets AllowPreorder
    /// </summary>
    [DataMember(Name="allowPreorder", EmitDefaultValue=false)]
    public bool? AllowPreorder { get; set; }

    
    /// <summary>
    /// Gets or Sets InTransit
    /// </summary>
    [DataMember(Name="inTransit", EmitDefaultValue=false)]
    public long? InTransit { get; set; }

    
    /// <summary>
    /// Gets or Sets PreorderAvailabilityDate
    /// </summary>
    [DataMember(Name="preorderAvailabilityDate", EmitDefaultValue=false)]
    public DateTime? PreorderAvailabilityDate { get; set; }

    
    /// <summary>
    /// Gets or Sets BackorderAvailabilityDate
    /// </summary>
    [DataMember(Name="backorderAvailabilityDate", EmitDefaultValue=false)]
    public DateTime? BackorderAvailabilityDate { get; set; }

    
    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    public string Status { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceInventoryModuleWebModelInventoryInfo {\n");
      
      sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
      
      sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
      
      sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
      
      sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
      
      sb.Append("  FulfillmentCenter: ").Append(FulfillmentCenter).Append("\n");
      
      sb.Append("  FulfillmentCenterId: ").Append(FulfillmentCenterId).Append("\n");
      
      sb.Append("  ProductId: ").Append(ProductId).Append("\n");
      
      sb.Append("  InStockQuantity: ").Append(InStockQuantity).Append("\n");
      
      sb.Append("  ReservedQuantity: ").Append(ReservedQuantity).Append("\n");
      
      sb.Append("  ReorderMinQuantity: ").Append(ReorderMinQuantity).Append("\n");
      
      sb.Append("  PreorderQuantity: ").Append(PreorderQuantity).Append("\n");
      
      sb.Append("  BackorderQuantity: ").Append(BackorderQuantity).Append("\n");
      
      sb.Append("  AllowBackorder: ").Append(AllowBackorder).Append("\n");
      
      sb.Append("  AllowPreorder: ").Append(AllowPreorder).Append("\n");
      
      sb.Append("  InTransit: ").Append(InTransit).Append("\n");
      
      sb.Append("  PreorderAvailabilityDate: ").Append(PreorderAvailabilityDate).Append("\n");
      
      sb.Append("  BackorderAvailabilityDate: ").Append(BackorderAvailabilityDate).Append("\n");
      
      sb.Append("  Status: ").Append(Status).Append("\n");
      
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
