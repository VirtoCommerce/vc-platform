using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class Payment : Entity
	{
		
		public string OuterId { get; set; }
		public string PaymentGatewayCode { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes Currency { get; set; }
		public decimal Amount { get; set; }

		public Address BillingAddress { get; set; }
	}
}
