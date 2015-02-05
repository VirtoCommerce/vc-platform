using Klarna.Api;
using Klarna.Checkout;
using Klarna.PaymentGatewaysModule.Web.Exceptions;
using Klarna.PaymentGatewaysModule.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Framework.Web.Settings;

namespace Klarna.PaymentGatewaysModule.Web.Controllers
{
	[RoutePrefix("api/klarnagateway")]
	public class KlarnaGatewayController : ApiController
	{
		private const string ContentType = "application/vnd.klarna.checkout.aggregated-order-v2+json";
		private int _klarnaEid;
		private string _klarnaSecret;

		private IPaymentGateway _paymentGateway;

		public KlarnaGatewayController(IPaymentGateway paymentGateway, int klarnaEid, string klarnaSecret)
		{

			if (paymentGateway == null)
				throw new ArgumentNullException("paymentGateway");

			if (klarnaEid == 0)
				throw new KlarnaCredentialsException("Eid is wrong");

			if (string.IsNullOrEmpty(klarnaSecret))
				throw new KlarnaCredentialsException("KlarnaSecret is wrong");

			_paymentGateway = paymentGateway;

			_klarnaEid = klarnaEid;
			_klarnaSecret = klarnaSecret;
		}

		[HttpGet]
		[Route("pusher")]
		public IHttpActionResult Push(string klarna, string sid)
		{
			return Ok();
		}

		[HttpGet]
		[Route("push")]
		public IHttpActionResult PushKlarnaResult(string klarna, string sid)
		{
			//Gets klarna order
			Uri resourceUri = new Uri(klarna);
			var SharedSecret = _klarnaSecret;
			var connector = Connector.Create(SharedSecret);
			Order order = new Order(connector, resourceUri)
			{
				ContentType = ContentType
			};

			order.Fetch();

			var status = (order.GetValue("status") ?? string.Empty).ToString();

			switch (status)
			{
				case "checkout_complete":
					//Update order status
					var data = new Dictionary<string, object> { { "status", "created" } };
					order.Update(data);
					//Activate order
					//ActivateKlarnaPayment(order);
					break;

				case "created":
					//Activate order
					//ActivateKlarnaPayment(order);
					break;

				default:
					break;
			}

			return Ok();

		}

		[HttpPost]
		[ResponseType(typeof(CreatePaymentResult))]
		[Route("create")]
		public IHttpActionResult CreateKlarnaPayment(KlarnaPaymentInfo model)
		{
			CreatePaymentResult retVal = new CreatePaymentResult(); ;

			try
			{
				//Create lineItems
				var cartItems = new List<Dictionary<string, object>>();
				foreach (var lineItem in model.LineItems)
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
				var connector = Connector.Create(_klarnaSecret);
				Order order = null;
				var merchant = new Dictionary<string, object>
				{
					{ "id", _klarnaEid.ToString() },
					{ "terms_uri", model.TermsUrl },
					{ "checkout_uri", model.CheckoutUrl },
					{ "confirmation_uri", model.ConfirmationUrl },
					{ "push_uri", model.PushUrl }
				};
				data.Add("purchase_country", model.PurchaseCountry);
				data.Add("purchase_currency", model.PurchaseCurrency);
				data.Add("locale", model.Locale);
				data.Add("merchant", merchant);
				order =
					new Order(connector)
					{
						BaseUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders"),
						ContentType = ContentType
					};
				order.Create(data);
				order.Fetch();

				//Gets snippet
				var gui = order.GetValue("gui") as JObject;
				var html = gui["snippet"].Value<string>();

				retVal.Html = html;
			}
			catch (Exception)
			{
				retVal.IsSuccess = false;
			}

			return Ok(retVal);
		}

		private void ActivateKlarnaPayment(Order order)
		{
			Api.Api api;

			var configuration = new Configuration(
				Country.FromIsoCode(order.GetValue("purchase_country") as string),
				Language.FromIsoCode(order.GetValue("locale") as string),
				Currency.FromIsoCode(order.GetValue("purchase_currency") as string),
				Encoding.CustomerNumber)
			{
				Eid = _klarnaEid,
				Secret = _klarnaSecret,
				IsLiveMode = true
			};

			api = new Api.Api(configuration) { PClassStorageUri = @"pclasses.xml" };

			var activateResponse = api.Activate(order.GetValue("id") as string);
			if (activateResponse == null || activateResponse.RiskStatus != RiskStatus.Ok)
				throw new KlarnaActivateException("activateResponse is wrong");
		}
	}
}
