using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.CoreModule.Web.Converters;
using VirtoCommerce.Domain.Fulfillment.Services;
using webModel = VirtoCommerce.CoreModule.Web.Model;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
    [RoutePrefix("api/fulfillment")]
    public class FulfillmentController : ApiController
    {
        private readonly IFulfillmentService _fulfillmentService;
        public FulfillmentController(IFulfillmentService fulfillmentService)
        {
            _fulfillmentService = fulfillmentService;
        }

        /// <summary>
        /// GET: api/fulfillment/centers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.FulfillmentCenter[]))]
        [Route("centers")]
        public IHttpActionResult GetFulfillmentCenters()
        {
            var retVal = _fulfillmentService.GetAllFulfillmentCenters().Select(x => x.ToWebModel()).ToArray();
            return Ok(retVal);
        }

        // GET: api/fulfillment/centers/{id}
        [HttpGet]
        [ResponseType(typeof(webModel.FulfillmentCenter))]
        [Route("centers/{id}")]
        public IHttpActionResult GetFulfillmentCenter(string id)
        {
            var retVal = _fulfillmentService.GetAllFulfillmentCenters().First(x => x.Id == id);
            return Ok(retVal.ToWebModel());
        }

        // PUT: api/fulfillment/centers
        [HttpPut]
        [ResponseType(typeof(webModel.FulfillmentCenter))]
        [Route("centers")]
        public IHttpActionResult UpdateFulfillmentCenter(webModel.FulfillmentCenter center)
        {
            var retVal = _fulfillmentService.UpsertFulfillmentCenter(center.ToCoreModel());
            return Ok(retVal);
        }

    }
}
