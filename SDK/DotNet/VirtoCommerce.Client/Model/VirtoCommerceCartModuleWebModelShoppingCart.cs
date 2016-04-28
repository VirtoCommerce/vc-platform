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
    public partial class VirtoCommerceCartModuleWebModelShoppingCart :  IEquatable<VirtoCommerceCartModuleWebModelShoppingCart>
    {
        /// <summary>
        /// Gets or sets the value of shopping cart name
        /// </summary>
        /// <value>Gets or sets the value of shopping cart name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of store id
        /// </summary>
        /// <value>Gets or sets the value of store id</value>
        [DataMember(Name="storeId", EmitDefaultValue=false)]
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the value of channel id
        /// </summary>
        /// <value>Gets or sets the value of channel id</value>
        [DataMember(Name="channelId", EmitDefaultValue=false)]
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart is anonymous
        /// </summary>
        /// <value>Gets or sets the flag of shopping cart is anonymous</value>
        [DataMember(Name="isAnonymous", EmitDefaultValue=false)]
        public bool? IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart customer id
        /// </summary>
        /// <value>Gets or sets the value of shopping cart customer id</value>
        [DataMember(Name="customerId", EmitDefaultValue=false)]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart customer name
        /// </summary>
        /// <value>Gets or sets the value of shopping cart customer name</value>
        [DataMember(Name="customerName", EmitDefaultValue=false)]
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart organization id
        /// </summary>
        /// <value>Gets or sets the value of shopping cart organization id</value>
        [DataMember(Name="organizationId", EmitDefaultValue=false)]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart currency
        /// </summary>
        /// <value>Gets or sets the value of shopping cart currency</value>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart coupon
        /// </summary>
        /// <value>Gets or sets the shopping cart coupon</value>
        [DataMember(Name="coupon", EmitDefaultValue=false)]
        public string Coupon { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart language code
        /// </summary>
        /// <value>Gets or sets the value of shopping cart language code</value>
        [DataMember(Name="languageCode", EmitDefaultValue=false)]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart has tax
        /// </summary>
        /// <value>Gets or sets the flag of shopping cart has tax</value>
        [DataMember(Name="taxIncluded", EmitDefaultValue=false)]
        public bool? TaxIncluded { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart is recurring
        /// </summary>
        /// <value>Gets or sets the flag of shopping cart is recurring</value>
        [DataMember(Name="isRecuring", EmitDefaultValue=false)]
        public bool? IsRecuring { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart text comment
        /// </summary>
        /// <value>Gets or sets the value of shopping cart text comment</value>
        [DataMember(Name="comment", EmitDefaultValue=false)]
        public string Comment { get; set; }

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
        /// Gets or sets the value of shopping cart weight
        /// </summary>
        /// <value>Gets or sets the value of shopping cart weight</value>
        [DataMember(Name="weight", EmitDefaultValue=false)]
        public double? Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of measurement unit
        /// </summary>
        /// <value>Gets or sets the value of measurement unit</value>
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
        /// Represent any line item validation type (noPriceValidate, noQuantityValidate etc) this value can be used in storefront \r\n            to select appropriate validation strategy
        /// </summary>
        /// <value>Represent any line item validation type (noPriceValidate, noQuantityValidate etc) this value can be used in storefront \r\n            to select appropriate validation strategy</value>
        [DataMember(Name="validationType", EmitDefaultValue=false)]
        public string ValidationType { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart total cost
        /// </summary>
        /// <value>Gets or sets the value of shopping cart total cost</value>
        [DataMember(Name="total", EmitDefaultValue=false)]
        public double? Total { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart subtotal
        /// </summary>
        /// <value>Gets or sets the value of shopping cart subtotal</value>
        [DataMember(Name="subTotal", EmitDefaultValue=false)]
        public double? SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping total cost
        /// </summary>
        /// <value>Gets or sets the value of shipping total cost</value>
        [DataMember(Name="shippingTotal", EmitDefaultValue=false)]
        public double? ShippingTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of handling total cost
        /// </summary>
        /// <value>Gets or sets the value of handling total cost</value>
        [DataMember(Name="handlingTotal", EmitDefaultValue=false)]
        public double? HandlingTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total discount amount
        /// </summary>
        /// <value>Gets or sets the value of total discount amount</value>
        [DataMember(Name="discountTotal", EmitDefaultValue=false)]
        public double? DiscountTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total tax cost
        /// </summary>
        /// <value>Gets or sets the value of total tax cost</value>
        [DataMember(Name="taxTotal", EmitDefaultValue=false)]
        public double? TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart addresses
        /// </summary>
        /// <value>Gets or sets the collection of shopping cart addresses</value>
        [DataMember(Name="addresses", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelAddress> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart line items
        /// </summary>
        /// <value>Gets or sets the value of shopping cart line items</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelLineItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart payments
        /// </summary>
        /// <value>Gets or sets the collection of shopping cart payments</value>
        [DataMember(Name="payments", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelPayment> Payments { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart shipments
        /// </summary>
        /// <value>Gets or sets the collection of shopping cart shipments</value>
        [DataMember(Name="shipments", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelShipment> Shipments { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart discounts
        /// </summary>
        /// <value>Gets or sets the collection of shopping cart discounts</value>
        [DataMember(Name="discounts", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelDiscount> Discounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of line item tax detalization lines
        /// </summary>
        /// <value>Gets or sets the collection of line item tax detalization lines</value>
        [DataMember(Name="taxDetails", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelTaxDetail> TaxDetails { get; set; }

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
            sb.Append("class VirtoCommerceCartModuleWebModelShoppingCart {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  StoreId: ").Append(StoreId).Append("\n");
            sb.Append("  ChannelId: ").Append(ChannelId).Append("\n");
            sb.Append("  IsAnonymous: ").Append(IsAnonymous).Append("\n");
            sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
            sb.Append("  CustomerName: ").Append(CustomerName).Append("\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Coupon: ").Append(Coupon).Append("\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  TaxIncluded: ").Append(TaxIncluded).Append("\n");
            sb.Append("  IsRecuring: ").Append(IsRecuring).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  VolumetricWeight: ").Append(VolumetricWeight).Append("\n");
            sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
            sb.Append("  Weight: ").Append(Weight).Append("\n");
            sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  ValidationType: ").Append(ValidationType).Append("\n");
            sb.Append("  Total: ").Append(Total).Append("\n");
            sb.Append("  SubTotal: ").Append(SubTotal).Append("\n");
            sb.Append("  ShippingTotal: ").Append(ShippingTotal).Append("\n");
            sb.Append("  HandlingTotal: ").Append(HandlingTotal).Append("\n");
            sb.Append("  DiscountTotal: ").Append(DiscountTotal).Append("\n");
            sb.Append("  TaxTotal: ").Append(TaxTotal).Append("\n");
            sb.Append("  Addresses: ").Append(Addresses).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  Payments: ").Append(Payments).Append("\n");
            sb.Append("  Shipments: ").Append(Shipments).Append("\n");
            sb.Append("  Discounts: ").Append(Discounts).Append("\n");
            sb.Append("  TaxDetails: ").Append(TaxDetails).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCartModuleWebModelShoppingCart);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCartModuleWebModelShoppingCart instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCartModuleWebModelShoppingCart to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCartModuleWebModelShoppingCart other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
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
                    this.OrganizationId == other.OrganizationId ||
                    this.OrganizationId != null &&
                    this.OrganizationId.Equals(other.OrganizationId)
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
                    this.LanguageCode == other.LanguageCode ||
                    this.LanguageCode != null &&
                    this.LanguageCode.Equals(other.LanguageCode)
                ) && 
                (
                    this.TaxIncluded == other.TaxIncluded ||
                    this.TaxIncluded != null &&
                    this.TaxIncluded.Equals(other.TaxIncluded)
                ) && 
                (
                    this.IsRecuring == other.IsRecuring ||
                    this.IsRecuring != null &&
                    this.IsRecuring.Equals(other.IsRecuring)
                ) && 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
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
                    this.ValidationType == other.ValidationType ||
                    this.ValidationType != null &&
                    this.ValidationType.Equals(other.ValidationType)
                ) && 
                (
                    this.Total == other.Total ||
                    this.Total != null &&
                    this.Total.Equals(other.Total)
                ) && 
                (
                    this.SubTotal == other.SubTotal ||
                    this.SubTotal != null &&
                    this.SubTotal.Equals(other.SubTotal)
                ) && 
                (
                    this.ShippingTotal == other.ShippingTotal ||
                    this.ShippingTotal != null &&
                    this.ShippingTotal.Equals(other.ShippingTotal)
                ) && 
                (
                    this.HandlingTotal == other.HandlingTotal ||
                    this.HandlingTotal != null &&
                    this.HandlingTotal.Equals(other.HandlingTotal)
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
                    this.Payments == other.Payments ||
                    this.Payments != null &&
                    this.Payments.SequenceEqual(other.Payments)
                ) && 
                (
                    this.Shipments == other.Shipments ||
                    this.Shipments != null &&
                    this.Shipments.SequenceEqual(other.Shipments)
                ) && 
                (
                    this.Discounts == other.Discounts ||
                    this.Discounts != null &&
                    this.Discounts.SequenceEqual(other.Discounts)
                ) && 
                (
                    this.TaxDetails == other.TaxDetails ||
                    this.TaxDetails != null &&
                    this.TaxDetails.SequenceEqual(other.TaxDetails)
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

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.StoreId != null)
                    hash = hash * 59 + this.StoreId.GetHashCode();

                if (this.ChannelId != null)
                    hash = hash * 59 + this.ChannelId.GetHashCode();

                if (this.IsAnonymous != null)
                    hash = hash * 59 + this.IsAnonymous.GetHashCode();

                if (this.CustomerId != null)
                    hash = hash * 59 + this.CustomerId.GetHashCode();

                if (this.CustomerName != null)
                    hash = hash * 59 + this.CustomerName.GetHashCode();

                if (this.OrganizationId != null)
                    hash = hash * 59 + this.OrganizationId.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.Coupon != null)
                    hash = hash * 59 + this.Coupon.GetHashCode();

                if (this.LanguageCode != null)
                    hash = hash * 59 + this.LanguageCode.GetHashCode();

                if (this.TaxIncluded != null)
                    hash = hash * 59 + this.TaxIncluded.GetHashCode();

                if (this.IsRecuring != null)
                    hash = hash * 59 + this.IsRecuring.GetHashCode();

                if (this.Comment != null)
                    hash = hash * 59 + this.Comment.GetHashCode();

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

                if (this.ValidationType != null)
                    hash = hash * 59 + this.ValidationType.GetHashCode();

                if (this.Total != null)
                    hash = hash * 59 + this.Total.GetHashCode();

                if (this.SubTotal != null)
                    hash = hash * 59 + this.SubTotal.GetHashCode();

                if (this.ShippingTotal != null)
                    hash = hash * 59 + this.ShippingTotal.GetHashCode();

                if (this.HandlingTotal != null)
                    hash = hash * 59 + this.HandlingTotal.GetHashCode();

                if (this.DiscountTotal != null)
                    hash = hash * 59 + this.DiscountTotal.GetHashCode();

                if (this.TaxTotal != null)
                    hash = hash * 59 + this.TaxTotal.GetHashCode();

                if (this.Addresses != null)
                    hash = hash * 59 + this.Addresses.GetHashCode();

                if (this.Items != null)
                    hash = hash * 59 + this.Items.GetHashCode();

                if (this.Payments != null)
                    hash = hash * 59 + this.Payments.GetHashCode();

                if (this.Shipments != null)
                    hash = hash * 59 + this.Shipments.GetHashCode();

                if (this.Discounts != null)
                    hash = hash * 59 + this.Discounts.GetHashCode();

                if (this.TaxDetails != null)
                    hash = hash * 59 + this.TaxDetails.GetHashCode();

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
