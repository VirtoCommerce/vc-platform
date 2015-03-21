using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeS.PaymentGatewaysModule.Web.Models
{
	public class GetPaymentParametersResult
	{
		public string MeSAccountId { get; set; }
		public string InvoiceNumber { get; set; }
	}
}