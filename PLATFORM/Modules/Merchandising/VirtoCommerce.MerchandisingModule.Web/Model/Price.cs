using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Price
    {
        /// <summary>
        /// Gets or sets the value of original price
        /// </summary>
        public decimal List { get; set; }

        /// <summary>
        /// Gets or sets the value of minimum catalog item quantity for current price
        /// </summary>
        public int MinQuantity { get; set; }

        /// <summary>
        /// Gets or sets the value of pricelist id
        /// </summary>
        public string PricelistId { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item id
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the value for sale price (include static discount amount)
        /// </summary>
        public decimal? Sale { get; set; }

        /// <summary>
        /// Gets or sets the value of price currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCodes Currency { get; set; }
    }
}