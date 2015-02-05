using System;

namespace FunctionalTests.Search
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using VirtoCommerce.Foundation.Catalogs.Search;
    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Search.Providers.Azure;
    using Xunit;
    using Xunit.Sdk;

    public class AzureSearchScenarios
    {
        private const string Datasource = "SOURCE";
        private const string AccessKey = "KEY";

  
        [Fact(Skip = "Needs existing configuration"), Trait("type", "azuresearch")]
        public void Can_create_azuresearch_index()
        {
            var scope = "test";
            var queryBuilder = new AzureSearchQueryBuilder();
            var conn = new SearchConnection(Datasource, scope, accessKey: AccessKey);
            var provider = new AzureSearchProvider(queryBuilder, conn);           
            SearchHelper.CreateSampleIndex(provider, scope);
            provider.RemoveAll(scope, String.Empty);
        }

        [Fact]
        public void Can_create_connection_azuresearch()
        {
            var connectionString =
                "server=virtocommerce;scope=default;key=accesskey;provider=azuresearch";

            var connection = new SearchConnection(connectionString);

            Assert.True(connection.DataSource == "virtocommerce");
            Assert.True(connection.Provider == "azuresearch");
            Assert.True(connection.Scope == "default");
            Assert.True(connection.AccessKey == "accesskey");
        }


        //[Fact]
        [Trait("type", "azuresearch")]
        public void Can_find_items_azuresearch()
        {
            var scope = "default";
            var queryBuilder = new AzureSearchQueryBuilder();
            var conn = new SearchConnection(Datasource, scope, accessKey: AccessKey);
            var provider = new AzureSearchProvider(queryBuilder, conn);

            var criteria = new CatalogItemSearchCriteria
                           {
                               SearchPhrase = "sony",
                               IsFuzzySearch = true,
                               Catalog = "vendorvirtual",
                               RecordsToRetrieve = 10,
                               StartingRecord = 0,
                               Pricelists = new string[] { },
                               StartDate = DateTime.UtcNow,
                               Sort = new SearchSort("price_usd_saleusd")
                           };

            criteria.Outlines.Add("vendorvirtual*");

            //"(startdate lt 2014-09-26T22:05:11Z) and sys__hidden eq 'false' and sys__outline/any(t:t eq 'VendorVirtual/e1b56012-d877-4bdd-92d8-3fc186899d0f*') and catalog/any(t: t eq 'VendorVirtual')"
            var results = provider.Search(scope, criteria);
        }

        [Fact(Skip = "Needs existing configuration"), Trait("type", "azuresearch")]
        public void Can_find_item_azuresearch()
        {
            var scope = "test";
            var queryBuilder = new AzureSearchQueryBuilder();
            var conn = new SearchConnection(Datasource, scope, accessKey: AccessKey);
            var provider = new AzureSearchProvider(queryBuilder, conn);

            provider.RemoveAll(scope, String.Empty);
            SearchHelper.CreateSampleIndex(provider, scope);

            var criteria = new CatalogItemSearchCriteria
            {
                SearchPhrase = "product",
                IsFuzzySearch = true,
                Catalog = "goods",
                RecordsToRetrieve = 10,
                StartingRecord = 0,
                Pricelists = new string[] { }
            };


            // force delay, otherwise records are not available
            Thread.Sleep(1000); 

            var results = provider.Search(scope, criteria);
            
            Assert.True(results.DocCount == 1, String.Format("Returns {0} instead of 1", results.DocCount));

            criteria = new CatalogItemSearchCriteria
            {
                SearchPhrase = "sample product ",
                IsFuzzySearch = true,
                Catalog = "goods",
                RecordsToRetrieve = 10,
                StartingRecord = 0,
                Pricelists = new string[] { }
            };


            results = provider.Search(scope, criteria);

            Assert.True(results.DocCount == 1, String.Format("\"Sample Product\" search returns {0} instead of 1", results.DocCount));

            provider.RemoveAll(scope, String.Empty);
        }
    }
}
