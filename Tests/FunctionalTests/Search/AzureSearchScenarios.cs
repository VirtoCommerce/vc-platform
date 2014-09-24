using System;

namespace FunctionalTests.Search
{
    using VirtoCommerce.Foundation.Catalogs.Search;
    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Search.Providers.Azure;
    using Xunit;

    public class AzureSearchScenarios
    {
        private const string Scope = "default";
        private const string Datasource = "virtocommerce";
        private const string AccessKey = "128EE67AC838DF328B3BEC97ADB1A1B1";

        [Fact, Trait("type", "azuresearch")]
        public void Can_create_azuresearch_index()
        {
            var queryBuilder = new AzureSearchQueryBuilder();
            var conn = new SearchConnection(Datasource, Scope, accessKey: AccessKey);
            var provider = new AzureSearchProvider(queryBuilder, conn);           
            SearchHelper.CreateSampleIndex(provider, Scope);
            provider.RemoveAll(Scope, String.Empty);
        }

        [Fact, Trait("type", "azuresearch")]
        public void Can_find_item_azuresearch()
        {
            var scope = "default";
            var queryBuilder = new AzureSearchQueryBuilder();
            var conn = new SearchConnection(Datasource, Scope, accessKey: AccessKey);
            var provider = new AzureSearchProvider(queryBuilder, conn);           

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

            provider.RemoveAll(Scope, String.Empty);
        }
    }
}
