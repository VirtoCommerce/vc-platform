using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Store.Services;

namespace Dibs.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix("api/dibs")]
    public class DibsController : ApiController
    {
        private const string dibsCode = "DIBS";
        private readonly ICustomerOrderService _customerOrderService;
        private readonly IStoreService _storeService;

        public DibsController(ICustomerOrderService customerOrderService, IStoreService storeService)
        {
            _customerOrderService = customerOrderService;
            _storeService = storeService;
        }

        [HttpPost]
        [Route("callback")]
        [AllowAnonymous]
        public IHttpActionResult RegisterPayment()
        {
            var orderId = HttpContext.Current.Request.Form["orderid"];
            var order = _customerOrderService.GetByOrderNumber(orderId, CustomerOrderResponseGroup.Full);
            if (order == null)
                order = _customerOrderService.GetById(orderId, CustomerOrderResponseGroup.Full);
            if (order == null)
            {
                throw new NullReferenceException("Order not found");
            }

            var store = _storeService.GetById(order.StoreId);

            var parameters = new NameValueCollection();

            foreach (var key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                parameters.Add(key, HttpContext.Current.Request.Form[key]);
            }

            foreach (var key in HttpContext.Current.Request.Form.AllKeys)
            {
                parameters.Add(key, HttpContext.Current.Request.Form[key]);
            }

            var paymentMethod = store.PaymentMethods.FirstOrDefault(x => x.Code == dibsCode);
            if (paymentMethod != null)
            {
                var validateResult = paymentMethod.ValidatePostProcessRequest(parameters);
                var paymentOuterId = validateResult.OuterId;

                var payment = order.InPayments.FirstOrDefault(x => x.GatewayCode == dibsCode && (int)(x.Sum * 100) == Convert.ToInt32(parameters["amount"], CultureInfo.InvariantCulture));

                if (payment == null)
                {
                    throw new NullReferenceException("appropriate paymentMethod not found");
                }

                var context = new PostProcessPaymentEvaluationContext
                {
                    Order = order,
                    Payment = payment,
                    Store = store,
                    OuterId = paymentOuterId,
                    Parameters = parameters
                };

                var retVal = paymentMethod.PostProcessPayment(context);

                if (retVal != null && retVal.IsSuccess)
                {
                    _customerOrderService.Update(new CustomerOrder[] { order });
                    return Ok();
                }
            }

            return StatusCode(System.Net.HttpStatusCode.NotFound);
        }
    }
}