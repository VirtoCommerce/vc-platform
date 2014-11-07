using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.MerchandisingModule.Web.Tests
{
    using VirtoCommerce.Caching.HttpCache;
    using VirtoCommerce.Foundation.Data.Catalogs;
    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.MerchandisingModule.Data.Services;
    using VirtoCommerce.MerchandisingModule.Models;
    using VirtoCommerce.MerchandisingModule.Web.Controllers;
    using VirtoCommerce.Search.Providers.Elastic;

    using Xunit;

    public class BrowseScenarios
    {
        [Fact]
        public void Can_browse_products_by_vendor_and_status()
        {
            var searchConnection = new SearchConnection("server=localhost:9200;scope=default;provider=elasticsearch");
            var search = new ElasticSearchProvider(
                new ElasticSearchQueryBuilder(),
                searchConnection);
            var service = new ItemBrowsingService(
                new EFCatalogRepository("VirtoCommerce"),
                search, new HttpCacheRepository(), searchConnection: searchConnection);

            var controller = new StoresController(service);
            var terms = new Dictionary<string, string[]>();
            terms.Add("brand", new []{"Apple"});

            var products = controller.GetProducts("default", new SearchParameters() { Terms = terms });
            Assert.NotNull(products);
        }
    }
}
