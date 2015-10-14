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
  public class VirtoCommerceCartModuleWebModelShipment {
    
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
    public List<VirtoCommerceCartModuleWebModelLineItem> Items { get; set; }

    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
