namespace VirtoCommerce.CartModule.Web.Controllers.Api
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.ModelBinding;

    using VirtoCommerce.CartModule.Web.Binders;
    using VirtoCommerce.CartModule.Web.Converters;
    using VirtoCommerce.Domain.Cart.Services;
    using VirtoCommerce.Foundation.Money;

    [RoutePrefix("api/cart")]
	public class CartController : ApiController
	{
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IShoppingCartSearchService _searchService;
		public CartController(IShoppingCartService cartService, IShoppingCartSearchService searchService)
		{
			this._shoppingCartService = cartService;
			this._searchService = searchService;
		}

		// GET: api/cart/store1/carts/current
		[HttpGet]
		[ResponseType(typeof(CatalogModule.Web.Model.ShoppingCart))]
		[Route("{storeId}/carts/current")]
		public IHttpActionResult GetCurrentCart(string storeId)
		{
            var customerId = this.User.Identity.Name;
            if (String.IsNullOrEmpty(customerId)) customerId = "anonymous";

			var criteria = new Domain.Cart.Model.SearchCriteria
			{
				CustomerId = customerId,
				StoreId = storeId
			};

			var searchResult = this._searchService.Search(criteria);
			var retVal = searchResult.ShopingCarts.FirstOrDefault(x=>x.Name == "default");
			if (retVal == null)
			{
				var newCart = new CatalogModule.Web.Model.ShoppingCart
				{
					Id = Guid.NewGuid().ToString(),
					CustomerId = customerId,
					IsAnonymous = this.User.Identity.IsAuthenticated,
					StoreId = storeId,
					Name = "default",
					Currency = CurrencyCodes.USD
				};
				retVal = this._shoppingCartService.Create(newCart.ToCoreModel());
			}
			return this.Ok(retVal.ToWebModel());
		}

		// GET: api/cart/carts/{id}
		[HttpGet]
		[ResponseType(typeof(CatalogModule.Web.Model.ShoppingCart))]
		[Route("carts/{id}")]
		public IHttpActionResult GetCartById(string id)
		{
			var retVal = this._shoppingCartService.GetById(id);
			if(retVal == null)
			{
				return this.NotFound();
			}
			return this.Ok(retVal.ToWebModel());
		}

		// GET: api/cart/carts?q=ddd&site=site1&customer=user1&start=0&count=20
		[HttpGet]
		[ResponseType(typeof(CatalogModule.Web.Model.SearchResult))]
		[Route("carts")]
		public IHttpActionResult SearchCarts([ModelBinder(typeof(SearchCriteriaBinder))] CatalogModule.Web.Model.SearchCriteria criteria)
		{
			var retVal = this._searchService.Search(criteria.ToCoreModel());
			return this.Ok(retVal.ToWebModel());
		}

		// POST: api/cart/carts
		[HttpPost]
		[ResponseType(typeof(void))]
		[Route("carts")]
		public IHttpActionResult Create(CatalogModule.Web.Model.ShoppingCart cart)
		{
			var coreCart = cart.ToCoreModel();
			this._shoppingCartService.Create(coreCart);
			return this.StatusCode(HttpStatusCode.NoContent);
		}

		// PUT: api/cart/carts
        [HttpPut]
        [ResponseType(typeof(CatalogModule.Web.Model.ShoppingCart))]
        [Route("carts")]
        public IHttpActionResult Update(CatalogModule.Web.Model.ShoppingCart cart)
        {
            var coreCart = cart.ToCoreModel();
            this._shoppingCartService.Update(new[] { coreCart });
            var retVal = this._shoppingCartService.GetById(coreCart.Id);
            return this.Ok(retVal.ToWebModel());
        }

		// GET: api/cart/carts/{cartId}/shipmentMethods
		[HttpGet]
		[ResponseType(typeof(CatalogModule.Web.Model.ShipmentMethod[]))]
		[Route("carts/{cartId}/shipmentMethods")]
		public IHttpActionResult GetShipmentMethods(string cartId)
		{
			var cart = this._shoppingCartService.GetById(cartId);
			var retVal = new[] 
			{
				 new CatalogModule.Web.Model.ShipmentMethod {
					 Currency = cart.Currency,
					 Name = "USPS",
					 Price = 10,
					 ShipmentMethodCode = "USPS"
				 }
			};

			return this.Ok(retVal);
		}

		// GET: api/cart/carts/{cartId}/paymentMethods
		[HttpGet]
		[ResponseType(typeof(CatalogModule.Web.Model.PaymentMethod[]))]
		[Route("carts/{cartId}/paymentMethods")]
		public IHttpActionResult GetPaymentMethods(string cartId)
		{
			var cart = this._shoppingCartService.GetById(cartId);
			var retVal = new[] 
			{
				 new CatalogModule.Web.Model.PaymentMethod {
					GatewayCode = "PayPal",
					Name = "PayPal"
				 }
			};

			return this.Ok(retVal);
		}

		// POST: api/cart/carts/{cartId}/coupons/{couponCode}
		[HttpPost]
		[ResponseType(typeof(CatalogModule.Web.Model.ShoppingCart))]
		[Route("carts/{cartId}/coupons/{couponCode}")]
		public IHttpActionResult ApplyCoupon(string cartId, string couponCode)
		{
			var retVal = this._shoppingCartService.GetById(cartId);

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
			this._shoppingCartService.Update(new[] { retVal });


			return this.Ok(retVal.ToWebModel());
		}
    }
}
