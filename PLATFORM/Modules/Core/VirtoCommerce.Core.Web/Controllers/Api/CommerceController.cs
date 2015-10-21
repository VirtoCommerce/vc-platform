using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CoreModule.Web.Converters;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using webModel = VirtoCommerce.CoreModule.Web.Model;

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
        /// Return all fulfillment centers registered in the system
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(webModel.FulfillmentCenter[]))]
        [Route("fulfillment/centers")]
        public IHttpActionResult GetFulfillmentCenters()
        {
            var retVal = _commerceService.GetAllFulfillmentCenters().Select(x => x.ToWebModel()).ToArray();
            return Ok(retVal);
        }

        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <param name="id">fulfillment center id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.FulfillmentCenter))]
        [Route("fulfillment/centers/{id}")]
        [CheckPermission(Permission = CommercePredefinedPermissions.Read)]
        public IHttpActionResult GetFulfillmentCenter(string id)
        {
            var retVal = _commerceService.GetAllFulfillmentCenters().First(x => x.Id == id);
            return Ok(retVal.ToWebModel());
        }

        /// <summary>
        ///  Update a existing fulfillment center 
        /// </summary>
        /// <param name="center">fulfillment center</param>
        [HttpPut]
        [ResponseType(typeof(webModel.FulfillmentCenter))]
        [Route("fulfillment/centers")]
        [CheckPermission(Permissions = new[] { CommercePredefinedPermissions.Create, CommercePredefinedPermissions.Update })]
        public IHttpActionResult UpdateFulfillmentCenter(webModel.FulfillmentCenter center)
        {
            var retVal = _commerceService.UpsertFulfillmentCenter(center.ToCoreModel());
            return Ok(retVal);
        }

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <param name="orderId">customer order id</param>
        [HttpGet]
        [Route("paymentcallback")]
        [ResponseType(typeof(PostProcessPaymentResult))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult PostProcessPayment(string orderId)
        {
            var order = _customerOrderService.GetById(orderId, CustomerOrderResponseGroup.Full);
            if (order == null)
            {
                throw new NullReferenceException("order");
            }

            var store = _storeService.GetById(order.StoreId);
            var parameters = HttpContext.Current.Request.QueryString;
            var paymentMethod = store.PaymentMethods.Where(x => x.IsActive).FirstOrDefault(x => x.ValidatePostProcessRequest(parameters).IsSuccess);
            if (paymentMethod != null)
            {
                var paymentOuterId = paymentMethod.ValidatePostProcessRequest(HttpContext.Current.Request.QueryString).OuterId;

                var payment = order.InPayments.FirstOrDefault(x => x.OuterId == paymentOuterId);
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

                if (retVal != null)
                {
                    _customerOrderService.Update(new CustomerOrder[] { order });
                }

                return Ok(retVal);
            }

            return Ok(new PostProcessPaymentResult { ErrorMessage = "cancel payment" });
        }

        /// <summary>
        /// Find seo informations by slug keyword
        /// </summary>
        /// <param name="slug">fulfillment center id</param>
        [HttpGet]
        [ResponseType(typeof(coreModel.SeoInfo[]))]
        [Route("seoinfos/{slug}")]
        [CheckPermission(Permission = CommercePredefinedPermissions.Read)]
        public IHttpActionResult GetSeoInfoBySlug(string slug)
        {
            var retVal = _commerceService.GetSeoByKeyword(slug).ToArray();
            return Ok(retVal);
        }
    }
}
