using System;
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
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes Currency { get; set; }
		public ICollection<ProductPrice> ProductPrices { get; set; }
		public ICollection<PricelistAssignment> Assignments { get; set; }

	}
}
