using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Order.Model
{
	public class PaymentIn : Operation
	{
		public string Purpose { get; set; }

		public string GatewayCode { get; set; }

		public string OrganizationId { get; set; }
		public string CustomerId { get; set; }

		public DateTime? IncomingDate { get; set; }
		public string OuterId { get; set; }
		public Address BillingAddress { get; set; }

		public override IEnumerable<Operation> ChildrenOperations
		{
			get { return Enumerable.Empty<Operation>(); }
		}
	}
}
