using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using Xunit;

namespace ApiClientTests
{
    public class CartScenarios
    {
        [Fact]
        public void Can_get_current_cart()
        {
            var client = new CartClient(new Uri("http://localhost/admin/api/"), "secret");
            var cart = Task.Run(()=>client.GetCurrentCartAsync("samplestore")).Result;
        }
    }
}
