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
  public class VirtoCommerceDomainQuoteModelQuoteRequest {
    
    /// <summary>
    /// Gets or Sets Number
    /// </summary>
    [DataMember(Name="number", EmitDefaultValue=false)]
    public string Number { get; set; }

    
    /// <summary>
    /// Gets or Sets StoreId
    /// </summary>
    [DataMember(Name="storeId", EmitDefaultValue=false)]
    public string StoreId { get; set; }

    
    /// <summary>
    /// Gets or Sets ChannelId
    /// </summary>
    [DataMember(Name="channelId", EmitDefaultValue=false)]
    public string ChannelId { get; set; }

    
    /// <summary>
    /// Gets or Sets IsAnonymous
    /// </summary>
    [DataMember(Name="isAnonymous", EmitDefaultValue=false)]
    public bool? IsAnonymous { get; set; }

    
    /// <summary>
    /// Gets or Sets CustomerId
    /// </summary>
    [DataMember(Name="customerId", EmitDefaultValue=false)]
    public string CustomerId { get; set; }

    
    /// <summary>
    /// Gets or Sets CustomerName
    /// </summary>
    [DataMember(Name="customerName", EmitDefaultValue=false)]
    public string CustomerName { get; set; }

    
    /// <summary>
    /// Gets or Sets OrganizationName
    /// </summary>
    [DataMember(Name="organizationName", EmitDefaultValue=false)]
    public string OrganizationName { get; set; }

    
    /// <summary>
    /// Gets or Sets OrganizationId
    /// </summary>
    [DataMember(Name="organizationId", EmitDefaultValue=false)]
    public string OrganizationId { get; set; }

    
    /// <summary>
    /// Gets or Sets EmployeeId
    /// </summary>
    [DataMember(Name="employeeId", EmitDefaultValue=false)]
    public string EmployeeId { get; set; }

    
    /// <summary>
    /// Gets or Sets EmployeeName
    /// </summary>
    [DataMember(Name="employeeName", EmitDefaultValue=false)]
    public string EmployeeName { get; set; }

    
    /// <summary>
    /// Gets or Sets ExpirationDate
    /// </summary>
    [DataMember(Name="expirationDate", EmitDefaultValue=false)]
    public DateTime? ExpirationDate { get; set; }

    
    /// <summary>
    /// Gets or Sets ReminderDate
    /// </summary>
    [DataMember(Name="reminderDate", EmitDefaultValue=false)]
    public DateTime? ReminderDate { get; set; }

    
    /// <summary>
    /// Gets or Sets EnableNotification
    /// </summary>
    [DataMember(Name="enableNotification", EmitDefaultValue=false)]
    public bool? EnableNotification { get; set; }

    
    /// <summary>
    /// Gets or Sets IsLocked
    /// </summary>
    [DataMember(Name="isLocked", EmitDefaultValue=false)]
    public bool? IsLocked { get; set; }

    
    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    public string Status { get; set; }

    
    /// <summary>
    /// Gets or Sets Tag
    /// </summary>
    [DataMember(Name="tag", EmitDefaultValue=false)]
    public string Tag { get; set; }

    
    /// <summary>
    /// Gets or Sets Comment
    /// </summary>
    [DataMember(Name="comment", EmitDefaultValue=false)]
    public string Comment { get; set; }

    
    /// <summary>
    /// Gets or Sets InnerComment
    /// </summary>
    [DataMember(Name="innerComment", EmitDefaultValue=false)]
    public string InnerComment { get; set; }

    
    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or Sets Coupon
    /// </summary>
    [DataMember(Name="coupon", EmitDefaultValue=false)]
    public string Coupon { get; set; }

    
    /// <summary>
    /// Gets or Sets ManualShippingTotal
    /// </summary>
    [DataMember(Name="manualShippingTotal", EmitDefaultValue=false)]
    public double? ManualShippingTotal { get; set; }

    
    /// <summary>
    /// Gets or Sets ManualSubTotal
    /// </summary>
    [DataMember(Name="manualSubTotal", EmitDefaultValue=false)]
    public double? ManualSubTotal { get; set; }

    
    /// <summary>
    /// Gets or Sets ManualRelDiscountAmount
    /// </summary>
    [DataMember(Name="manualRelDiscountAmount", EmitDefaultValue=false)]
    public double? ManualRelDiscountAmount { get; set; }

    
    /// <summary>
    /// Gets or Sets Totals
    /// </summary>
    [DataMember(Name="totals", EmitDefaultValue=false)]
    public VirtoCommerceDomainQuoteModelQuoteRequestTotals Totals { get; set; }

    
    /// <summary>
    /// Gets or Sets ShipmentMethod
    /// </summary>
    [DataMember(Name="shipmentMethod", EmitDefaultValue=false)]
    public VirtoCommerceDomainQuoteModelShipmentMethod ShipmentMethod { get; set; }

    
    /// <summary>
    /// Gets or Sets Addresses
    /// </summary>
    [DataMember(Name="addresses", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCommerceModelAddress> Addresses { get; set; }

    
    /// <summary>
    /// Gets or Sets Items
    /// </summary>
    [DataMember(Name="items", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainQuoteModelQuoteItem> Items { get; set; }

    
    /// <summary>
    /// Gets or Sets Attachments
    /// </summary>
    [DataMember(Name="attachments", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainQuoteModelQuoteAttachment> Attachments { get; set; }

    
    /// <summary>
    /// Gets or Sets OperationsLog
    /// </summary>
    [DataMember(Name="operationsLog", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreChangeLogOperationLog> OperationsLog { get; set; }

    
    /// <summary>
    /// Gets or Sets LanguageCode
    /// </summary>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    
    /// <summary>
    /// Gets or Sets TaxDetails
    /// </summary>
    [DataMember(Name="taxDetails", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCommerceModelTaxDetail> TaxDetails { get; set; }

    
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
    /// Gets or Sets ObjectType
    /// </summary>
    [DataMember(Name="objectType", EmitDefaultValue=false)]
    public string ObjectType { get; set; }

    
    /// <summary>
    /// Gets or Sets DynamicProperties
    /// </summary>
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
      sb.Append("class VirtoCommerceDomainQuoteModelQuoteRequest {\n");
      
      sb.Append("  Number: ").Append(Number).Append("\n");
      
      sb.Append("  StoreId: ").Append(StoreId).Append("\n");
      
      sb.Append("  ChannelId: ").Append(ChannelId).Append("\n");
      
      sb.Append("  IsAnonymous: ").Append(IsAnonymous).Append("\n");
      
      sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
      
      sb.Append("  CustomerName: ").Append(CustomerName).Append("\n");
      
      sb.Append("  OrganizationName: ").Append(OrganizationName).Append("\n");
      
      sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
      
      sb.Append("  EmployeeId: ").Append(EmployeeId).Append("\n");
      
      sb.Append("  EmployeeName: ").Append(EmployeeName).Append("\n");
      
      sb.Append("  ExpirationDate: ").Append(ExpirationDate).Append("\n");
      
      sb.Append("  ReminderDate: ").Append(ReminderDate).Append("\n");
      
      sb.Append("  EnableNotification: ").Append(EnableNotification).Append("\n");
      
      sb.Append("  IsLocked: ").Append(IsLocked).Append("\n");
      
      sb.Append("  Status: ").Append(Status).Append("\n");
      
      sb.Append("  Tag: ").Append(Tag).Append("\n");
      
      sb.Append("  Comment: ").Append(Comment).Append("\n");
      
      sb.Append("  InnerComment: ").Append(InnerComment).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  Coupon: ").Append(Coupon).Append("\n");
      
      sb.Append("  ManualShippingTotal: ").Append(ManualShippingTotal).Append("\n");
      
      sb.Append("  ManualSubTotal: ").Append(ManualSubTotal).Append("\n");
      
      sb.Append("  ManualRelDiscountAmount: ").Append(ManualRelDiscountAmount).Append("\n");
      
      sb.Append("  Totals: ").Append(Totals).Append("\n");
      
      sb.Append("  ShipmentMethod: ").Append(ShipmentMethod).Append("\n");
      
      sb.Append("  Addresses: ").Append(Addresses).Append("\n");
      
      sb.Append("  Items: ").Append(Items).Append("\n");
      
      sb.Append("  Attachments: ").Append(Attachments).Append("\n");
      
      sb.Append("  OperationsLog: ").Append(OperationsLog).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
      sb.Append("  TaxDetails: ").Append(TaxDetails).Append("\n");
      
      sb.Append("  IsCancelled: ").Append(IsCancelled).Append("\n");
      
      sb.Append("  CancelledDate: ").Append(CancelledDate).Append("\n");
      
      sb.Append("  CancelReason: ").Append(CancelReason).Append("\n");
      
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
