using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CartModule.Web.Converters;
using VirtoCommerce.CartModule.Web.Model;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Security;
using webModel = VirtoCommerce.CartModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Controllers.Api
{
    [RoutePrefix("api/cart")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
    public class CartModuleController : ApiController
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IShoppingCartSearchService _searchService;
        private readonly IStoreService _storeService;
        public CartModuleController(IShoppingCartService cartService, IShoppingCartSearchService searchService, IStoreService storeService)
        {
            _shoppingCartService = cartService;
            _searchService = searchService;
            _storeService = storeService;
        }

        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// Returns shopping cart or null if it is not found
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <response code="200"></response>
        [HttpGet]
        [ResponseType(typeof(webModel.ShoppingCart))]
        [Route("{storeId}/{customerId}/carts/current")]
        public IHttpActionResult GetCurrentCart(string storeId, string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                customerId = "anonymous";
            }

            var criteria = new Domain.Cart.Model.SearchCriteria
            {
                CustomerId = customerId,
                StoreId = storeId
            };

            var searchResult = this._searchService.Search(criteria);
            var retVal = searchResult.ShopingCarts.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Equals("default", StringComparison.OrdinalIgnoreCase));
            if (retVal == null)
            {
                retVal = searchResult.ShopingCarts.FirstOrDefault();
            }
            if (retVal == null)
            {
                return Ok();
            }
            //need load whole cart
            return GetCartById(retVal.Id);
        }

        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        /// <response code="200"></response>
        /// <response code="404">Shopping cart not found</response>
        [HttpGet]
        [ResponseType(typeof(webModel.ShoppingCart))]
        [Route("carts/{id}")]
        public IHttpActionResult GetCartById(string id)
        {
            var retVal = _shoppingCartService.GetById(id);
            if (retVal == null)
            {
                return NotFound();
            }
            return Ok(retVal.ToWebModel());
        }

        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        [HttpPost]
        [ResponseType(typeof(webModel.SearchResult))]
        [Route("search")]
        public IHttpActionResult Search(webModel.SearchCriteria criteria)
        {
            var retVal = _searchService.Search(criteria.ToCoreModel());
            return Ok(retVal.ToWebModel());
        }

        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        /// <response code="204">Operation completed</response>
        [HttpPost]
        [ResponseType(typeof(ShoppingCart))]
        [Route("carts")]
        [CheckPermission(Permission = PredefinedPermissions.Create)]
        public IHttpActionResult Create(webModel.ShoppingCart cart)
        {
            var coreCart = cart.ToCoreModel();
            coreCart = _shoppingCartService.Create(coreCart);
            return Ok(coreCart.ToWebModel());
        }

        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        [HttpPut]
        [ResponseType(typeof(ShoppingCart))]
        [Route("carts")]
        [CheckPermission(Permission = PredefinedPermissions.Update)]
        public IHttpActionResult Update(webModel.ShoppingCart cart)
        {
            var coreCart = cart.ToCoreModel();
            _shoppingCartService.Update(new[] { coreCart });
            var retVal = _shoppingCartService.GetById(coreCart.Id);
            return Ok(retVal.ToWebModel());
        }

        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.ShippingMethod[]))]
        [Route("carts/{cartId}/shipmentMethods")]
        public IHttpActionResult GetShipmentMethods(string cartId)
        {
            var cart = _shoppingCartService.GetById(cartId);
            var store = _storeService.GetById(cart.StoreId);
            var evalContext = new ShippingEvaluationContext(cart);

            var retVal = store.ShippingMethods.Where(x => x.IsActive)
                                              .SelectMany(x => x.CalculateRates(evalContext))
                                              .Select(x => x.ToWebModel()).ToArray();

            return Ok(retVal);
        }

        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.PaymentMethod[]))]
        [Route("carts/{cartId}/paymentMethods")]
        public IHttpActionResult GetPaymentMethods(string cartId)
        {
            var cart = _shoppingCartService.GetById(cartId);

            var store = _storeService.GetById(cart.StoreId);

            var retVal = store.PaymentMethods.Where(x => x.IsActive).Select(x => x.ToWebModel()).ToArray();

            return this.Ok(retVal);
        }

        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <param name="storeId">Store id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.PaymentMethod[]))]
        [Route("stores/{storeId}/paymentMethods")]
        public IHttpActionResult GetPaymentMethodsForStore(string storeId)
        {
            var store = _storeService.GetById(storeId);

            var retVal = store.PaymentMethods.Where(x => x.IsActive).Select(x => x.ToWebModel()).ToArray();

            return this.Ok(retVal);
        }


        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <response code="204">Operation completed</response>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("carts")]
        [CheckPermission(Permission = PredefinedPermissions.Delete)]
        public IHttpActionResult DeleteCarts([FromUri] string[] ids)
        {
            _shoppingCartService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

    
    }
}
