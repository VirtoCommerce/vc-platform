#region

using u = Lucene.Net.Util;

#endregion

namespace VirtoCommerce.Search.Providers.Lucene
{
    #region

    using System;
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
            var query = base.BuildQuery(criteria) as BooleanQuery;
            var analyzer = new StandardAnalyzer(u.Version.LUCENE_30);

            if (criteria is CatalogItemSearchCriteria)
            {
                var c = criteria as CatalogItemSearchCriteria;
                var datesFilterStart = new TermRangeQuery(
                    "startdate", null, DateTools.DateToString(c.StartDate, DateTools.Resolution.SECOND), false, true);
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
                    if (c.IsFuzzySearch)
                    {

                        var keywords = c.SearchPhrase.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        //var keywords = Regex.Split(c.SearchPhrase, @"\s+");
                        var searchPhrase = string.Empty;
                        searchPhrase = keywords.Aggregate(
                            searchPhrase,
                            (current, keyword) => current + String.Format("{0}~{1}", keyword.Replace("~", ""), c.FuzzyMinSimilarity));

                        var parser = new QueryParser(u.Version.LUCENE_30, "__content", analyzer)
                                         {
                                             DefaultOperator =
                                                 QueryParser
                                                 .Operator.AND
                                         };

                        var searchQuery = parser.Parse(searchPhrase);
                        query.Add(searchQuery, Occur.MUST);
                    }
                    else
                    {
                        var parser = new QueryParser(u.Version.LUCENE_30, "__content", analyzer)
                                         {
                                             DefaultOperator =
                                                 QueryParser
                                                 .Operator.AND
                                         };
                        var searchQuery = parser.Parse(c.SearchPhrase);
                        query.Add(searchQuery, Occur.MUST);
                    }
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

            return query;
        }

        #endregion
    }
}