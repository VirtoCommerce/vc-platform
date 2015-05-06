using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;

namespace VirtoCommerce.SearchModule.Data.Providers.ElasticSearch
{
    public class ElasticSearchQueryBuilder : ISearchQueryBuilder
    {
        #region ISearchQueryBuilder Members
        public object BuildQuery(ISearchCriteria criteria)
        {
            var builder = new QueryBuilder<ESDocument>();
            var mainFilter = new Filter<ESDocument>();
            var mainQuery = new BoolQuery<ESDocument>();

            #region Sorting

            // Add sort order
            if (criteria.Sort != null)
            {
                var fields = criteria.Sort.GetSort();
                foreach (var field in fields)
                {
                    builder.Sort(d => d.Field(field.FieldName, field.IsDescending ? SortDirection.desc : SortDirection.asc, ignoreUnmapped: field.IgnoredUnmapped));
                }
            }

            #endregion

            #region Filters
            // Perform facet filters
            if (criteria.CurrentFilters != null && criteria.CurrentFilters.Any())
            {
                var combinedFilter = new BoolFilter<ESDocument>();
                // group filters
                foreach (var filter in criteria.CurrentFilters)
                {
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

                    var filterQuery = ElasticQueryHelper.CreateQuery(criteria, filter);

                    if (filterQuery != null)
                    {
                        combinedFilter.Must(c => c.Bool(q => filterQuery));
                    }
                }

                mainFilter.Bool(bl => combinedFilter);
            }
            #endregion

            #region CatalogItemSearchCriteria
            if (criteria is CatalogIndexedSearchCriteria)
            {
                var c = criteria as CatalogIndexedSearchCriteria;

                mainQuery.Must(m => m
                    .Range(r => r.Field("startdate").To(c.StartDate.ToString("s")))
                    );


                if (c.StartDateFrom.HasValue)
                {
                    mainQuery.Must(m => m
                        .Range(r => r.Field("startdate").From(c.StartDateFrom.Value.ToString("s")))
                   );
                }

                if (c.EndDate.HasValue)
                {
                    mainQuery.Must(m => m
                        .Range(r => r.Field("enddate").From(c.EndDate.Value.ToString("s")))
                   );
                }

                mainQuery.Must(m => m.Term(t => t.Field("__hidden").Value("false")));

                if (c.Outlines != null && c.Outlines.Count > 0)
                    AddQuery("__outline", mainQuery, c.Outlines);

                if (!String.IsNullOrEmpty(c.SearchPhrase))
                {
                    var contentField = string.Format("__content_{0}", c.Locale.ToLower());
                    AddQueryString(mainQuery, c, "__content", contentField);
                }

                if (!String.IsNullOrEmpty(c.Catalog))
                {
                    AddQuery("catalog", mainQuery, c.Catalog);
                }

                if (c.ClassTypes != null && c.ClassTypes.Count > 0)
                {
                    AddQuery("__type", mainQuery, c.ClassTypes, false);
                }
            }
            #endregion

            if (criteria is ElasticSearchCriteria)
            {
                var c = criteria as ElasticSearchCriteria;
                mainQuery.Must(m => m.Custom(c.RawQuery));
            }

            builder.Query(q => q.Bool(b => mainQuery));
            builder.Filter(f => mainFilter);

            // Add search facets
            var facets = GetFacets(criteria);
            builder.Facets(f => facets);

            return builder;
        }
        #endregion

        protected void AddQuery(string fieldName, BoolQuery<ESDocument> query, StringCollection filter, bool lowerCase = true)
        {
            fieldName = fieldName.ToLower();
            if (filter.Count > 0)
            {
                if (filter.Count == 1)
                {
                    if (!String.IsNullOrEmpty(filter[0]))
                    {
                        AddQuery(fieldName, query, filter[0], lowerCase);
                    }
                }
                else
                {
                    var booleanQuery = new BoolQuery<ESDocument>();
                    var containsFilter = false;
                    foreach (var index in filter.Cast<string>().Where(index => !String.IsNullOrEmpty(index)))
                    {
                        booleanQuery.Should(q => q.Custom("{{\"wildcard\" : {{ \"{0}\" : \"{1}\" }}}}", fieldName.ToLower(), lowerCase ? index.ToLower() : index));
                        containsFilter = true;
                    }
                    if (containsFilter)
                        query.Must(q => q.Bool(b => booleanQuery));
                }
            }
        }

        protected void AddQuery(string fieldName, BoolQuery<ESDocument> query, string filter, bool lowerCase = true)
        {
            query.Must(q => q.Custom("{{\"wildcard\" : {{ \"{0}\" : \"{1}\" }}}}", fieldName.ToLower(), lowerCase ? filter.ToLower() : filter));
        }

        protected void AddQueryString(BoolQuery<ESDocument> query, CatalogIndexedSearchCriteria filter, params string[] fields)
        {
            var searchPhrase = filter.SearchPhrase;
            if (filter.IsFuzzySearch)
            {
                query.Must(
                    q =>
                    q.MultiMatch(
                        x =>
                        x.Fields(fields).Operator(Operator.AND).Fuzziness(filter.FuzzyMinSimilarity).Query(searchPhrase)));
            }
            else
            {
                query.Must(
                    q =>
                    q.MultiMatch(
                        x =>
                        x.Fields(fields).Operator(Operator.AND).Query(searchPhrase)));
            }
        }

