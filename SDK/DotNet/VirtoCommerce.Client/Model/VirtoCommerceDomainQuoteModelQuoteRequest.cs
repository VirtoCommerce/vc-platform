using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VirtoCommerceDomainQuoteModelQuoteRequest : IEquatable<VirtoCommerceDomainQuoteModelQuoteRequest>
    {
        
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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
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
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommerceDomainQuoteModelQuoteRequest);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainQuoteModelQuoteRequest instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceDomainQuoteModelQuoteRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainQuoteModelQuoteRequest other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Number == other.Number ||
                    this.Number != null &&
                    this.Number.Equals(other.Number)
                ) && 
                (
                    this.StoreId == other.StoreId ||
                    this.StoreId != null &&
                    this.StoreId.Equals(other.StoreId)
                ) && 
                (
                    this.ChannelId == other.ChannelId ||
                    this.ChannelId != null &&
                    this.ChannelId.Equals(other.ChannelId)
                ) && 
                (
                    this.IsAnonymous == other.IsAnonymous ||
                    this.IsAnonymous != null &&
                    this.IsAnonymous.Equals(other.IsAnonymous)
                ) && 
                (
                    this.CustomerId == other.CustomerId ||
                    this.CustomerId != null &&
                    this.CustomerId.Equals(other.CustomerId)
                ) && 
                (
                    this.CustomerName == other.CustomerName ||
                    this.CustomerName != null &&
                    this.CustomerName.Equals(other.CustomerName)
                ) && 
                (
                    this.OrganizationName == other.OrganizationName ||
                    this.OrganizationName != null &&
                    this.OrganizationName.Equals(other.OrganizationName)
                ) && 
                (
                    this.OrganizationId == other.OrganizationId ||
                    this.OrganizationId != null &&
                    this.OrganizationId.Equals(other.OrganizationId)
                ) && 
                (
                    this.EmployeeId == other.EmployeeId ||
                    this.EmployeeId != null &&
                    this.EmployeeId.Equals(other.EmployeeId)
                ) && 
                (
                    this.EmployeeName == other.EmployeeName ||
                    this.EmployeeName != null &&
                    this.EmployeeName.Equals(other.EmployeeName)
                ) && 
                (
                    this.ExpirationDate == other.ExpirationDate ||
                    this.ExpirationDate != null &&
                    this.ExpirationDate.Equals(other.ExpirationDate)
                ) && 
                (
                    this.ReminderDate == other.ReminderDate ||
                    this.ReminderDate != null &&
                    this.ReminderDate.Equals(other.ReminderDate)
                ) && 
                (
                    this.EnableNotification == other.EnableNotification ||
                    this.EnableNotification != null &&
                    this.EnableNotification.Equals(other.EnableNotification)
                ) && 
                (
                    this.IsLocked == other.IsLocked ||
                    this.IsLocked != null &&
                    this.IsLocked.Equals(other.IsLocked)
                ) && 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
                ) && 
                (
                    this.Tag == other.Tag ||
                    this.Tag != null &&
                    this.Tag.Equals(other.Tag)
                ) && 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) && 
                (
                    this.InnerComment == other.InnerComment ||
                    this.InnerComment != null &&
                    this.InnerComment.Equals(other.InnerComment)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.Coupon == other.Coupon ||
                    this.Coupon != null &&
                    this.Coupon.Equals(other.Coupon)
                ) && 
                (
                    this.ManualShippingTotal == other.ManualShippingTotal ||
                    this.ManualShippingTotal != null &&
                    this.ManualShippingTotal.Equals(other.ManualShippingTotal)
                ) && 
                (
                    this.ManualSubTotal == other.ManualSubTotal ||
                    this.ManualSubTotal != null &&
                    this.ManualSubTotal.Equals(other.ManualSubTotal)
                ) && 
                (
                    this.ManualRelDiscountAmount == other.ManualRelDiscountAmount ||
                    this.ManualRelDiscountAmount != null &&
                    this.ManualRelDiscountAmount.Equals(other.ManualRelDiscountAmount)
                ) && 
                (
                    this.Totals == other.Totals ||
                    this.Totals != null &&
                    this.Totals.Equals(other.Totals)
                ) && 
                (
                    this.ShipmentMethod == other.ShipmentMethod ||
                    this.ShipmentMethod != null &&
                    this.ShipmentMethod.Equals(other.ShipmentMethod)
                ) && 
                (
                    this.Addresses == other.Addresses ||
                    this.Addresses != null &&
                    this.Addresses.SequenceEqual(other.Addresses)
                ) && 
                (
                    this.Items == other.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(other.Items)
                ) && 
                (
                    this.Attachments == other.Attachments ||
                    this.Attachments != null &&
                    this.Attachments.SequenceEqual(other.Attachments)
                ) && 
                (
                    this.OperationsLog == other.OperationsLog ||
                    this.OperationsLog != null &&
                    this.OperationsLog.SequenceEqual(other.OperationsLog)
                ) && 
                (
                    this.LanguageCode == other.LanguageCode ||
                    this.LanguageCode != null &&
                    this.LanguageCode.Equals(other.LanguageCode)
                ) && 
                (
                    this.TaxDetails == other.TaxDetails ||
                    this.TaxDetails != null &&
                    this.TaxDetails.SequenceEqual(other.TaxDetails)
                ) && 
                (
                    this.IsCancelled == other.IsCancelled ||
                    this.IsCancelled != null &&
                    this.IsCancelled.Equals(other.IsCancelled)
                ) && 
                (
                    this.CancelledDate == other.CancelledDate ||
                    this.CancelledDate != null &&
                    this.CancelledDate.Equals(other.CancelledDate)
                ) && 
                (
                    this.CancelReason == other.CancelReason ||
                    this.CancelReason != null &&
                    this.CancelReason.Equals(other.CancelReason)
                ) && 
                (
                    this.ObjectType == other.ObjectType ||
                    this.ObjectType != null &&
                    this.ObjectType.Equals(other.ObjectType)
                ) && 
                (
                    this.DynamicProperties == other.DynamicProperties ||
                    this.DynamicProperties != null &&
                    this.DynamicProperties.SequenceEqual(other.DynamicProperties)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                
                if (this.Number != null)
                    hash = hash * 57 + this.Number.GetHashCode();
                
                if (this.StoreId != null)
                    hash = hash * 57 + this.StoreId.GetHashCode();
                
                if (this.ChannelId != null)
                    hash = hash * 57 + this.ChannelId.GetHashCode();
                
                if (this.IsAnonymous != null)
                    hash = hash * 57 + this.IsAnonymous.GetHashCode();
                
                if (this.CustomerId != null)
                    hash = hash * 57 + this.CustomerId.GetHashCode();
                
                if (this.CustomerName != null)
                    hash = hash * 57 + this.CustomerName.GetHashCode();
                
                if (this.OrganizationName != null)
                    hash = hash * 57 + this.OrganizationName.GetHashCode();
                
                if (this.OrganizationId != null)
                    hash = hash * 57 + this.OrganizationId.GetHashCode();
                
                if (this.EmployeeId != null)
                    hash = hash * 57 + this.EmployeeId.GetHashCode();
                
                if (this.EmployeeName != null)
                    hash = hash * 57 + this.EmployeeName.GetHashCode();
                
                if (this.ExpirationDate != null)
                    hash = hash * 57 + this.ExpirationDate.GetHashCode();
                
                if (this.ReminderDate != null)
                    hash = hash * 57 + this.ReminderDate.GetHashCode();
                
                if (this.EnableNotification != null)
                    hash = hash * 57 + this.EnableNotification.GetHashCode();
                
                if (this.IsLocked != null)
                    hash = hash * 57 + this.IsLocked.GetHashCode();
                
                if (this.Status != null)
                    hash = hash * 57 + this.Status.GetHashCode();
                
                if (this.Tag != null)
                    hash = hash * 57 + this.Tag.GetHashCode();
                
                if (this.Comment != null)
                    hash = hash * 57 + this.Comment.GetHashCode();
                
                if (this.InnerComment != null)
                    hash = hash * 57 + this.InnerComment.GetHashCode();
                
                if (this.Currency != null)
                    hash = hash * 57 + this.Currency.GetHashCode();
                
                if (this.Coupon != null)
                    hash = hash * 57 + this.Coupon.GetHashCode();
                
                if (this.ManualShippingTotal != null)
                    hash = hash * 57 + this.ManualShippingTotal.GetHashCode();
                
                if (this.ManualSubTotal != null)
                    hash = hash * 57 + this.ManualSubTotal.GetHashCode();
                
                if (this.ManualRelDiscountAmount != null)
                    hash = hash * 57 + this.ManualRelDiscountAmount.GetHashCode();
                
                if (this.Totals != null)
                    hash = hash * 57 + this.Totals.GetHashCode();
                
                if (this.ShipmentMethod != null)
                    hash = hash * 57 + this.ShipmentMethod.GetHashCode();
                
                if (this.Addresses != null)
                    hash = hash * 57 + this.Addresses.GetHashCode();
                
                if (this.Items != null)
                    hash = hash * 57 + this.Items.GetHashCode();
                
                if (this.Attachments != null)
                    hash = hash * 57 + this.Attachments.GetHashCode();
                
                if (this.OperationsLog != null)
                    hash = hash * 57 + this.OperationsLog.GetHashCode();
                
                if (this.LanguageCode != null)
                    hash = hash * 57 + this.LanguageCode.GetHashCode();
                
                if (this.TaxDetails != null)
                    hash = hash * 57 + this.TaxDetails.GetHashCode();
                
                if (this.IsCancelled != null)
                    hash = hash * 57 + this.IsCancelled.GetHashCode();
                
                if (this.CancelledDate != null)
                    hash = hash * 57 + this.CancelledDate.GetHashCode();
                
                if (this.CancelReason != null)
                    hash = hash * 57 + this.CancelReason.GetHashCode();
                
                if (this.ObjectType != null)
                    hash = hash * 57 + this.ObjectType.GetHashCode();
                
                if (this.DynamicProperties != null)
                    hash = hash * 57 + this.DynamicProperties.GetHashCode();
                
                if (this.CreatedDate != null)
                    hash = hash * 57 + this.CreatedDate.GetHashCode();
                
                if (this.ModifiedDate != null)
                    hash = hash * 57 + this.ModifiedDate.GetHashCode();
                
                if (this.CreatedBy != null)
                    hash = hash * 57 + this.CreatedBy.GetHashCode();
                
                if (this.ModifiedBy != null)
                    hash = hash * 57 + this.ModifiedBy.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                return hash;
            }
        }

    }


}
