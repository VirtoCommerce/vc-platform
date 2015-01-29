using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Order.Services;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.OrderModule.Web.Converters;
using VirtoCommerce.Domain.Cart.Services;
using System.Web.Http.ModelBinding;
using VirtoCommerce.OrderModule.Web.Binders;

namespace VirtoCommerce.OrderModule.Web.Controllers.Api
{
	[RoutePrefix("api/order/customerOrders")]
    public class CustomerOrderController : ApiController
    {
		private readonly ICustomerOrderService _customerOrderService;
		private readonly ICustomerOrderSearchService _searchService;
	
		public CustomerOrderController(ICustomerOrderService customerOrderService, ICustomerOrderSearchService searchService)
		{
			_customerOrderService = customerOrderService;
			_searchService = searchService;
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
		public IHttpActionResult Update(webModel.CustomerOrder order)
		{
			var coreOrder = order.ToCoreModel();
			_customerOrderService.Update(new coreModel.CustomerOrder[] { coreOrder });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: /api/order/customerOrders?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("")]
		public IHttpActionResult Delete([FromUri] string[] ids)
		{
			_customerOrderService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
    }
}
