using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.CoreModule.Web.Converters;
using webModel = VirtoCommerce.CoreModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.CoreModule.Web.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
	[RoutePrefix("api")]
	public class CommerceController : ApiController
	{
		private readonly ICommerceService _commerceService;
		private readonly ICustomerOrderService _customerOrderService;
		private readonly IStoreService _storeService;
		public CommerceController(ICommerceService commerceService, ICustomerOrderService customerOrderService, IStoreService storeService)
		{
			_commerceService = commerceService;
			_customerOrderService = customerOrderService;
			_storeService = storeService;
		}

		/// <summary>
		/// GET: api/fulfillment/centers
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ResponseType(typeof(webModel.FulfillmentCenter[]))]
		[Route("fulfillment/centers")]
		public IHttpActionResult GetFulfillmentCenters()
		{
			var retVal = _commerceService.GetAllFulfillmentCenters().Select(x => x.ToWebModel()).ToArray();
			return Ok(retVal);
		}

		// GET: api/fulfillment/centers/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.FulfillmentCenter))]
		[Route("fulfillment/centers/{id}")]
		public IHttpActionResult GetFulfillmentCenter(string id)
		{
			var retVal = _commerceService.GetAllFulfillmentCenters().First(x => x.Id == id);
			return Ok(retVal.ToWebModel());
		}

		// PUT: api/fulfillment/centers
		[HttpPut]
		[ResponseType(typeof(webModel.FulfillmentCenter))]
		[Route("fulfillment/centers")]
		public IHttpActionResult UpdateFulfillmentCenter(webModel.FulfillmentCenter center)
		{
			var retVal = _commerceService.UpsertFulfillmentCenter(center.ToCoreModel());
			return Ok(retVal);
		}

		[HttpGet]
		[Route("paymentcallback")]
		public IHttpActionResult PostProcessPayment(string orderId, string token, bool? cancel)
		{
			var isContinue = !cancel.HasValue;
			if (!isContinue)
			{
				isContinue = (cancel.HasValue && !cancel.Value);
			}

			if (isContinue)
			{
				var order = _customerOrderService.GetById(orderId, CustomerOrderResponseGroup.Full);
				if (order == null)
				{
					throw new NullReferenceException("order");
				}
				var payment = order.InPayments.FirstOrDefault(x => x.OuterId == token);
				if (payment == null)
				{
					throw new NullReferenceException("payment");
				}
				var store = _storeService.GetById(order.StoreId);
				var paymentMethod = store.PaymentMethods.FirstOrDefault(x => x.Code == payment.GatewayCode);
				if (payment == null)
				{
					throw new NullReferenceException("appropriate paymentMethod not found");
				}

				var context = new PostProcessPaymentEvaluationContext
				{
					Order = order,
					Payment = payment,
					Store = store,
					OuterId = token
				};

				var result = paymentMethod.PostProcessPayment(context);

				if(result.NewPaymentStatus == PaymentStatus.Paid)
				{
					payment.IsApproved = true;
					_customerOrderService.Update(new CustomerOrder[] { order });
				}

				return Ok(result);
			}

			return Ok(new PostProcessPaymentResult { Error = "cancel payment" });
		}


	}
}
