using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class Payment : Entity
	{
		public string OuterId { get; set; }
		public CurrencyCodes Currency { get; set; }
		public string PaymentGatewayCode { get; set; }
		public decimal Amount { get; set; }

		public Address BillingAddress { get; set; }
	}
}
