using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.PricingModule.Web.Model
{
    /// <summary>
    /// Represents a price of a Product in depends on batch quantity.
    /// </summary>
    public class Price : AuditableEntity
	{
		public string PricelistId { get; set; }
		
		public string Currency { get; set; }

		public string ProductId { get; set; }
        
        /// <summary>
        /// Sale price of a product. It can be null, then Sale price will be equal List price
        /// </summary>
        public decimal? Sale { get; set; }

        /// <summary>
        /// Price of a product. It can be catalog price or purchase price
        /// </summary>
        public decimal List { get; set; }

        /// <summary>
        /// It defines the minimum quantity of Products. Use it for creating tier prices.
        /// </summary>
        public int MinQuantity { get; set; }
	}
}
