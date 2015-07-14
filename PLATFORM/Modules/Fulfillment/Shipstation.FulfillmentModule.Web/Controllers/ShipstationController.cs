using Shipstation.FulfillmentModule.Web.Services;
using System.Web.Http;

namespace Shipstation.FulfillmentModule.Web.Controllers
{
    [RoutePrefix("api/fulfillment/shipstation")]
    public class ShipstationController : ApiController
    {
        private readonly IFulfillmentSettings _fulfillmentSettings;

        public ShipstationController(IFulfillmentSettings fulfillmentSettings)
        {
            _fulfillmentSettings = fulfillmentSettings;
        }
    }
}
