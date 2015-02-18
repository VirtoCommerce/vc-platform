using System;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using Xunit;

namespace ApiClientTests
{
    using VirtoCommerce.ApiClient.DataContracts.Cart;

    public class CartScenarios
    {
        private CartClient Cart
        {
            get
            {
                return new CartClient(new Uri("http://localhost/admin/api/"), "27e0d789f12641049bd0e939185b4fd2", "34f0a3c12c9dbb59b63b5fece955b7b2b9a3b20f84370cba1524dd5c53503a2e2cb733536ecf7ea1e77319a47084a3a2c9d94d36069a432ecc73b72aeba6ea78");
            }
        }
        [Fact]
        public void Can_get_current_cart()
        {
            var client = Cart;
            var cart = Task.Run(() => client.GetCurrentCartAsync()).Result;
            Assert.NotNull(cart);
        }

        [Fact]
        public void Can_save_current_cart()
        {
            var client = Cart;
            var cart = Task.Run(() => client.GetCurrentCartAsync()).Result;
            cart.CustomerName = "Sample Customer";
            cart.Items.Add(CreateItem("shoes"));
            cart.Items.Add(CreateItem("socks"));
            var updatedCart = Task.Run(() => client.UpdateCurrentCartAsync(cart)).Result;
            Assert.Equal(cart.CustomerName, updatedCart.CustomerName);
        }

        [Fact]
        public void Can_create_order_from_cart()
        {
            var client = Cart;
            var cart = Task.Run(() => client.GetCurrentCartAsync()).Result;
        }

        private CartItem CreateItem(string name)
        {
            var item = new CartItem
            {
                CatalogId = "sample",
                ProductId = "asdasd",
                CategoryId = "category",
                ListPrice = 10,
                PlacedPrice = 20,
                Quantity = 2,
                Name = name,
                Currency = "USD"
            };

            return item;
        }
    }
}
