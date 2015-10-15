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
  public class VirtoCommerceOrderModuleWebModelLineItem {
    
    /// <summary>
    /// Price with tax and without dicount
    /// </summary>
    /// <value>Price with tax and without dicount</value>
    [DataMember(Name="basePrice", EmitDefaultValue=false)]
    public double? BasePrice { get; set; }

    
    /// <summary>
    /// Price with tax and discount
    /// </summary>
    /// <value>Price with tax and discount</value>
    [DataMember(Name="price", EmitDefaultValue=false)]
    public double? Price { get; set; }

    
    /// <summary>
    /// discount amount
    /// </summary>
    /// <value>discount amount</value>
    [DataMember(Name="discountAmount", EmitDefaultValue=false)]
    public double? DiscountAmount { get; set; }

    
    /// <summary>
    /// Tax sum
    /// </summary>
    /// <value>Tax sum</value>
    [DataMember(Name="tax", EmitDefaultValue=false)]
    public double? Tax { get; set; }

    
    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Reserve quantity
    /// </summary>
    /// <value>Reserve quantity</value>
    [DataMember(Name="reserveQuantity", EmitDefaultValue=false)]
    public int? ReserveQuantity { get; set; }

    
    /// <summary>
    /// Gets or Sets Quantity
    /// </summary>
    [DataMember(Name="quantity", EmitDefaultValue=false)]
    public int? Quantity { get; set; }

    
    /// <summary>
    /// Gets or Sets ProductId
    /// </summary>
    [DataMember(Name="productId", EmitDefaultValue=false)]
    public string ProductId { get; set; }

    
    /// <summary>
    /// Gets or Sets CatalogId
    /// </summary>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or Sets CategoryId
    /// </summary>
    [DataMember(Name="categoryId", EmitDefaultValue=false)]
    public string CategoryId { get; set; }

    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets ImageUrl
    /// </summary>
    [DataMember(Name="imageUrl", EmitDefaultValue=false)]
    public string ImageUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets DisplayName
    /// </summary>
    [DataMember(Name="displayName", EmitDefaultValue=false)]
    public string DisplayName { get; set; }

    
    /// <summary>
    /// Gets or Sets IsGift
    /// </summary>
    [DataMember(Name="isGift", EmitDefaultValue=false)]
    public bool? IsGift { get; set; }

    
    /// <summary>
    /// Gets or Sets ShippingMethodCode
    /// </summary>
    [DataMember(Name="shippingMethodCode", EmitDefaultValue=false)]
    public string ShippingMethodCode { get; set; }

    
    /// <summary>
    /// Gets or Sets FulfilmentLocationCode
    /// </summary>
    [DataMember(Name="fulfilmentLocationCode", EmitDefaultValue=false)]
    public string FulfilmentLocationCode { get; set; }

    
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
    /// Flag represent that line item was canceled
    /// </summary>
    /// <value>Flag represent that line item was canceled</value>
    [DataMember(Name="isCancelled", EmitDefaultValue=false)]
    public bool? IsCancelled { get; set; }

    
    /// <summary>
    /// Gets or Sets CancelledDate
    /// </summary>
    [DataMember(Name="cancelledDate", EmitDefaultValue=false)]
    public DateTime? CancelledDate { get; set; }

    
    /// <summary>
    /// Text representation of cancel reason
    /// </summary>
    /// <value>Text representation of cancel reason</value>
    [DataMember(Name="cancelReason", EmitDefaultValue=false)]
    public string CancelReason { get; set; }

    
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
      sb.Append("class VirtoCommerceOrderModuleWebModelLineItem {\n");
      
      sb.Append("  BasePrice: ").Append(BasePrice).Append("\n");
      
      sb.Append("  Price: ").Append(Price).Append("\n");
      
      sb.Append("  DiscountAmount: ").Append(DiscountAmount).Append("\n");
      
      sb.Append("  Tax: ").Append(Tax).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  ReserveQuantity: ").Append(ReserveQuantity).Append("\n");
      
      sb.Append("  Quantity: ").Append(Quantity).Append("\n");
      
      sb.Append("  ProductId: ").Append(ProductId).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  ImageUrl: ").Append(ImageUrl).Append("\n");
      
      sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
      
      sb.Append("  IsGift: ").Append(IsGift).Append("\n");
      
      sb.Append("  ShippingMethodCode: ").Append(ShippingMethodCode).Append("\n");
      
      sb.Append("  FulfilmentLocationCode: ").Append(FulfilmentLocationCode).Append("\n");
      
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      
      sb.Append("  Weight: ").Append(Weight).Append("\n");
      
      sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
      
      sb.Append("  Height: ").Append(Height).Append("\n");
      
      sb.Append("  Length: ").Append(Length).Append("\n");
      
      sb.Append("  Width: ").Append(Width).Append("\n");
      
      sb.Append("  TaxType: ").Append(TaxType).Append("\n");
      
      sb.Append("  IsCancelled: ").Append(IsCancelled).Append("\n");
      
      sb.Append("  CancelledDate: ").Append(CancelledDate).Append("\n");
      
      sb.Append("  CancelReason: ").Append(CancelReason).Append("\n");
      
      sb.Append("  Discount: ").Append(Discount).Append("\n");
      
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
