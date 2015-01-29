using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeS.PaymentGatewaysModule.Web.Models
{
	public class CreatePaymentResult
	{
		public CreatePaymentResult()
		{
			this.IsSuccess = true;
		}

		public bool IsSuccess { get; set; }
		public string Html { get; set; }
	}
}