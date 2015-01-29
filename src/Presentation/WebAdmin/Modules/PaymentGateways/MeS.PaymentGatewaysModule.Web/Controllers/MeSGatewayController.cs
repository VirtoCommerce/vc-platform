using MeS.PaymentGatewaysModule.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Framework.Web.Settings;

namespace MeS.PaymentGatewaysModule.Web.Controllers
{
	[RoutePrefix("api/mesgateway")]
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
		[Route("push")]
		public IHttpActionResult PushMesResult()
		{


			return Ok();
		}

		[HttpGet]
		[ResponseType(typeof(GetPaymentParametersResult))]
		[Route("getpaymentparameters")]
		public IHttpActionResult GetPaymentParameters()
		{
			var retVal = new GetPaymentParametersResult();

			retVal.MeSAccountId = _mesAccountId;

			return Ok(retVal);
		}
	}
}
