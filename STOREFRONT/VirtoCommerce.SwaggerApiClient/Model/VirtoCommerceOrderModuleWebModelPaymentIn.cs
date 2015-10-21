using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Represent incoming payment operation
  /// </summary>
  [DataContract]
  public class VirtoCommerceOrderModuleWebModelPaymentIn {
    
    /// <summary>
    /// Customer organization
    /// </summary>
    /// <value>Customer organization</value>
    [DataMember(Name="organizationName", EmitDefaultValue=false)]
    public string OrganizationName { get; set; }

    
    /// <summary>
    /// Gets or Sets OrganizationId
    /// </summary>
    [DataMember(Name="organizationId", EmitDefaultValue=false)]
    public string OrganizationId { get; set; }

    
    /// <summary>
    /// Gets or Sets CustomerName
    /// </summary>
    [DataMember(Name="customerName", EmitDefaultValue=false)]
    public string CustomerName { get; set; }

    
    /// <summary>
    /// Gets or Sets CustomerId
    /// </summary>
    [DataMember(Name="customerId", EmitDefaultValue=false)]
    public string CustomerId { get; set; }

    
    /// <summary>
    /// Payment purpose text
    /// </summary>
    /// <value>Payment purpose text</value>
    [DataMember(Name="purpose", EmitDefaultValue=false)]
    public string Purpose { get; set; }

    
    /// <summary>
    /// Payment gateway code used for link with gateway provider realization
    /// </summary>
    /// <value>Payment gateway code used for link with gateway provider realization</value>
    [DataMember(Name="gatewayCode", EmitDefaultValue=false)]
    public string GatewayCode { get; set; }

    
    /// <summary>
    /// Expected date of receipt of payment
    /// </summary>
    /// <value>Expected date of receipt of payment</value>
    [DataMember(Name="incomingDate", EmitDefaultValue=false)]
    public DateTime? IncomingDate { get; set; }

    
    /// <summary>
    /// Outer id used for link with payment in external systems
    /// </summary>
    /// <value>Outer id used for link with payment in external systems</value>
    [DataMember(Name="outerId", EmitDefaultValue=false)]
    public string OuterId { get; set; }

    
    /// <summary>
    /// Operation type string representation (CustomerOrder, Shipment etc)
    /// </summary>
    /// <value>Operation type string representation (CustomerOrder, Shipment etc)</value>
    [DataMember(Name="operationType", EmitDefaultValue=false)]
    public string OperationType { get; set; }

    
    /// <summary>
    /// Unique user friendly document number (generate automatically based on special algorithm realization)
    /// </summary>
    /// <value>Unique user friendly document number (generate automatically based on special algorithm realization)</value>
    [DataMember(Name="number", EmitDefaultValue=false)]
    public string Number { get; set; }

    
    /// <summary>
    /// Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status\r\n            (Order completion, Shipment send etc)
    /// </summary>
    /// <value>Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status\r\n            (Order completion, Shipment send etc)</value>
    [DataMember(Name="isApproved", EmitDefaultValue=false)]
    public bool? IsApproved { get; set; }

    
    /// <summary>
    /// Current operation status may have any values defined by concrete business process
    /// </summary>
    /// <value>Current operation status may have any values defined by concrete business process</value>
    [DataMember(Name="status", EmitDefaultValue=false)]
    public string Status { get; set; }

    
    /// <summary>
    /// Gets or Sets Comment
    /// </summary>
    [DataMember(Name="comment", EmitDefaultValue=false)]
    public string Comment { get; set; }

    
    /// <summary>
    /// Currecy code
    /// </summary>
    /// <value>Currecy code</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or Sets TaxIncluded
    /// </summary>
    [DataMember(Name="taxIncluded", EmitDefaultValue=false)]
    public bool? TaxIncluded { get; set; }

    
    /// <summary>
    /// Money amount without tax
    /// </summary>
    /// <value>Money amount without tax</value>
    [DataMember(Name="sum", EmitDefaultValue=false)]
    public double? Sum { get; set; }

    
    /// <summary>
    /// Tax total
    /// </summary>
    /// <value>Tax total</value>
    [DataMember(Name="tax", EmitDefaultValue=false)]
    public double? Tax { get; set; }

    
    /// <summary>
    /// Gets or Sets IsCancelled
    /// </summary>
    [DataMember(Name="isCancelled", EmitDefaultValue=false)]
    public bool? IsCancelled { get; set; }

    
    /// <summary>
    /// Gets or Sets CancelledDate
    /// </summary>
    [DataMember(Name="cancelledDate", EmitDefaultValue=false)]
    public DateTime? CancelledDate { get; set; }

    
    /// <summary>
    /// Gets or Sets CancelReason
    /// </summary>
    [DataMember(Name="cancelReason", EmitDefaultValue=false)]
    public string CancelReason { get; set; }

    
    /// <summary>
    /// Used for construct hierarchy of operation and represent parent operation id
    /// </summary>
    /// <value>Used for construct hierarchy of operation and represent parent operation id</value>
    [DataMember(Name="parentOperationId", EmitDefaultValue=false)]
    public string ParentOperationId { get; set; }

    
    /// <summary>
    /// Gets or Sets ChildrenOperations
    /// </summary>
    [DataMember(Name="childrenOperations", EmitDefaultValue=false)]
    public List<VirtoCommerceOrderModuleWebModelOperation> ChildrenOperations { get; set; }

    
    /// <summary>
    /// Used for dynamic properties management, contains object type string
    /// </summary>
    /// <value>Used for dynamic properties management, contains object type string</value>
    [DataMember(Name="objectType", EmitDefaultValue=false)]
    public string ObjectType { get; set; }

    
    /// <summary>
    /// Dynamic properties collections
    /// </summary>
    /// <value>Dynamic properties collections</value>
    [DataMember(Name="dynamicProperties", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty> DynamicProperties { get; set; }

    
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
      sb.Append("class VirtoCommerceOrderModuleWebModelPaymentIn {\n");
      
      sb.Append("  OrganizationName: ").Append(OrganizationName).Append("\n");
      
      sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
      
      sb.Append("  CustomerName: ").Append(CustomerName).Append("\n");
      
      sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
      
      sb.Append("  Purpose: ").Append(Purpose).Append("\n");
      
      sb.Append("  GatewayCode: ").Append(GatewayCode).Append("\n");
      
      sb.Append("  IncomingDate: ").Append(IncomingDate).Append("\n");
      
      sb.Append("  OuterId: ").Append(OuterId).Append("\n");
      
      sb.Append("  OperationType: ").Append(OperationType).Append("\n");
      
      sb.Append("  Number: ").Append(Number).Append("\n");
      
      sb.Append("  IsApproved: ").Append(IsApproved).Append("\n");
      
      sb.Append("  Status: ").Append(Status).Append("\n");
      
      sb.Append("  Comment: ").Append(Comment).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  TaxIncluded: ").Append(TaxIncluded).Append("\n");
      
      sb.Append("  Sum: ").Append(Sum).Append("\n");
      
      sb.Append("  Tax: ").Append(Tax).Append("\n");
      
      sb.Append("  IsCancelled: ").Append(IsCancelled).Append("\n");
      
      sb.Append("  CancelledDate: ").Append(CancelledDate).Append("\n");
      
      sb.Append("  CancelReason: ").Append(CancelReason).Append("\n");
      
      sb.Append("  ParentOperationId: ").Append(ParentOperationId).Append("\n");
      
      sb.Append("  ChildrenOperations: ").Append(ChildrenOperations).Append("\n");
      
      sb.Append("  ObjectType: ").Append(ObjectType).Append("\n");
      
      sb.Append("  DynamicProperties: ").Append(DynamicProperties).Append("\n");
      
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
