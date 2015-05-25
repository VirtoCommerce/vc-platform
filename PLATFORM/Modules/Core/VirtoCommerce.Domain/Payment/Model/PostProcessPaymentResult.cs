using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Model
{
	public class PostProcessPaymentResult
	{
		public PaymentStatus NewPaymentStatus { get; set; }

		public bool IsSuccess { get; set; }

		public string Error { get; set; }

		public string ReturnUrl { get; set; }
	}
}
