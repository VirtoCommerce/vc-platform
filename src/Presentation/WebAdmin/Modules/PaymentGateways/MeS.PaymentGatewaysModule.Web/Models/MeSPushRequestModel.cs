using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace MeS.PaymentGatewaysModule.Web.Models
{
	public class MeSPushRequestModel
	{
		public string TranType { get; set; }
		public decimal TranAmount { get; set; }
		public string InvoiceNumber { get; set; }
		public string CurrencyCode { get; set; }
		public string ClientRefNumber { get; set; }
		public string AcctNumber { get; set; }
		public DateTime ExpDate { get; set; }
		public string BillingAddress { get; set; }
		public string BillingZip { get; set; }
		public string RetrievalRefNumber { get; set; }
		public string AuthCode { get; set; }
		public string RespCode { get; set; }
		public string RespText { get; set; }
		public string TranId { get; set; }
		public string TranDate { get; set; }
		public string CardId { get; set; }
		public string CardholderFirstName { get; set; }
		public string CardholderLastName { get; set; }
		public string CardholderStreetAddress { get; set; }
		public string CardholderZip { get; set; }
		public string CardholderPhone { get; set; }
		public string ShipToFirstName { get; set; }
		public string ShipToLastName { get; set; }
		public string ShipToAddress { get; set; }
	}
}