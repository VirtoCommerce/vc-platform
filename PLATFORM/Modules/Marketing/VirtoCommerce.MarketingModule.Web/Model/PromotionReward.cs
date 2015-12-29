using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model
{
    public class PromotionReward
    {
        /// <summary>
        /// Gets or sets the flag of promotion reward is valid. Also used as a flag for applicability (applied or potential)
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion reward description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value of coupon amount
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// Gets or sets the value of coupon code
        /// </summary>
        public string Coupon { get; set; }

        /// <summary>
        /// Gets or sets the value of minimum order total cost for applying coupon
        /// </summary>
        public decimal? CouponMinOrderAmount { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion id
        /// </summary>
        public string PromotionId { get; set; }

        /// <summary>
        /// Gets or sets the promotion
        /// </summary>
        /// <value>
        /// Promotion object
        /// </value>
        public Promotion Promotion { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion reward type
        /// </summary>
        public string RewardType { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion reward amount type
        /// </summary>
        /// <value>
        /// "Absolute" or "Relative"
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public RewardAmountType AmountType { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion reward amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the value of line item quantity for applying promotion reward
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the value of line item id
        /// </summary>
        public string LineItemId { get; set; }

        /// <summary>
        /// Gets or sets the value of product id
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the value of category id
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the value of measurement unit
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion reward logo absolute URL
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Gets or sets the value of reward shipping method code
        /// </summary>
        public string ShippingMethod { get; set; }
    }
}