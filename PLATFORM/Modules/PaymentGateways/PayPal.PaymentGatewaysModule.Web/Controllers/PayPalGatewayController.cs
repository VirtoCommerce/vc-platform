using PayPal.PaymentGatewaysModule.Web.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Store.Services;
using coreModels = VirtoCommerce.Domain.Payment.Model;

namespace PayPal.PaymentGatewaysModule.Web.Controllers
{
	[RoutePrefix("api/paymentgateway/paypal")]
	public class PayPalGatewayController : ApiController
	{
		private PaypalPaymentMethod _paymentMethod;
		private IStoreService _storeService;
		private ICustomerOrderService _customerOrderService;

		public PayPalGatewayController(PaypalPaymentMethod paymentMethod,
			IStoreService storeService,
			ICustomerOrderService customerOrderService)
		{
			if (paymentMethod == null)
				throw new ArgumentNullException("paymentGateway");

			if (storeService == null)
				throw new ArgumentNullException("storeService");

			if (customerOrderService == null)
				throw new ArgumentNullException("customerOrderService");

			_paymentMethod = paymentMethod;
			_storeService = storeService;
			_customerOrderService = customerOrderService;
		}

		[HttpGet]
		[Route("push")]
		public IHttpActionResult ApprovePayment(string token, string orderId, bool? cancel, string redirectUrl)
		{
			if (cancel.HasValue && !cancel.Value)
			{
				var paymentEvaluationContext = new PaymentEvaluationContext();
				paymentEvaluationContext.OrderId = orderId;
				paymentEvaluationContext.OuterId = token;

				var paymentResult = _paymentMethod.PostProcessPayment(paymentEvaluationContext);

				return Redirect(paymentResult.ReturnUrl);
			}

			return Redirect(string.Format("{0}/checkout/thanks?orderId={1}&isSuccess=false", redirectUrl, orderId));
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
