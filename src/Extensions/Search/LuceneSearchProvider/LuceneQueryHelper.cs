
namespace VirtoCommerce.Search.Providers.Lucene
{
    #region

    using System;
    using System.Linq;

    using global::Lucene.Net.Documents;
    using global::Lucene.Net.Index;

    using VirtoCommerce.Foundation.Catalogs.Search;
    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Foundation.Search.Schemas;

    using global::Lucene.Net.Search;

    #endregion

    public class LuceneQueryHelper
    {
        #region Public Methods and Operators
        /// <summary>
        ///     Converts the decimal to sortable.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ConvertDecimalToSortable(decimal input)
        {
            var field = new NumericField("");
            field.SetFloatValue((float)input);
            return field.StringValue;
        }

        /// <summary>
        ///     Converts to searchable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertToSearchable(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return String.Empty;
            }

            decimal decimalVal = 0;
            var intVal = 0;

            // Try converting to a known type
            if (Decimal.TryParse(value.ToString(), out decimalVal))
            {
                value = decimalVal;
            }
            else if (Int32.TryParse(value.ToString(), out intVal))
            {
                value = intVal;
            }

            if (value is string)
            {
                return value.ToString();
            }

            if (value is decimal)
            {
                return ConvertDecimalToSortable((decimal)value);
            }

            if (value.GetType() != typeof(int) || value.GetType() != typeof(long) || value.GetType() != typeof(double))
            {
                return value.ToString();
            }

            var field = new NumericField("");
            field.SetDoubleValue(long.Parse(value.ToString()));
            return field.StringValue;
        }

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Query CreateQuery(string field, ISearchFilterValue value)
        {
            field = field.ToLower();

            if (value.GetType() == typeof(AttributeFilterValue))
            {
                return CreateQuery(field, value as AttributeFilterValue);
            }
            if (value.GetType() == typeof(RangeFilterValue))
            {
                return CreateQuery(field, value as RangeFilterValue);
            }

            return null;
        }

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Query CreateQuery(string field, RangeFilterValue value)
        {
            object lowerbound = value.Lower;
            object upperbound = value.Upper;

            var query = new TermRangeQuery(
                field, ConvertToSearchable(lowerbound), ConvertToSearchable(upperbound), true, false);
            return query;
        }

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Query CreateQuery(ISearchCriteria criteria, string field, RangeFilterValue value)
        {
            var query = new BooleanQuery();

            object lowerbound = value.Lower;
            object upperbound = value.Upper;

            const bool lowerboundincluded = true;
            const bool upperboundincluded = false;

            var currency = criteria.Currency.ToLower();

            var upper = upperbound == null ? String.Empty : upperbound.ToString();
            if (String.IsNullOrEmpty(upper))
            {
                upper = NumberTools.MAX_STRING_VALUE;
            }
            else
            {
                upper = ConvertToSearchable(upperbound);
            }

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
            {
                return null;
            }

            var priceListId = pls[0].ToLower();

            var filter = new TermRangeFilter(
                String.Format("{0}_{1}_{2}", field, currency, priceListId),
                ConvertToSearchable(lowerbound),
                upper,
                lowerboundincluded,
                upperboundincluded);

            query.Add(new ConstantScoreQuery(filter), Occur.SHOULD);

            if (pls.Count() > 1)
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
                query.Add(q, Occur.SHOULD);
            }

            return query;
        }

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Query CreateQuery(string field, AttributeFilterValue value)
        {
            object val = value.Value;
            var query = new TermQuery(new Term(field, ConvertToSearchable(val)));
            return query;
        }

        #endregion

        #region Methods

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
        private static BooleanQuery CreatePriceRangeQuery(
            string[] priceLists,
            int index,
            string field,
            string currency,
            string lowerbound,
            string upperbound,
            bool lowerboundincluded,
            bool upperboundincluded)
        {
            var query = new BooleanQuery();

            // create left part
            var filter =
                new TermRangeFilter(
                    String.Format("{0}_{1}_{2}", field, currency, priceLists[index - 1].ToLower()),
                    "*",
                    "*",
                    true,
                    false);
            var leftClause = new BooleanClause(new ConstantScoreQuery(filter), Occur.MUST_NOT);
            query.Add(leftClause);

            // create right part
            if (index == priceLists.Count()) // last element
            {
                //var rangefilter = NumericRangeFilter.;
                var filter2 =
                    new TermRangeFilter(
                        String.Format("{0}_{1}_{2}", field, currency, priceLists[index - 1].ToLower()),
                        lowerbound,
                        upperbound,
                        lowerboundincluded,
                        upperboundincluded);
                var rightClause = new BooleanClause(new ConstantScoreQuery(filter2), Occur.MUST);
                query.Add(rightClause);
            }
            else
            {
                query.Add(
                    CreatePriceRangeQuery(
                        priceLists,
                        index + 1,
                        field,
                        currency,
                        lowerbound,
                        upperbound,
                        lowerboundincluded,
                        upperboundincluded),
                    Occur.SHOULD);
            }

            return query;
        }

        #endregion
    }
}