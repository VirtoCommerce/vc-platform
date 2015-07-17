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

namespace Klarna.Checkout.Euro.Managers
{
	public class KlarnaCheckoutEuroPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
	{
		private const string _euroTestBaseUrl = "https://checkout.testdrive.klarna.com/checkout/orders";
		private const string _euroLiveBaseUrl = "https://checkout.klarna.com/checkout/orders";
		private const string _contentType = "application/vnd.klarna.checkout.aggregated-order-v2+json";

		private const string _klarnaModeStoreSetting = "Klarna.Checkout.Euro.Mode";
		private const string _klarnaAppKeyStoreSetting = "Klarna.Checkout.Euro.AppKey";
		private const string _klarnaAppSecretStoreSetting = "Klarna.Checkout.Euro.SecretKey";
		private const string _klarnaTermsUrl = "Klarna.Checkout.Euro.TermsUrl";
		private const string _klarnaCheckoutUrl = "Klarna.Checkout.Euro.CheckoutUrl";
		private const string _klarnaConfirmationUrl = "Klarna.Checkout.Euro.ConfirmationUrl";
		private const string _klarnaPaymentActionType = "Klarna.Checkout.Euro.PaymentActionType";

		private const string _klarnaSalePaymentActionType = "Sale";

		public KlarnaCheckoutEuroPaymentMethod()
			: base("KlarnaCheckoutEuro")
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

