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

namespace Authorize.Net.Controllers
{
    [ApiExplorerSettings(IgnoreApi=true)]
    [RoutePrefix("api/payments/an")]
    public class AuthorizeNetController : ApiController
    {
        private readonly ICustomerOrderService _customerOrderService;
        private readonly IStoreService _storeService;

        public AuthorizeNetController(ICustomerOrderService customerOrderService, IStoreService storeService)
        {
            _customerOrderService = customerOrderService;
            _storeService = storeService;
        }

        [HttpPost]
        [Route("registerpayment/{orderId}")]
        [AllowAnonymous]
        public IHttpActionResult RegisterPayment(string orderId)
        {
            var order = _customerOrderService.GetById(orderId, CustomerOrderResponseGroup.Full);
            if (order == null)
            {
                throw new NullReferenceException("order");
            }

            var store = _storeService.GetById(order.StoreId);
            var parameters = new NameValueCollection();

            foreach(var key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                parameters.Add(key, HttpContext.Current.Request.Form[key]);
            }

            foreach(var key in HttpContext.Current.Request.Form.AllKeys)
            {
                parameters.Add(key, HttpContext.Current.Request.Form[key]);
            }
            
            var paymentMethod = store.PaymentMethods.FirstOrDefault(x => x.Code == "AuthorizeNet");
            if (paymentMethod != null)
            {
                var validateResult = paymentMethod.ValidatePostProcessRequest(parameters);
                var paymentOuterId = validateResult.OuterId;

                var payment = order.InPayments.FirstOrDefault(x => x.GatewayCode == "AuthorizeNet" && x.Sum == Convert.ToDecimal(parameters["x_amount"], CultureInfo.InvariantCulture));
                if (payment == null)
                {
                    throw new NullReferenceException("payment");
                }

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

                    var returnHtml = string.Format("<html><head><script type='text/javascript' charset='utf-8'>window.location='{0}';</script><noscript><meta http-equiv='refresh' content='1;url={0}'></noscript></head><body></body></html>", retVal.ReturnUrl);

                    return Ok(returnHtml);
                }
            }

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}