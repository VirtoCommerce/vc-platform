using MeS.PaymentGatewaysModule.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Framework.Web.Settings;

namespace MeS.PaymentGatewaysModule.Web.Controllers
{
	[RoutePrefix("api/paymentgateway")]
	public class MeSGatewayController : ApiController
	{
		private string _mesAccountId;

		private IPaymentGateway _paymentGateway;

		public MeSGatewayController(IPaymentGateway paymentGateway, string mesAccountId)
		{
			if (paymentGateway == null)
				throw new ArgumentNullException("paymentGateway");

			if (string.IsNullOrEmpty(mesAccountId))
				throw new ArgumentNullException("mesAccountId");

			_paymentGateway = paymentGateway;

			_mesAccountId = mesAccountId;
		}

		[HttpPost]
		[Route("mes/push")]
		public IHttpActionResult PushMes([FromBody] MeSPushRequestModel mesPushRequest)
		{
			return Ok();
		}

		[HttpGet]
		[ResponseType(typeof(GetPaymentParametersResult))]
		[Route("mes/create")]
		public IHttpActionResult CreatePayment()
		{
			var retVal = new GetPaymentParametersResult();

			retVal.MeSAccountId = _mesAccountId;
			retVal.InvoiceNumber = Guid.NewGuid().ToString();

			return Ok(retVal);
		}
	}
}
