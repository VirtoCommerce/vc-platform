using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;


namespace VirtoCommerce.CartModule.Web.Model
{
    public class Discount : ValueObject<Discount>
    {
        /// <summary>
        /// Gets or sets the value of promotion id
        /// </summary>
        public string PromotionId { get; set; }

        /// <summary>
        /// Gets or sets the value of currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of discount amount
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the value of discount description
        /// </summary>
        public string Description { get; set; }
    }
}