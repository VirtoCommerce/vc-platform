using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Web.Model
{
    public class CardSummary : ValueObject<CardSummary>
    {
        /// <summary>
        /// Gets or sets the value of currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart total cost
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart total line items count
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart total quantity (line items count * quantity)
        /// </summary>
        public int TotalQuantity { get; set; }
    }
}