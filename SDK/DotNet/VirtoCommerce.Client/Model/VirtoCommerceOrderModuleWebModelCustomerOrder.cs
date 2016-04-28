using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// Represent customer order
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceOrderModuleWebModelCustomerOrder :  IEquatable<VirtoCommerceOrderModuleWebModelCustomerOrder>
    {
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
        /// Chanel (Web site, mobile application etc)
        /// </summary>
        /// <value>Chanel (Web site, mobile application etc)</value>
        [DataMember(Name="channelId", EmitDefaultValue=false)]
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or Sets StoreId
        /// </summary>
        [DataMember(Name="storeId", EmitDefaultValue=false)]
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or Sets StoreName
        /// </summary>
        [DataMember(Name="storeName", EmitDefaultValue=false)]
        public string StoreName { get; set; }

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
        /// Employee who should handle that order
        /// </summary>
        /// <value>Employee who should handle that order</value>
        [DataMember(Name="employeeName", EmitDefaultValue=false)]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Gets or Sets EmployeeId
        /// </summary>
        [DataMember(Name="employeeId", EmitDefaultValue=false)]
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or Sets DiscountAmount
        /// </summary>
        [DataMember(Name="discountAmount", EmitDefaultValue=false)]
        public double? DiscountAmount { get; set; }

        /// <summary>
        /// All shipping and billing order addresses
        /// </summary>
        /// <value>All shipping and billing order addresses</value>
        [DataMember(Name="addresses", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelAddress> Addresses { get; set; }

        /// <summary>
        /// Incoming payments operations
        /// </summary>
        /// <value>Incoming payments operations</value>
        [DataMember(Name="inPayments", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelPaymentIn> InPayments { get; set; }

        /// <summary>
        /// All customer order line items
        /// </summary>
        /// <value>All customer order line items</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelLineItem> Items { get; set; }

        /// <summary>
        /// All customer order shipments
        /// </summary>
        /// <value>All customer order shipments</value>
        [DataMember(Name="shipments", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelShipment> Shipments { get; set; }

        /// <summary>
        /// All customer order discount
        /// </summary>
        /// <value>All customer order discount</value>
        [DataMember(Name="discount", EmitDefaultValue=false)]
        public VirtoCommerceOrderModuleWebModelDiscount Discount { get; set; }

        /// <summary>
        /// Tax details
        /// </summary>
        /// <value>Tax details</value>
        [DataMember(Name="taxDetails", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelTaxDetail> TaxDetails { get; set; }

        /// <summary>
        /// Security permission scopes used for security check on UI
        /// </summary>
        /// <value>Security permission scopes used for security check on UI</value>
        [DataMember(Name="scopes", EmitDefaultValue=false)]
        public List<string> Scopes { get; set; }

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
        /// Currency code
        /// </summary>
        /// <value>Currency code</value>
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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceOrderModuleWebModelCustomerOrder {\n");
            sb.Append("  CustomerName: ").Append(CustomerName).Append("\n");
            sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
            sb.Append("  ChannelId: ").Append(ChannelId).Append("\n");
            sb.Append("  StoreId: ").Append(StoreId).Append("\n");
            sb.Append("  StoreName: ").Append(StoreName).Append("\n");
            sb.Append("  OrganizationName: ").Append(OrganizationName).Append("\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
            sb.Append("  EmployeeName: ").Append(EmployeeName).Append("\n");
            sb.Append("  EmployeeId: ").Append(EmployeeId).Append("\n");
            sb.Append("  DiscountAmount: ").Append(DiscountAmount).Append("\n");
            sb.Append("  Addresses: ").Append(Addresses).Append("\n");
            sb.Append("  InPayments: ").Append(InPayments).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  Shipments: ").Append(Shipments).Append("\n");
            sb.Append("  Discount: ").Append(Discount).Append("\n");
            sb.Append("  TaxDetails: ").Append(TaxDetails).Append("\n");
            sb.Append("  Scopes: ").Append(Scopes).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelCustomerOrder);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelCustomerOrder instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelCustomerOrder to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelCustomerOrder other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.CustomerName == other.CustomerName ||
                    this.CustomerName != null &&
                    this.CustomerName.Equals(other.CustomerName)
                ) && 
                (
                    this.CustomerId == other.CustomerId ||
                    this.CustomerId != null &&
                    this.CustomerId.Equals(other.CustomerId)
                ) && 
                (
                    this.ChannelId == other.ChannelId ||
                    this.ChannelId != null &&
                    this.ChannelId.Equals(other.ChannelId)
                ) && 
                (
                    this.StoreId == other.StoreId ||
                    this.StoreId != null &&
                    this.StoreId.Equals(other.StoreId)
                ) && 
                (
                    this.StoreName == other.StoreName ||
                    this.StoreName != null &&
                    this.StoreName.Equals(other.StoreName)
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
                    this.EmployeeName == other.EmployeeName ||
                    this.EmployeeName != null &&
                    this.EmployeeName.Equals(other.EmployeeName)
                ) && 
                (
                    this.EmployeeId == other.EmployeeId ||
                    this.EmployeeId != null &&
                    this.EmployeeId.Equals(other.EmployeeId)
                ) && 
                (
                    this.DiscountAmount == other.DiscountAmount ||
                    this.DiscountAmount != null &&
                    this.DiscountAmount.Equals(other.DiscountAmount)
                ) && 
                (
                    this.Addresses == other.Addresses ||
                    this.Addresses != null &&
                    this.Addresses.SequenceEqual(other.Addresses)
                ) && 
                (
                    this.InPayments == other.InPayments ||
                    this.InPayments != null &&
                    this.InPayments.SequenceEqual(other.InPayments)
                ) && 
                (
                    this.Items == other.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(other.Items)
                ) && 
                (
                    this.Shipments == other.Shipments ||
                    this.Shipments != null &&
                    this.Shipments.SequenceEqual(other.Shipments)
                ) && 
                (
                    this.Discount == other.Discount ||
                    this.Discount != null &&
                    this.Discount.Equals(other.Discount)
                ) && 
                (
                    this.TaxDetails == other.TaxDetails ||
                    this.TaxDetails != null &&
                    this.TaxDetails.SequenceEqual(other.TaxDetails)
                ) && 
                (
                    this.Scopes == other.Scopes ||
                    this.Scopes != null &&
                    this.Scopes.SequenceEqual(other.Scopes)
                ) && 
                (
                    this.OperationType == other.OperationType ||
                    this.OperationType != null &&
                    this.OperationType.Equals(other.OperationType)
                ) && 
                (
                    this.Number == other.Number ||
                    this.Number != null &&
                    this.Number.Equals(other.Number)
                ) && 
                (
                    this.IsApproved == other.IsApproved ||
                    this.IsApproved != null &&
                    this.IsApproved.Equals(other.IsApproved)
                ) && 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
                ) && 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.TaxIncluded == other.TaxIncluded ||
                    this.TaxIncluded != null &&
                    this.TaxIncluded.Equals(other.TaxIncluded)
                ) && 
                (
                    this.Sum == other.Sum ||
                    this.Sum != null &&
                    this.Sum.Equals(other.Sum)
                ) && 
                (
                    this.Tax == other.Tax ||
                    this.Tax != null &&
                    this.Tax.Equals(other.Tax)
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
                    this.ParentOperationId == other.ParentOperationId ||
                    this.ParentOperationId != null &&
                    this.ParentOperationId.Equals(other.ParentOperationId)
                ) && 
                (
                    this.ChildrenOperations == other.ChildrenOperations ||
                    this.ChildrenOperations != null &&
                    this.ChildrenOperations.SequenceEqual(other.ChildrenOperations)
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

                if (this.CustomerName != null)
                    hash = hash * 59 + this.CustomerName.GetHashCode();

                if (this.CustomerId != null)
                    hash = hash * 59 + this.CustomerId.GetHashCode();

                if (this.ChannelId != null)
                    hash = hash * 59 + this.ChannelId.GetHashCode();

                if (this.StoreId != null)
                    hash = hash * 59 + this.StoreId.GetHashCode();

                if (this.StoreName != null)
                    hash = hash * 59 + this.StoreName.GetHashCode();

                if (this.OrganizationName != null)
                    hash = hash * 59 + this.OrganizationName.GetHashCode();

                if (this.OrganizationId != null)
                    hash = hash * 59 + this.OrganizationId.GetHashCode();

                if (this.EmployeeName != null)
                    hash = hash * 59 + this.EmployeeName.GetHashCode();

                if (this.EmployeeId != null)
                    hash = hash * 59 + this.EmployeeId.GetHashCode();

                if (this.DiscountAmount != null)
                    hash = hash * 59 + this.DiscountAmount.GetHashCode();

                if (this.Addresses != null)
                    hash = hash * 59 + this.Addresses.GetHashCode();

                if (this.InPayments != null)
                    hash = hash * 59 + this.InPayments.GetHashCode();

                if (this.Items != null)
                    hash = hash * 59 + this.Items.GetHashCode();

                if (this.Shipments != null)
                    hash = hash * 59 + this.Shipments.GetHashCode();

                if (this.Discount != null)
                    hash = hash * 59 + this.Discount.GetHashCode();

                if (this.TaxDetails != null)
                    hash = hash * 59 + this.TaxDetails.GetHashCode();

                if (this.Scopes != null)
                    hash = hash * 59 + this.Scopes.GetHashCode();

                if (this.OperationType != null)
                    hash = hash * 59 + this.OperationType.GetHashCode();

                if (this.Number != null)
                    hash = hash * 59 + this.Number.GetHashCode();

                if (this.IsApproved != null)
                    hash = hash * 59 + this.IsApproved.GetHashCode();

                if (this.Status != null)
                    hash = hash * 59 + this.Status.GetHashCode();

                if (this.Comment != null)
                    hash = hash * 59 + this.Comment.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.TaxIncluded != null)
                    hash = hash * 59 + this.TaxIncluded.GetHashCode();

                if (this.Sum != null)
                    hash = hash * 59 + this.Sum.GetHashCode();

                if (this.Tax != null)
                    hash = hash * 59 + this.Tax.GetHashCode();

                if (this.IsCancelled != null)
                    hash = hash * 59 + this.IsCancelled.GetHashCode();

                if (this.CancelledDate != null)
                    hash = hash * 59 + this.CancelledDate.GetHashCode();

                if (this.CancelReason != null)
                    hash = hash * 59 + this.CancelReason.GetHashCode();

                if (this.ParentOperationId != null)
                    hash = hash * 59 + this.ParentOperationId.GetHashCode();

                if (this.ChildrenOperations != null)
                    hash = hash * 59 + this.ChildrenOperations.GetHashCode();

                if (this.ObjectType != null)
                    hash = hash * 59 + this.ObjectType.GetHashCode();

                if (this.DynamicProperties != null)
                    hash = hash * 59 + this.DynamicProperties.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
