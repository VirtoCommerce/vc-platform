using System;
using System.Linq;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Schemas;
using PlainElastic.Net.Queries;
using VirtoCommerce.Foundation.Catalogs.Search;

namespace VirtoCommerce.Search.Providers.Elastic
{
    public class ElasticQueryHelper
    {
        public static BoolFilter<ESDocument> CreateQuery(ISearchCriteria criteria, ISearchFilter filter)
        {
            var values = GetFilterValues(filter);
            if (values == null) return null;

            var query = new BoolFilter<ESDocument>();
            foreach (var value in values)
            {
                var valueQuery = CreateQueryForValue(criteria, filter, value);
                //var boolQuery = new Query<ESDocument>();
                //boolQuery.Bool(x => valueQuery);
                query.Should(x=>x.Bool(y=>valueQuery));
            }

            return query;
        }

        public static BoolFilter<ESDocument> CreateQueryForValue(ISearchCriteria criteria, ISearchFilter filter, ISearchFilterValue value)
        {
            var query = new BoolFilter<ESDocument>();
            var field = filter.Key.ToLower();
            if (filter.GetType() == typeof(PriceRangeFilter))
            {
                var tempQuery = CreatePriceRangeFilter(criteria, field, value as RangeFilterValue);
                if (tempQuery != null)
                {
                    query.Must(q => q.Bool(b => tempQuery));
                }
            }
            else
            {
                if (value.GetType() == typeof(AttributeFilterValue))
                {
                    query.Must(q => q.Term(t=>t.Field(field).Value(((AttributeFilterValue)value).Value)));
                }
                else if (value.GetType() == typeof(RangeFilterValue))
                {
                    var tempValue = value as RangeFilterValue;
                    var tempFilter = new RangeFilter<ESDocument>();
                    tempFilter.Field(field).From(tempValue.Lower).To(tempValue.Upper).IncludeLower(true).IncludeUpper(false);
                    query.Should(q => q.Range(r => tempFilter));
                }
            }

            return query;
        }

        public static ISearchFilterValue[] GetFilterValues(ISearchFilter filter)
        {
            ISearchFilterValue[] values = null;
            if (filter is AttributeFilter)
            {
                values = ((AttributeFilter)filter).Values;
            }
            else if (filter is RangeFilter)
            {
                values = ((RangeFilter)filter).Values;
            }
            else if (filter is PriceRangeFilter)
            {
                values = ((PriceRangeFilter)filter).Values;
            }
            else if (filter is CategoryFilter)
            {
                values = ((CategoryFilter)filter).Values;
            }

            return values;
        }

        /// <summary>
        /// Creates the price range filter.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static BoolFilter<ESDocument> CreatePriceRangeFilter(ISearchCriteria criteria, string field, RangeFilterValue value)
        {
            var query = new BoolFilter<ESDocument>();

            var lowerbound = value.Lower;
            var upperbound = value.Upper;

            var lowerboundincluded = true;
            var upperboundincluded = false;

            var currency = criteria.Currency.ToLower();

            // format is "fieldname_store_currency_pricelist"
            string[] pls = null;
            if (criteria is CatalogItemSearchCriteria)
            {
                pls = ((CatalogItemSearchCriteria)criteria).Pricelists;
            }

            var parentPriceList = String.Empty;

            // Create  filter of type 
            // price_USD_pricelist1:[100 TO 200} (-price_USD_pricelist1:[* TO *} +(price_USD_pricelist2:[100 TO 200} (-price_USD_pricelist2:[* TO *} (+price_USD_pricelist3[100 TO 200}))))

            if (pls == null || !pls.Any())
                return null;

            var priceListId = pls[0].ToLower();

            var filter = new RangeFilter<ESDocument>();
            filter.Field(String.Format("{0}_{1}_{2}", field, currency, priceListId)).From(lowerbound).To(upperbound).IncludeLower(lowerboundincluded).IncludeUpper(upperboundincluded);

            //query.Should(q => q.ConstantScore(c => c.Filter(f => f.Range(r => filter))));
            query.Should(q => q.Range(r => filter));

            if (pls.Count() > 1)
            {
                var temp = CreatePriceRangeFilter(pls, 1, field, currency, lowerbound, upperbound, lowerboundincluded, upperboundincluded);
                query.Should(q => q.Bool(b => temp));
            }

            //Query query = new ConstantScoreQuery(filter);
            return query;
        }

        private static BoolFilter<ESDocument> CreatePriceRangeFilter(string[] priceLists, int index, string field, string currency, string lowerbound, string upperbound, bool lowerboundincluded, bool upperboundincluded)
        {
            var query = new BoolFilter<ESDocument>();

            // create left part
            var filter = new RangeFilter<ESDocument>();
            filter.Field(String.Format("{0}_{1}_{2}", field, currency, priceLists[index - 1].ToLower()))/*.From("*").To("*")*/.IncludeLower(lowerboundincluded).IncludeUpper(upperboundincluded);
            //query.MustNot(q => q.ConstantScore(c => c.Filter(f => f.Range(r => filter))));
            query.MustNot(q => q.Range(r => filter));

            // create right part
            if (index == priceLists.Count() - 1) // last element
            {
                var filter2 = new RangeFilter<ESDocument>();
                filter2.Field(String.Format("{0}_{1}_{2}", field, currency, priceLists[index].ToLower())).From(lowerbound).To(upperbound).IncludeLower(lowerboundincluded).IncludeUpper(upperboundincluded);
                //query.Must(q => q.ConstantScore(c => c.Filter(f => f.Range(r => filter2))));
                query.Must(q => q.Range(r => filter2));
            }
            else
            {
                query.Should(q => q.Bool(b => CreatePriceRangeFilter(priceLists, index + 1, field, currency, lowerbound, upperbound, lowerboundincluded, upperboundincluded)));
            }

            return query;
        }
    }
}
