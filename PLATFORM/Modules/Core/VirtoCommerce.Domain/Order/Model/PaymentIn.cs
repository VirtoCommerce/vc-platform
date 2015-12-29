using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Payment.Model;

namespace VirtoCommerce.Domain.Order.Model
{
	public class PaymentIn : Operation, IFinanceInOperation
	{
		public string Purpose { get; set; }
        /// <summary>
        /// Payment method (gateway) code
        /// </summary>
		public string GatewayCode { get; set; }
        /// <summary>
        /// Payment method contains additional payment method information
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }
		public string OrganizationId { get; set; }
		public string OrganizationName { get; set; }

		public string CustomerId { get; set; }
		public string CustomerName { get; set; }

		public DateTime? IncomingDate { get; set; }
		public string OuterId { get; set; }
		public Address BillingAddress { get; set; }

		public PaymentStatus PaymentStatus { get; set; }
		public DateTime? AuthorizedDate { get; set; }
		public DateTime? CapturedDate { get; set; }
		public DateTime? VoidedDate { get; set; }
	}
}
