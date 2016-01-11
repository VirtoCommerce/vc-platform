using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.PricingModule.Web.Model
{
    public class Pricelist : AuditableEntity
	{
		public string Name { get; set; }

		public string Description { get; set; }

        /// <summary>
        /// Currency defined for all prices in the price list
        /// </summary>
		public string Currency { get; set; }

        /// <summary>
        /// List of Products prices. It includes tiered prices also
        /// </summary>
		public ICollection<ProductPrice> ProductPrices { get; set; }

        /// <summary>
        /// Assignments define condition and rules to use the price list
        /// </summary>
		public ICollection<PricelistAssignment> Assignments { get; set; }

	}
}
