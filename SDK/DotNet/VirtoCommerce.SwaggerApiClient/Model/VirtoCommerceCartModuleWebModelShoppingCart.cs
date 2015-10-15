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
  public class VirtoCommerceCartModuleWebModelShoppingCart {
    
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
    public VirtoCommerceCartModuleWebModelCoupon Coupon { get; set; }

    
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
