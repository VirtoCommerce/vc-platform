using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;

namespace Zendesk.HelpdeskModule.Web.Controllers.Api
{
    [RoutePrefix("api/help")]
    public class DataFetchController: ApiController
    {
        private readonly ICustomerOrderService _orderSearchService;

        public DataFetchController(ICustomerOrderService orderSearchService)
        {
            _orderSearchService = orderSearchService;
        }
        

        [HttpGet]
        [ResponseType(typeof(CustomerOrder))]
        [Route("order/{id}")]
        public IHttpActionResult GetOrderData(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var orderData = _orderSearchService.GetById(id, CustomerOrderResponseGroup.Full);
                if (orderData != null)
                    return Ok(orderData);
                
            }
            return NotFound();
        }
    }
}