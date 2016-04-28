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
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCartModuleWebModelShipment :  IEquatable<VirtoCommerceCartModuleWebModelShipment>
    {
        /// <summary>
        /// Gets or sets the value of shipping method code
        /// </summary>
        /// <value>Gets or sets the value of shipping method code</value>
        [DataMember(Name="shipmentMethodCode", EmitDefaultValue=false)]
        public string ShipmentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method option
        /// </summary>
        /// <value>Gets or sets the value of shipping method option</value>
        [DataMember(Name="shipmentMethodOption", EmitDefaultValue=false)]
        public string ShipmentMethodOption { get; set; }

        /// <summary>
        /// Gets or sets the value of fulfillment center id
        /// </summary>
        /// <value>Gets or sets the value of fulfillment center id</value>
        [DataMember(Name="fulfilmentCenterId", EmitDefaultValue=false)]
        public string FulfilmentCenterId { get; set; }

        /// <summary>
        /// Gets or sets the delivery address
        /// </summary>
        /// <value>Gets or sets the delivery address</value>
        [DataMember(Name="deliveryAddress", EmitDefaultValue=false)]
        public VirtoCommerceCartModuleWebModelAddress DeliveryAddress { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping currency
        /// </summary>
        /// <value>Gets or sets the value of shipping currency</value>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of volumetric weight
        /// </summary>
        /// <value>Gets or sets the value of volumetric weight</value>
        [DataMember(Name="volumetricWeight", EmitDefaultValue=false)]
        public double? VolumetricWeight { get; set; }

        /// <summary>
        /// Gets or sets the value of weight unit
        /// </summary>
        /// <value>Gets or sets the value of weight unit</value>
        [DataMember(Name="weightUnit", EmitDefaultValue=false)]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of weight
        /// </summary>
        /// <value>Gets or sets the value of weight</value>
        [DataMember(Name="weight", EmitDefaultValue=false)]
        public double? Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of measurement units
        /// </summary>
        /// <value>Gets or sets the value of measurement units</value>
        [DataMember(Name="measureUnit", EmitDefaultValue=false)]
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of height
        /// </summary>
        /// <value>Gets or sets the value of height</value>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public double? Height { get; set; }

        /// <summary>
        /// Gets or sets the value of length
        /// </summary>
        /// <value>Gets or sets the value of length</value>
        [DataMember(Name="length", EmitDefaultValue=false)]
        public double? Length { get; set; }

        /// <summary>
        /// Gets or sets the value of width
        /// </summary>
        /// <value>Gets or sets the value of width</value>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public double? Width { get; set; }

        /// <summary>
        /// Gets or sets the flag of shipping has tax
        /// </summary>
        /// <value>Gets or sets the flag of shipping has tax</value>
        [DataMember(Name="taxIncluded", EmitDefaultValue=false)]
        public bool? TaxIncluded { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping price
        /// </summary>
        /// <value>Gets or sets the value of shipping price</value>
        [DataMember(Name="shippingPrice", EmitDefaultValue=false)]
        public double? ShippingPrice { get; set; }

        /// <summary>
        /// Gets or sets the value of total shipping price
        /// </summary>
        /// <value>Gets or sets the value of total shipping price</value>
        [DataMember(Name="total", EmitDefaultValue=false)]
        public double? Total { get; set; }

        /// <summary>
        /// Gets or sets the value of total shipping discount amount
        /// </summary>
        /// <value>Gets or sets the value of total shipping discount amount</value>
        [DataMember(Name="discountTotal", EmitDefaultValue=false)]
        public double? DiscountTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total shipping tax amount
        /// </summary>
        /// <value>Gets or sets the value of total shipping tax amount</value>
        [DataMember(Name="taxTotal", EmitDefaultValue=false)]
        public double? TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping items subtotal
        /// </summary>
        /// <value>Gets or sets the value of shipping items subtotal</value>
        [DataMember(Name="itemSubtotal", EmitDefaultValue=false)]
        public double? ItemSubtotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping subtotal
        /// </summary>
        /// <value>Gets or sets the value of shipping subtotal</value>
        [DataMember(Name="subtotal", EmitDefaultValue=false)]
        public double? Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the collection of shipping discounts
        /// </summary>
        /// <value>Gets or sets the collection of shipping discounts</value>
        [DataMember(Name="discounts", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelDiscount> Discounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of shipping items
        /// </summary>
        /// <value>Gets or sets the collection of shipping items</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelShipmentItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping tax type
        /// </summary>
        /// <value>Gets or sets the value of shipping tax type</value>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the collection of line item tax detalization lines
        /// </summary>
        /// <value>Gets or sets the collection of line item tax detalization lines</value>
        [DataMember(Name="taxDetails", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelTaxDetail> TaxDetails { get; set; }

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
            sb.Append("class VirtoCommerceCartModuleWebModelShipment {\n");
            sb.Append("  ShipmentMethodCode: ").Append(ShipmentMethodCode).Append("\n");
            sb.Append("  ShipmentMethodOption: ").Append(ShipmentMethodOption).Append("\n");
            sb.Append("  FulfilmentCenterId: ").Append(FulfilmentCenterId).Append("\n");
            sb.Append("  DeliveryAddress: ").Append(DeliveryAddress).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  VolumetricWeight: ").Append(VolumetricWeight).Append("\n");
            sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
            sb.Append("  Weight: ").Append(Weight).Append("\n");
            sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  TaxIncluded: ").Append(TaxIncluded).Append("\n");
            sb.Append("  ShippingPrice: ").Append(ShippingPrice).Append("\n");
            sb.Append("  Total: ").Append(Total).Append("\n");
            sb.Append("  DiscountTotal: ").Append(DiscountTotal).Append("\n");
            sb.Append("  TaxTotal: ").Append(TaxTotal).Append("\n");
            sb.Append("  ItemSubtotal: ").Append(ItemSubtotal).Append("\n");
            sb.Append("  Subtotal: ").Append(Subtotal).Append("\n");
            sb.Append("  Discounts: ").Append(Discounts).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
            sb.Append("  TaxDetails: ").Append(TaxDetails).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCartModuleWebModelShipment);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCartModuleWebModelShipment instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCartModuleWebModelShipment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCartModuleWebModelShipment other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ShipmentMethodCode == other.ShipmentMethodCode ||
                    this.ShipmentMethodCode != null &&
                    this.ShipmentMethodCode.Equals(other.ShipmentMethodCode)
                ) && 
                (
                    this.ShipmentMethodOption == other.ShipmentMethodOption ||
                    this.ShipmentMethodOption != null &&
                    this.ShipmentMethodOption.Equals(other.ShipmentMethodOption)
                ) && 
                (
                    this.FulfilmentCenterId == other.FulfilmentCenterId ||
                    this.FulfilmentCenterId != null &&
                    this.FulfilmentCenterId.Equals(other.FulfilmentCenterId)
                ) && 
                (
                    this.DeliveryAddress == other.DeliveryAddress ||
                    this.DeliveryAddress != null &&
                    this.DeliveryAddress.Equals(other.DeliveryAddress)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.VolumetricWeight == other.VolumetricWeight ||
                    this.VolumetricWeight != null &&
                    this.VolumetricWeight.Equals(other.VolumetricWeight)
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
                    this.TaxIncluded == other.TaxIncluded ||
                    this.TaxIncluded != null &&
                    this.TaxIncluded.Equals(other.TaxIncluded)
                ) && 
                (
                    this.ShippingPrice == other.ShippingPrice ||
                    this.ShippingPrice != null &&
                    this.ShippingPrice.Equals(other.ShippingPrice)
                ) && 
                (
                    this.Total == other.Total ||
                    this.Total != null &&
                    this.Total.Equals(other.Total)
                ) && 
                (
                    this.DiscountTotal == other.DiscountTotal ||
                    this.DiscountTotal != null &&
                    this.DiscountTotal.Equals(other.DiscountTotal)
                ) && 
                (
                    this.TaxTotal == other.TaxTotal ||
                    this.TaxTotal != null &&
                    this.TaxTotal.Equals(other.TaxTotal)
                ) && 
                (
                    this.ItemSubtotal == other.ItemSubtotal ||
                    this.ItemSubtotal != null &&
                    this.ItemSubtotal.Equals(other.ItemSubtotal)
                ) && 
                (
                    this.Subtotal == other.Subtotal ||
                    this.Subtotal != null &&
                    this.Subtotal.Equals(other.Subtotal)
                ) && 
                (
                    this.Discounts == other.Discounts ||
                    this.Discounts != null &&
                    this.Discounts.SequenceEqual(other.Discounts)
                ) && 
                (
                    this.Items == other.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(other.Items)
                ) && 
                (
                    this.TaxType == other.TaxType ||
                    this.TaxType != null &&
                    this.TaxType.Equals(other.TaxType)
                ) && 
                (
                    this.TaxDetails == other.TaxDetails ||
                    this.TaxDetails != null &&
                    this.TaxDetails.SequenceEqual(other.TaxDetails)
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

                if (this.ShipmentMethodCode != null)
                    hash = hash * 59 + this.ShipmentMethodCode.GetHashCode();

                if (this.ShipmentMethodOption != null)
                    hash = hash * 59 + this.ShipmentMethodOption.GetHashCode();

                if (this.FulfilmentCenterId != null)
                    hash = hash * 59 + this.FulfilmentCenterId.GetHashCode();

                if (this.DeliveryAddress != null)
                    hash = hash * 59 + this.DeliveryAddress.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.VolumetricWeight != null)
                    hash = hash * 59 + this.VolumetricWeight.GetHashCode();

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

                if (this.TaxIncluded != null)
                    hash = hash * 59 + this.TaxIncluded.GetHashCode();

                if (this.ShippingPrice != null)
                    hash = hash * 59 + this.ShippingPrice.GetHashCode();

                if (this.Total != null)
                    hash = hash * 59 + this.Total.GetHashCode();

                if (this.DiscountTotal != null)
                    hash = hash * 59 + this.DiscountTotal.GetHashCode();

                if (this.TaxTotal != null)
                    hash = hash * 59 + this.TaxTotal.GetHashCode();

                if (this.ItemSubtotal != null)
                    hash = hash * 59 + this.ItemSubtotal.GetHashCode();

                if (this.Subtotal != null)
                    hash = hash * 59 + this.Subtotal.GetHashCode();

                if (this.Discounts != null)
                    hash = hash * 59 + this.Discounts.GetHashCode();

                if (this.Items != null)
                    hash = hash * 59 + this.Items.GetHashCode();

                if (this.TaxType != null)
                    hash = hash * 59 + this.TaxType.GetHashCode();

                if (this.TaxDetails != null)
                    hash = hash * 59 + this.TaxDetails.GetHashCode();

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
