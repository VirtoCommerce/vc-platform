using Klarna.Api;
using Klarna.Checkout;
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

namespace Klarna.PaymentGatewaysModule.Web.Controllers
{
	[RoutePrefix("api/paymentgateway")]
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
				throw new ArgumentNullException("Eid");

			if (string.IsNullOrEmpty(klarnaSecret))
				throw new ArgumentNullException("KlarnaSecret");

			_paymentGateway = paymentGateway;

			_klarnaEid = klarnaEid;
			_klarnaSecret = klarnaSecret;
		}

		[HttpGet]
		[Route("klarna/push")]
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
					ActivateKlarnaPayment(order);
					break;

				case "created":
					//Activate order
					ActivateKlarnaPayment(order);
					break;

				default:
					break;
			}

			return Ok();

		}

		[HttpPost]
		[ResponseType(typeof(CreatePaymentResult))]
		[Route("klarna/create")]
		public IHttpActionResult CreateKlarnaPayment(KlarnaPaymentInfo model)
		{
			CreatePaymentResult retVal = new CreatePaymentResult();
			var paymentInfo = _paymentGateway.CreatePayment(model);

			var klarnaPaymentInfo = paymentInfo as KlarnaPaymentInfo;

			if (klarnaPaymentInfo != null)
			{
				retVal.Html = klarnaPaymentInfo.Html;
				retVal.IsSuccess = klarnaPaymentInfo.Success;
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
				throw new NullReferenceException("activateResponse is wrong");
		}
	}
}