		private string PaymentActionType
		{
			get
			{
				var retVal = GetSetting(_klarnaPaymentActionType);
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
					retVal = ProcessKlarnaOrder(localization, context);
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

			retVal = PostProcessKlarnaOrder(context);

			return retVal;
		}

		public override CaptureProcessPaymentResult CaptureProcessPayment(CaptureProcessPaymentEvaluationContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (context.Payment == null)
				throw new ArgumentNullException("context.Payment");

			var retVal = new CaptureProcessPaymentResult();

			Uri resourceUri = new Uri(string.Format("{0}/{1}", _euroTestBaseUrl, context.Payment.OuterId));
			var connector = Connector.Create(AppSecret);
			Order order = new Order(connector, resourceUri)
			{
				ContentType = _contentType
			};
			order.Fetch();

			var reservation = order.GetValue("reservation") as string;
			if (!string.IsNullOrEmpty(reservation))
			{
				try
				{
					Configuration configuration = new Configuration(Country.Code.SE, Language.Code.SV, Currency.Code.SEK, Encoding.Sweden)
					{
						Eid = Convert.ToInt32(AppKey),
						Secret = AppSecret,
						IsLiveMode = false
					};
					Api.Api api = new Api.Api(configuration);
					var response = api.Activate(reservation);

					retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Paid;
					context.Payment.CapturedDate = DateTime.UtcNow;
					context.Payment.IsApproved = true;
					retVal.IsSuccess = true;
					retVal.OuterId = context.Payment.OuterId = response.InvoiceNumber;
				}
				catch(Exception ex)
				{
					retVal.ErrorMessage = ex.Message;
				}
			}
			else
			{
				retVal.ErrorMessage = "No reservation for this order";
			}

			return retVal;
		}

		public override VoidProcessPaymentResult VoidProcessPayment(VoidProcessPaymentEvaluationContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (context.Payment == null)
				throw new ArgumentNullException("context.Payment");

			var retVal = new VoidProcessPaymentResult();

			if (!context.Payment.IsApproved && (context.Payment.PaymentStatus == PaymentStatus.Authorized || context.Payment.PaymentStatus == PaymentStatus.Cancelled))
			{
				Uri resourceUri = new Uri(string.Format("{0}/{1}", _euroTestBaseUrl, context.Payment.OuterId));
				var connector = Connector.Create(AppSecret);
				Order order = new Order(connector, resourceUri)
				{
					ContentType = _contentType
				};
				order.Fetch();

				var reservation = order.GetValue("reservation") as string;
				if (!string.IsNullOrEmpty(reservation))
				{
					try
					{
						Configuration configuration = new Configuration(Country.Code.SE, Language.Code.SV, Currency.Code.SEK, Encoding.Sweden)
						{
							Eid = Convert.ToInt32(AppKey),
							Secret = AppSecret,
							IsLiveMode = false
						};
						Api.Api api = new Api.Api(configuration);
						var result = api.CancelReservation(reservation);
						if (result)
						{
							retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Voided;
							context.Payment.VoidedDate = context.Payment.CancelledDate = DateTime.UtcNow;
							context.Payment.IsCancelled = true;
							retVal.IsSuccess = true;
						}
						else
						{
							retVal.ErrorMessage = "Payment was not canceled, try later";
						}
					}
					catch(Exception ex)
					{
						retVal.ErrorMessage = ex.Message;
					}
				}
			}
			else if(context.Payment.IsApproved)
			{
				retVal.ErrorMessage = "Payment already approved, use refund";
				retVal.NewPaymentStatus = PaymentStatus.Paid;
			}
			else if(context.Payment.IsCancelled)
			{
				retVal.ErrorMessage = "Payment already canceled";
				retVal.NewPaymentStatus = PaymentStatus.Voided;
			}

			return retVal;
		}

		public override RefundProcessPaymentResult RefundProcessPayment(RefundProcessPaymentEvaluationContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (context.Payment == null)
				throw new ArgumentNullException("context.Payment");

			var retVal = new RefundProcessPaymentResult();

			if (context.Payment.IsApproved && (context.Payment.PaymentStatus == PaymentStatus.Paid || context.Payment.PaymentStatus == PaymentStatus.Cancelled))
			{
				Configuration configuration = new Configuration(Country.Code.SE, Language.Code.SV, Currency.Code.SEK, Encoding.Sweden)
				{
					Eid = Convert.ToInt32(AppKey),
					Secret = AppSecret,
					IsLiveMode = false
				};
				Api.Api api = new Api.Api(configuration);

				var result = api.CreditInvoice(context.Payment.OuterId);
			}

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

		#region Private Methods

		private ProcessPaymentResult ProcessKlarnaOrder(KlarnaLocalization localization, ProcessPaymentEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

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
					BaseUri = new Uri(GetCheckoutBaseUrl(localization.Currency)),
					ContentType = _contentType
				};
			order.Create(data);
			order.Fetch();

			//Gets snippet
			var gui = order.GetValue("gui") as JObject;
			var html = gui["snippet"].Value<string>();

			retVal.IsSuccess = true;
			retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Pending;
			retVal.HtmlForm = html;
			retVal.OuterId = context.Payment.OuterId = order.GetValue("id") as string;

			return retVal;
		}

		private PostProcessPaymentResult PostProcessKlarnaOrder(PostProcessPaymentEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			Uri resourceUri = new Uri(string.Format("{0}/{1}", _euroTestBaseUrl, context.OuterId));

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

			if (status == "created" && IsSale())
			{
				var result = CaptureProcessPayment(new CaptureProcessPaymentEvaluationContext { Payment = context.Payment });

				retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Paid;
				context.Payment.OuterId = retVal.OuterId;
				context.Payment.IsApproved = true;
				context.Payment.CapturedDate = DateTime.UtcNow;
				retVal.IsSuccess = true;
			}
			else if (status == "created")
			{
				retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Authorized;
				context.Payment.OuterId = retVal.OuterId = context.OuterId;
				context.Payment.AuthorizedDate = DateTime.UtcNow;
				retVal.IsSuccess = true;
			}
			else
			{
				retVal.ErrorMessage = "order not created";
			}

			retVal.OrderId = context.Order.Id;

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
					addedItem.Add("unit_price", (int)((lineItem.Price + lineItem.Tax / lineItem.Quantity) * 100));
					//addedItem.Add("total_price_excluding_tax", (int)(lineItem.Price * lineItem.Quantity * 100));
				}

				if (lineItem.Tax > 0)
				{
					//addedItem.Add("total_price_including_tax", (int)((lineItem.Price * lineItem.Quantity + lineItem.Tax) * 100));
					//addedItem.Add("total_tax_amount", (int)(lineItem.Tax * 100));
					addedItem.Add("tax_rate", (int)(lineItem.TaxDetails.Sum(td => td.Rate) * 10000));
				}
				else
				{
					addedItem.Add("tax_rate", 0);
				}

				addedItem.Add("discount_rate", 0);
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

		private bool IsSale()
		{
			return PaymentActionType.Equals(_klarnaSalePaymentActionType);
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
			return Mode.ToLower() == "test" ? _euroTestBaseUrl : _euroLiveBaseUrl;
		}

		private string GetApiBaseUrl(string currency)
		{
			return Mode.ToLower() == "test" ? _euroTestBaseUrl : _euroLiveBaseUrl;
		}


		private KlarnaLocalization GetLocalization(string currency, string country)
		{
			return new KlarnaLocalization { CountryName = "SE", Currency = "SEK", Locale = "sv-se", FullCountryName = "Sweden" };

			//var localizations = GetLocalizations();
			//if (!string.IsNullOrEmpty(country))
			//{
			//	return localizations.FirstOrDefault(l => l.Currency == currency && l.FullCountryName == country);
			//}
			//else
			//{
			//	return localizations.FirstOrDefault(l => l.Currency == currency);
			//}
		}

		#endregion
	}
}