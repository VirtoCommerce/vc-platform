#region

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

    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Foundation.Search.Facets;

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
            this.Results = new SearchResults(criteria, null);
            this.CreateDocuments(searcher, docs);
            this.CreateFacets(reader, query);
            this.CreateSuggestions(reader, criteria);
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
            else if (value is s.RangeFilterValue)
            {
                var v = value as s.RangeFilterValue;
                if (v.Displays != null)
                {
                    var returnVal = from d in v.Displays
                                    where d.Language.Equals(locale, StringComparison.OrdinalIgnoreCase)
                                    select d.Value;
                    return returnVal.ToString();
                }
                else
                {
                    return v.Id;
                }
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
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        private int CalculateResultCount(
            IndexReader reader, DocIdSet baseDocIdSet, FacetGroup facetGroup, ISearchFilter filter, string currency)
        {
            var count = 0;

            ISearchFilterValue[] values = null;
            var priceQuery = false;
            if (filter is s.AttributeFilter)
            {
                values = ((s.AttributeFilter)filter).Values;
            }
            else if (filter is s.RangeFilter)
            {
                values = ((s.RangeFilter)filter).Values;
            }
            else if (filter is s.PriceRangeFilter)
            {
                values = ((s.PriceRangeFilter)filter).Values;
                priceQuery = true;
            }

            if (values == null)
            {
                return 0;
            }

            foreach (var value in values)
            {
                Query q = null;
                if (priceQuery)
                {
                    q = LuceneQueryHelper.CreateQuery(
                        this.Results.SearchCriteria, filter.Key, value as s.RangeFilterValue);
                }
                else
                {
                    q = LuceneQueryHelper.CreateQuery(filter.Key, value);
                }

                if (q == null) continue;

                var queryFilter = new CachingWrapperFilter(new QueryWrapperFilter(q));
                var filterArray = queryFilter.GetDocIdSet(reader);
                var newCount = (int)this.CalculateFacetCount(baseDocIdSet, filterArray);
                if (newCount == 0) continue;

                var newFacet = new Facet(facetGroup, value.Id, this.GetDescription(value, this.Results.SearchCriteria.Locale), newCount);
                facetGroup.Facets.Add(newFacet);
                count += newCount;
            }

            return count;
        }

        /// <summary>
        ///     Gets the similar words.
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
        ///     Creates result document collection from Lucene documents.
        /// </summary>
        /// <param name="topDocs">The hits.</param>
        private void CreateDocuments(Searcher searcher, TopDocs topDocs)
        {
            // if no documents found return
            if (topDocs == null) return;

            var entries = new List<ResultDocument>();

            // get total hits
            var totalCount = topDocs.TotalHits;
            var recordsToRetrieve = this.Results.SearchCriteria.RecordsToRetrieve;
            var startIndex = this.Results.SearchCriteria.StartingRecord;
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
                            doc.Add(new DocumentField(field.Name, field.StringValue));
                        }
                    }
                }

                entries.Add(doc);
            }

            var searchDocuments = new ResultDocumentSet();
            searchDocuments.Name = "Items";
            searchDocuments.Documents = entries.ToArray();
            searchDocuments.TotalCount = totalCount;

            this.Results.Documents = new[] { searchDocuments };
        }

        /// <summary>
        /// Creates facets.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="query">The query.</param>
        private void CreateFacets(IndexReader reader, Query query)
        {
            var groups = new List<FacetGroup>();

            if (this.Results.SearchCriteria.Filters != null && this.Results.SearchCriteria.Filters.Length > 0)
            {
                var baseQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(query));

                var baseBitArray = baseQueryFilter.GetDocIdSet(reader);
                foreach (var filter in this.Results.SearchCriteria.Filters)
                {
                    var group = new FacetGroup(filter.Key);

                    var groupCount = 0;

                    if (!String.IsNullOrEmpty(this.Results.SearchCriteria.Currency) && filter is s.PriceRangeFilter)
                    {
                        var valCurrency = ((s.PriceRangeFilter)filter).Currency;
                        if (
                            !valCurrency.Equals(
                                this.Results.SearchCriteria.Currency, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }

                    groupCount += this.CalculateResultCount(
                        reader, baseBitArray, group, filter, this.Results.SearchCriteria.Currency);

                    // Add only if items exist under
                    if (groupCount > 0)
                    {
                        groups.Add(group);
                    }
                }
            }

            this.Results.FacetGroups = groups.ToArray();
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
			        this.Results.Suggestions = SuggestSimilar(reader, "_content", phrase);
			    }
			}
        }

        #endregion
    }
}