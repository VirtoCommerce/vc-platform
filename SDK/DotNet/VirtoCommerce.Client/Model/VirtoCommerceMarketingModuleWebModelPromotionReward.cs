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
    public partial class VirtoCommerceMarketingModuleWebModelPromotionReward :  IEquatable<VirtoCommerceMarketingModuleWebModelPromotionReward>
    {
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
        public VirtoCommerceMarketingModuleWebModelPromotion Promotion { get; set; }

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
        /// Gets or sets the value of reward shipping method code
        /// </summary>
        /// <value>Gets or sets the value of reward shipping method code</value>
        [DataMember(Name="shippingMethod", EmitDefaultValue=false)]
        public string ShippingMethod { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMarketingModuleWebModelPromotionReward {\n");
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
            sb.Append("  ShippingMethod: ").Append(ShippingMethod).Append("\n");
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
            return this.Equals(obj as VirtoCommerceMarketingModuleWebModelPromotionReward);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMarketingModuleWebModelPromotionReward instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceMarketingModuleWebModelPromotionReward to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMarketingModuleWebModelPromotionReward other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.IsValid == other.IsValid ||
                    this.IsValid != null &&
                    this.IsValid.Equals(other.IsValid)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.CouponAmount == other.CouponAmount ||
                    this.CouponAmount != null &&
                    this.CouponAmount.Equals(other.CouponAmount)
                ) && 
                (
                    this.Coupon == other.Coupon ||
                    this.Coupon != null &&
                    this.Coupon.Equals(other.Coupon)
                ) && 
                (
                    this.CouponMinOrderAmount == other.CouponMinOrderAmount ||
                    this.CouponMinOrderAmount != null &&
                    this.CouponMinOrderAmount.Equals(other.CouponMinOrderAmount)
                ) && 
                (
                    this.PromotionId == other.PromotionId ||
                    this.PromotionId != null &&
                    this.PromotionId.Equals(other.PromotionId)
                ) && 
                (
                    this.Promotion == other.Promotion ||
                    this.Promotion != null &&
                    this.Promotion.Equals(other.Promotion)
                ) && 
                (
                    this.RewardType == other.RewardType ||
                    this.RewardType != null &&
                    this.RewardType.Equals(other.RewardType)
                ) && 
                (
                    this.AmountType == other.AmountType ||
                    this.AmountType != null &&
                    this.AmountType.Equals(other.AmountType)
                ) && 
                (
                    this.Amount == other.Amount ||
                    this.Amount != null &&
                    this.Amount.Equals(other.Amount)
                ) && 
                (
                    this.Quantity == other.Quantity ||
                    this.Quantity != null &&
                    this.Quantity.Equals(other.Quantity)
                ) && 
                (
                    this.LineItemId == other.LineItemId ||
                    this.LineItemId != null &&
                    this.LineItemId.Equals(other.LineItemId)
                ) && 
                (
                    this.ProductId == other.ProductId ||
                    this.ProductId != null &&
                    this.ProductId.Equals(other.ProductId)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.MeasureUnit == other.MeasureUnit ||
                    this.MeasureUnit != null &&
                    this.MeasureUnit.Equals(other.MeasureUnit)
                ) && 
                (
                    this.ImageUrl == other.ImageUrl ||
                    this.ImageUrl != null &&
                    this.ImageUrl.Equals(other.ImageUrl)
                ) && 
                (
                    this.ShippingMethod == other.ShippingMethod ||
                    this.ShippingMethod != null &&
                    this.ShippingMethod.Equals(other.ShippingMethod)
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

                if (this.IsValid != null)
                    hash = hash * 59 + this.IsValid.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.CouponAmount != null)
                    hash = hash * 59 + this.CouponAmount.GetHashCode();

                if (this.Coupon != null)
                    hash = hash * 59 + this.Coupon.GetHashCode();

                if (this.CouponMinOrderAmount != null)
                    hash = hash * 59 + this.CouponMinOrderAmount.GetHashCode();

                if (this.PromotionId != null)
                    hash = hash * 59 + this.PromotionId.GetHashCode();

                if (this.Promotion != null)
                    hash = hash * 59 + this.Promotion.GetHashCode();

                if (this.RewardType != null)
                    hash = hash * 59 + this.RewardType.GetHashCode();

                if (this.AmountType != null)
                    hash = hash * 59 + this.AmountType.GetHashCode();

                if (this.Amount != null)
                    hash = hash * 59 + this.Amount.GetHashCode();

                if (this.Quantity != null)
                    hash = hash * 59 + this.Quantity.GetHashCode();

                if (this.LineItemId != null)
                    hash = hash * 59 + this.LineItemId.GetHashCode();

                if (this.ProductId != null)
                    hash = hash * 59 + this.ProductId.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                if (this.MeasureUnit != null)
                    hash = hash * 59 + this.MeasureUnit.GetHashCode();

                if (this.ImageUrl != null)
                    hash = hash * 59 + this.ImageUrl.GetHashCode();

                if (this.ShippingMethod != null)
                    hash = hash * 59 + this.ShippingMethod.GetHashCode();

                return hash;
            }
        }

    }
}
