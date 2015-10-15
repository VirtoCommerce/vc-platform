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
  public class VirtoCommerceMerchandisingModuleWebModelPromotionReward {
    
    /// <summary>
    /// Gets or sets the flag of promotion reward is valid. Also used as a flag for applicability (applied or potential)
    /// </summary>
    /// <value>Gets or sets the flag of promotion reward is valid. Also used as a flag for applicability (applied or potential)</value>
    [DataMember(Name="isValid", EmitDefaultValue=false)]
    public bool? IsValid { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion reward description
    /// </summary>
    /// <value>Gets or sets the value of promotion reward description</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or sets the value of coupon amount
    /// </summary>
    /// <value>Gets or sets the value of coupon amount</value>
    [DataMember(Name="couponAmount", EmitDefaultValue=false)]
    public double? CouponAmount { get; set; }

    
    /// <summary>
    /// Gets or sets the value of coupon code
    /// </summary>
    /// <value>Gets or sets the value of coupon code</value>
    [DataMember(Name="coupon", EmitDefaultValue=false)]
    public string Coupon { get; set; }

    
    /// <summary>
    /// Gets or sets the value of minimum order total cost for applying coupon
    /// </summary>
    /// <value>Gets or sets the value of minimum order total cost for applying coupon</value>
    [DataMember(Name="couponMinOrderAmount", EmitDefaultValue=false)]
    public double? CouponMinOrderAmount { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion id
    /// </summary>
    /// <value>Gets or sets the value of promotion id</value>
    [DataMember(Name="promotionId", EmitDefaultValue=false)]
    public string PromotionId { get; set; }

    
    /// <summary>
    /// Gets or sets the promotion
    /// </summary>
    /// <value>Gets or sets the promotion</value>
    [DataMember(Name="promotion", EmitDefaultValue=false)]
    public VirtoCommerceMerchandisingModuleWebModelPromotion Promotion { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion reward type
    /// </summary>
    /// <value>Gets or sets the value of promotion reward type</value>
    [DataMember(Name="rewardType", EmitDefaultValue=false)]
    public string RewardType { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion reward amount type
    /// </summary>
    /// <value>Gets or sets the value of promotion reward amount type</value>
    [DataMember(Name="amountType", EmitDefaultValue=false)]
    public string AmountType { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion reward amount
    /// </summary>
    /// <value>Gets or sets the value of promotion reward amount</value>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    public double? Amount { get; set; }

    
    /// <summary>
    /// Gets or sets the value of line item quantity for applying promotion reward
    /// </summary>
    /// <value>Gets or sets the value of line item quantity for applying promotion reward</value>
    [DataMember(Name="quantity", EmitDefaultValue=false)]
    public int? Quantity { get; set; }

    
    /// <summary>
    /// Gets or sets the value of line item id
    /// </summary>
    /// <value>Gets or sets the value of line item id</value>
    [DataMember(Name="lineItemId", EmitDefaultValue=false)]
    public string LineItemId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of product id
    /// </summary>
    /// <value>Gets or sets the value of product id</value>
    [DataMember(Name="productId", EmitDefaultValue=false)]
    public string ProductId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of category id
    /// </summary>
    /// <value>Gets or sets the value of category id</value>
    [DataMember(Name="categoryId", EmitDefaultValue=false)]
    public string CategoryId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of measurement unit
    /// </summary>
    /// <value>Gets or sets the value of measurement unit</value>
    [DataMember(Name="measureUnit", EmitDefaultValue=false)]
    public string MeasureUnit { get; set; }

    
    /// <summary>
    /// Gets or sets the value of promotion reward logo absolute URL
    /// </summary>
    /// <value>Gets or sets the value of promotion reward logo absolute URL</value>
    [DataMember(Name="imageUrl", EmitDefaultValue=false)]
    public string ImageUrl { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelPromotionReward {\n");
      
      sb.Append("  IsValid: ").Append(IsValid).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  CouponAmount: ").Append(CouponAmount).Append("\n");
      
      sb.Append("  Coupon: ").Append(Coupon).Append("\n");
      
      sb.Append("  CouponMinOrderAmount: ").Append(CouponMinOrderAmount).Append("\n");
      
      sb.Append("  PromotionId: ").Append(PromotionId).Append("\n");
      
      sb.Append("  Promotion: ").Append(Promotion).Append("\n");
      
      sb.Append("  RewardType: ").Append(RewardType).Append("\n");
      
      sb.Append("  AmountType: ").Append(AmountType).Append("\n");
      
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      
      sb.Append("  Quantity: ").Append(Quantity).Append("\n");
      
      sb.Append("  LineItemId: ").Append(LineItemId).Append("\n");
      
      sb.Append("  ProductId: ").Append(ProductId).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
      sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
      
      sb.Append("  ImageUrl: ").Append(ImageUrl).Append("\n");
      
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
