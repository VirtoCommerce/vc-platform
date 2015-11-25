using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Builders
{
    public class CartBuilder : ICartBuilder
    {
        private readonly IShoppingCartModuleApi _cartApi;

        private Store _store;
        private Customer _customer;
        private Currency _currency;

        private ShoppingCart _cart;

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

            cart = await _cartApi.CartModuleGetCurrentCartAsync(_store.Id, _customer.Id);
            if (cart == null)
            {
                _cart = new ShoppingCart(_store.Id, _customer.Id, _customer.Name, "Default", _currency.Code);
            }
            else
            {
                _cart = cart.ToWebModel();
            }

            return this;
        }

        public CartBuilder AddItem(Product product, int quantity)
        {
            var existingLineItem = _cart.Items.FirstOrDefault(i => i.Sku == product.Sku);
            if (existingLineItem != null)
            {
                existingLineItem.Quantity += quantity;
            }
            else
            {
                _cart.Items.Add(product.ToLineItem(quantity));
            }

            return this;
        }

        public CartBuilder UpdateItem(string id, int quantity)
        {
            var lineItem = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (lineItem != null)
            {
                if (quantity > 0)
                {
                    lineItem.Quantity = quantity;
                }
                else
                {
                    _cart.Items.Remove(lineItem);
                }
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

        public CartBuilder AddAddress(Address address)
        {
            _cart.Addresses.Add(address);

            return this;
        }

        public async Task SaveAsync()
        {
            var cart = _cart.ToServiceModel();

            if (_cart.IsTransient())
            {
                await _cartApi.CartModuleCreateAsync(cart);
            }
            else
            {
                await _cartApi.CartModuleUpdateAsync(cart);
            }
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