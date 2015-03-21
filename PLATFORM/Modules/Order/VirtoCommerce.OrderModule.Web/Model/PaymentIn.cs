using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class PaymentIn : Operation
	{
		public string Organization { get; set; }
		public string OrganizationId { get; set; }
		public string Customer { get; set; }
		public string CustomerId { get; set; }

		public string Purpose { get; set; }

		public string GatewayCode { get; set; }

		public DateTime? IncomingDate { get; set; }
		public string OuterId { get; set; }
	}
}