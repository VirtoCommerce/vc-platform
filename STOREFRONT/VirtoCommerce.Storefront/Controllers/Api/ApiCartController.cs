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
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [RoutePrefix("api/cart")]
    public class ApiCartController : ApiControllerBase
    {
        private readonly ICartBuilder _cartBuilder;
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly IOrderModuleApi _orderApi;

        public ApiCartController(WorkContext workContext, ICatalogSearchService catalogSearchService, ICatalogSearchService productService, IStoreModuleApi storeApi, ICartBuilder cartBuilder, IShoppingCartModuleApi cartApi, IOrderModuleApi orderApi)
            : base(workContext, catalogSearchService, productService, storeApi)
        {
            _cartBuilder = cartBuilder;
            _cartApi = cartApi;
            _orderApi = orderApi;
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

            var products = await _catalogSearchService.GetProductsAsync(new string[] { id }, Model.Catalog.ItemResponseGroup.ItemLarge);
            if (products != null && products.Any())
            {
                await _cartBuilder.AddItemAsync(products.First(), quantity);
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

            var shippingMethods = await _cartBuilder.GetAvailableShippingMethodsAsync();
            // shippingMethods.Add(new Client.Model.VirtoCommerceCartModuleWebModelShippingMethod { Name= "test", Price = 555, Currency = "USD", ShipmentMethodCode = "testZZ"  });
            return shippingMethods;
        }

        // POST: /cart/shippingmethod?shippingMethodCode=...
        [HttpPost]
        [Route("shippingmethod")]
        public async Task<IHttpActionResult> SetShippingMethod(string shippingMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            WorkContext.CurrentCart = _cartBuilder.Cart;
            var shippingMethods = await _cartBuilder.GetAvailableShippingMethodsAsync();
            var shippingMethod = shippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == shippingMethodCode);
            if (shippingMethod != null)
            {
                await _cartBuilder.AddShipmentAsync(shippingMethod);
                await _cartBuilder.SaveAsync();
            }

            return Ok();
        }

        // GET: /cart/paymentmethods
        [Route("paymentmethods")]
        public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var paymentMethods = await _cartApi.CartModuleGetPaymentMethodsAsync(_cartBuilder.Cart.Id);

            return paymentMethods.Select(pm => pm.ToWebModel());
        }

        // POST: /cart/paymentmethod?paymentMethodCode=...
        [HttpPost]
        [Route("paymentmethod")]
        public async Task<IHttpActionResult> SetPaymentMethod(string paymentMethodCode)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            WorkContext.CurrentCart = _cartBuilder.Cart;
            var paymentMethods = await _cartApi.CartModuleGetPaymentMethodsAsync(WorkContext.CurrentCart.Id);
            var paymentMethod = paymentMethods.FirstOrDefault(pm => pm.GatewayCode == paymentMethodCode);
            if (paymentMethod != null)
            {
                await _cartBuilder.AddPaymentAsync(paymentMethod.ToWebModel());
                await _cartBuilder.SaveAsync();
            }

            return Ok();
        }

        // POST: /cart/createorder
        [HttpPost]
        [Route("createorder")]
        public async Task<object> CreateOrder(VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var order = await _orderApi.OrderModuleCreateOrderFromCartAsync(_cartBuilder.Cart.Id);

            await _cartBuilder.RemoveCartAsync();

            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    var contact = await _customerApi.CustomerModuleGetContactByIdAsync(WorkContext.CurrentCustomer.Id);

            //    foreach (var orderAddress in order.Addresses)
            //    {
            //        contact.Addresses.Add(orderAddress.ToCustomerModel());
            //    }

            //    await _customerApi.CustomerModuleUpdateContactAsync(contact);
            //}

            var processingResult = await GetOrderProcessingResultAsync(order, bankCardInfo);

            return new { Order = order, OrderProcessingResult = processingResult };
        }

        private async Task<VirtoCommerceOrderModuleWebModelProcessPaymentResult> GetOrderProcessingResultAsync(
            VirtoCommerceOrderModuleWebModelCustomerOrder order, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo)
        {
            if (bankCardInfo == null)
            {
                bankCardInfo = new VirtoCommerceDomainPaymentModelBankCardInfo();
            }

            VirtoCommerceOrderModuleWebModelProcessPaymentResult processingResult = null;
            var incomingPayment = order.InPayments != null ? order.InPayments.FirstOrDefault() : null;
            if (incomingPayment != null)
            {
                processingResult = await _orderApi.OrderModuleProcessOrderPaymentsAsync(bankCardInfo, order.Id, incomingPayment.Id);
            }

            return processingResult;
        }
    }
}