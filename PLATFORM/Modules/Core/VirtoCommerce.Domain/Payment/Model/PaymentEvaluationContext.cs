using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Payment.Model
{
	public class PaymentEvaluationContext : IEvaluationContext
	{
		public PaymentEvaluationContext()
		{

		}

		public PaymentEvaluationContext(PaymentIn payment)
		{
			this.Payment = payment;
		}

		public PaymentIn Payment { get; set; }

		public string OrderId { get; set; }

		public string OuterId { get; set; }
	}
}
