using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class Discount
	{
		public string PromotionId { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes? Currency { get; set; }
		public decimal DiscountAmount { get; set; }

		public Coupon Coupon { get; set; }
		public string Description { get; set; }
	}
}