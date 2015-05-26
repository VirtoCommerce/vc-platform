using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;


namespace VirtoCommerce.CartModule.Web.Model
{
	public class Discount : ValueObject<Coupon>
	{
		public string PromotionId { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes Currency { get; set; }
		public decimal DiscountAmount { get; set; }
	
		public string Description { get; set; }
	}
}
