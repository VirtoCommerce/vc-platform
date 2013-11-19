namespace FunctionalTests.Search
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using VirtoCommerce.Foundation.Catalogs.Search;
    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Foundation.Search.Schemas;
    using VirtoCommerce.Search.Providers.Lucene;

    using Xunit;

    public class LuceneScenarios
    {
        [Fact]
        public void Can_create_lucene_index()
        {
            var scope = "default";
            var queryBuilder = new LuceneSearchQueryBuilder();
            var conn = new SearchConnection("c:\\windows\\temp\\lucene", scope);
            var provider = new LuceneSearchProvider(queryBuilder, conn);
            SearchHelper.CreateSampleIndex(provider, scope);
            Directory.Delete("c:\\windows\\temp\\lucene");
        }

        [Fact]
        public void Can_find_item_lucene()
        {
            var directory = "e:\\temp\\lucene";
            var scope = "default";
            var queryBuilder = new LuceneSearchQueryBuilder();
            var conn = new SearchConnection(directory, scope);
            var provider = new LuceneSearchProvider(queryBuilder, conn);


            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

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

            Assert.True(results.DocCount == 1);

            Directory.Delete(directory, true);
        }

        [Fact]
        public void Can_get_item_facets_lucene()
        {
            var directory = "e:\\temp\\lucene";
            var scope = "default";
            var queryBuilder = new LuceneSearchQueryBuilder();
            var conn = new SearchConnection(directory, scope);
            var provider = new LuceneSearchProvider(queryBuilder, conn);

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            SearchHelper.CreateSampleIndex(provider, scope);

            var criteria = new CatalogItemSearchCriteria
            {
                SearchPhrase = "",
                IsFuzzySearch = true,
                Catalog = "goods",
                RecordsToRetrieve = 10,
                StartingRecord = 0,
                Currency = "USD",
                Pricelists = new [] { "default" }
            };

            var filter = new AttributeFilter { Key = "Color" };
            filter.Values = new []
                                {
                                    new AttributeFilterValue{Id = "red",Value = "red"},new AttributeFilterValue{Id = "blue",Value = "blue"},new AttributeFilterValue{Id = "black",Value = "black"}
                                };

            var rangefilter = new RangeFilter { Key = "size" };
            rangefilter.Values = new []
                                     {
                                         new RangeFilterValue { Id = "0_to_5", Lower = "0", Upper = "5" },
                                         new RangeFilterValue { Id = "5_to_10", Lower = "5", Upper = "10" }
                                     };

            var priceRangefilter = new PriceRangeFilter { Currency = "usd" };
            priceRangefilter.Values = new []
                                     {
                                         new RangeFilterValue { Id = "0_to_100", Lower = "0", Upper = "100" },
                                         new RangeFilterValue { Id = "100_to_700", Lower = "100", Upper = "700" }
                                     };

            criteria.Add(filter);
            criteria.Add(rangefilter);
            criteria.Add(priceRangefilter);

            var results = provider.Search(scope, criteria);

            Assert.True(results.DocCount == 1);

            Directory.Delete(directory, true);
        }

    }
}
