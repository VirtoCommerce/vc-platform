using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using SpellChecker.Net.Search.Spell;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.SearchModule.Data.Services;

namespace VirtoCommerce.SearchModule.Data.Providers.Lucene
{
    public class LuceneSearchResults
    {

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

        private FacetGroup CalculateResultCount(IndexReader reader, DocIdSet baseDocIdSet, ISearchFilter filter, ISearchCriteria criteria)
        {
            FacetGroup result = null;

            var values = filter.GetValues();
            if (values != null)
            {
                var count = 0;

                BooleanFilter ffilter = null;
                foreach (var f in criteria.CurrentFilters)
                {
                    if (!f.Key.Equals(filter.Key))
                    {
                        if (ffilter == null)
                            ffilter = new BooleanFilter();

                        var q = LuceneQueryHelper.CreateQuery(criteria, f, Occur.SHOULD);
                        ffilter.Add(new FilterClause(q, Occur.MUST));
                    }
                }

                var locale = Results.SearchCriteria.Locale;
                string localeShort = null;
                try
                {
                    localeShort = new CultureInfo(locale).TwoLetterISOLanguageName;
                }
                catch
                {
                }

                var facetGroup = new FacetGroup(filter.Key, filter.GetDisplayName(locale, localeShort));

                foreach (var group in values.GroupBy(v => v.Id))
                {
                    var value = GetLocalizedValue(group.ToList(), locale, localeShort);
                    var valueFilter = LuceneQueryHelper.CreateQueryForValue(Results.SearchCriteria, filter, value);

                    if (valueFilter == null)
                        continue;

                    var queryFilter = new BooleanFilter();
                    queryFilter.Add(new FilterClause(valueFilter, Occur.MUST));
                    if (ffilter != null)
                        queryFilter.Add(new FilterClause(ffilter, Occur.MUST));

                    var filterArray = queryFilter.GetDocIdSet(reader);
                    var newCount = (int)CalculateFacetCount(baseDocIdSet, filterArray);
                    if (newCount == 0)
                        continue;

                    var displayValue = value.GetDisplayValue(locale, localeShort);
                    var newFacet = new Facet(facetGroup, value.Id, displayValue, newCount);
                    facetGroup.Facets.Add(newFacet);
                    count += newCount;
                }

                if (count > 0)
                {
                    result = facetGroup;
                }
            }

            return result;
        }

        private static ISearchFilterValue GetLocalizedValue(List<ISearchFilterValue> values, string locale, string localeShort)
        {
            ISearchFilterValue result = values.FirstOrDefault();

            var attributeFilterValues = values.OfType<AttributeFilterValue>().ToList();
            if (attributeFilterValues.Any())
            {
                result = GetLocalizedValue(attributeFilterValues, locale);

                if (result == null)
                {
                    result = GetLocalizedValue(attributeFilterValues, localeShort);
                }
            }

            return result;
        }

        private static ISearchFilterValue GetLocalizedValue(List<AttributeFilterValue> values, string locale)
        {
            return values.FirstOrDefault(v => v.Language.Equals(locale, StringComparison.OrdinalIgnoreCase));
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
            var spell = new SpellChecker.Net.Search.Spell.SpellChecker(reader.Directory());
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
            if (topDocs == null)
                return;

            var entries = new List<ResultDocument>();

            // get total hits
            var totalCount = topDocs.TotalHits;
            var recordsToRetrieve = Results.SearchCriteria.RecordsToRetrieve;
            var startIndex = Results.SearchCriteria.StartingRecord;
            if (recordsToRetrieve > totalCount)
                recordsToRetrieve = totalCount;

            for (var index = startIndex; index < startIndex + recordsToRetrieve; index++)
            {
                if (index >= totalCount)
                    break;

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
                            doc.Add(new DocumentField(field.Name, field.StringValue));
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

                    if (!string.IsNullOrEmpty(Results.SearchCriteria.Currency) && filter is PriceRangeFilter)
                    {
                        var valCurrency = ((PriceRangeFilter)filter).Currency;
                        if (!valCurrency.Equals(Results.SearchCriteria.Currency, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }

                    var group = CalculateResultCount(reader, baseDocIdSet, filter, Results.SearchCriteria);
                    if (group != null)
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
                if (!string.IsNullOrEmpty(phrase))
                {
                    Results.Suggestions = SuggestSimilar(reader, "_content", phrase);
                }
            }
        }

        #endregion
    }
}