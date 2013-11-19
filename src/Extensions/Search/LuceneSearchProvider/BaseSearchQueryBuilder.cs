namespace VirtoCommerce.Search.Providers.Lucene
{
    #region

    using System;
    using System.Collections.Specialized;
    using System.Linq;

    using global::Lucene.Net.Index;
    using global::Lucene.Net.Search;

    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Foundation.Search.Schemas;

    #endregion

    public class BaseSearchQueryBuilder : ISearchQueryBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the query.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public virtual object BuildQuery(ISearchCriteria criteria)
        {
            var query = new BooleanQuery();

            if (criteria.CurrentFilterValues != null)
            {
                for (var index = 0; index < criteria.CurrentFilterFields.Length; index++)
                {
                    var filter = criteria.CurrentFilters.ElementAt(index);
                    var value = criteria.CurrentFilterValues.ElementAt(index);
                    var field = criteria.CurrentFilterFields.ElementAt(index);

                    // Skip currencies that are not part of the filter
                    if (filter.GetType() == typeof(PriceRangeFilter)) // special filtering 
                    {
                        var currency = (filter as PriceRangeFilter).Currency;
                        if (!currency.Equals(criteria.Currency, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }

                    var filterQuery = filter.GetType() == typeof(PriceRangeFilter)
                                          ? LuceneQueryHelper.CreateQuery(criteria, field, value as RangeFilterValue)
                                          : LuceneQueryHelper.CreateQuery(field, value);

                    if (filterQuery != null)
                    {
                        query.Add(filterQuery, Occur.MUST);
                    }
                }
            }

            return query;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the query.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="query">The query.</param>
        /// <param name="filter">The filter.</param>
        protected void AddQuery(string fieldName, BooleanQuery query, StringCollection filter)
        {
            fieldName = fieldName.ToLower();
            if (filter.Count > 0)
            {
                if (filter.Count != 1)
                {
                    var booleanQuery = new BooleanQuery();
                    var containsFilter = false;
                    foreach (var index in filter)
                    {
                        if (String.IsNullOrEmpty(index))
                        {
                            continue;
                        }

                        var nodeQuery = new WildcardQuery(new Term(fieldName, index));
                        booleanQuery.Add(nodeQuery, Occur.SHOULD);
                        containsFilter = true;
                    }
                    if (containsFilter)
                    {
                        query.Add(booleanQuery, Occur.MUST);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(filter[0]))
                    {
                        this.AddQuery(fieldName, query, filter[0].ToLower());
                    }
                }
            }
        }

        /// <summary>
        ///     Adds the query.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="query">The query.</param>
        /// <param name="filter">The filter.</param>
        protected void AddQuery(string fieldName, BooleanQuery query, string filter)
        {
            fieldName = fieldName.ToLower();
            var nodeQuery = new WildcardQuery(new Term(fieldName, filter.ToLower()));
            query.Add(nodeQuery, Occur.MUST);
        }

        #endregion
    }
}