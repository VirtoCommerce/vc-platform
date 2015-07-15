using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;

namespace VirtoCommerce.Domain.Payment.Model
{
	public class RefundProcessPaymentEvaluationContext
	{
		public PaymentIn Payment { get; set; }
	}
}
