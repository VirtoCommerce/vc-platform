using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Schemas;
using PlainElastic.Net.Queries;
using VirtoCommerce.Foundation.Catalogs.Search;

namespace VirtoCommerce.Search.Providers.Elastic
{
    public class ElasticQueryHelper
    {
        /// <summary>
        /// Creates the price range filter.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static BoolFilter<ESDocument> CreatePriceRangeFilter(ISearchCriteria criteria, string field, RangeFilterValue value)
        {
            BoolFilter<ESDocument> query = new BoolFilter<ESDocument>();

            string lowerbound = value.Lower;
            string upperbound = value.Upper;

            bool lowerboundincluded = true;
            bool upperboundincluded = false;

            string currency = criteria.Currency.ToLower();

            // format is "fieldname_store_currency_pricelist"
            string[] pls = null;
            if (criteria is CatalogItemSearchCriteria)
            {
                pls = ((CatalogItemSearchCriteria)criteria).Pricelists;
            }

            string parentPriceList = String.Empty;

            // Create  filter of type 
            // price_USD_pricelist1:[100 TO 200} (-price_USD_pricelist1:[* TO *} +(price_USD_pricelist2:[100 TO 200} (-price_USD_pricelist2:[* TO *} (+price_USD_pricelist3[100 TO 200}))))

            if (pls == null || pls.Count() == 0)
                return null;

            string priceListId = pls[0].ToLower();

            RangeFilter<ESDocument> filter = new RangeFilter<ESDocument>();
            filter.Field(String.Format("{0}_{1}_{2}", field, currency, priceListId)).From(lowerbound).To(upperbound).IncludeLower(lowerboundincluded).IncludeUpper(upperboundincluded);

            //query.Should(q => q.ConstantScore(c => c.Filter(f => f.Range(r => filter))));
            query.Should(q => q.Range(r => filter));

            if (pls.Count() > 1)
            {
                BoolFilter<ESDocument> temp = CreatePriceRangeFilter(pls, 1, field, currency, lowerbound, upperbound, lowerboundincluded, upperboundincluded);
                query.Should(q => q.Bool(b => temp));
            }

            //Query query = new ConstantScoreQuery(filter);
            return query;
        }

        private static BoolFilter<ESDocument> CreatePriceRangeFilter(string[] priceLists, int index, string field, string currency, string lowerbound, string upperbound, bool lowerboundincluded, bool upperboundincluded)
        {
            BoolFilter<ESDocument> query = new BoolFilter<ESDocument>();

            // create left part
            RangeFilter<ESDocument> filter = new RangeFilter<ESDocument>();
            filter.Field(String.Format("{0}_{1}_{2}", field, currency, priceLists[index - 1].ToLower()))/*.From("*").To("*")*/.IncludeLower(lowerboundincluded).IncludeUpper(upperboundincluded);
            //query.MustNot(q => q.ConstantScore(c => c.Filter(f => f.Range(r => filter))));
            query.MustNot(q => q.Range(r => filter));

            // create right part
            if (index == priceLists.Count() - 1) // last element
            {
                RangeFilter<ESDocument> filter2 = new RangeFilter<ESDocument>();
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
