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
    public partial class VirtoCommerceCartModuleWebModelLineItem :  IEquatable<VirtoCommerceCartModuleWebModelLineItem>
    {
        /// <summary>
        /// Gets or sets the value of product id
        /// </summary>
        /// <value>Gets or sets the value of product id</value>
        [DataMember(Name="productId", EmitDefaultValue=false)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog id
        /// </summary>
        /// <value>Gets or sets the value of catalog id</value>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the value of category id
        /// </summary>
        /// <value>Gets or sets the value of category id</value>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the value of product SKU
        /// </summary>
        /// <value>Gets or sets the value of product SKU</value>
        [DataMember(Name="sku", EmitDefaultValue=false)]
        public string Sku { get; set; }

        /// <summary>
        /// Gets or sets the value of product type (Digital, physical etc)
        /// </summary>
        /// <value>Gets or sets the value of product type (Digital, physical etc)</value>
        [DataMember(Name="productType", EmitDefaultValue=false)]
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the value of line item name
        /// </summary>
        /// <value>Gets or sets the value of line item name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of line item quantity
        /// </summary>
        /// <value>Gets or sets the value of line item quantity</value>
        [DataMember(Name="quantity", EmitDefaultValue=false)]
        public int? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the value of line item currency
        /// </summary>
        /// <value>Gets or sets the value of line item currency</value>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of line item warehouse location
        /// </summary>
        /// <value>Gets or sets the value of line item warehouse location</value>
        [DataMember(Name="warehouseLocation", EmitDefaultValue=false)]
        public string WarehouseLocation { get; set; }

        /// <summary>
        /// Gets or sets the value of line item shipping method code
        /// </summary>
        /// <value>Gets or sets the value of line item shipping method code</value>
        [DataMember(Name="shipmentMethodCode", EmitDefaultValue=false)]
        public string ShipmentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the requirement for line item shipping
        /// </summary>
        /// <value>Gets or sets the requirement for line item shipping</value>
        [DataMember(Name="requiredShipping", EmitDefaultValue=false)]
        public bool? RequiredShipping { get; set; }

        /// <summary>
        /// Gets or sets the value of line item thumbnail image absolute URL
        /// </summary>
        /// <value>Gets or sets the value of line item thumbnail image absolute URL</value>
        [DataMember(Name="thumbnailImageUrl", EmitDefaultValue=false)]
        public string ThumbnailImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the value of line item image absolute URL
        /// </summary>
        /// <value>Gets or sets the value of line item image absolute URL</value>
        [DataMember(Name="imageUrl", EmitDefaultValue=false)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the flag of line item is a gift
        /// </summary>
        /// <value>Gets or sets the flag of line item is a gift</value>
        [DataMember(Name="isGift", EmitDefaultValue=false)]
        public bool? IsGift { get; set; }

        /// <summary>
        /// Gets or sets the collection of line item discounts
        /// </summary>
        /// <value>Gets or sets the collection of line item discounts</value>
        [DataMember(Name="discounts", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelDiscount> Discounts { get; set; }

        /// <summary>
        /// Gets or sets the value of language code
        /// </summary>
        /// <value>Gets or sets the value of language code</value>
        [DataMember(Name="languageCode", EmitDefaultValue=false)]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the value of line item comment
        /// </summary>
        /// <value>Gets or sets the value of line item comment</value>
        [DataMember(Name="comment", EmitDefaultValue=false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the flag of line item is recurring
        /// </summary>
        /// <value>Gets or sets the flag of line item is recurring</value>
        [DataMember(Name="isReccuring", EmitDefaultValue=false)]
        public bool? IsReccuring { get; set; }

        /// <summary>
        /// Gets or sets flag of line item has tax
        /// </summary>
        /// <value>Gets or sets flag of line item has tax</value>
        [DataMember(Name="taxIncluded", EmitDefaultValue=false)]
        public bool? TaxIncluded { get; set; }

        /// <summary>
        /// Gets or sets the value of line item volumetric weight
        /// </summary>
        /// <value>Gets or sets the value of line item volumetric weight</value>
        [DataMember(Name="volumetricWeight", EmitDefaultValue=false)]
        public double? VolumetricWeight { get; set; }

        /// <summary>
        /// Gets or sets the value of line item weight unit
        /// </summary>
        /// <value>Gets or sets the value of line item weight unit</value>
        [DataMember(Name="weightUnit", EmitDefaultValue=false)]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of line item weight
        /// </summary>
        /// <value>Gets or sets the value of line item weight</value>
        [DataMember(Name="weight", EmitDefaultValue=false)]
        public double? Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of line item measurement unit
        /// </summary>
        /// <value>Gets or sets the value of line item measurement unit</value>
        [DataMember(Name="measureUnit", EmitDefaultValue=false)]
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of line item height
        /// </summary>
        /// <value>Gets or sets the value of line item height</value>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public double? Height { get; set; }

        /// <summary>
        /// Gets or sets the value of line item length
        /// </summary>
        /// <value>Gets or sets the value of line item length</value>
        [DataMember(Name="length", EmitDefaultValue=false)]
        public double? Length { get; set; }

        /// <summary>
        /// Gets or sets the value of line item width
        /// </summary>
        /// <value>Gets or sets the value of line item width</value>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public double? Width { get; set; }

        /// <summary>
        /// Represent any line item validation type (noPriceValidate, noQuantityValidate etc) this value can be used in storefront \r\n            to select appropriate validation strategy
        /// </summary>
        /// <value>Represent any line item validation type (noPriceValidate, noQuantityValidate etc) this value can be used in storefront \r\n            to select appropriate validation strategy</value>
        [DataMember(Name="validationType", EmitDefaultValue=false)]
        public string ValidationType { get; set; }

        /// <summary>
        /// Price id which that was used in the formation of this line item
        /// </summary>
        /// <value>Price id which that was used in the formation of this line item</value>
        [DataMember(Name="priceId", EmitDefaultValue=false)]
        public string PriceId { get; set; }

        /// <summary>
        /// Gets or sets the value of line item original price
        /// </summary>
        /// <value>Gets or sets the value of line item original price</value>
        [DataMember(Name="listPrice", EmitDefaultValue=false)]
        public double? ListPrice { get; set; }

        /// <summary>
        /// Gets or sets the value of line item sale price (include static discount)
        /// </summary>
        /// <value>Gets or sets the value of line item sale price (include static discount)</value>
        [DataMember(Name="salePrice", EmitDefaultValue=false)]
        public double? SalePrice { get; set; }

        /// <summary>
        /// Gets or sets the value of line item actual price (include all types of discounts)
        /// </summary>
        /// <value>Gets or sets the value of line item actual price (include all types of discounts)</value>
        [DataMember(Name="placedPrice", EmitDefaultValue=false)]
        public double? PlacedPrice { get; set; }

        /// <summary>
        /// Gets or sets the value of line item subtotal price (actual price * line item quantity)
        /// </summary>
        /// <value>Gets or sets the value of line item subtotal price (actual price * line item quantity)</value>
        [DataMember(Name="extendedPrice", EmitDefaultValue=false)]
        public double? ExtendedPrice { get; set; }

        /// <summary>
        /// Gets or sets the value of line item total discount amount
        /// </summary>
        /// <value>Gets or sets the value of line item total discount amount</value>
        [DataMember(Name="discountTotal", EmitDefaultValue=false)]
        public double? DiscountTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of line item total tax amount
        /// </summary>
        /// <value>Gets or sets the value of line item total tax amount</value>
        [DataMember(Name="taxTotal", EmitDefaultValue=false)]
        public double? TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of line item tax type
        /// </summary>
        /// <value>Gets or sets the value of line item tax type</value>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }

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
            sb.Append("class VirtoCommerceCartModuleWebModelLineItem {\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  Sku: ").Append(Sku).Append("\n");
            sb.Append("  ProductType: ").Append(ProductType).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Quantity: ").Append(Quantity).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  WarehouseLocation: ").Append(WarehouseLocation).Append("\n");
            sb.Append("  ShipmentMethodCode: ").Append(ShipmentMethodCode).Append("\n");
            sb.Append("  RequiredShipping: ").Append(RequiredShipping).Append("\n");
            sb.Append("  ThumbnailImageUrl: ").Append(ThumbnailImageUrl).Append("\n");
            sb.Append("  ImageUrl: ").Append(ImageUrl).Append("\n");
            sb.Append("  IsGift: ").Append(IsGift).Append("\n");
            sb.Append("  Discounts: ").Append(Discounts).Append("\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  IsReccuring: ").Append(IsReccuring).Append("\n");
            sb.Append("  TaxIncluded: ").Append(TaxIncluded).Append("\n");
            sb.Append("  VolumetricWeight: ").Append(VolumetricWeight).Append("\n");
            sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
            sb.Append("  Weight: ").Append(Weight).Append("\n");
            sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  ValidationType: ").Append(ValidationType).Append("\n");
            sb.Append("  PriceId: ").Append(PriceId).Append("\n");
            sb.Append("  ListPrice: ").Append(ListPrice).Append("\n");
            sb.Append("  SalePrice: ").Append(SalePrice).Append("\n");
            sb.Append("  PlacedPrice: ").Append(PlacedPrice).Append("\n");
            sb.Append("  ExtendedPrice: ").Append(ExtendedPrice).Append("\n");
            sb.Append("  DiscountTotal: ").Append(DiscountTotal).Append("\n");
            sb.Append("  TaxTotal: ").Append(TaxTotal).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCartModuleWebModelLineItem);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCartModuleWebModelLineItem instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCartModuleWebModelLineItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCartModuleWebModelLineItem other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ProductId == other.ProductId ||
                    this.ProductId != null &&
                    this.ProductId.Equals(other.ProductId)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.Sku == other.Sku ||
                    this.Sku != null &&
                    this.Sku.Equals(other.Sku)
                ) && 
                (
                    this.ProductType == other.ProductType ||
                    this.ProductType != null &&
                    this.ProductType.Equals(other.ProductType)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Quantity == other.Quantity ||
                    this.Quantity != null &&
                    this.Quantity.Equals(other.Quantity)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.WarehouseLocation == other.WarehouseLocation ||
                    this.WarehouseLocation != null &&
                    this.WarehouseLocation.Equals(other.WarehouseLocation)
                ) && 
                (
                    this.ShipmentMethodCode == other.ShipmentMethodCode ||
                    this.ShipmentMethodCode != null &&
                    this.ShipmentMethodCode.Equals(other.ShipmentMethodCode)
                ) && 
                (
                    this.RequiredShipping == other.RequiredShipping ||
                    this.RequiredShipping != null &&
                    this.RequiredShipping.Equals(other.RequiredShipping)
                ) && 
                (
                    this.ThumbnailImageUrl == other.ThumbnailImageUrl ||
                    this.ThumbnailImageUrl != null &&
                    this.ThumbnailImageUrl.Equals(other.ThumbnailImageUrl)
                ) && 
                (
                    this.ImageUrl == other.ImageUrl ||
                    this.ImageUrl != null &&
                    this.ImageUrl.Equals(other.ImageUrl)
                ) && 
                (
                    this.IsGift == other.IsGift ||
                    this.IsGift != null &&
                    this.IsGift.Equals(other.IsGift)
                ) && 
                (
                    this.Discounts == other.Discounts ||
                    this.Discounts != null &&
                    this.Discounts.SequenceEqual(other.Discounts)
                ) && 
                (
                    this.LanguageCode == other.LanguageCode ||
                    this.LanguageCode != null &&
                    this.LanguageCode.Equals(other.LanguageCode)
                ) && 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) && 
                (
                    this.IsReccuring == other.IsReccuring ||
                    this.IsReccuring != null &&
                    this.IsReccuring.Equals(other.IsReccuring)
                ) && 
                (
                    this.TaxIncluded == other.TaxIncluded ||
                    this.TaxIncluded != null &&
                    this.TaxIncluded.Equals(other.TaxIncluded)
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
                    this.PriceId == other.PriceId ||
                    this.PriceId != null &&
                    this.PriceId.Equals(other.PriceId)
                ) && 
                (
                    this.ListPrice == other.ListPrice ||
                    this.ListPrice != null &&
                    this.ListPrice.Equals(other.ListPrice)
                ) && 
                (
                    this.SalePrice == other.SalePrice ||
                    this.SalePrice != null &&
                    this.SalePrice.Equals(other.SalePrice)
                ) && 
                (
                    this.PlacedPrice == other.PlacedPrice ||
                    this.PlacedPrice != null &&
                    this.PlacedPrice.Equals(other.PlacedPrice)
                ) && 
                (
                    this.ExtendedPrice == other.ExtendedPrice ||
                    this.ExtendedPrice != null &&
                    this.ExtendedPrice.Equals(other.ExtendedPrice)
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

                if (this.ProductId != null)
                    hash = hash * 59 + this.ProductId.GetHashCode();

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                if (this.Sku != null)
                    hash = hash * 59 + this.Sku.GetHashCode();

                if (this.ProductType != null)
                    hash = hash * 59 + this.ProductType.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Quantity != null)
                    hash = hash * 59 + this.Quantity.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.WarehouseLocation != null)
                    hash = hash * 59 + this.WarehouseLocation.GetHashCode();

                if (this.ShipmentMethodCode != null)
                    hash = hash * 59 + this.ShipmentMethodCode.GetHashCode();

                if (this.RequiredShipping != null)
                    hash = hash * 59 + this.RequiredShipping.GetHashCode();

                if (this.ThumbnailImageUrl != null)
                    hash = hash * 59 + this.ThumbnailImageUrl.GetHashCode();

                if (this.ImageUrl != null)
                    hash = hash * 59 + this.ImageUrl.GetHashCode();

                if (this.IsGift != null)
                    hash = hash * 59 + this.IsGift.GetHashCode();

                if (this.Discounts != null)
                    hash = hash * 59 + this.Discounts.GetHashCode();

                if (this.LanguageCode != null)
                    hash = hash * 59 + this.LanguageCode.GetHashCode();

                if (this.Comment != null)
                    hash = hash * 59 + this.Comment.GetHashCode();

                if (this.IsReccuring != null)
                    hash = hash * 59 + this.IsReccuring.GetHashCode();

                if (this.TaxIncluded != null)
                    hash = hash * 59 + this.TaxIncluded.GetHashCode();

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

                if (this.PriceId != null)
                    hash = hash * 59 + this.PriceId.GetHashCode();

                if (this.ListPrice != null)
                    hash = hash * 59 + this.ListPrice.GetHashCode();

                if (this.SalePrice != null)
                    hash = hash * 59 + this.SalePrice.GetHashCode();

                if (this.PlacedPrice != null)
                    hash = hash * 59 + this.PlacedPrice.GetHashCode();

                if (this.ExtendedPrice != null)
                    hash = hash * 59 + this.ExtendedPrice.GetHashCode();

                if (this.DiscountTotal != null)
                    hash = hash * 59 + this.DiscountTotal.GetHashCode();

                if (this.TaxTotal != null)
                    hash = hash * 59 + this.TaxTotal.GetHashCode();

                if (this.TaxType != null)
                    hash = hash * 59 + this.TaxType.GetHashCode();

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
