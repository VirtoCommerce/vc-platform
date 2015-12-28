using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// The discount object contains information about a discount, including its id,
    /// code, amount, savings, and type. 
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/discount
    /// </remarks>
    [DataContract]
    public class Discount : Drop
    {
        /// <summary>
        /// Returns the amount of the discount. Use one of the money filters to return
        /// the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Returns the title or discount code of the discount. Same as discount.title.
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Returns the id of the discount.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Returns the amount of the discount's savings. The negative opposite of amount.
        /// Use one of the money filters to return the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal Savings { get; set; }

        /// <summary>
        /// Returns the type of the discount. The possible values of discount.type are:
        /// FixedAmountDiscount, PercentageDiscount, ShippingDiscount
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Returns the promotion id
        /// </summary>
        public string PromotionId { get; set; }

        /// <summary>
        /// Returns the coupon code
        /// </summary>
        public string Coupon { get; set; }
    }
}