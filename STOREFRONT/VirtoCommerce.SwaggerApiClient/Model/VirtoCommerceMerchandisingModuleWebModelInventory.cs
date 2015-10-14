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
  public class VirtoCommerceMerchandisingModuleWebModelInventory {
    
    /// <summary>
    /// Gets or sets the value of fulfillment canter id
    /// </summary>
    /// <value>Gets or sets the value of fulfillment canter id</value>
    [DataMember(Name="fulfillmentCenterId", EmitDefaultValue=false)]
    public string FulfillmentCenterId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of inventory quantity in stock
    /// </summary>
    /// <value>Gets or sets the value of inventory quantity in stock</value>
    [DataMember(Name="inStockQuantity", EmitDefaultValue=false)]
    public long? InStockQuantity { get; set; }

    
    /// <summary>
    /// Gets or sets the value of reserved inventory quantity
    /// </summary>
    /// <value>Gets or sets the value of reserved inventory quantity</value>
    [DataMember(Name="reservedQuantity", EmitDefaultValue=false)]
    public long? ReservedQuantity { get; set; }

    
    /// <summary>
    /// Gets or sets the value of reorder inventory minimum quanitity
    /// </summary>
    /// <value>Gets or sets the value of reorder inventory minimum quanitity</value>
    [DataMember(Name="reorderMinQuantity", EmitDefaultValue=false)]
    public long? ReorderMinQuantity { get; set; }

    
    /// <summary>
    /// Gets or sets the value of preorder inventory quantity
    /// </summary>
    /// <value>Gets or sets the value of preorder inventory quantity</value>
    [DataMember(Name="preorderQuantity", EmitDefaultValue=false)]
    public long? PreorderQuantity { get; set; }

    
    /// <summary>
    /// Gets or sets the value of backorder inventory quantity
    /// </summary>
    /// <value>Gets or sets the value of backorder inventory quantity</value>
    [DataMember(Name="backorderQuantity", EmitDefaultValue=false)]
    public long? BackorderQuantity { get; set; }

    
    /// <summary>
    /// Gets or sets the flag of backorder is allowed
    /// </summary>
    /// <value>Gets or sets the flag of backorder is allowed</value>
    [DataMember(Name="allowBackorder", EmitDefaultValue=false)]
    public bool? AllowBackorder { get; set; }

    
    /// <summary>
    /// Gets or sets the flag of preorder is allowed
    /// </summary>
    /// <value>Gets or sets the flag of preorder is allowed</value>
    [DataMember(Name="allowPreorder", EmitDefaultValue=false)]
    public bool? AllowPreorder { get; set; }

    
    /// <summary>
    /// Gets or sets the value for inventory quantity in transit
    /// </summary>
    /// <value>Gets or sets the value for inventory quantity in transit</value>
    [DataMember(Name="inTransit", EmitDefaultValue=false)]
    public long? InTransit { get; set; }

    
    /// <summary>
    /// Gets or sets the value of date/time limit for preorder availability
    /// </summary>
    /// <value>Gets or sets the value of date/time limit for preorder availability</value>
    [DataMember(Name="preorderAvailabilityDate", EmitDefaultValue=false)]
    public DateTime? PreorderAvailabilityDate { get; set; }

    
    /// <summary>
    /// Gets or sets the value of date/time limit for backorder availability
    /// </summary>
    /// <value>Gets or sets the value of date/time limit for backorder availability</value>
    [DataMember(Name="backorderAvailabilityDate", EmitDefaultValue=false)]
    public DateTime? BackorderAvailabilityDate { get; set; }

    
    /// <summary>
    /// Gets or sets the value of inventory status
    /// </summary>
    /// <value>Gets or sets the value of inventory status</value>
    [DataMember(Name="status", EmitDefaultValue=false)]
    public int? Status { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelInventory {\n");
      
      sb.Append("  FulfillmentCenterId: ").Append(FulfillmentCenterId).Append("\n");
      
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
