using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.CartModule.Web.Model
{
	public class CardSummary : ValueObject<CardSummary>
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes Currency { get; set; }
		public decimal Total { get; set; }
		public int ItemCount { get; set; }
		public int TotalQuantity { get; set; }
	}
}