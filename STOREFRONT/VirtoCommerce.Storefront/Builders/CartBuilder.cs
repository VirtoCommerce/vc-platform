using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Builders
{
    public class CartBuilder : ICartBuilder
    {
        private readonly IShoppingCartModuleApi _cartApi;

        private Store _store;
        private Customer _customer;
        private Currency _currency;

        private ShoppingCart _cart = new ShoppingCart();

        public CartBuilder(IShoppingCartModuleApi cartApi)
        {
            _cartApi = cartApi;
        }

        public async Task<CartBuilder> GetOrCreateNewTransientCartAsync(Store store, Customer customer, Currency currency)
        {
            VirtoCommerceCartModuleWebModelShoppingCart cart = null;

            _store = store;
            _customer = customer;
            _currency = currency;

            try
            {
                cart = await _cartApi.CartModuleGetCurrentCartAsync(_store.Id, _customer.Id);
            }
            catch (Client.Client.ApiException exception)
            {
                if (exception.ErrorCode != 404)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
            finally
            {
                // TODO: Remake with factory or something about it
                cart = new VirtoCommerceCartModuleWebModelShoppingCart();
                cart.Currency = _currency.Code;
                cart.CustomerId = _customer.Id;
                cart.CustomerName = _customer.Name;
                cart.Name = "Default";
                cart.StoreId = _store.Id;

                _cart = cart.ToWebModel();
            }

            return this;
        }

        public CartBuilder AddItem(LineItem lineItem)
        {
            var existingLineItem = _cart.Items.FirstOrDefault(i => i.Sku == lineItem.Sku);
            if (existingLineItem != null)
            {
                existingLineItem.Quantity += lineItem.Quantity;
            }
            else
            {
                _cart.Items.Add(lineItem);
            }

            return this;
        }

        public CartBuilder RemoveItem(string id)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
                _cart.Items.Remove(lineItem);
            }

            return this;
        }

        public CartBuilder UpdateItem(string id, int quantity)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
                lineItem.Quantity = quantity;
            }

            return this;
        }

        public CartBuilder MergeWith(ShoppingCart cart)
        {
            foreach (var lineItem in cart.Items)
            {
                AddItem(lineItem);
            }

            return this;
        }

        public async Task SaveAsync()
        {
            await _cartApi.CartModuleUpdateAsync(_cart.ToServiceModel());
        }

        public ShoppingCart Cart
        {
            get
            {
                return _cart;
            }
        }
    }
}