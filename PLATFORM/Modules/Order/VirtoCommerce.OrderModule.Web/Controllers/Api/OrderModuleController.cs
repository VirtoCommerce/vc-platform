using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.OrderModule.Web.Converters;
using VirtoCommerce.Domain.Cart.Services;
using System.Web.Http.ModelBinding;
using VirtoCommerce.OrderModule.Web.Binders;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Payment.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Caching;
using Hangfire;
using VirtoCommerce.OrderModule.Web.BackgroundJobs;

namespace VirtoCommerce.OrderModule.Web.Controllers.Api
{
    [RoutePrefix("api/order/customerOrders")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
    public class OrderModuleController : ApiController
    {
        private readonly ICustomerOrderService _customerOrderService;
        private readonly ICustomerOrderSearchService _searchService;
        private readonly IOperationNumberGenerator _operationNumberGenerator;
        private readonly IStoreService _storeService;
		private readonly CacheManager _cacheManager;

        public OrderModuleController(ICustomerOrderService customerOrderService, ICustomerOrderSearchService searchService, IStoreService storeService, IOperationNumberGenerator numberGenerator, CacheManager cacheManager)
        {
            _customerOrderService = customerOrderService;
            _searchService = searchService;
            _operationNumberGenerator = numberGenerator;
            _storeService = storeService;
			_cacheManager = cacheManager;
        }

        // GET: api/order/customerOrders?q=ddd&site=site1&customer=user1&start=0&count=20
        [HttpGet]
        [ResponseType(typeof(webModel.SearchResult))]
        [Route("")]
        public IHttpActionResult Search([ModelBinder(typeof(SearchCriteriaBinder))] coreModel.SearchCriteria criteria)
        {
            var retVal = _searchService.Search(criteria);
            return Ok(retVal.ToWebModel());
        }

        // GET: api/order/customerOrders/{id}
        [HttpGet]
        [ResponseType(typeof(webModel.CustomerOrder))]
        [Route("{id}")]
        public IHttpActionResult GetById(string id)
        {
            var retVal = _customerOrderService.GetById(id, coreModel.CustomerOrderResponseGroup.Full);
            if (retVal == null)
            {
                return NotFound();
            }
            return Ok(retVal.ToWebModel());
        }

        // POST: api/order/customerOrders/{cartId}
        [HttpPost]
        [ResponseType(typeof(webModel.CustomerOrder))]
        [Route("{cartId}")]
        public IHttpActionResult CreateOrderFromCart(string cartId)
        {
            var retVal = _customerOrderService.CreateByShoppingCart(cartId);
            return Ok(retVal.ToWebModel());
        }

        // GET: api/order/customerOrders/{orderId}/processPayment/{paymentId}
        [HttpGet]
        [ResponseType(typeof(webModel.CustomerOrder))]
        [Route("{orderId}/processPayment/{paymentId}")]
        public IHttpActionResult ProcessOrderPayments(string orderId, string paymentId)
        {
            var order = _customerOrderService.GetById(orderId, coreModel.CustomerOrderResponseGroup.Full);
            if (order == null)
            {
                throw new NullReferenceException("order");
            }
            var payment = order.InPayments.FirstOrDefault(x => x.Id == paymentId);
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

            var context = new ProcessPaymentEvaluationContext
            {
                Order = order,
                Payment = payment,
                Store = store
            };

            var result = paymentMethod.ProcessPayment(context);
            if (result.NewPaymentStatus == PaymentStatus.Pending)
            {
                payment.OuterId = result.OuterId;
                _customerOrderService.Update(new coreModel.CustomerOrder[] { order });
            }
            var retVal = new webModel.ProcessPaymentResult();
            retVal.InjectFrom(result);
            retVal.PaymentMethodType = paymentMethod.PaymentMethodType;

            return Ok(retVal);
        }

        // POST: api/order/customerOrders
        [HttpPost]
        [ResponseType(typeof(webModel.CustomerOrder))]
        [Route("")]
        public IHttpActionResult CreateOrder(webModel.CustomerOrder customerOrder)
        {
            var retVal = _customerOrderService.Create(customerOrder.ToCoreModel());
            return Ok(retVal.ToWebModel());
        }

        // PUT: api/order/customerOrders
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult Update(webModel.CustomerOrder order)
        {
            var coreOrder = order.ToCoreModel();
            _customerOrderService.Update(new coreModel.CustomerOrder[] { coreOrder });
            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET:  api/order/customerOrders/{id}/shipments/new
        [HttpGet]
        [ResponseType(typeof(webModel.Shipment))]
        [Route("{id}/shipments/new")]
        public IHttpActionResult GetNewShipment(string id)
        {
            coreModel.Shipment retVal = null;
            var order = _customerOrderService.GetById(id, coreModel.CustomerOrderResponseGroup.Full);
            if (order != null)
            {
                retVal = new coreModel.Shipment
                {
                    Currency = order.Currency
                };
                retVal.Number = _operationNumberGenerator.GenerateNumber(retVal);

                //distribute not shipped items
                var shippedItems = order.Shipments.SelectMany(x => x.Items).ToArray();
                retVal.Items = order.Items.Where(x => !shippedItems.Any(y => y.Id == x.Id)).ToList();
                return Ok(retVal.ToWebModel());
            }

            return NotFound();
        }

        // GET:  api/order/customerOrders/{id}/payments/new
        [HttpGet]
        [ResponseType(typeof(webModel.Shipment))]
        [Route("{id}/payments/new")]
        public IHttpActionResult GetNewPayment(string id)
        {
            coreModel.PaymentIn retVal = null;
            var order = _customerOrderService.GetById(id, coreModel.CustomerOrderResponseGroup.Full);
            if (order != null)
            {
                retVal = new coreModel.PaymentIn
                {
                    Id = Guid.NewGuid().ToString(),
                    Currency = order.Currency,
                    CustomerId = order.CustomerId
                };
                retVal.Number = _operationNumberGenerator.GenerateNumber(retVal);
                return Ok(retVal.ToWebModel());
            }

            return NotFound();
        }



        // DELETE: /api/order/customerOrders?ids=21
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult Delete([FromUri] string[] ids)
        {
            _customerOrderService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: /api/order/customerOrders/id/operations/id
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("~/api/order/customerOrders/{id}/operations/{operationId}")]
        public IHttpActionResult Delete(string id, string operationId)
        {
            var order = _customerOrderService.GetById(id, coreModel.CustomerOrderResponseGroup.Full);
            if (order != null)
            {
                var operation = ((coreModel.Operation)order).Traverse(x => x.ChildrenOperations).FirstOrDefault(x => x.Id == operationId);
                if (operation != null)
                {
                    var shipment = operation as coreModel.Shipment;
                    var payment = operation as coreModel.PaymentIn;
                    if (shipment != null)
                    {
                        order.Shipments.Remove(shipment);
                    }
                    else if (payment != null)
                    {
                        //If payment not belong to order need remove payment in shipment
                        if (!order.InPayments.Remove(payment))
                        {
                            var paymentContainsShipment = order.Shipments.FirstOrDefault(x => x.InPayments.Contains(payment));
                            paymentContainsShipment.InPayments.Remove(payment);
                        }
                    }
                }
                _customerOrderService.Update(new coreModel.CustomerOrder[] { order });
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET:  api/order/dashboardStatistics
        [HttpGet]
        [ResponseType(typeof(webModel.DashboardStatisticsResult))]
        [Route("~/api/order/dashboardStatistics")]
		public IHttpActionResult GetDashboardStatistics([FromUri]DateTime? start = null, [FromUri]DateTime? end = null)
        {
			start = start ?? DateTime.UtcNow.AddYears(-1);
			end = end ?? DateTime.UtcNow;
			var cacheKey = CacheKey.Create("Statistic", start.Value.ToString("yyyy-MM-dd"), end.Value.ToString("yyyy-MM-dd"));
			var retVal = _cacheManager.Get<webModel.DashboardStatisticsResult>(cacheKey);
			if(retVal == null)
			{
				var collectStaticJob = new CollectOrderStatisticJob();
				BackgroundJob.Enqueue(() => collectStaticJob.CollectStatistics(start.Value, end.Value));
			}
	        return Ok(retVal);
        }

    }
}
