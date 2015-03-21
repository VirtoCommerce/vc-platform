using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CoreModule.Web.Model;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Framework.Web.Notification;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.CoreModule.Web.Converters;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
	[RoutePrefix("api/paymentgateways")]
	public class PaymentGatewayController : ApiController
	{
		private readonly IPaymentGatewayManager _paymentGatewayManager;
		public PaymentGatewayController(IPaymentGatewayManager paymentGatewayManager)
		{
			_paymentGatewayManager = paymentGatewayManager;
		}

		/// <summary>
		/// api/paymentgateways
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ResponseType(typeof(PaymentGateway[]))]
		[Route("")]
		public IHttpActionResult GetGateways()
		{
			var retVal = _paymentGatewayManager.PaymentGateways.Select(x => x.ToWebModel()).ToArray();
			return Ok(retVal);
		}
	}
}
