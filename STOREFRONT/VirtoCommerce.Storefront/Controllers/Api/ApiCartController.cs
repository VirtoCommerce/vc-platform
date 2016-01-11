using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Converters;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [RoutePrefix("api/cart")]
    public class ApiCartController : ApiControllerBase
    {
        private readonly ICartBuilder _cartBuilder;
        private readonly IShoppingCartModuleApi _cartApi;

        public ApiCartController(WorkContext workContext, ICatalogSearchService catalogSearchService, ICatalogSearchService productService, IStoreModuleApi storeApi, ICartBuilder cartBuilder, IShoppingCartModuleApi cartApi)
            : base(workContext, catalogSearchService, productService, storeApi)
        {
            _cartBuilder = cartBuilder;
            _cartApi = cartApi;
        }

        //Load cart
        [Route("")]
        public async Task<ShoppingCart> GetCart()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);
            return _cartBuilder.Cart;
        }

        // POST: /cart/additem?id=...&quantity=...
        [HttpPost]
        [Route("additem")]
        public async Task<object> AddItem(string id, int quantity = 1)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var product = await _catalogService.GetProductAsync(id, Model.Catalog.ItemResponseGroup.ItemLarge);
            if (product != null)
            {
                await _cartBuilder.AddItemAsync(product, quantity);
                await _cartBuilder.SaveAsync();
            }

            return new { ItemsCount = _cartBuilder.Cart.Items.Sum(i => i.Quantity) };
        }

        // POST: /cart/changeitem?lineItemId=...&quantity=...
        [HttpPost]
        [Route("changeitem")]
        public async Task<IHttpActionResult> ChangeItem(string lineItemId, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ChangeItemQuantityAsync(lineItemId, quantity);
            await _cartBuilder.SaveAsync();

            return Ok();
        }

        // POST: /cart/removeitem?lineItemId=...
        [HttpPost]
        [Route("removeitem")]
        public async Task<object> RemoveItem(string lineItemId)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveItemAsync(lineItemId);
            await _cartBuilder.SaveAsync();

            return new { ItemsCount = _cartBuilder.Cart.Items.Sum(i => i.Quantity) };
        }

        // POST: /cart/addaddress
        [HttpPost]
        [Route("addaddress")]
        public async Task<IHttpActionResult> AddAddress(Address address)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.AddAddressAsync(address);
            await _cartBuilder.SaveAsync();

            return Ok();
        }

        // GET: /cart/shippingmethods
        [Route("shippingmethods")]
        public async Task<IEnumerable<ShippingMethod>> GetShippingMethods()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var shippingMethods = await _cartApi.CartModuleGetShipmentMethodsAsync(_cartBuilder.Cart.Id);
            // shippingMethods.Add(new Client.Model.VirtoCommerceCartModuleWebModelShippingMethod { Name= "test", Price = 555, Currency = "USD", ShipmentMethodCode = "testZZ"  });
            return shippingMethods.Select(sm => sm.ToWebModel());
        }

        // POST: /cart/shippingmethod?shippingMethodCode=...
        [HttpPost]
        [Route("shippingmethod")]
        public async Task<IHttpActionResult> SetShippingMethod(string shippingMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            WorkContext.CurrentCart = _cartBuilder.Cart;
            var shippingMethods = await _cartApi.CartModuleGetShipmentMethodsAsync(WorkContext.CurrentCart.Id);
            var shippingMethod = shippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == shippingMethodCode);
            if (shippingMethod != null)
            {
                await _cartBuilder.AddShipmentAsync(shippingMethod.ToWebModel());
                await _cartBuilder.SaveAsync();
            }

            return Ok();
        }
    }
}