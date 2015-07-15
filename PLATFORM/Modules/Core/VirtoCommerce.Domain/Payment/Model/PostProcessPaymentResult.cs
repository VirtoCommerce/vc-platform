using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Model
{
	public class PostProcessPaymentResult : IProcessResult
	{
		public PaymentStatus NewPaymentStatus { get; set; }

		public bool IsSuccess { get; set; }

		public string ErrorMessage { get; set; }

		public string ReturnUrl { get; set; }

		public string OrderId { get; set; }

		public string OuterId { get; set; }
	}
}
