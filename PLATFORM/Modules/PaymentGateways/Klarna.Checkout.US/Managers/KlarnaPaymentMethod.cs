using Klarna.Checkout;
using Klarna.Rest;
using Klarna.Rest.Checkout;
using Klarna.Rest.Models;
using Klarna.Rest.Transport;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;

namespace Klarna.Checkout.US.Managers
{
	public class KlarnaCheckoutUSPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
	{
		private const string _americaTestBaseUrl = "https://checkout-na.playground.klarna.com/checkout/orders";
		private const string _americaApiTestBaseUrl = "https://api-na.playground.klarna.com/checkout/orders";
		private const string _unitedKingdomTestBaseUrl = "https://checkout.playground.klarna.com/checkout/orders";
		private const string _unitedKingdomApiTestBaseUrl = "https://api.playground.klarna.com/checkout/orders";
		private const string _americaLiveBaseUrl = "https://checkout-na.klarna.com/checkout/orders";
		private const string _americaLiveApiBaseUrl = "https://api-na.klarna.com/checkout/orders";
		private const string _unitedKingdomBaseUrl = "https://checkout.klarna.com/checkout/orders";
		private const string _unitedKingdomApiBaseUrl = "https://api.klarna.com/checkout/orders";

		private const string _klarnaModeStoreSetting = "Klarna.Checkout.US.Mode";
		private const string _klarnaAppKeyStoreSetting = "Klarna.Checkout.US.AppKey";
		private const string _klarnaAppSecretStoreSetting = "Klarna.Checkout.US.SecretKey";
		private const string _klarnaTermsUrl = "Klarn.Checkout.US.TermsUrl";
		private const string _klarnaCheckoutUrl = "Klarna.Checkout.US.CheckoutUrl";
		private const string _klarnaConfirmationUrl = "Klarna.Checkout.US.ConfirmationUrl";

		public KlarnaCheckoutUSPaymentMethod()
			: base("KlarnaCheckoutUS")
		{
		}

		private string Mode
		{
			get
			{
				var retVal = GetSetting(_klarnaModeStoreSetting);
				return retVal;
			}
		}

		private string AppKey
		{
			get
			{
				var retVal = GetSetting(_klarnaAppKeyStoreSetting);
				return retVal;
			}
		}

		private string AppSecret
		{
			get
			{
				var retVal = GetSetting(_klarnaAppSecretStoreSetting);
				return retVal;
			}
		}

		private string TermsUrl
		{
			get
			{
				var retVal = GetSetting(_klarnaTermsUrl);
				return retVal;
			}
		}

		private string ConfirmationUrl
		{
			get
			{
				var retVal = GetSetting(_klarnaConfirmationUrl);
				return retVal;
			}
		}

