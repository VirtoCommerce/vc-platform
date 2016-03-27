using System;
using System.Collections.Generic;
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
        public LuceneSearchResults(Searcher searcher, IndexReader reader, TopDocs docs, ISearchCriteria criteria, Query query)
        {
            Results = new SearchResults(criteria, null);
            CreateDocuments(searcher, docs);
            CreateFacets(reader, query);
            CreateSuggestions(reader, criteria);
        }

        /// <summary>
        ///     Gets or sets the results.
        /// </summary>
        /// <value>
        ///     The results.
        /// </value>
        public SearchResults Results { get; set; }


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
                Documents = entries.OfType<IDocument>().ToArray(),
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

                    var facetGroup = CalculateResultCount(reader, baseDocIdSet, filter, Results.SearchCriteria);
                    if (facetGroup != null)
                    {
                        groups.Add(facetGroup);
                    }
                }
            }

            Results.FacetGroups = groups.ToArray();
        }

        private FacetGroup CalculateResultCount(IndexReader reader, DocIdSet baseDocIdSet, ISearchFilter filter, ISearchCriteria criteria)
        {
            FacetGroup result = null;

            var values = filter.GetValues();
            if (values != null)
            {
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

                var groupLabels = filter.GetLabels();
                var facetGroup = new FacetGroup(filter.Key, groupLabels);

                foreach (var group in values.GroupBy(v => v.Id))
                {
                    var value = group.FirstOrDefault();
                    var valueFilter = LuceneQueryHelper.CreateQueryForValue(Results.SearchCriteria, filter, value);

                    if (valueFilter != null)
                    {
                        var queryFilter = new BooleanFilter();
                        queryFilter.Add(new FilterClause(valueFilter, Occur.MUST));
                        if (ffilter != null)
                            queryFilter.Add(new FilterClause(ffilter, Occur.MUST));

                        var filterArray = queryFilter.GetDocIdSet(reader);
                        var newCount = (int)CalculateFacetCount(baseDocIdSet, filterArray);

                        if (newCount > 0)
                        {
                            var valueLabels = group.GetValueLabels();
                            var newFacet = new Facet(facetGroup, group.Key, newCount, valueLabels);
                            facetGroup.Facets.Add(newFacet);
                        }
                    }
                }

                if (facetGroup.Facets.Any())
                {
                    result = facetGroup;
                }
            }

            return result;
        }

        private static long CalculateFacetCount(DocIdSet baseBitSet, DocIdSet filterDocSet)
        {
            var baseDisi = new OpenBitSetDISI(baseBitSet.Iterator(), 25000);
            var filterIterator = filterDocSet.Iterator();
            baseDisi.InPlaceAnd(filterIterator);
            var total = baseDisi.Cardinality();
            return total;
        }

        /// <summary>
        /// Creates the suggestions.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="criteria">The criteria.</param>
        private void CreateSuggestions(IndexReader reader, ISearchCriteria criteria)
        {
            var keywordSearchCriteria = criteria as KeywordSearchCriteria;
            if (keywordSearchCriteria != null)
            {
                var c = keywordSearchCriteria;
                var phrase = c.SearchPhrase;
                if (!string.IsNullOrEmpty(phrase))
                {
                    Results.Suggestions = SuggestSimilar(reader, "_content", phrase);
                }
            }
        }

        /// <summary>
        /// Gets the similar words.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        private static string[] SuggestSimilar(IndexReader reader, string fieldName, string word)
        {
            var spell = new SpellChecker.Net.Search.Spell.SpellChecker(reader.Directory());
            spell.IndexDictionary(new LuceneDictionary(reader, fieldName));
            var similarWords = spell.SuggestSimilar(word, 2);

            // now make sure to close the spell checker
            spell.Close();

            return similarWords;
        }
    }
}
