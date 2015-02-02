using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class PaymentEntity : Entity
	{
		public string OuterId { get; set; }
		public string Currency { get; set; }
		public string PaymentGatewayCode { get; set; }
		public decimal Amount { get; set; }

		public ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }

		public AddressEntity BillingAddress { get; set; }
	}
}
