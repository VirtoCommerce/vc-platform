using System;
using System.Linq;
using System.Text.RegularExpressions;
using PlainElastic.Net;
using VirtoCommerce.Foundation.Search;
using PlainElastic.Net.Queries;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.Foundation.Catalogs.Search;
using System.Collections.Specialized;

namespace VirtoCommerce.Search.Providers.Elastic
{
    public class ElasticSearchQueryBuilder : ISearchQueryBuilder
    {
        #region ISearchQueryBuilder Members
        public object BuildQuery(ISearchCriteria criteria)
        {
            var query = new BoolQuery<ESDocument>();

            #region Filters
            if (criteria.CurrentFilterValues != null)
            {
                for (var index = 0; index < criteria.CurrentFilterFields.Length; index++)
                {
                    var filter = criteria.CurrentFilters.ElementAt(index);
                    var value = criteria.CurrentFilterValues.ElementAt(index);
                    var field = criteria.CurrentFilterFields.ElementAt(index).ToLower();

                    // Skip currencies that are not part of the filter
                    if (filter.GetType() == typeof(PriceRangeFilter)) // special filtering 
                    {
	                    var priceRangeFilter = filter as PriceRangeFilter;
	                    if (priceRangeFilter != null)
	                    {
		                    var currency = priceRangeFilter.Currency;
		                    if (!currency.Equals(criteria.Currency, StringComparison.OrdinalIgnoreCase))
			                    continue;
	                    }
                    }

	                if (filter.GetType() == typeof(PriceRangeFilter))
                    {
                        var tempQuery = ElasticQueryHelper.CreatePriceRangeFilter(criteria, field, value as RangeFilterValue);
                        if (tempQuery != null)
                        {
                            query.Must(q => q.ConstantScore(c => c.Filter(f => f.Bool(b=>tempQuery))));
                        }
                    }
                    else
                    {
                        if (value.GetType() == typeof(AttributeFilterValue))
                        {
                            query.Must(q => q.Match(t => t.Field(field).Query(((AttributeFilterValue)value).Value)));
                        }
                        else if (value.GetType() == typeof(RangeFilterValue))
                        {
                            var tempValue = value as RangeFilterValue;
                            var tempFilter = new RangeFilter<ESDocument>();
                            tempFilter.Field(field).From(tempValue.Lower).To(tempValue.Upper).IncludeLower(true).IncludeUpper(false);
                            query.Should(q => q.ConstantScore(c => c.Filter(f => f.Range(r => tempFilter))));
                        }
                    }
                }
            }
            #endregion

            #region CatalogItemSearchCriteria
            if (criteria is CatalogItemSearchCriteria)
            {
                var c = criteria as CatalogItemSearchCriteria;

                query.Must(m => m
                    .Range(r => r.Field("startdate").To(c.StartDate.ToString("s")))
					);


                if (c.StartDateFrom.HasValue)
                {
                    query.Must(m => m
                        .Range(r => r.Field("startdate").From(c.StartDateFrom.Value.ToString("s")))
                   );
                }

				if (c.EndDate.HasValue)
				{
					query.Must(m => m
						.Range(r => r.Field("enddate").From(c.EndDate.Value.ToString("s")))
				   );
				}

				query.Must(m => m.Term(t => t.Field("__hidden").Value("false")));

                if (c.Outlines != null && c.Outlines.Count > 0)
                    AddQuery("__outline", query, c.Outlines);

                if (!String.IsNullOrEmpty(c.SearchPhrase))
                {
					AddQueryString("__content", query, c);
                }

				if (!String.IsNullOrEmpty(c.Catalog))
				{
					AddQuery("catalog", query, c.Catalog);
				}
            }
            #endregion

            if (criteria is ElasticSearchCriteria)
            {
                var c = criteria as ElasticSearchCriteria;
                query.Must(m => m.Custom(c.RawQuery));
            }

            return query;
        }
        #endregion

        protected void AddQuery(string fieldName, BoolQuery<ESDocument> query, StringCollection filter)
        {
            fieldName = fieldName.ToLower();
            if (filter.Count > 0)
            {
                if (filter.Count == 1)
                {
                    if (!String.IsNullOrEmpty(filter[0]))
                    {
                        AddQuery(fieldName, query, filter[0].ToLower());
                    }
                }
                else
                {
                    var booleanQuery = new BoolQuery<ESDocument>();
                    var containsFilter = false;
                    foreach (var index in filter.Cast<string>().Where(index => !String.IsNullOrEmpty(index)))
                    {
	                    booleanQuery.Should(q => q.Custom("{{\"wildcard\" : {{ \"{0}\" : \"{1}\" }}}}", fieldName.ToLower(), index.ToLower()));
	                    containsFilter = true;
                    }
                    if (containsFilter)
                        query.Must(q => q.Bool(b => booleanQuery));
                }
            }
        }

        protected void AddQuery(string fieldName, BoolQuery<ESDocument> query, string filter)
        {
            query.Must(q => q.Custom("{{\"wildcard\" : {{ \"{0}\" : \"{1}\" }}}}", fieldName.ToLower(), filter.ToLower()));
        }

		protected void AddQueryString(string fieldName, BoolQuery<ESDocument> query, CatalogItemSearchCriteria filter)
		{
			var searchPhrase = filter.SearchPhrase;
            /*
			if (filter.IsFuzzySearch)
			{
				var keywords = Regex.Split(searchPhrase, @"\s+");
				searchPhrase = string.Empty;
				searchPhrase = keywords.Aggregate(searchPhrase, (current, keyword) => 
					current + String.Format("{0}~{1}", keyword, filter.FuzzyMinSimilarity));
			}
             * */

			//query.Must(q => q.QueryString(t => t.DefaultField(fieldName).DefaultOperator(Operator.AND).Query(searchPhrase)));
            query.Must(q => q.Match(x => x.Field(fieldName).Operator(Operator.AND).Fuzziness(filter.FuzzyMinSimilarity).Query(searchPhrase)));
		}
    }
}
