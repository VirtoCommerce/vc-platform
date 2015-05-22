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
		private PayPalPaymentGatewayImpl _paymentGateway;
		private IStoreService _storeService;
		private ICustomerOrderService _customerOrderService;

		public PayPalGatewayController(PayPalPaymentGatewayImpl paymentGateway,
			IStoreService storeService,
			ICustomerOrderService customerOrderService)
		{
			if (paymentGateway == null)
				throw new ArgumentNullException("paymentGateway");

			if (storeService == null)
				throw new ArgumentNullException("storeService");

			if (customerOrderService == null)
				throw new ArgumentNullException("customerOrderService");

			_paymentGateway = paymentGateway;
			_storeService = storeService;
			_customerOrderService = customerOrderService;
		}

		//[HttpGet]
		//[Route("create")]
		//[ResponseType(typeof(webModels.DirectRedirectUrlPaymentInfo))]
		//public IHttpActionResult CreatePayment(string orderId)
		//{
		//	var payment = new PaymentInfo
		//	{
		//		OrderId = orderId
		//	};

		//	var paymentInfo = _paymentGateway.CreatePayment(payment);

		//	return Ok((paymentInfo as coreModels.DirectRedirectUrlPaymentInfo).ToWebModel());
		//}

		[HttpGet]
		[Route("push")]
		public IHttpActionResult ApprovePayment(string token, string orderId, bool cancel)
		{
			if (!cancel)
			{
				var paymentInfo = _paymentGateway.GetPayment(token, orderId) as coreModels.DirectRedirectUrlPaymentInfo;

				return Redirect(paymentInfo.RedirectUrl);
			}
			return Redirect(string.Format("http://localhost/checkout/thanks?orderId={0}&isSuccess=false", orderId));

			//return Ok((paymentInfo as coreModels.DirectRedirectUrlPaymentInfo).ToWebModel());
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
