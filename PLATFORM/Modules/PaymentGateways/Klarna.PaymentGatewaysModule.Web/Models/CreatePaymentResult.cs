using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klarna.PaymentGatewaysModule.Web.Models
{
	public class CreatePaymentResult
	{
		public string Html { get; set; }
		public bool IsSuccess { get; set; }
	}
}