using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;

namespace MeS.PaymentGatewaysModule.Web.Models
{
	public class MeSPaymentInfo
	{
		public MeSPaymentInfo()
		{
			this.GatewayCode = "MeS";
			this.CreatedDate = DateTime.UtcNow;
			this.Approved = false;
		}

		public string GatewayCode { get; private set; }
		public DateTime? CreatedDate { get; private set; }
		public bool Approved { get; set; }

		public string PurchaseCurrency { get; set; }
		public string PurchaseCountry { get; set; }
		public string Locale { get; set; }

		public string TermsUrl { get; set; }
		public string CheckoutUrl { get; set; }
		public string ConfirmationUrl { get; set; }
		public string PushUrl { get; set; }

		public decimal Amount { get; set; }

		public PaymentCart Cart { get; set; }
	}

	public class PaymentCart
	{
		private Collection<PaymentLineItem> _lineItems;
		public Collection<PaymentLineItem> LineItems { get { return _lineItems ?? (_lineItems = new Collection<PaymentLineItem>()); } }
	}

	public class PaymentLineItem
	{
		public string Type { get; set; }
		public string Reference { get; set; }
		public string Name { get; set; }
		public decimal Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal DiscountRate { get; set; }
		public decimal TaxRate { get; set; }
	}
}