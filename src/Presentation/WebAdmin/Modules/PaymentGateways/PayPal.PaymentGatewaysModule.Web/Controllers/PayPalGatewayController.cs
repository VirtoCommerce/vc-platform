using PayPal.PaymentGatewaysModule.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;

namespace PayPal.PaymentGatewaysModule.Web.Controllers
{
	[RoutePrefix("api/paymentgateway")]
	public class PayPalGatewayController : ApiController
	{
		private string _appKey;
		private string _secret;

		private IPaymentGateway _paymentGateway;

		public PayPalGatewayController(IPaymentGateway paymentGateway, string appKey, string secret)
		{
			if (paymentGateway == null)
				throw new ArgumentNullException("paymentGateway");

			if (string.IsNullOrEmpty(appKey))
				throw new ArgumentNullException("appKey");

			if (string.IsNullOrEmpty(secret))
				throw new ArgumentNullException("secret");

			_paymentGateway = paymentGateway;

			_appKey = appKey;
			_secret = secret;
		}

		[HttpPost]
		[Route("push")]
		public IHttpActionResult PushMes([FromBody] PayPalPushRequestModel pushModel)
		{
			return Ok();
		}

		//[HttpGet]
		//[ResponseType(typeof(GetPaymentParametersResult))]
		//[Route("create")]
		//public IHttpActionResult CreatePayment()
		//{
		//	var retVal = new GetPaymentParametersResult();

		//	retVal.MeSAccountId = _mesAccountId;
		//	retVal.InvoiceNumber = Guid.NewGuid().ToString();

		//	return Ok(retVal);
		//}
	}
}
