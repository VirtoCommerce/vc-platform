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
  public class VirtoCommerceCartModuleWebModelLineItem {
    
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
      sb.Append("class VirtoCommerceCartModuleWebModelLineItem {\n");
      
      sb.Append("  ProductId: ").Append(ProductId).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
      sb.Append("  Sku: ").Append(Sku).Append("\n");
      
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
      
      sb.Append("  ListPrice: ").Append(ListPrice).Append("\n");
      
      sb.Append("  SalePrice: ").Append(SalePrice).Append("\n");
      
      sb.Append("  PlacedPrice: ").Append(PlacedPrice).Append("\n");
      
      sb.Append("  ExtendedPrice: ").Append(ExtendedPrice).Append("\n");
      
      sb.Append("  DiscountTotal: ").Append(DiscountTotal).Append("\n");
      
      sb.Append("  TaxTotal: ").Append(TaxTotal).Append("\n");
      
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
