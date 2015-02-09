using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Foundation.Money;

namespace Klarna.PaymentGatewaysModule.Web.Models
{
	public class KlarnaPaymentInfo : PaymentInfo
	{


		public string PurchaseCurrency { get; set; }
		public string PurchaseCountry { get; set; }
		public string Locale { get; set; }

		public string TermsUrl { get; set; }
		public string CheckoutUrl { get; set; }
		public string ConfirmationUrl { get; set; }
		public string PushUrl { get; set; }

		//public decimal Amount { get; set; }

		public string Html { get; set; }
		public bool Success
		{
			get
			{
				return !string.IsNullOrEmpty(this.Html);
			}
		}

		public PaymentLineItem[] LineItems { get; set; }
	}
}