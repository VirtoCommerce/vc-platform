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

namespace VirtoCommerce.OrderModule.Web.Controllers.Api
{
	[RoutePrefix("api/order/customerOrders")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
    public class CustomerOrderController : ApiController
    {
		private readonly ICustomerOrderService _customerOrderService;
		private readonly ICustomerOrderSearchService _searchService;
		private readonly IOperationNumberGenerator _operationNumberGenerator;
	
		public CustomerOrderController(ICustomerOrderService customerOrderService, ICustomerOrderSearchService searchService, IOperationNumberGenerator numberGenerator)
		{
			_customerOrderService = customerOrderService;
			_searchService = searchService;
			_operationNumberGenerator = numberGenerator;
		}

		// GET: api/order/customerOrders?q=ddd&site=site1&customer=user1&start=0&count=20
		[HttpGet]
		[ResponseType(typeof(webModel.SearchResult))]
		[Route("")]
		public IHttpActionResult Search([ModelBinder(typeof(SearchCriteriaBinder))] webModel.SearchCriteria criteria)
		{
			var retVal = _searchService.Search(criteria.ToCoreModel());
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
			if(order != null)
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
				if(operation != null)
				{
					var shipment = operation as coreModel.Shipment;
					var payment = operation as coreModel.PaymentIn;
					if (shipment != null)
					{
						order.Shipments.Remove(shipment);
					}
					else if (payment != null)
					{
						//If payment not belong to order need remove paymnet in shipment
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


    }
}
