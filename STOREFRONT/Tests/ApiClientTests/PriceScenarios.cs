using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using Xunit;

namespace ApiClientTests
{
    public class PriceScenarios
    {
        private PriceClient Client
        {
            get
            {
                return new PriceClient(new Uri("http://localhost/admin/api/"), "27e0d789f12641049bd0e939185b4fd2", "34f0a3c12c9dbb59b63b5fece955b7b2b9a3b20f84370cba1524dd5c53503a2e2cb733536ecf7ea1e77319a47084a3a2c9d94d36069a432ecc73b72aeba6ea78");
            }
        }

        [Fact]
        public void Can_get_pricelists()
        {
            var client = Client;
            for (int index = 1; index < 1000; index++)
            {
                var pricelists = Task.Run(() => client.GetPriceListsAsync("vendorvirtual", "USD", new TagQuery())).Result;
                //Assert.NotNull(pricelists);
                //Assert.True(pricelists.Count() == 2);
            }
        }

        [Fact]
        public void Can_get_prices()
        {
            var client = Client;
            var prices = Task.Run(() => client.GetPrices(new[] { "SaleUSD", "DefaultUSD" }, new[] {"v-b000ephb26","v-b0013av4m4"})).Result;
            Assert.NotNull(prices);
            Assert.True(prices.Any());
        }
    }
}
