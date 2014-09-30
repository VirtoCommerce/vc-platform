using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Controllers.Api
{
    /// <summary>
    /// Class CartController.
    /// </summary>
    public class CartController : ApiController
    {
        /// <summary>
        /// Estimates the shipment.
        /// </summary>
        /// <param name="shippingEstimateModel">The shipping estimate model.</param>
        /// <returns>Json containing ShippingMethodModel with minimum price.</returns>
        [HttpPost]
        [ModelValidationFilter]
        public IHttpActionResult EstimatePost(ShippingEstimateModel shippingEstimateModel)
        {
            if (ModelState.IsValid)
            {
                var helper = new CartHelper(CartHelper.CartName);

                //Find best shipping method
	            var store = StoreHelper.StoreClient.GetCurrentStore();
	            var storeShippingMethods = helper.ShippingClient.GetAllShippingMethods()
					  .Where(sm => sm.PaymentMethodShippingMethods.Select(x => x.PaymentMethod)
		                   .Any(pm => store.PaymentGateways.Any(pg => pg.PaymentGateway == pm.Name)))
						   .Select(s=>s.ShippingMethodId).ToList();
				var bestShipping = helper.GetShippingMethods(storeShippingMethods).Min();

                //Update line items
                foreach (var lineItem in helper.OrderForm.LineItems)
                {
                    lineItem.ShippingMethodId = bestShipping.Id;
                    lineItem.ShippingMethodName = bestShipping.DisplayName;
                }

                // run workflow
                helper.RunWorkflow("ShoppingCartPrepareWorkflow");
                helper.SaveChanges();

                return Ok(bestShipping);
            }

            return BadRequest();
        }
    }
}