		private string CheckoutUrl
		{
			get
			{
				var retVal = GetSetting(_klarnaCheckoutUrl);
				return retVal;
			}
		}

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.PreparedForm; }
		}

		public override PaymentMethodGroupType PaymentMethodGroupType
		{
			get { return PaymentMethodGroupType.Alternative; }
		}

		public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			if (context.Order != null && context.Store != null && context.Payment != null)
			{
				KlarnaLocalization localization;
				string countryName = null;
				string currency = context.Order.Currency.ToString();

				if (context.Order.Addresses != null && context.Order.Addresses.Count > 0)
				{
					var address = context.Order.Addresses.FirstOrDefault();
					countryName = address.CountryName;
				}

				if (!string.IsNullOrEmpty(countryName))
				{
					localization = GetLocalization(currency, countryName);
				}
				else
				{
					localization = GetLocalization(currency, null);
				}

				if (localization != null)
				{
					retVal = NewCreateKlarnaOrder(localization, context);
				}
				else
				{
					retVal.Error = "Klarna is not available for this order";
				}
			}

			return retVal;
		}

		public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			var localization = GetLocalization(context.Order.Currency.ToString(), null);

			retVal = NewRegisterKlarnaOrder(context);

			return retVal;
		}

		public override ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection queryString)
		{
			var retVal = new ValidatePostProcessRequestResult();

			var klarnaOrder = queryString["klarna_order"];
			var sid = queryString["sid"];

			if (!string.IsNullOrEmpty(klarnaOrder) && !string.IsNullOrEmpty(sid))
			{
				var outerId = HttpUtility.UrlDecode(klarnaOrder).Split('/').LastOrDefault();
				if (!string.IsNullOrEmpty(outerId))
				{
					retVal.IsSuccess = true;
					retVal.OuterId = outerId;
				}
			}

			return retVal;
		}

		private ProcessPaymentResult NewCreateKlarnaOrder(KlarnaLocalization localization, ProcessPaymentEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			var orderLineItems = GetOrderLineItems(context.Order);

			MerchantUrls merchantUrls = new MerchantUrls
			{
				Terms = new System.Uri(
					string.Format("{0}/{1}", context.Store.Url, TermsUrl)),
				Checkout = new System.Uri(
					string.Format("{0}/{1}", context.Store.Url, CheckoutUrl)),
				Confirmation = new System.Uri(
					string.Format("{0}/{1}?sid=123&orderId={2}&", context.Store.Url, ConfirmationUrl, context.Order.Id) + "klarna_order={checkout.order.uri}"),
				Push = new System.Uri(
					string.Format("{0}/{1}?sid=123&orderId={2}&", context.Store.Url, "admin/api/paymentcallback", context.Order.Id) + "klarna_order={checkout.order.uri}")
			};

			CheckoutOrderData orderData = new CheckoutOrderData()
			{
				PurchaseCountry = localization.CountryName,
				PurchaseCurrency = localization.Currency,
				Locale = localization.Locale,
				OrderAmount = (int)(context.Order.Sum * 100),
				OrderTaxAmount = (int)(context.Order.Tax * 100),
				OrderLines = orderLineItems,
				MerchantUrls = merchantUrls
			};

			var connector = ConnectorFactory.Create(
				AppKey,
				AppSecret,
				Client.TestBaseUrl);
			Client client = new Client(connector);

			var checkout = client.NewCheckoutOrder();
			checkout.Create(orderData);

			orderData = checkout.Fetch();
			retVal.IsSuccess = true;
			retVal.NewPaymentStatus = PaymentStatus.Pending;
			retVal.OuterId = orderData.OrderId;
			retVal.HtmlForm = string.Format("<div>{0}</div>", orderData.HtmlSnippet);

			return retVal;
		}

		private PostProcessPaymentResult NewRegisterKlarnaOrder(PostProcessPaymentEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			var connector = ConnectorFactory.Create(
				AppKey,
				AppSecret,
				Client.TestBaseUrl);

			Client client = new Client(connector);
			var order = client.NewOrder(context.OuterId);
			OrderData orderData = order.Fetch();

			if (orderData.Status != "CAPTURED")
			{
				var capture = client.NewCapture(order.Location);

				CaptureData captureData = new CaptureData()
				{
					CapturedAmount = orderData.OrderAmount,
					Description = "All order items is shipped",
					OrderLines = orderData.OrderLines
				};

				capture.Create(captureData);
				orderData = order.Fetch();
			}

			retVal.IsSuccess = orderData.Status == "CAPTURED";
			retVal.NewPaymentStatus = retVal.IsSuccess ? PaymentStatus.Paid : PaymentStatus.Pending;
			retVal.OrderId = context.Order.Id;

			return retVal;
		}

		private List<OrderLine> GetOrderLineItems(CustomerOrder order)
		{
			List<OrderLine> retVal = new List<OrderLine>();

			foreach (var lineItem in order.Items)
			{
				OrderLine orderLine = new OrderLine
				{
					Type = "physical",
					Reference = lineItem.ProductId,
					Name = lineItem.Name,
					Quantity = lineItem.Quantity,
					UnitPrice = (int)(lineItem.Price * 100),
					TotalAmount = (int)(lineItem.Price * 100) * lineItem.Quantity
				};

				if (lineItem.Tax > 0)
				{
					orderLine.TaxRate = (int)((lineItem.Price + lineItem.Tax * lineItem.Quantity) * 100);
					orderLine.TotalAmount = orderLine.TotalAmount + (int)(lineItem.Tax * lineItem.Quantity * 100);
					orderLine.TotalTaxAmount = (int)(lineItem.Tax * lineItem.Quantity *100);
				}

				retVal.Add(orderLine);
			}

			if (order.Shipments != null && order.Shipments.Any(s => s.Sum > 0))
			{
				foreach (var shipment in order.Shipments.Where(s => s.Sum > 0))
				{
					OrderLine orderLine = new OrderLine
					{
						Type = "shipping_fee",
						Reference = "SHIPPING",
						Quantity = 1,
						Name = "Shipping Fee",
						UnitPrice = (int)(shipment.Sum * 100),
						TotalAmount = (int)(shipment.Sum * 100)
					};

					if(shipment.Tax > 0)
					{
						orderLine.TaxRate = (int)((shipment.Sum + shipment.Tax) * 100);
						orderLine.TotalAmount = orderLine.TotalAmount + (int)(shipment.Tax * 100);
						orderLine.TotalTaxAmount = (int)(shipment.Tax * 100);
					}

					retVal.Add(orderLine);
				}
			}

			return retVal;
		}

		private class KlarnaLocalization
		{
			public string CountryName { get; set; }
			public string Locale { get; set; }
			public string Currency { get; set; }
			public string FullCountryName { get; set; }
		}

		private List<KlarnaLocalization> GetLocalizations()
		{
			var retVal = new List<KlarnaLocalization>();

			retVal.Add(new KlarnaLocalization { CountryName = "US", Currency = "USD", Locale = "en-us", FullCountryName = "United States" });
			retVal.Add(new KlarnaLocalization { CountryName = "GB", Currency = "GBP", Locale = "en-gb", FullCountryName = "United Kingdom" });

			return retVal;
		}

		private string GetCheckoutBaseUrl(string currency)
		{
			switch (currency)
			{
				case "GBP":
					return Mode.ToLower() == "test" ? _unitedKingdomTestBaseUrl : _unitedKingdomBaseUrl;
				default:
					return Mode.ToLower() == "test" ? _americaTestBaseUrl : _americaLiveBaseUrl;
			}
		}

		private string GetApiBaseUrl(string currency)
		{
			switch (currency)
			{
				case "GBP":
					return Mode.ToLower() == "test" ? _unitedKingdomApiTestBaseUrl : _unitedKingdomApiBaseUrl;
				default:
					return Mode.ToLower() == "test" ? _americaApiTestBaseUrl : _americaLiveApiBaseUrl;
			}
		}


		private KlarnaLocalization GetLocalization(string currency, string country)
		{
			var localizations = GetLocalizations();
			if (!string.IsNullOrEmpty(country))
			{
				return localizations.FirstOrDefault(l => l.Currency == currency && l.FullCountryName == country);
			}
			else
			{
				return localizations.FirstOrDefault(l => l.Currency == currency);
			}
		}

		public override VoidProcessPaymentResult VoidProcessPayment(VoidProcessPaymentEvaluationContext context)
		{
			throw new NotImplementedException();
		}

		public override CaptureProcessPaymentResult CaptureProcessPayment(CaptureProcessPaymentEvaluationContext context)
		{
			throw new NotImplementedException();
		}

		public override RefundProcessPaymentResult RefundProcessPayment(RefundProcessPaymentEvaluationContext context)
		{
			throw new NotImplementedException();
		}
	}
}