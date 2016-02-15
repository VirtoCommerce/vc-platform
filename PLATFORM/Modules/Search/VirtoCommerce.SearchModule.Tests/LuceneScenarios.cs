using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.SearchModule.Data.Model;
using VirtoCommerce.SearchModule.Data.Providers.Lucene;
using Xunit;

namespace VirtoCommerce.SearchModule.Tests
{
    public class LuceneScenarios
    {
        private string _LuceneStorageDir = Path.Combine(Path.GetTempPath(), "lucene");

        [Fact, Trait("type", "lucene"), Trait("Category", "CI")]
        public void Can_create_lucene_index()
        {
            var scope = "default";
            var queryBuilder = new LuceneSearchQueryBuilder();
            var conn = new SearchConnection(_LuceneStorageDir, scope);
            var provider = new LuceneSearchProvider(queryBuilder, conn);
            SearchHelper.CreateSampleIndex(provider, scope);
            Directory.Delete(_LuceneStorageDir, true);
        }

        [Fact, Trait("type", "lucene"), Trait("Category", "CI")]
        public void Can_find_item_lucene()
        {
            var scope = "default";
            var queryBuilder = new LuceneSearchQueryBuilder();
            var conn = new SearchConnection(_LuceneStorageDir, scope);
            var provider = new LuceneSearchProvider(queryBuilder, conn);

            if (Directory.Exists(_LuceneStorageDir))
            {
                Directory.Delete(_LuceneStorageDir, true);
            }

            SearchHelper.CreateSampleIndex(provider, scope);

            var criteria = new CatalogIndexedSearchCriteria
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

            criteria = new CatalogIndexedSearchCriteria
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

            Directory.Delete(_LuceneStorageDir, true);
        }

        [Fact, Trait("type", "lucene"), Trait("Category", "CI")]
        public void Can_get_item_facets_lucene()
        {
            var scope = "default";
            var queryBuilder = new LuceneSearchQueryBuilder();
            var conn = new SearchConnection(_LuceneStorageDir, scope);
            var provider = new LuceneSearchProvider(queryBuilder, conn);
            Debug.WriteLine("Lucene connection: {0}", conn.ToString());

            if (Directory.Exists(_LuceneStorageDir))
            {
                Directory.Delete(_LuceneStorageDir, true);
            }

            SearchHelper.CreateSampleIndex(provider, scope);

            var criteria = new CatalogIndexedSearchCriteria
            {
                SearchPhrase = "",
                IsFuzzySearch = true,
                Catalog = "goods",
                RecordsToRetrieve = 10,
                StartingRecord = 0,
                Currency = "USD",
                Pricelists = new[] { "default" }
            };

            var filter = new AttributeFilter { Key = "Color" };
            filter.Values = new[]
                                {
                                    new AttributeFilterValue { Id = "red", Value = "red" },
                                    new AttributeFilterValue { Id = "blue", Value = "blue" },
                                    new AttributeFilterValue { Id = "black", Value = "black" }
                                };

            var rangefilter = new RangeFilter { Key = "size" };
            rangefilter.Values = new[]
                                     {
                                         new RangeFilterValue { Id = "0_to_5", Lower = "0", Upper = "5" },
                                         new RangeFilterValue { Id = "5_to_10", Lower = "5", Upper = "10" }
                                     };

            var priceRangefilter = new PriceRangeFilter { Currency = "usd" };
            priceRangefilter.Values = new[]
                                          {
                                              new RangeFilterValue { Id = "0_to_100", Lower = "0", Upper = "100" },
                                              new RangeFilterValue { Id = "100_to_700", Lower = "100", Upper = "700" }
                                          };

            criteria.Add(filter);
            criteria.Add(rangefilter);
            criteria.Add(priceRangefilter);

            var results = provider.Search(scope, criteria);

            Assert.True(results.DocCount == 4, String.Format("Returns {0} instead of 4", results.DocCount));

            var redCount = GetFacetCount(results, "Color", "red");
            Assert.True(redCount == 2, String.Format("Returns {0} facets of red instead of 2", redCount));

            var priceCount = GetFacetCount(results, "Price", "0_to_100");
            Assert.True(priceCount == 2, String.Format("Returns {0} facets of 0_to_100 prices instead of 2", priceCount));

            var priceCount2 = GetFacetCount(results, "Price", "100_to_700");
            Assert.True(priceCount2 == 2, String.Format("Returns {0} facets of 100_to_700 prices instead of 2", priceCount2));

            var sizeCount = GetFacetCount(results, "size", "0_to_5");
            Assert.True(sizeCount == 2, String.Format("Returns {0} facets of 0_to_5 size instead of 2", sizeCount));

            var sizeCount2 = GetFacetCount(results, "size", "5_to_10");
            Assert.True(sizeCount2 == 1, String.Format("Returns {0} facets of 5_to_10 size instead of 1", sizeCount2)); // only 1 result because upper bound is not included

            var outlineCount = results.Documents[0].Documents[0]["__outline"].Values.Count();
            Assert.True(outlineCount == 2, String.Format("Returns {0} outlines instead of 2", outlineCount));

            Directory.Delete(_LuceneStorageDir, true);
        }

