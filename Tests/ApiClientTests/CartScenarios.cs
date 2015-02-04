using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                return new CartClient(new Uri("http://localhost/admin/api/"), "secret");
            }
        }
        [Fact]
        public void Can_get_current_cart()
        {
            var client = Cart;
            var cart = Task.Run(()=>client.GetCurrentCartAsync("samplestore")).Result;
            Assert.NotNull(cart);
        }

        [Fact]
        public void Can_save_current_cart()
        {
            var client = Cart;
            var cart = Task.Run(() => client.GetCurrentCartAsync("samplestore")).Result;
            cart.CustomerName = "Sample Customer";
            Task.Run(() => client.UpdateCurrentCartAsync(cart));
            var updatedCart = Task.Run(() => client.GetCurrentCartAsync("samplestore")).Result;
            Assert.Equal(cart.CustomerName, updatedCart.CustomerName);
        }

        [Fact]
        public void Can_create_order_from_cart()
        {
            var client = Cart;
            var cart = Task.Run(() => client.GetCurrentCartAsync("samplestore")).Result;
        }
    }
}