        #region Facet Query
        /// <summary>
        /// Gets the facet parameters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        protected virtual Facets<ESDocument> GetFacets(ISearchCriteria criteria)
        {
            // Now add facets
            var facetParams = new Facets<ESDocument>();
            foreach (var filter in criteria.Filters)
            {
                if (filter is AttributeFilter)
                {
                    AddFacetQueries(facetParams, filter.Key, ((AttributeFilter)filter).Values, criteria);
                }
                else if (filter is RangeFilter)
                {
                    AddFacetQueries(facetParams, filter.Key, ((RangeFilter)filter).Values, criteria);
                }
                else if (filter is PriceRangeFilter)
                {
                    var currency = ((PriceRangeFilter)filter).Currency;
                    if (currency.Equals(criteria.Currency, StringComparison.OrdinalIgnoreCase))
                    {
                        AddFacetPriceQueries(facetParams, filter.Key, ((PriceRangeFilter)filter).Values, criteria);
                    }
                }
                else if (filter is CategoryFilter)
                {
                    AddFacetQueries(facetParams, filter.Key, ((CategoryFilter)filter).Values);
                }
            }

            /*
            var catalogCriteria = criteria as CatalogItemSearchCriteria;

            if (catalogCriteria != null)
            {
                AddSubCategoryFacetQueries(facetParams, catalogCriteria);
            }
             * */

            return facetParams;
        }

        private void AddFacetQueries(Facets<ESDocument> param, string fieldName, IEnumerable<CategoryFilterValue> values)
        {
            foreach (var val in values)
            {
                var facetName = String.Format("{0}-{1}", fieldName.ToLower(), val.Id.ToLower());
                param.FilterFacets(ff =>
                    ff.FacetName(facetName).Filter(f => f.Query(q => q.Bool(bf => bf.Must(bfm =>
                    bfm.Custom("{{\"wildcard\" : {{ \"{0}\" : \"{1}\" }}}}", fieldName.ToLower(), val.Outline.ToLower()))))));
            }
        }

        /// <summary>
        /// Adds the facet queries.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="values">The values.</param>
        private void AddFacetQueries(
            Facets<ESDocument> param, string fieldName, IEnumerable<AttributeFilterValue> values, ISearchCriteria criteria)
        {
            if (values == null)
                return;

            var ffilter = new BoolFilter<ESDocument>();
            foreach (var f in criteria.CurrentFilters)
            {
                if (!f.Key.Equals(fieldName))
                {
                    var q = ElasticQueryHelper.CreateQuery(criteria, f);
                    ffilter.Must(ff => ff.Bool(bb => q));
                }
            }

            var facetFilter = new FacetFilter<ESDocument>();
            facetFilter.Bool(f => ffilter);

            //var filter = new FacetFilter<ESDocument>();
            //facetFilter.Terms(x => x.Values(values.Select(y => y.Value).ToArray()));
            //var filterFacet = new FilterFacet<ESDocument>();
            //filterFacet.FacetName(fieldName.ToLower()).FacetFilter(f => facetFilter);

            param.Terms(t => t.FacetName(fieldName.ToLower()).Field(fieldName.ToLower()).FacetFilter(ff => facetFilter));
        }

        /// <summary>
        /// Adds the facet queries.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="values">The values.</param>
        private void AddFacetQueries(Facets<ESDocument> param, string fieldName, IEnumerable<RangeFilterValue> values, ISearchCriteria criteria)
        {
            if (values == null)
                return;

            var ffilter = new Filter<ESDocument>();
            foreach (var f in criteria.CurrentFilters)
            {
                if (!f.Key.Equals(fieldName))
                {
                    var q = ElasticQueryHelper.CreateQuery(criteria, f);
                    ffilter.Bool(ff => q);
                }
            }

            foreach (var value in values)
            {
                var filter = new FacetFilter<ESDocument>();
                filter.Range(r => r.IncludeLower(false).IncludeUpper().From(value.Lower).To(value.Upper));
                filter.And(b => ffilter);
                param.FilterFacets(ff => ff.FacetName(String.Format("{0}-{1}", fieldName, value.Id)).Filter(f => filter));
            }
        }

        /// <summary>
        /// Adds the facet queries.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="values">The values.</param>
        /// <param name="criteria">The criteria.</param>
        private void AddFacetPriceQueries(Facets<ESDocument> param, string fieldName, IEnumerable<RangeFilterValue> values, ISearchCriteria criteria)
        {
            if (values == null)
                return;

            var ffilter = new MustFilter<ESDocument>();
            foreach (var f in criteria.CurrentFilters)
            {
                if (!f.Key.Equals(fieldName))
                {
                    var q = ElasticQueryHelper.CreateQuery(criteria, f);
                    ffilter.Bool(ff => q);
                }
            }

            foreach (var value in values)
            {
                var query = ElasticQueryHelper.CreatePriceRangeFilter(criteria, fieldName, value);

                if (query != null)
                {
                    query.Must(b => ffilter);
                    param.FilterFacets(
                        ff => ff.FacetName(String.Format("{0}-{1}", fieldName, value.Id)).Filter(f => f.Bool(b => query)));
                }
            }
        }
        #endregion
    }
}
