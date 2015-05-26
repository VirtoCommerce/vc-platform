using Klarna.Checkout;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;

namespace Klarna.PaymentGatewaysModule.Web.Managers
{
	public class KlarnaPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
	{
		private static string TestBaseUrl = "https://checkout.testdrive.klarna.com/checkout/orders";
		private static string ContentType = "application/vnd.klarna.checkout.aggregated-order-v2+json";

		private static string KlarnaModeStoreSetting = "Klarna.Mode";
		private static string KlarnaAppKeyStoreSetting = "Klarna.AppKey";
		private static string KlarnaAppSecretStoreSetting = "Klarna.AppSecret";

		public KlarnaPaymentMethod()
			: base("Klarna")
		{
		}

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.PreparedForm; }
		}

		public override ProcessPaymentResult ProcessPayment(VirtoCommerce.Domain.Common.IEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			//var paymentEvaluationContext = context as PaymentEvaluationContext;

			//if (paymentEvaluationContext.Order != null && paymentEvaluationContext.Store != null && paymentEvaluationContext.Payment != null)
			//{
			//	var cartItems = CreateKlarnaCartItems(paymentEvaluationContext.Order);

			//	//Create cart
			//	var cart = new Dictionary<string, object> { { "items", cartItems } };
			//	var data = new Dictionary<string, object>
			//	{
			//		{ "cart", cart }
			//	};

			//	//Create klarna order
			//	var connector = Connector.Create(_secretKey);
			//	Order order = null;
			//	var merchant = new Dictionary<string, object>
			//	{
			//		{ "id", _appKey.ToString() },
			//		{ "terms_uri", klarnaPaymentInfo.TermsUrl },
			//		{ "checkout_uri", klarnaPaymentInfo.CheckoutUrl },
			//		{ "confirmation_uri", klarnaPaymentInfo.ConfirmationUrl },
			//		{ "push_uri", klarnaPaymentInfo.PushUrl }
			//	};
			//	//data.Add("purchase_country", paymentEvaluationContext.Order.C);
			//	data.Add("purchase_currency", paymentEvaluationContext.Order.Currency.ToString());
			//	data.Add("locale", paymentEvaluationContext.Order.Locale);
			//	data.Add("merchant", merchant);
			//	order =
			//		new Order(connector)
			//		{
			//			BaseUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders"),
			//			ContentType = ContentType
			//		};
			//	order.Create(data);
			//	order.Fetch();

			//	//Gets snippet
			//	var gui = order.GetValue("gui") as JObject;
			//	var html = gui["snippet"].Value<string>();

			//	retVal.HtmlForm = html;
			//}

			return retVal;
		}

		public override PostProcessPaymentResult PostProcessPayment(VirtoCommerce.Domain.Common.IEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			var paymentEvaluationContext = context as PaymentEvaluationContext;

			return retVal;
		}

		public new List<Dictionary<string, object>> CreateKlarnaCartItems(CustomerOrder order)
		{
			var cartItems = new List<Dictionary<string, object>>();
			foreach (var lineItem in order.Items)
			{
				var addedItem = new Dictionary<string, object>();

				addedItem.Add("type", "physical");

				if (!string.IsNullOrEmpty(lineItem.Name))
				{
					addedItem.Add("name", lineItem.Name);
				}
				if (lineItem.Quantity > 0)
				{
					addedItem.Add("quantity", lineItem.Quantity);
				}
				if (lineItem.BasePrice > 0)
				{
					addedItem.Add("unit_price", lineItem.BasePrice);
				}
				cartItems.Add(addedItem);
			}

			return cartItems;
		}

		//public 
	}
}