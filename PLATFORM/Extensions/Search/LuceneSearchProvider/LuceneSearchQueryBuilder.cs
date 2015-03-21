#region

using System.Globalization;
using u = Lucene.Net.Util;

#endregion

namespace VirtoCommerce.Search.Providers.Lucene
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using global::Lucene.Net.Analysis.Standard;

    using global::Lucene.Net.Documents;

    using global::Lucene.Net.Index;

    using global::Lucene.Net.QueryParsers;

    using global::Lucene.Net.Search;

    using VirtoCommerce.Foundation.Catalogs.Search;
    using VirtoCommerce.Foundation.Orders.Search;
    using VirtoCommerce.Foundation.Search;

    #endregion

    public class LuceneSearchQueryBuilder : BaseSearchQueryBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the query.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public override object BuildQuery(ISearchCriteria criteria)
        {

            var builder = base.BuildQuery(criteria) as QueryBuilder;
            var query = builder.Query as BooleanQuery;
            var analyzer = new StandardAnalyzer(u.Version.LUCENE_30);

            if (criteria is CatalogItemSearchCriteria)
            {
                var c = criteria as CatalogItemSearchCriteria;
                var datesFilterStart = new TermRangeQuery(
                    "startdate", c.StartDateFrom.HasValue ? DateTools.DateToString(c.StartDateFrom.Value, DateTools.Resolution.SECOND) : null, DateTools.DateToString(c.StartDate, DateTools.Resolution.SECOND), false, true);
                query.Add(datesFilterStart, Occur.MUST);

                if (c.EndDate.HasValue)
                {
                    var datesFilterEnd = new TermRangeQuery(
                        "enddate",
                        DateTools.DateToString(c.EndDate.Value, DateTools.Resolution.SECOND),
                        null,
                        true,
                        false);

                    query.Add(datesFilterEnd, Occur.MUST);
                }

                if (c.Outlines != null && c.Outlines.Count > 0)
                {
                    AddQuery("__outline", query, c.Outlines);
                }

                query.Add(new TermQuery(new Term("__hidden", "false")), Occur.MUST);

                if (!String.IsNullOrEmpty(c.Catalog))
                {
                    AddQuery("catalog", query, c.Catalog);
                }

                // Add search
                if (!String.IsNullOrEmpty(c.SearchPhrase))
                {
                    var searchPhrase = c.SearchPhrase;
                    if (c.IsFuzzySearch)
                    {

                        var keywords = c.SearchPhrase.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        searchPhrase = string.Empty;
                        searchPhrase = keywords.Aggregate(
                            searchPhrase,
                            (current, keyword) =>
                                current + String.Format("{0}~{1}", keyword.Replace("~", ""), c.FuzzyMinSimilarity.ToString(CultureInfo.InvariantCulture)));
                    }

                    var fields = new List<string> { "__content" };
                    if (c.Locale != null)
                    {
                        var contentField = string.Format("__content_{0}", c.Locale.ToLower());    
                        fields.Add(contentField);
                    }
                    
                    var parser = new MultiFieldQueryParser(u.Version.LUCENE_30, fields.ToArray(), analyzer)
                                     {
                                         DefaultOperator =
                                             QueryParser
                                             .Operator.OR
                                     };

                    var searchQuery = parser.Parse(searchPhrase);
                    query.Add(searchQuery, Occur.MUST);

                }
            }
            else if (criteria is OrderSearchCriteria)
            {
                var c = criteria as OrderSearchCriteria;

                if (!String.IsNullOrEmpty(c.CustomerId))
                {
                    AddQuery("customerid", query, c.CustomerId);
                }
            }

            return builder;
        }

        #endregion
    }

    public class QueryBuilder
    {
        public Query Query { get; set; }
        public Filter Filter { get; set; }
    }
}