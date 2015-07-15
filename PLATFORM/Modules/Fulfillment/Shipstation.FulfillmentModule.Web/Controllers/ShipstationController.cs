using System.Web.Http.Description;
using Shipstation.FulfillmentModule.Web.Services;
using System.Web.Http;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;

namespace Shipstation.FulfillmentModule.Web.Controllers
{
    [RoutePrefix("api/fulfillment/shipstation")]
    public class ShipstationController : ApiController
    {
        private readonly IFulfillmentSettings _fulfillmentSettings;
        private readonly ICustomerOrderService _orderSearchService;

        public ShipstationController(IFulfillmentSettings fulfillmentSettings, ICustomerOrderService orderSearchService)
        {
            _fulfillmentSettings = fulfillmentSettings;
            _orderSearchService = orderSearchService;
        }

        [HttpGet]
        [Route("orders")]
        public IHttpActionResult GetNewOrders()
        {
            return NotFound();
        }

        [HttpPost]
        [Route("orders")]
        public IHttpActionResult UpdateOrders()
        {
            return Ok();
        }
    }
}
