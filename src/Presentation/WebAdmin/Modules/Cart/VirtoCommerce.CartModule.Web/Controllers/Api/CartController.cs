using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.CartModule.Web.Converters;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CartModule.Web.Binders;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
	[RoutePrefix("api/cart")]
	public class CartController : ApiController
	{
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IShoppingCartSearchService _searchService;
		public CartController(IShoppingCartService cartService, IShoppingCartSearchService searchService)
		{
			_shoppingCartService = cartService;
			_searchService = searchService;
		}

		// GET: api/cart/store1/carts/current
		[HttpGet]
		[ResponseType(typeof(webModel.ShoppingCart))]
		[Route("{siteId}/carts/current")]
		public IHttpActionResult GetCurrentCart(string siteId)
		{
			var customerId = User.Identity.Name;
			var criteria = new coreModel.SearchCriteria
			{
				CustomerId = customerId,
				SiteId = siteId
			};

			var searchResult = _searchService.Search(criteria);
			var retVal = searchResult.ShopingCarts.FirstOrDefault(x=>x.Name == "default");
			if (retVal == null)
			{
				var newCart = new webModel.ShoppingCart
				{
					Id = Guid.NewGuid().ToString(),
					CustomerId = customerId,
					IsAnonymous = User.Identity.IsAuthenticated,
					SiteId = siteId,
					Name = "default",
					Currency = CurrencyCodes.USD
				};
				retVal = _shoppingCartService.Create(newCart.ToCoreModel());
			}
			return Ok(retVal.ToWebModel());
		}

		// GET: api/cart/carts/{id}
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

		// GET: api/cart/carts?q=ddd&site=site1&customer=user1&start=0&count=20
		[HttpGet]
		[ResponseType(typeof(webModel.SearchResult))]
		[Route("carts")]
		public IHttpActionResult SearchCarts([ModelBinder(typeof(SearchCriteriaBinder))] webModel.SearchCriteria criteria)
		{
			var retVal = _searchService.Search(criteria.ToCoreModel());
			return Ok(retVal.ToWebModel());
		}

		// POST: api/cart/carts
		[HttpPost]
		[ResponseType(typeof(void))]
		[Route("carts")]
		public IHttpActionResult Create(webModel.ShoppingCart cart)
		{
			var coreCart = cart.ToCoreModel();
			_shoppingCartService.Create(coreCart);
			return StatusCode(HttpStatusCode.NoContent);
		}

		// PUT: api/cart/carts
		[HttpPut]
        [ResponseType(typeof(webModel.ShoppingCart))]
		[Route("carts")]
		public IHttpActionResult Update(webModel.ShoppingCart cart)
		{
			var coreCart = cart.ToCoreModel();
			_shoppingCartService.Update(new [] { coreCart });
            var retVal = _shoppingCartService.GetById(coreCart.Id);
			return this.Ok(retVal.ToWebModel());
		}

		// GET: api/cart/carts/{cartId}/shipmentMethods
		[HttpGet]
		[ResponseType(typeof(webModel.ShipmentMethod[]))]
		[Route("carts/{cartId}/shipmentMethods")]
		public IHttpActionResult GetShipmentMethods(string cartId)
		{
			var cart = _shoppingCartService.GetById(cartId);
			var retVal = new [] 
			{
				 new webModel.ShipmentMethod {
					 Currency = cart.Currency,
					 Name = "USPS",
					 Price = 10,
					 ShipmentMethodCode = "USPS"
				 }
			};

			return Ok(retVal);
		}

		// GET: api/cart/carts/{cartId}/paymentMethods
		[HttpGet]
		[ResponseType(typeof(webModel.PaymentMethod[]))]
		[Route("carts/{cartId}/paymentMethods")]
		public IHttpActionResult GetPaymentMethods(string cartId)
		{
			var cart = _shoppingCartService.GetById(cartId);
			var retVal = new [] 
			{
				 new webModel.PaymentMethod {
					GatewayCode = "PayPal",
					Name = "PayPal"
				 }
			};

			return Ok(retVal);
		}

		// POST: api/cart/carts/{cartId}/coupons/{couponCode}
		[HttpPost]
		[ResponseType(typeof(webModel.ShoppingCart))]
		[Route("carts/{cartId}/coupons/{couponCode}")]
		public IHttpActionResult ApplyCoupon(string cartId, string couponCode)
		{
			var retVal = _shoppingCartService.GetById(cartId);
			
			//TODO: check coupon from marketing service 
			if(!retVal.Discounts.Any(x=>x.Coupon != null && x.Coupon.CouponCode == couponCode))
			{
				var coupon = new coreModel.Coupon
				{
					CouponCode = couponCode,
					IsValid = true
				};
				var discount = new coreModel.Discount
				{
					Coupon = coupon,
					Description = couponCode,
					PromotionId = couponCode,
					DiscountAmount = 10
				};
				retVal.Discounts.Add(discount);
				_shoppingCartService.Update(new [] { retVal });
			}
			
			return Ok(retVal.ToWebModel());
		}

	}
}
