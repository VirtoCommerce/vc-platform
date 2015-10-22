using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CartModule.Web.Binders;
using VirtoCommerce.CartModule.Web.Converters;
using VirtoCommerce.CartModule.Web.Model;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Payment.Services;
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
			this._shoppingCartService = cartService;
			this._searchService = searchService;
			_storeService = storeService;
		}

        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <response code="200"></response>
        /// <response code="404">Shopping cart not found</response>
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
			var retVal = searchResult.ShopingCarts.FirstOrDefault(x=>x.Name == "default");
			if(retVal == null)
			{
				return NotFound();
			}
			return Ok(retVal.ToWebModel());
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
			if(retVal == null)
			{
				return NotFound();
			}
			return Ok(retVal.ToWebModel());
		}

		/// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <param name="criteria">Search criteria</param>
		[HttpGet]
		[ResponseType(typeof(webModel.SearchResult))]
		[Route("carts")]
		public IHttpActionResult SearchCarts([ModelBinder(typeof(SearchCriteriaBinder))] webModel.SearchCriteria criteria)
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
		[ResponseType(typeof(void))]
		[Route("carts")]
        [CheckPermission(Permission = PredefinedPermissions.Create)]
		public IHttpActionResult Create(webModel.ShoppingCart cart)
		{
			var coreCart = cart.ToCoreModel();
			_shoppingCartService.Create(coreCart);
			return this.StatusCode(HttpStatusCode.NoContent);
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

			var retVal = store.ShippingMethods.Where(x => x.IsActive).SelectMany(x => x.CalculateRates(evalContext))
				.Select(x => new webModel.ShippingMethod
				{
					Currency = cart.Currency,
					Name = x.ShippingMethod.Description,
					OptionName = x.OptionName,
					OptionDescription = x.OptionDescription,
					Price = x.Rate,
					ShipmentMethodCode = x.ShippingMethod.Code,
					LogoUrl = x.ShippingMethod.LogoUrl,
					TaxType = x.ShippingMethod.TaxType
				});
			
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

			var retVal = store.PaymentMethods.Where(p => p.IsActive).Select(p => new webModel.PaymentMethod
							{
								GatewayCode = p.Code,
								Name = p.Name,
								IconUrl = p.LogoUrl,
								Type = p.PaymentMethodType.ToString(),
								Group = p.PaymentMethodGroupType.ToString(),
								Description = p.Description,
								Priority = p.Priority,
                                IsAvailableForPartial = p.IsAvailableForPartial
							}).ToArray();

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

            var retVal = store.PaymentMethods.Where(p => p.IsActive).Select(p => new webModel.PaymentMethod
            {
                GatewayCode = p.Code,
                Name = p.Description,
                IconUrl = p.LogoUrl,
                Type = p.PaymentMethodType.ToString(),
                Group = p.PaymentMethodGroupType.ToString(),
                Description = p.Description,
                Priority = p.Priority,
                IsAvailableForPartial = p.IsAvailableForPartial
            }).ToArray();

            return this.Ok(retVal);
        }

		/// <summary>
        /// Apply coupon for shopping cart
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        /// <param name="couponCode">Coupon code</param>
		[HttpPost]
		[ResponseType(typeof(webModel.ShoppingCart))]
		[Route("carts/{cartId}/coupons/{couponCode}")]
		public IHttpActionResult ApplyCoupon(string cartId, string couponCode)
		{
			var retVal = _shoppingCartService.GetById(cartId);

			//TODO: check coupon from marketing service 

			var coupon = new Domain.Cart.Model.Coupon
			{
				CouponCode = couponCode
			};
			var discount = new Domain.Cart.Model.Discount
			{
				Description = couponCode,
				PromotionId = couponCode,
				DiscountAmount = 10
			};
			retVal.Discounts.Add(discount);
			retVal.Coupon = coupon;
			_shoppingCartService.Update(new[] { retVal });


			return this.Ok(retVal.ToWebModel());
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
