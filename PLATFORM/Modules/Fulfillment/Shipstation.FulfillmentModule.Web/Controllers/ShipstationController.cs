using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http.Description;
using Shipstation.FulfillmentModule.Web.Converters;
using Shipstation.FulfillmentModule.Web.Models.Order;
using Shipstation.FulfillmentModule.Web.Services;
using System.Web.Http;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;

namespace Shipstation.FulfillmentModule.Web.Controllers
{
    [RoutePrefix("api/fulfillment/shipstation")]
    [ControllerConfig]
    [AllowAnonymous]
    public class ShipstationController : ApiController
    {
        private readonly IFulfillmentSettings _fulfillmentSettings;
        private readonly ICustomerOrderService _orderService;
        private readonly ICustomerOrderSearchService _orderSearchService;
        private const string dateTimeFormat = "MM'/'dd'/'yyyy HH:mm:ss tt";

        public ShipstationController(IFulfillmentSettings fulfillmentSettings, ICustomerOrderService orderService, ICustomerOrderSearchService orderSearchService)
        {
            _fulfillmentSettings = fulfillmentSettings;
            _orderSearchService = orderSearchService;
            _orderService = orderService;
        }

        [HttpGet]
        [Route("orders")]
        [ResponseType(typeof(Orders))]
        public IHttpActionResult GetNewOrders(string action, string start_date, string end_date, int page)
        {
            if (action == "export")
            {
                var searchCriteria = new SearchCriteria
                {
                    StartDate = DateTime.Parse(start_date, new CultureInfo("en-US")),
                    EndDate = DateTime.Parse(end_date, new CultureInfo("en-US"))
                };
                var searchResult = _orderSearchService.Search(searchCriteria);

                if (searchResult.CustomerOrders != null && searchResult.CustomerOrders.Any())
                {
                    var shipstationOrders = new Orders();

                    var shipstationOrdersList = new List<OrdersOrder>();

                    searchResult.CustomerOrders.ForEach(cu => shipstationOrdersList.Add(cu.ToShipstationOrder()));
                    
                    shipstationOrders.Order = shipstationOrdersList.ToArray();

                    return Ok(shipstationOrders);
                }

                return Ok();
            }

            if (action == "shipnotify")
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("orders")]
        public IHttpActionResult UpdateOrders(string action)
        {
            return Ok();
        }
    }
}
