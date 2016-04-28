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
    /// Represent customer order shipment operation (document)\r\n            contains information as delivery address, items, dimensions etc.
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceOrderModuleWebModelShipment :  IEquatable<VirtoCommerceOrderModuleWebModelShipment>
    {
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
        /// Fulfillment center where shipment will be handled
        /// </summary>
        /// <value>Fulfillment center where shipment will be handled</value>
        [DataMember(Name="fulfillmentCenterName", EmitDefaultValue=false)]
        public string FulfillmentCenterName { get; set; }

        /// <summary>
        /// Gets or Sets FulfillmentCenterId
        /// </summary>
        [DataMember(Name="fulfillmentCenterId", EmitDefaultValue=false)]
        public string FulfillmentCenterId { get; set; }

        /// <summary>
        /// Selected shipping method to deliver current shipment
        /// </summary>
        /// <value>Selected shipping method to deliver current shipment</value>
        [DataMember(Name="shippingMethod", EmitDefaultValue=false)]
        public VirtoCommerceOrderModuleWebModelShippingMethod ShippingMethod { get; set; }

        /// <summary>
        /// Employee who responsible for handling current shipment
        /// </summary>
        /// <value>Employee who responsible for handling current shipment</value>
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
        /// Gets or Sets WeightUnit
        /// </summary>
        [DataMember(Name="weightUnit", EmitDefaultValue=false)]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [DataMember(Name="weight", EmitDefaultValue=false)]
        public double? Weight { get; set; }

        /// <summary>
        /// Gets or Sets MeasureUnit
        /// </summary>
        [DataMember(Name="measureUnit", EmitDefaultValue=false)]
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or Sets Height
        /// </summary>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public double? Height { get; set; }

        /// <summary>
        /// Gets or Sets Length
        /// </summary>
        [DataMember(Name="length", EmitDefaultValue=false)]
        public double? Length { get; set; }

        /// <summary>
        /// Gets or Sets Width
        /// </summary>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public double? Width { get; set; }

        /// <summary>
        /// Gets or Sets TaxType
        /// </summary>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }

        /// <summary>
        /// Information about quantity and order items belongs to current shipment
        /// </summary>
        /// <value>Information about quantity and order items belongs to current shipment</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelShipmentItem> Items { get; set; }

        /// <summary>
        /// Information about packages belongs to current shipment
        /// </summary>
        /// <value>Information about packages belongs to current shipment</value>
        [DataMember(Name="packages", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelShipmentPackage> Packages { get; set; }

        /// <summary>
        /// Gets or Sets InPayments
        /// </summary>
        [DataMember(Name="inPayments", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelPaymentIn> InPayments { get; set; }

        /// <summary>
        /// Gets or Sets DeliveryAddress
        /// </summary>
        [DataMember(Name="deliveryAddress", EmitDefaultValue=false)]
        public VirtoCommerceOrderModuleWebModelAddress DeliveryAddress { get; set; }

        /// <summary>
        /// Gets or Sets Discount
        /// </summary>
        [DataMember(Name="discount", EmitDefaultValue=false)]
        public VirtoCommerceOrderModuleWebModelDiscount Discount { get; set; }

        /// <summary>
        /// Gets or Sets TaxDetails
        /// </summary>
        [DataMember(Name="taxDetails", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelTaxDetail> TaxDetails { get; set; }

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
            sb.Append("class VirtoCommerceOrderModuleWebModelShipment {\n");
            sb.Append("  OrganizationName: ").Append(OrganizationName).Append("\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
            sb.Append("  FulfillmentCenterName: ").Append(FulfillmentCenterName).Append("\n");
            sb.Append("  FulfillmentCenterId: ").Append(FulfillmentCenterId).Append("\n");
            sb.Append("  ShippingMethod: ").Append(ShippingMethod).Append("\n");
            sb.Append("  EmployeeName: ").Append(EmployeeName).Append("\n");
            sb.Append("  EmployeeId: ").Append(EmployeeId).Append("\n");
            sb.Append("  DiscountAmount: ").Append(DiscountAmount).Append("\n");
            sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
            sb.Append("  Weight: ").Append(Weight).Append("\n");
            sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  Packages: ").Append(Packages).Append("\n");
            sb.Append("  InPayments: ").Append(InPayments).Append("\n");
            sb.Append("  DeliveryAddress: ").Append(DeliveryAddress).Append("\n");
            sb.Append("  Discount: ").Append(Discount).Append("\n");
            sb.Append("  TaxDetails: ").Append(TaxDetails).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelShipment);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelShipment instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelShipment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelShipment other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
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
                    this.FulfillmentCenterName == other.FulfillmentCenterName ||
                    this.FulfillmentCenterName != null &&
                    this.FulfillmentCenterName.Equals(other.FulfillmentCenterName)
                ) && 
                (
                    this.FulfillmentCenterId == other.FulfillmentCenterId ||
                    this.FulfillmentCenterId != null &&
                    this.FulfillmentCenterId.Equals(other.FulfillmentCenterId)
                ) && 
                (
                    this.ShippingMethod == other.ShippingMethod ||
                    this.ShippingMethod != null &&
                    this.ShippingMethod.Equals(other.ShippingMethod)
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
                    this.WeightUnit == other.WeightUnit ||
                    this.WeightUnit != null &&
                    this.WeightUnit.Equals(other.WeightUnit)
                ) && 
                (
                    this.Weight == other.Weight ||
                    this.Weight != null &&
                    this.Weight.Equals(other.Weight)
                ) && 
                (
                    this.MeasureUnit == other.MeasureUnit ||
                    this.MeasureUnit != null &&
                    this.MeasureUnit.Equals(other.MeasureUnit)
                ) && 
                (
                    this.Height == other.Height ||
                    this.Height != null &&
                    this.Height.Equals(other.Height)
                ) && 
                (
                    this.Length == other.Length ||
                    this.Length != null &&
                    this.Length.Equals(other.Length)
                ) && 
                (
                    this.Width == other.Width ||
                    this.Width != null &&
                    this.Width.Equals(other.Width)
                ) && 
                (
                    this.TaxType == other.TaxType ||
                    this.TaxType != null &&
                    this.TaxType.Equals(other.TaxType)
                ) && 
                (
                    this.Items == other.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(other.Items)
                ) && 
                (
                    this.Packages == other.Packages ||
                    this.Packages != null &&
                    this.Packages.SequenceEqual(other.Packages)
                ) && 
                (
                    this.InPayments == other.InPayments ||
                    this.InPayments != null &&
                    this.InPayments.SequenceEqual(other.InPayments)
                ) && 
                (
                    this.DeliveryAddress == other.DeliveryAddress ||
                    this.DeliveryAddress != null &&
                    this.DeliveryAddress.Equals(other.DeliveryAddress)
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

                if (this.OrganizationName != null)
                    hash = hash * 59 + this.OrganizationName.GetHashCode();

                if (this.OrganizationId != null)
                    hash = hash * 59 + this.OrganizationId.GetHashCode();

                if (this.FulfillmentCenterName != null)
                    hash = hash * 59 + this.FulfillmentCenterName.GetHashCode();

                if (this.FulfillmentCenterId != null)
                    hash = hash * 59 + this.FulfillmentCenterId.GetHashCode();

                if (this.ShippingMethod != null)
                    hash = hash * 59 + this.ShippingMethod.GetHashCode();

                if (this.EmployeeName != null)
                    hash = hash * 59 + this.EmployeeName.GetHashCode();

                if (this.EmployeeId != null)
                    hash = hash * 59 + this.EmployeeId.GetHashCode();

                if (this.DiscountAmount != null)
                    hash = hash * 59 + this.DiscountAmount.GetHashCode();

                if (this.WeightUnit != null)
                    hash = hash * 59 + this.WeightUnit.GetHashCode();

                if (this.Weight != null)
                    hash = hash * 59 + this.Weight.GetHashCode();

                if (this.MeasureUnit != null)
                    hash = hash * 59 + this.MeasureUnit.GetHashCode();

                if (this.Height != null)
                    hash = hash * 59 + this.Height.GetHashCode();

                if (this.Length != null)
                    hash = hash * 59 + this.Length.GetHashCode();

                if (this.Width != null)
                    hash = hash * 59 + this.Width.GetHashCode();

                if (this.TaxType != null)
                    hash = hash * 59 + this.TaxType.GetHashCode();

                if (this.Items != null)
                    hash = hash * 59 + this.Items.GetHashCode();

                if (this.Packages != null)
                    hash = hash * 59 + this.Packages.GetHashCode();

                if (this.InPayments != null)
                    hash = hash * 59 + this.InPayments.GetHashCode();

                if (this.DeliveryAddress != null)
                    hash = hash * 59 + this.DeliveryAddress.GetHashCode();

                if (this.Discount != null)
                    hash = hash * 59 + this.Discount.GetHashCode();

                if (this.TaxDetails != null)
                    hash = hash * 59 + this.TaxDetails.GetHashCode();

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
