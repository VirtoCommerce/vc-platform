using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Payment.Model;

namespace VirtoCommerce.Domain.Payment.Model
{
	public class ProcessPaymentResult
	{
		public PaymentStatus NewPaymentStatus { get; set; }

		public string RedirectUrl { get; set; }

		public string HtmlForm { get; set; }

		public bool IsSuccess { get; set; }

		public string Error { get; set; }

		public string OuterId { get; set; }
	}
}
