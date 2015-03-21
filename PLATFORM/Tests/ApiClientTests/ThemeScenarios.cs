using System;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using Xunit;

namespace ApiClientTests
{
    public class ThemeScenarios
    {
        private ThemeClient Client
        {
            get
            {
                return new ThemeClient(new Uri("http://localhost/admin/api/cms/"), "27e0d789f12641049bd0e939185b4fd2", "34f0a3c12c9dbb59b63b5fece955b7b2b9a3b20f84370cba1524dd5c53503a2e2cb733536ecf7ea1e77319a47084a3a2c9d94d36069a432ecc73b72aeba6ea78");
            }
        }

        [Fact]
        public void Can_get_themeassets()
        {
            var client = Client;
            var assets = Task.Run(() => client.GetThemeAssetsAsync("SampleStore", "Default", DateTime.Now.AddYears(-1))).Result;
            Assert.NotNull(assets);
        }
    }
}
