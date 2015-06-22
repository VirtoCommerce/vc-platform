using Klarna.Api;
using Klarna.Checkout;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;

namespace Klarna.PaymentGatewaysModule.Web.Managers
{
	public class KlarnaPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
	{
		private const string _euroTestBaseUrl = "https://checkout.testdrive.klarna.com/checkout/orders";
		private const string _americaTestBaseUrl = "https://checkout-na.playground.klarna.com/checkout/orders";
		private const string _americaApiTestBaseUrl = "https://api-na.playground.klarna.com/checkout/orders";
		private const string _unitedKingdomTestBaseUrl = "https://checkout.playground.klarna.com/checkout/orders";
		private const string _unitedKingdomApiTestBaseUrl = "https://api.playground.klarna.com/checkout/orders";
		private const string _euroLiveBaseUrl = "https://checkout.klarna.com/checkout/orders";
		private const string _americaLiveBaseUrl = "https://checkout-na.klarna.com/checkout/orders";
		private const string _americaLiveApiBaseUrl = "https://api-na.klarna.com/checkout/orders";
		private const string _unitedKingdomBaseUrl = "https://checkout.klarna.com/checkout/orders";
		private const string _unitedKingdomApiBaseUrl = "https://api.klarna.com/checkout/orders";
		private const string _contentType = "application/vnd.klarna.checkout.aggregated-order-v2+json";

		private const string _klarnaModeStoreSetting = "Klarna.Mode";
		private const string _klarnaAppKeyStoreSetting = "Klarna.AppKey";
		private const string _klarnaAppSecretStoreSetting = "Klarna.SecretKey";
		private const string _klarnaTermsUrl = "Klarna.TermsUrl";
		private const string _klarnaCheckoutUrl = "Klarna.CheckoutUrl";
		private const string _klarnaConfirmationUrl = "Klarna.ConfirmationUrl";

		public KlarnaPaymentMethod()
			: base("Klarna")
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
					var cartItems = CreateKlarnaCartItems(context.Order);

					//Create cart
					var cart = new Dictionary<string, object> { { "items", cartItems } };
					var data = new Dictionary<string, object>
					{
						{ "cart", cart }
					};

					//Create klarna order "http://example.com" context.Store.Url
					var connector = Connector.Create(AppSecret);
					Order order = null;
					var merchant = new Dictionary<string, object>
					{
						{ "id", AppKey },
						{ "terms_uri", string.Format("{0}/{1}", context.Store.Url, TermsUrl) },
						{ "checkout_uri", string.Format("{0}/{1}", context.Store.Url, CheckoutUrl) },
						{ "confirmation_uri", string.Format("{0}/{1}?sid=123&orderId={2}&", context.Store.Url, ConfirmationUrl, context.Order.Id) + "klarna_order={checkout.order.uri}" },
						{ "push_uri", string.Format("{0}/{1}?sid=123&orderId={2}&", context.Store.Url, "admin/api/paymentcallback", context.Order.Id) + "klarna_order={checkout.order.uri}" },
						{ "back_to_store_uri", context.Store.Url }
					};

					var layout = new Dictionary<string, object>
					{
						{ "layout", "desktop" }
					};

					data.Add("purchase_country", localization.CountryName);
					data.Add("purchase_currency", localization.Currency);
					data.Add("locale", localization.Locale);
					data.Add("merchant", merchant);
					data.Add("gui", layout);

					order =
						new Order(connector)
						{
							BaseUri = new Uri(GetCheckoutBaseUrl(currency)),
							ContentType = _contentType
						};
					order.Create(data);
					order.Fetch();

					//Gets snippet
					var gui = order.GetValue("gui") as JObject;
					var html = gui["snippet"].Value<string>();

					retVal.IsSuccess = true;
					retVal.NewPaymentStatus = PaymentStatus.Pending;
					retVal.HtmlForm = html;
					retVal.OuterId = order.GetValue("id") as string;
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

			Uri resourceUri = new Uri(string.Format("{0}/{1}", GetCheckoutBaseUrl(context.Payment.Currency.ToString()), context.OuterId));

