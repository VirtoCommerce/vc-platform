using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Foundation.Money;

namespace Klarna.PaymentGatewaysModule.Web.Models
{
	public class KlarnaPaymentInfo
	{
		//public KlarnaPaymentInfo()
		//{
		//	this.GatewayCode = "Klarna";
		//	this.CreatedDate = DateTime.UtcNow;
		//	this.Approved = false;
		//}

		//public string GatewayCode { get; private set; }
		//public DateTime? CreatedDate { get; private set; }
		//public bool Approved { get; set; }

		public string PurchaseCurrency { get; set; }
		public string PurchaseCountry { get; set; }
		public string Locale { get; set; }

		public string TermsUrl { get; set; }
		public string CheckoutUrl { get; set; }
		public string ConfirmationUrl { get; set; }
		public string PushUrl { get; set; }

		public decimal Amount { get; set; }

		public PaymentLineItem[] LineItems { get; set; }
	}

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