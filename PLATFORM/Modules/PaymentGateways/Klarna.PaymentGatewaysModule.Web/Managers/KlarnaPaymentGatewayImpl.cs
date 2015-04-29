using Klarna.Checkout;
using Klarna.PaymentGatewaysModule.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Common;

namespace Klarna.PaymentGatewaysModule.Web.Managers
{
	public class KlarnaPaymentGatewayImpl : IPaymentGateway
	{
		private int _appKey;
		private string _secretKey;
		private string _contentType = "application/vnd.klarna.checkout.aggregated-order-v2+json";

		private string _gatewayCode;
		private string _description;
		private string _logoUrl;

		public KlarnaPaymentGatewayImpl(int appKey, string secretKey, string gatewayCode, string description, string logoUrl)
		{
			if (appKey == 0)
				throw new ArgumentNullException("appKey");

			if (string.IsNullOrEmpty(secretKey))
				throw new ArgumentNullException("secretKey");

			_appKey = appKey;
			_secretKey = secretKey;

			_gatewayCode = gatewayCode;
			_description = description;
			_logoUrl = logoUrl;
		}

		public string GatewayCode
		{
			get { return _gatewayCode; }
		}

		public string Description
		{
			get { return _description; }
		}

		public string LogoUrl
		{
			get { return _logoUrl; }
		}

		public PaymentInfo GetPaymentById(string id)
		{
			var retVal = new PaymentInfo();

			Uri resourceUri = new Uri(id);

			var connector = Connector.Create(_secretKey);

			Order order = new Order(connector, resourceUri)
			{
				ContentType = _contentType
			};

			order.Fetch();
			var status = order.GetValue("status") as string;

			if (status == "checkout_complete")
			{
				var data = new Dictionary<string, object> { { "status", "created" } };
				order.Update(data);
			}

			var klarnaCart = order.GetValue("cart") as JObject;

			var amount = (decimal)klarnaCart["total_price_including_tax"];
			retVal.Amount = amount;

			retVal.IsApproved = status == "checkout_complete";

			var createdDate = order.GetValue("started_at") as string;
			DateTime dateTime;
			retVal.CreatedDate = DateTime.TryParse(createdDate, out dateTime) ? dateTime : new Nullable<DateTime>();

			var currency = order.GetValue("purchase_currency") as string;
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), currency);

			var gatewayCode = order.GetValue("id") as string;
			retVal.GatewayCode = gatewayCode;

			return retVal;
		}

		public PaymentInfo CreatePayment(PaymentInfo paymentInfo)
		{
			var klarnaPaymentInfo = paymentInfo as KlarnaPaymentInfo;

			if (klarnaPaymentInfo != null)
			{
				//Create lineItems
				var cartItems = new List<Dictionary<string, object>>();
				foreach (var lineItem in klarnaPaymentInfo.LineItems)
				{
					var addedItem = new Dictionary<string, object>();

					if (!string.IsNullOrEmpty(lineItem.Type))
					{
						addedItem.Add("type", lineItem.Type);
					}
					if (!string.IsNullOrEmpty(lineItem.Name))
					{
						addedItem.Add("name", lineItem.Name);
					}
					if (!string.IsNullOrEmpty(lineItem.Reference))
					{
						addedItem.Add("reference", lineItem.Reference);
					}
					if (lineItem.Quantity > 0)
					{
						addedItem.Add("quantity", lineItem.Quantity);
					}
					if (lineItem.UnitPrice > 0)
					{
						addedItem.Add("unit_price", lineItem.UnitPrice);
					}
					if (lineItem.DiscountRate > 0)
					{
						addedItem.Add("discount_rate", lineItem.DiscountRate);
					}
					if (lineItem.TaxRate > 0)
					{
						addedItem.Add("tax_rate", lineItem.TaxRate);
					}
					cartItems.Add(addedItem);
				}

				//Create cart
				var cart = new Dictionary<string, object> { { "items", cartItems } };
				var data = new Dictionary<string, object>
				{
					{ "cart", cart }
				};

				//Create klarna order
				var connector = Connector.Create(_secretKey);
				Order order = null;
				var merchant = new Dictionary<string, object>
				{
					{ "id", _appKey.ToString() },
					{ "terms_uri", klarnaPaymentInfo.TermsUrl },
					{ "checkout_uri", klarnaPaymentInfo.CheckoutUrl },
					{ "confirmation_uri", klarnaPaymentInfo.ConfirmationUrl },
					{ "push_uri", klarnaPaymentInfo.PushUrl }
				};
				data.Add("purchase_country", klarnaPaymentInfo.PurchaseCountry);
				data.Add("purchase_currency", klarnaPaymentInfo.PurchaseCurrency);
				data.Add("locale", klarnaPaymentInfo.Locale);
				data.Add("merchant", merchant);
				order =
					new Order(connector)
					{
						BaseUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders"),
						ContentType = _contentType
					};
				order.Create(data);
				order.Fetch();

				//Gets snippet
				var gui = order.GetValue("gui") as JObject;
				var html = gui["snippet"].Value<string>();

				klarnaPaymentInfo.Html = html;
			}

			return klarnaPaymentInfo;
		}
	}
}