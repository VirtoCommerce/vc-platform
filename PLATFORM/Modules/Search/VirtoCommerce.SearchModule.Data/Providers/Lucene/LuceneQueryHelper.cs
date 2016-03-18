﻿using System.Globalization;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.SearchModule.Data.Services;

namespace VirtoCommerce.SearchModule.Data.Providers.Lucene
{
    public class LuceneQueryHelper
    {
        /// <summary>
        /// Converts to searchable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="tryConvertToNumber">if set to <c>true</c> [try convert to number].</param>
        /// <returns></returns>
        public static string ConvertToSearchable(object value, bool tryConvertToNumber = true)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return string.Empty;
            }

            if (tryConvertToNumber)
            {
                decimal decimalVal;
                int intVal;

                // Try converting to a known type
                if (decimal.TryParse(value.ToString(), out decimalVal))
                {
                    value = decimalVal;
                }
                else if (int.TryParse(value.ToString(), out intVal))
                {
                    value = intVal;
                }
            }

            if (value is string)
            {
                return value.ToString();//.ToLower();//.Replace("-", @"\-");
            }

            if (value is decimal)
            {
                return NumericUtils.DoubleToPrefixCoded(double.Parse(value.ToString()));
            }

            if (value.GetType() != typeof(int) || value.GetType() != typeof(long) || value.GetType() != typeof(double))
            {
                return value.ToString();
            }

            return NumericUtils.DoubleToPrefixCoded((double)value);
        }

        public static Filter CreateQuery(ISearchCriteria criteria, ISearchFilter filter, Occur clause)
        {
            var values = filter.GetValues();
            if (values == null)
                return null;

            var query = new BooleanFilter();
            foreach (var value in values)
            {
                var valueQuery = CreateQueryForValue(criteria, filter, value);
                query.Add(new FilterClause(valueQuery, Occur.SHOULD));
            }

            return query;
        }

        public static Filter CreateQueryForValue(ISearchCriteria criteria, ISearchFilter filter, ISearchFilterValue value)
        {
            Filter q;
            var priceQuery = filter is PriceRangeFilter;

            var rangeFilterValue = value as RangeFilterValue;
            if (rangeFilterValue != null && priceQuery)
            {
                q = CreateQuery(criteria, filter.Key, rangeFilterValue);
            }
            else if (value is CategoryFilterValue)
            {
                q = CreateQuery(filter.Key, (CategoryFilterValue)value);
            }
            else
            {
                q = CreateQuery(filter.Key, value);
            }

            return q;
        }

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Filter CreateQuery(string field, ISearchFilterValue value)
        {
            field = field.ToLower();

            var attributeFilterValue = value as AttributeFilterValue;
            if (attributeFilterValue != null)
            {
                return CreateTermsFilter(field, attributeFilterValue);
            }

            var rangeFilterValue = value as RangeFilterValue;
            if (rangeFilterValue != null)
            {
                return CreateTermRangeFilter(field, rangeFilterValue);
            }

            return null;
        }

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Filter CreateTermsFilter(string field, AttributeFilterValue value)
        {
            object val = value.Id;
            var query = new TermsFilter();
            query.AddTerm(new Term(field, ConvertToSearchable(val, false)));
            return query;
        }

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Filter CreateTermRangeFilter(string field, RangeFilterValue value)
        {
            object lowerbound = value.Lower;
            object upperbound = value.Upper;

            var query = new TermRangeFilter(field, ConvertToSearchable(lowerbound), ConvertToSearchable(upperbound), true, false);
            return query;
        }

        /// <summary>
        /// Creates the query.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Filter CreateQuery(string field, CategoryFilterValue value)
        {
            var query = new BooleanFilter();
            if (!string.IsNullOrEmpty(value.Outline))
            {
                // workaround since there is no wildcard filter in current lucene version
                var outline = value.Outline.TrimEnd('*');
                var nodeQuery = new PrefixFilter(new Term(field, outline.ToLower()));
                query.Add(new FilterClause(nodeQuery, Occur.MUST));
            }
            return query;
        }

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <param name="criteria">The search criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Filter CreateQuery(ISearchCriteria criteria, string field, RangeFilterValue value)
        {
            var query = new BooleanFilter();

            object lowerbound = value.Lower;
            object upperbound = value.Upper;

            const bool lowerboundincluded = true;
            const bool upperboundincluded = false;

            var currency = criteria.Currency.ToLower();

            var upper = upperbound == null ? NumericUtils.LongToPrefixCoded(long.MaxValue) : ConvertToSearchable(upperbound);

            // format is "fieldname_store_currency_pricelist"
            string[] pls = null;
            var searchCriteria = criteria as CatalogIndexedSearchCriteria;
            if (searchCriteria != null)
            {
                pls = searchCriteria.Pricelists;
            }

            // Create  filter of type 
            // price_USD_pricelist1:[100 TO 200} (-price_USD_pricelist1:[* TO *} +(price_USD_pricelist2:[100 TO 200} (-price_USD_pricelist2:[* TO *} (+price_USD_pricelist3[100 TO 200}))))

            if (pls == null || !pls.Any())
            {
                return null;
            }

            var priceListId = pls[0].ToLower();

            var filter = new TermRangeFilter(
                string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", field, currency, priceListId),
                ConvertToSearchable(lowerbound),
                upper,
                lowerboundincluded,
                upperboundincluded);

            query.Add(new FilterClause(filter, Occur.SHOULD));

            if (pls.Length > 1)
            {
                var q = CreatePriceRangeQuery(
                    pls,
                    1,
                    field,
                    currency,
                    ConvertToSearchable(lowerbound),
                    upper,
                    lowerboundincluded,
                    upperboundincluded);
                query.Add(new FilterClause(q, Occur.SHOULD));
            }

            return query;
        }


        /// <summary>
        ///     Creates the price range query.
        /// </summary>
        /// <param name="priceLists">The price lists.</param>
        /// <param name="index">The index.</param>
        /// <param name="field">The field.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="lowerbound">The lowerbound.</param>
        /// <param name="upperbound">The upperbound.</param>
        /// <param name="lowerboundincluded">
        ///     if set to <c>true</c> [lowerboundincluded].
        /// </param>
        /// <param name="upperboundincluded">
        ///     if set to <c>true</c> [upperboundincluded].
        /// </param>
        /// <returns></returns>
        private static BooleanFilter CreatePriceRangeQuery(
            string[] priceLists,
            int index,
            string field,
            string currency,
            string lowerbound,
            string upperbound,
            bool lowerboundincluded,
            bool upperboundincluded)
        {
            var query = new BooleanFilter();

            // create left part
            var filter =
                new TermRangeFilter(
                    string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", field, currency, priceLists[index - 1].ToLower()),
                    "*",
                    "*",
                    true,
                    false);
            var leftClause = new FilterClause(filter, Occur.MUST_NOT);
            query.Add(leftClause);

            // create right part
            if (index == priceLists.Length) // last element
            {
                //var rangefilter = NumericRangeFilter.;
                var filter2 =
                    new TermRangeFilter(
                        string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", field, currency, priceLists[index - 1].ToLower()),
                        lowerbound,
                        upperbound,
                        lowerboundincluded,
                        upperboundincluded);
                var rightClause = new FilterClause(filter2, Occur.MUST);
                query.Add(rightClause);
            }
            else
            {
                query.Add(new FilterClause(
                    CreatePriceRangeQuery(
                        priceLists,
                        index + 1,
                        field,
                        currency,
                        lowerbound,
                        upperbound,
                        lowerboundincluded,
                        upperboundincluded),
                    Occur.SHOULD));
            }

            return query;
        }

    }
}