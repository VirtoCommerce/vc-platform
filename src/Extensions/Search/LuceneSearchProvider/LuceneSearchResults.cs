#region

using VirtoCommerce.Foundation.Catalogs.Search;
using s = VirtoCommerce.Foundation.Search.Schemas;

#endregion

namespace VirtoCommerce.Search.Providers.Lucene
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Lucene.Net.Index;

    using global::Lucene.Net.Search;

    using global::Lucene.Net.Util;

    using SpellChecker.Net.Search.Spell;

    using Foundation.Search;
    using Foundation.Search.Facets;

    #endregion

    public class LuceneSearchResults
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResults" /> class.
        /// </summary>
        /// <param name="searcher">The searcher.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="docs">The hits.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="query">The query.</param>
        public LuceneSearchResults(
            Searcher searcher, IndexReader reader, TopDocs docs, ISearchCriteria criteria, Query query)
        {
            Results = new SearchResults(criteria, null);
            CreateDocuments(searcher, docs);
            CreateFacets(reader, query);
            CreateSuggestions(reader, criteria);
        }

        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the results.
        /// </summary>
        /// <value>
        ///     The results.
        /// </value>
        public SearchResults Results { get; set; }
        #endregion

        #region Methods

        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="locale">The locale.</param>
        /// <returns></returns>
        private string GetDescription(ISearchFilterValue value, string locale)
        {
            if (value is s.AttributeFilterValue)
            {
                var v = value as s.AttributeFilterValue;
                return v.Value;
            }
            if (value is s.RangeFilterValue)
            {
                var v = value as s.RangeFilterValue;
                if (v.Displays != null)
                {
                    var returnVal = from d in v.Displays
                        where d.Language.Equals(locale, StringComparison.OrdinalIgnoreCase)
                        select d.Value;
                    return returnVal.ToString();
                }
                return v.Id;
            }
            if (value is CategoryFilterValue)
            {
                var v = value as CategoryFilterValue;
                return v.Name;
            }

            return String.Empty;
        }

        /// <summary>
        ///     Calculates number of facets found in the filter doc set.
        /// </summary>
        /// <param name="baseBitSet">The base bit set.</param>
        /// <param name="filterDocSet">The filter bit set.</param>
        /// <returns></returns>
        private long CalculateFacetCount(DocIdSet baseBitSet, DocIdSet filterDocSet)
        {
            var baseDISI = new OpenBitSetDISI(baseBitSet.Iterator(), 25000);
            var filterIterator = filterDocSet.Iterator();
            baseDISI.InPlaceAnd(filterIterator);
            var total = baseDISI.Cardinality();
            return total;
        }

        /// <summary>
        /// Calculates the result count.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="baseDocIdSet">The base doc id set.</param>
        /// <param name="facetGroup">The facet group.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        private int CalculateResultCount(IndexReader reader, DocIdSet baseDocIdSet, FacetGroup facetGroup, ISearchFilter filter, ISearchCriteria criteria)
        {
            var count = 0;

            var values = LuceneQueryHelper.GetFilterValues(filter);

            if (values == null)
            {
                return 0;
            }

            BooleanFilter ffilter = null;
            foreach (var f in criteria.CurrentFilters)
            {
                if (!f.Key.Equals(facetGroup.FieldName))
                {
                    if (ffilter == null) ffilter = new BooleanFilter();

                    var q = LuceneQueryHelper.CreateQuery(criteria, f, Occur.SHOULD);
                    ffilter.Add(new FilterClause(q, Occur.MUST));
                }
            }

            foreach (var value in values)
            {
                var queryFilter = new BooleanFilter();
                    
                var valueFilter = LuceneQueryHelper.CreateQueryForValue(Results.SearchCriteria, filter, value);

                if (valueFilter == null) continue;

                queryFilter.Add(new FilterClause(valueFilter, Occur.MUST));
                if(ffilter!=null)
                    queryFilter.Add(new FilterClause(ffilter, Occur.MUST));

                var filterArray = queryFilter.GetDocIdSet(reader);
                var newCount = (int)CalculateFacetCount(baseDocIdSet, filterArray);
                if (newCount == 0) continue;

                var newFacet = new Facet(facetGroup, value.Id, GetDescription(value, Results.SearchCriteria.Locale), newCount);
                facetGroup.Facets.Add(newFacet);
                count += newCount;
            }

            return count;
        }

        /// <summary>
        /// Gets the similar words.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        private string[] SuggestSimilar(IndexReader reader, string fieldName, string word)
        {
            var spell = new SpellChecker(reader.Directory());
            spell.IndexDictionary(new LuceneDictionary(reader, fieldName));
            var similarWords = spell.SuggestSimilar(word, 2);

            // now make sure to close the spell checker
            spell.Close();

            return similarWords;
        }

        /// <summary>
        /// Creates result document collection from Lucene documents.
        /// </summary>
        /// <param name="searcher">The searcher.</param>
        /// <param name="topDocs">The hits.</param>
        private void CreateDocuments(Searcher searcher, TopDocs topDocs)
        {
            // if no documents found return
            if (topDocs == null) return;

            var entries = new List<ResultDocument>();

            // get total hits
            var totalCount = topDocs.TotalHits;
            var recordsToRetrieve = Results.SearchCriteria.RecordsToRetrieve;
            var startIndex = Results.SearchCriteria.StartingRecord;
            if (recordsToRetrieve > totalCount) recordsToRetrieve = totalCount;

            for (var index = startIndex; index < startIndex + recordsToRetrieve; index++)
            {
                if (index >= totalCount) break;

                var document = searcher.Doc(topDocs.ScoreDocs[index].Doc);
                var doc = new ResultDocument();

                var documentFields = document.GetFields();
                using (var fi = documentFields.GetEnumerator())
                {
                    while (fi.MoveNext())
                    {
                        if (fi.Current != null)
                        {
                            var field = fi.Current;

                            // make sure document field doens't exist, if it does, simply add another value
                            if (doc.ContainsKey(field.Name))
                            {
                                var existingField = doc[field.Name] as DocumentField;
                                if (existingField != null) existingField.AddValue(field.StringValue);
                            }
                            else // add new
                            {
                                doc.Add(new DocumentField(field.Name, field.StringValue));
                            }
                        }
                    }
                }

                entries.Add(doc);
            }

            var searchDocuments = new ResultDocumentSet
            {
                Name = "Items",
                Documents = entries.ToArray(),
                TotalCount = totalCount
            };

            Results.Documents = new[] { searchDocuments };
        }

        /// <summary>
        /// Creates facets.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="query">The query.</param>
        private void CreateFacets(IndexReader reader, Query query)
        {
            var groups = new List<FacetGroup>();
            var baseQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(query));
            var baseDocIdSet = baseQueryFilter.GetDocIdSet(reader);

            #region Subcategory filters

           
            /* 
            var catalogCriteria = Results.SearchCriteria as CatalogItemSearchCriteria;
            if (catalogCriteria != null && catalogCriteria.ChildCategoryFilters.Any())
            {
                var group = new FacetGroup("Subcategory");
                var groupCount = 0;

                foreach (var value in catalogCriteria.ChildCategoryFilters)
                {
                    var q = LuceneQueryHelper.CreateQuery(catalogCriteria.OutlineField, value);

                    if (q == null) continue;

                    var queryFilter = new CachingWrapperFilter(new QueryWrapperFilter(q));
                    var filterArray = queryFilter.GetDocIdSet(reader);
                    var newCount = (int)CalculateFacetCount(baseDocIdSet, filterArray);
                    if (newCount == 0) continue;

                    var newFacet = new Facet(group, value.Code, value.Name, newCount);
                    group.Facets.Add(newFacet);
                    groupCount += newCount;
                }

                // Add only if items exist under
                if (groupCount > 0)
                {
                    groups.Add(group);
                }
            }
             * */

            #endregion

            if (Results.SearchCriteria.Filters != null && Results.SearchCriteria.Filters.Length > 0)
            {
                foreach (var filter in Results.SearchCriteria.Filters)
                {
                    var group = new FacetGroup(filter.Key);
                    var groupCount = 0;

                    if (!String.IsNullOrEmpty(Results.SearchCriteria.Currency) && filter is s.PriceRangeFilter)
                    {
                        var valCurrency = ((s.PriceRangeFilter) filter).Currency;
                        if (
                            !valCurrency.Equals(
                                Results.SearchCriteria.Currency, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }

                    groupCount += CalculateResultCount(reader, baseDocIdSet, group, filter,
                        Results.SearchCriteria);

                    // Add only if items exist under
                    if (groupCount > 0)
                    {
                        groups.Add(group);
                    }
                }
            }

            Results.FacetGroups = groups.ToArray();
        }

        /// <summary>
        /// Creates the suggestions.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="criteria">The criteria.</param>
        private void CreateSuggestions(IndexReader reader, ISearchCriteria criteria)
        {
			if (criteria is KeywordSearchCriteria)
			{
			    var c = criteria as KeywordSearchCriteria;
				var phrase = c.SearchPhrase;
			    if (!String.IsNullOrEmpty(phrase))
			    {
			        Results.Suggestions = SuggestSimilar(reader, "_content", phrase);
			    }
			}
        }

        #endregion
    }
}