			var connector = Connector.Create(AppSecret);

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
				order.Fetch();
				status = order.GetValue("status") as string;
			}

			if (status == "created")
			{
				var reservation = order.GetValue("reservation") as string;

				if (!string.IsNullOrEmpty(reservation))
				{
					Configuration configuration = new Configuration(Country.Code.SE, Language.Code.SV, Currency.Code.SEK, Encoding.Sweden)
					{
						Eid = Convert.ToInt32(AppKey),
						Secret = AppSecret,
						IsLiveMode = true
					};

					Api.Api api = new Api.Api(configuration);

					var response = api.Activate(reservation);

					order.Fetch();

					var klarnaCart = order.GetValue("cart") as JObject;
				}
			}

			retVal.IsSuccess = status == "created";
			retVal.NewPaymentStatus = retVal.IsSuccess ? PaymentStatus.Paid : PaymentStatus.Pending;
			retVal.OrderId = context.Order.Id;

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

		private List<Dictionary<string, object>> CreateKlarnaCartItems(CustomerOrder order)
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
				if (lineItem.Price > 0)
				{
					addedItem.Add("unit_price", (int)(lineItem.Price * 100));
				}

				addedItem.Add("discount_rate", 0);
				addedItem.Add("tax_rate", 0);
				addedItem.Add("reference", lineItem.ProductId);

				cartItems.Add(addedItem);
			}

			if (order.Shipments != null && order.Shipments.Any(s => s.Sum > 0))
			{
				foreach(var shipment in order.Shipments.Where(s => s.Sum > 0))
				{
					var addedItem = new Dictionary<string, object>();

					addedItem.Add("type", "shipping_fee");
					addedItem.Add("reference", "SHIPPING");
					addedItem.Add("name", "Shipping Fee");
					addedItem.Add("quantity", 1);
					addedItem.Add("unit_price", (int)(shipment.Sum * 100));

					addedItem.Add("tax_rate", 0);

					cartItems.Add(addedItem);
				}
			}

			return cartItems;
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

			retVal.Add(new KlarnaLocalization { CountryName = "SE", Currency = "SEK", Locale = "sv-se", FullCountryName = "Sweden" });
			retVal.Add(new KlarnaLocalization { CountryName = "FI", Currency = "EUR", Locale = "fi-fi", FullCountryName = "Finland" });
			retVal.Add(new KlarnaLocalization { CountryName = "NO", Currency = "NOK", Locale = "nb-no", FullCountryName = "Norway" });
			retVal.Add(new KlarnaLocalization { CountryName = "DE", Currency = "EUR", Locale = "de-de", FullCountryName = "Germany" });
			retVal.Add(new KlarnaLocalization { CountryName = "AT", Currency = "EUR", Locale = "de-at", FullCountryName = "Austria" });
			retVal.Add(new KlarnaLocalization { CountryName = "US", Currency = "USD", Locale = "en-us", FullCountryName = "United States" });
			retVal.Add(new KlarnaLocalization { CountryName = "GB", Currency = "GBP", Locale = "en-gb", FullCountryName = "United Kingdom" });

			return retVal;
		}

		private string GetCheckoutBaseUrl(string currency)
		{
			switch (currency)
			{
				case "USD":
					return Mode.ToLower() == "test" ? _americaTestBaseUrl : _americaLiveBaseUrl;
				case "GBP":
					return Mode.ToLower() == "test" ? _unitedKingdomTestBaseUrl : _unitedKingdomBaseUrl;
				default:
					return Mode.ToLower() == "test" ? _euroTestBaseUrl : _euroLiveBaseUrl;
			}
		}

		private string GetApiBaseUrl(string currency)
		{
			switch (currency)
			{
				case "USD":
					return Mode.ToLower() == "test" ? _americaApiTestBaseUrl : _americaLiveApiBaseUrl;
				case "GBP":
					return Mode.ToLower() == "test" ? _unitedKingdomApiTestBaseUrl : _unitedKingdomApiBaseUrl;
				default:
					return Mode.ToLower() == "test" ? _euroTestBaseUrl : _euroLiveBaseUrl;
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
	}
}