        [Fact, Trait("type", "lucene"), Trait("Category", "CI")]
        public void Can_get_item_multiple_filters_lucene()
        {
            var scope = "default";
            var queryBuilder = new LuceneSearchQueryBuilder();
            var conn = new SearchConnection(_LuceneStorageDir, scope);
            var provider = new LuceneSearchProvider(queryBuilder, conn);
            Debug.WriteLine("Lucene connection: {0}", conn.ToString());

            if (Directory.Exists(_LuceneStorageDir))
            {
                Directory.Delete(_LuceneStorageDir, true);
            }

            SearchHelper.CreateSampleIndex(provider, scope);

            var criteria = new CatalogIndexedSearchCriteria
            {
                SearchPhrase = "",
                IsFuzzySearch = true,
                Catalog = "goods",
                RecordsToRetrieve = 10,
                StartingRecord = 0,
                Currency = "USD",
                Pricelists = new[] { "default" }
            };

            var colorFilter = new AttributeFilter { Key = "Color" };
            colorFilter.Values = new[]
                                {
                                    new AttributeFilterValue { Id = "red", Value = "red" },
                                    new AttributeFilterValue { Id = "blue", Value = "blue" },
                                    new AttributeFilterValue { Id = "black", Value = "black" }
                                };

            var filter = new AttributeFilter { Key = "Color" };
            filter.Values = new[]
                                {
                                    new AttributeFilterValue { Id = "black", Value = "black" }
                                };

            var rangefilter = new RangeFilter { Key = "size" };
            rangefilter.Values = new[]
                                     {
                                         new RangeFilterValue { Id = "0_to_5", Lower = "0", Upper = "5" },
                                         new RangeFilterValue { Id = "5_to_10", Lower = "5", Upper = "11" }
                                     };

            var priceRangefilter = new PriceRangeFilter { Currency = "usd" };
            priceRangefilter.Values = new[]
                                          {
                                              new RangeFilterValue { Id = "100_to_700", Lower = "100", Upper = "700" }
                                          };

            criteria.Add(colorFilter);
            criteria.Add(priceRangefilter);

            // add applied filters
            criteria.Apply(filter);
            //criteria.Apply(rangefilter);
            //criteria.Apply(priceRangefilter);

            var results = provider.Search(scope, criteria);

            var blackCount = GetFacetCount(results, "Color", "black");
            Assert.True(blackCount == 1, String.Format("Returns {0} facets of black instead of 2", blackCount));

            //Assert.True(results.DocCount == 1, String.Format("Returns {0} instead of 1", results.DocCount));

            Directory.Delete(_LuceneStorageDir, true);
        }

        private int GetFacetCount(ISearchResults results, string fieldName, string facetKey)
        {
            if (results.FacetGroups == null || results.FacetGroups.Length == 0)
            {
                return 0;
            }

            var group = (from fg in results.FacetGroups where fg.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase) select fg).SingleOrDefault();

            return @group == null ? 0 : (from facet in @group.Facets where facet.Key == facetKey select facet.Count).FirstOrDefault();
        }
    }
}