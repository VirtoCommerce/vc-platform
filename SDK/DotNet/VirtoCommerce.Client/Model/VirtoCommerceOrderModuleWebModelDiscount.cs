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
    public partial class VirtoCommerceOrderModuleWebModelDiscount :  IEquatable<VirtoCommerceOrderModuleWebModelDiscount>
    {
        /// <summary>
        /// Gets or Sets PromotionId
        /// </summary>
        [DataMember(Name="promotionId", EmitDefaultValue=false)]
        public string PromotionId { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or Sets DiscountAmount
        /// </summary>
        [DataMember(Name="discountAmount", EmitDefaultValue=false)]
        public double? DiscountAmount { get; set; }

        /// <summary>
        /// Gets or Sets Coupon
        /// </summary>
        [DataMember(Name="coupon", EmitDefaultValue=false)]
        public VirtoCommerceOrderModuleWebModelCoupon Coupon { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceOrderModuleWebModelDiscount {\n");
            sb.Append("  PromotionId: ").Append(PromotionId).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  DiscountAmount: ").Append(DiscountAmount).Append("\n");
            sb.Append("  Coupon: ").Append(Coupon).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelDiscount);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelDiscount instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelDiscount to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelDiscount other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.PromotionId == other.PromotionId ||
                    this.PromotionId != null &&
                    this.PromotionId.Equals(other.PromotionId)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.DiscountAmount == other.DiscountAmount ||
                    this.DiscountAmount != null &&
                    this.DiscountAmount.Equals(other.DiscountAmount)
                ) && 
                (
                    this.Coupon == other.Coupon ||
                    this.Coupon != null &&
                    this.Coupon.Equals(other.Coupon)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
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

                if (this.PromotionId != null)
                    hash = hash * 59 + this.PromotionId.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.DiscountAmount != null)
                    hash = hash * 59 + this.DiscountAmount.GetHashCode();

                if (this.Coupon != null)
                    hash = hash * 59 + this.Coupon.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                return hash;
            }
        }

    }
}
