using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Payment.Model;

namespace VirtoCommerce.OrderModule.Web.Model
{
	/// <summary>
	/// Represent process payment request result 
	/// </summary>
	public class ProcessPaymentResult
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public PaymentStatus NewPaymentStatus { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public PaymentMethodType PaymentMethodType { get; set; }

		/// <summary>
		/// Redirect url used for OutSite payment processing
		/// </summary>
		public string RedirectUrl { get; set; }

		public bool IsSuccess { get; set; }

		public string Error { get; set; }

		/// <summary>
		/// Generated Html form used for InSite payment processing 
		/// </summary>
		public string HtmlForm { get; set; }

		public string OuterId { get; set; }
	}
}