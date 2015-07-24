using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http.Description;
using Shipstation.FulfillmentModule.Web.Converters;
using Shipstation.FulfillmentModule.Web.Models.Notice;
using Shipstation.FulfillmentModule.Web.Models.Order;
using Shipstation.FulfillmentModule.Web.Services;
using System.Web.Http;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Platform.Data.Security.Authentication.Basic.Filters;

namespace Shipstation.FulfillmentModule.Web.Controllers
{
    [RoutePrefix("api/fulfillment/shipstation")]
    [ControllerConfig]
    public class ShipstationController : ApiController
    {
        private readonly ICustomerOrderService _orderService;
        private readonly ICustomerOrderSearchService _orderSearchService;
        
        public ShipstationController(ICustomerOrderService orderService, ICustomerOrderSearchService orderSearchService)
        {
            _orderSearchService = orderSearchService;
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(Orders))]
        [IdentityBasicAuthentication]
        public IHttpActionResult GetNewOrders(string action, string start_date, string end_date, int page)
        {
            if (action == "export")
            {
                var shipstationOrders = new Orders();
                
                var searchCriteria = new SearchCriteria
                {
                    StartDate = DateTime.Parse(start_date, new CultureInfo("en-US")),
                    EndDate = DateTime.Parse(end_date, new CultureInfo("en-US")),
                    ResponseGroup = ResponseGroup.Full
                };

                //if page more than 1 shipstation requests second or later page to be returned. move start position to that page.
                if (page > 1)
                {
                    searchCriteria.Start += searchCriteria.Count * (page-1);
                }

                var searchResult = _orderSearchService.Search(searchCriteria);
                
                if (searchResult.CustomerOrders != null && searchResult.CustomerOrders.Any())
                {
                    var shipstationOrdersList = new List<OrdersOrder>();
                    searchResult.CustomerOrders.ForEach(cu => shipstationOrdersList.Add(cu.ToShipstationOrder()));
                    shipstationOrders.Order = shipstationOrdersList.ToArray();

                    //if first page was requested and total orders more than returned add to response overall pages count that shipstation should request.
                    if ((page == 1) && searchResult.TotalCount > searchCriteria.Count)
                    {
                        shipstationOrders.pages = (short)(searchResult.TotalCount / searchCriteria.Count);
                        shipstationOrders.pages += (short)(searchResult.TotalCount % searchCriteria.Count == 0 ? 0 : 1);
                        shipstationOrders.pagesSpecified = true;
                    }
                }

                return Ok(shipstationOrders);
            }
            
            return BadRequest();
        }
        
        [HttpPost]
        [Route("")]
        [IdentityBasicAuthentication]
        public IHttpActionResult UpdateOrders(string action, string order_number, string carrier, string service, string tracking_number, ShipNotice shipnotice)
        {
            var order = _orderService.GetByOrderNumber(shipnotice.OrderNumber, CustomerOrderResponseGroup.Full);
            if (order == null)
            {
                return BadRequest("Order not found");
            }

            order.Patch(shipnotice);
            _orderService.Update(new[] { order });
            return Ok(shipnotice);
        }
    }
}
