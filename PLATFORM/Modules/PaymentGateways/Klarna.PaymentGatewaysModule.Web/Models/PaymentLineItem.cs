using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klarna.PaymentGatewaysModule.Web.Models
{
	[Serializable]
	public class PaymentLineItem
	{
		public string Type { get; set; }
		public string Reference { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public int UnitPrice { get; set; }
		public int DiscountRate { get; set; }
		public int TaxRate { get; set; }
	}
}