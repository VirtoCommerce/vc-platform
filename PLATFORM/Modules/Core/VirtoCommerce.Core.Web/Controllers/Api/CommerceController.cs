using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.CoreModule.Web.Converters;
using webModel = VirtoCommerce.CoreModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.CoreModule.Web.Model;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class CommerceController : ApiController
    {
		private readonly IPaymentMethodsService _paymentMethodsService;
	    private readonly ICommerceService _commerceService;
		public CommerceController(ICommerceService commerceService, IPaymentMethodsService paymentMethodsService)
        {
			_commerceService = commerceService;
			_paymentMethodsService = paymentMethodsService;
        }

		/// <summary>
		/// api/paymentgateways
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ResponseType(typeof(PaymentMethod[]))]
		[Route("paymentgateways")]
		public IHttpActionResult GetGateways()
		{
			var retVal = _paymentMethodsService.GetAllPaymentMethods().Select(x => x.ToWebModel()).ToArray();
			return Ok(retVal);
		}

        /// <summary>
        /// GET: api/fulfillment/centers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.FulfillmentCenter[]))]
		[Route("fulfillment/centers")]
        public IHttpActionResult GetFulfillmentCenters()
        {
            var retVal = _commerceService.GetAllFulfillmentCenters().Select(x => x.ToWebModel()).ToArray();
            return Ok(retVal);
        }

        // GET: api/fulfillment/centers/{id}
        [HttpGet]
        [ResponseType(typeof(webModel.FulfillmentCenter))]
		[Route("fulfillment/centers/{id}")]
        public IHttpActionResult GetFulfillmentCenter(string id)
        {
            var retVal = _commerceService.GetAllFulfillmentCenters().First(x => x.Id == id);
            return Ok(retVal.ToWebModel());
        }

        // PUT: api/fulfillment/centers
        [HttpPut]
        [ResponseType(typeof(webModel.FulfillmentCenter))]
		[Route("fulfillment/centers")]
        public IHttpActionResult UpdateFulfillmentCenter(webModel.FulfillmentCenter center)
        {
            var retVal = _commerceService.UpsertFulfillmentCenter(center.ToCoreModel());
            return Ok(retVal);
        }

    }
}
