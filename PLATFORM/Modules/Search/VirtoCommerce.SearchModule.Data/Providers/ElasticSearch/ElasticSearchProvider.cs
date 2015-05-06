using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PlainElastic.Net;
using PlainElastic.Net.IndexSettings;
using PlainElastic.Net.Mappings;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;

namespace VirtoCommerce.SearchModule.Data.Providers.ElasticSearch
{
    public class ElasticSearchProvider : ISearchProvider
    {
        private readonly ISearchConnection _connection;
        private readonly Dictionary<string, List<ESDocument>> _pendingDocuments = new Dictionary<string, List<ESDocument>>();
        private readonly Dictionary<string, string> _mappings = new Dictionary<string, string>();

        private bool _settingsUpdated;
        private const string SearchAnalyzer = "trigrams";

        #region Private Properties
        ElasticClient<ESDocument> _client;
        private ElasticClient<ESDocument> Client
        {
            get
            {
                if (_client == null)
                {
                    var url = ElasticServerUrl;

                    if (url.StartsWith("https://"))
                    {
                        ThrowException("https connection is not supported", null);
                    }

                    if (url.StartsWith("http://")) // remove http prefix
                    {
                        url = url.Substring("http://".Length);
                    }

                    if (url.EndsWith("/"))
                    {
                        url = url.Remove(url.LastIndexOf("/", StringComparison.Ordinal));
                    }

                    var arr = url.Split(':');
                    var host = arr[0];
                    var port = "8080";
                    if (arr.Length > 1)
                        port = arr[1];

                    _client = new ElasticClient<ESDocument>(host, Int32.Parse(port));
                }

                return _client;
            }
        }
        #endregion

        #region Public Properties
        public string DefaultIndex { get; set; }

        private ISearchQueryBuilder _queryBuilder = new ElasticSearchQueryBuilder();

        public ISearchQueryBuilder QueryBuilder
        {
            get { return _queryBuilder; }
            set { _queryBuilder = value; }
        }

        private bool _autoCommit = true;

        /// <summary>
        /// Gets or sets a value indicating whether [auto commit].
        /// </summary>
        /// <value><c>true</c> if [auto commit]; otherwise, <c>false</c>.</value>
        public bool AutoCommit
        {
            get { return _autoCommit; }
            set { _autoCommit = value; }
        }

        private int _autoCommitCount = 100;

        /// <summary>
        /// Gets or sets the auto commit count.
        /// </summary>
        /// <value>The auto commit count.</value>
        public int AutoCommitCount
        {
            get { return _autoCommitCount; }
            set { _autoCommitCount = value; }
        }

        private string _elasticServerUrl = String.Empty;

        /// <summary>
        /// Gets or sets the solr server URL without Core secified.
        /// </summary>
        /// <example>localhost:9200</example>
        /// <value>The solr server URL.</value>
        public string ElasticServerUrl
        {
            get { return _elasticServerUrl; }
            set { _elasticServerUrl = value; }
        }
        #endregion

        public ElasticSearchProvider()
        {
            Init();
        }

        public ElasticSearchProvider(ISearchQueryBuilder queryBuilder, ISearchConnection connection)
        {
            _queryBuilder = queryBuilder;
            _connection = connection;
            Init();
        }

        private bool _isInitialized;
        private void Init()
        {
            if (!_isInitialized)
            {
                if (_connection != null && !String.IsNullOrEmpty(_connection.DataSource))
                {
                    _elasticServerUrl = _connection.DataSource;
                }
                else
                {
                    _elasticServerUrl = "localhost:9200";
                }

                _isInitialized = true;
            }
        }

        public virtual ISearchResults Search(string scope, ISearchCriteria criteria)
        {
            var command = new SearchCommand(scope, criteria.DocumentType);

            command.Size(criteria.RecordsToRetrieve);
            command.From(criteria.StartingRecord);

            // Add spell checking
            // TODO: options.SpellCheck = new SpellCheckingParameters { Collate = true };

            // Build query
            var builder = (QueryBuilder<ESDocument>)_queryBuilder.BuildQuery(criteria);

            SearchResult<ESDocument> resultDocs;

            // Add some error handling
            try
            {
                resultDocs = Client.Search(command, builder);
            }
            catch (Exception ex)
            {
                throw new ElasticSearchException("Search using Elastic Search server failed, check logs for more details.", ex);
            }

            // Parse documents returned
            var documents = new ResultDocumentSet { TotalCount = resultDocs.hits.total };
            var docList = new List<ResultDocument>();
            foreach (var indexDoc in resultDocs.Documents)
            {
                var document = new ResultDocument();
                foreach (var field in indexDoc.Keys)
                    document.Add(new DocumentField(field, indexDoc[field]));

                docList.Add(document);
            }

            documents.Documents = docList.ToArray();

            // Create search results object
            var results = new SearchResults(criteria, new[] { documents });

            // Now add facet results
            var groups = new List<FacetGroup>();

            if (resultDocs.facets != null)
            {
                foreach (var filter in criteria.Filters)
                {
                    var groupCount = 0;

                    var group = new FacetGroup(filter.Key);

                    if (filter is AttributeFilter)
                    {
                        group.FacetType = FacetTypes.Attribute;
                        var attributeFilter = filter as AttributeFilter;
                        var myFilter = attributeFilter;
                        var values = myFilter.Values;
                        if (values != null)
                        {
                            var key = filter.Key.ToLower();
                            if (!resultDocs.facets.ContainsKey(key))
                                continue;

                            var facet = resultDocs.facets[key] as TermsFacetResult;
                            if (facet != null)
                            {
                                foreach (var value in values)
                                {
                                    //facet.terms
                                    var termCount = from f in facet.terms where f.term.Equals(value.Id, StringComparison.OrdinalIgnoreCase) select f.count;

                                    var enumerable = termCount as int[] ?? termCount.ToArray();
                                    if (!enumerable.Any())
                                        continue;

                                    //var facet = from resultFacet
                                    var newFacet = new Facet(@group, value.Id, GetDescription(value, criteria.Locale), enumerable.SingleOrDefault());
                                    @group.Facets.Add(newFacet);
                                }

                                groupCount++;
                            }
                        }
                    }
                    else if (filter is PriceRangeFilter)
                    {
                        group.FacetType = FacetTypes.PriceRange;
                        var rangeFilter = filter as PriceRangeFilter;
                        if (rangeFilter != null
                            && rangeFilter.Currency.Equals(criteria.Currency, StringComparison.OrdinalIgnoreCase))
                        {
                            var myFilter = rangeFilter;
                            var values = myFilter.Values;
                            if (values != null)
                            {
                                values = rangeFilter.Values;

                                foreach (var value in values)
                                {
                                    var key = String.Format("{0}-{1}", myFilter.Key, value.Id).ToLower();

                                    if (!resultDocs.facets.ContainsKey(key))
                                        continue;

                                    var facet = resultDocs.facets[key] as FilterFacetResult;

                                    if (facet != null && facet.count > 0)
                                    {
                                        if (facet.count == 0)
                                            continue;

                                        var myFacet = new Facet(
                                            @group, value.Id, GetDescription(value, criteria.Locale), facet.count);
                                        @group.Facets.Add(myFacet);

                                        groupCount++;
                                    }
                                }
                            }
                        }
                    }
                    else if (filter is RangeFilter)
                    {
                        group.FacetType = FacetTypes.Range;
                        var myFilter = filter as RangeFilter;
                        if (myFilter != null)
                        {
                            var values = myFilter.Values;
                            if (values != null)
                            {
                                foreach (var value in values)
                                {
                                    var facet = resultDocs.facets[filter.Key] as FilterFacetResult;

                                    if (facet == null || facet.count <= 0)
                                    {
                                        continue;
                                    }

                                    var myFacet = new Facet(
                                        @group, value.Id, GetDescription(value, criteria.Locale), facet.count);
                                    @group.Facets.Add(myFacet);

                                    groupCount++;
                                }
                            }
                        }
                    }
                    else if (filter is CategoryFilter)
                    {
                        group.FacetType = FacetTypes.Category;
                        var myFilter = filter as CategoryFilter;
                        if (myFilter != null)
                        {
                            var values = myFilter.Values;
                            if (values != null)
                            {
                                foreach (var value in values)
                                {
                                    var key = String.Format("{0}-{1}", myFilter.Key.ToLower(), value.Id.ToLower()).ToLower();
                                    var facet = resultDocs.facets[key] as FilterFacetResult;

                                    if (facet == null || facet.count <= 0)
                                    {
                                        continue;
                                    }

                                    var myFacet = new Facet(
                                        @group, value.Id, GetDescription(value, criteria.Locale), facet.count);
                                    @group.Facets.Add(myFacet);

                                    groupCount++;
                                }
                            }
                        }
                    }

                    // Add only if items exist under
                    if (groupCount > 0)
                    {
                        groups.Add(group);
                    }
                }
            }

            results.FacetGroups = groups.ToArray();
            return results;
        }

        public virtual void Index(string scope, string documentType, IDocument document)
        {
            var core = GetCoreName(scope, documentType);
            if (!_pendingDocuments.ContainsKey(core))
            {
                _pendingDocuments.Add(core, new List<ESDocument>());
            }

            string mapping = null;
            if (!_mappings.ContainsKey(core))
            {
                // Get mapping info
                if (Client.IndexExists(new IndexExistsCommand(scope)))
                {
                    try
                    {
                        mapping = Client.GetMapping(new GetMappingCommand(scope, documentType));
                    }
                    catch (OperationException ex)
                    {
                        if (ex.HttpStatusCode != 404 || !ex.Message.Contains("TypeMissingException"))
                        {
                            throw;
                        }
                    }

                    if (mapping != null)
                        _mappings.Add(core, mapping);
                }
            }
            else
            {
                mapping = _mappings[core];
            }

            var submitMapping = false;

            var properties = new Properties<ESDocument>();
            var localDocument = new ESDocument();

            for (var index = 0; index < document.FieldCount; index++)
            {
                var field = document[index];

                var key = field.Name.ToLower();

                if (localDocument.ContainsKey(key))
                {
                    var objTemp = localDocument[key];
                    object[] objListTemp;
                    var temp = objTemp as object[];
                    if (temp != null)
                    {
                        var objList = new List<object>(temp) { field.Value };
                        objListTemp = objList.ToArray();
                    }
                    else
                    {
                        objListTemp = new[] { objTemp, field.Value };
                    }

                    localDocument[key] = objListTemp;
                }
                else
                {
                    if (String.IsNullOrEmpty(mapping) || !mapping.Contains(String.Format("\"{0}\"", key)))
                    {
                        var type = field.Value != null ? field.Value.GetType() : null;
                        var propertyMap = new CustomPropertyMap<ESDocument>(field.Name, type)
                        .Store(field.ContainsAttribute(IndexStore.YES))
                        .When(field.ContainsAttribute(IndexType.NOT_ANALYZED), p => p.Index(IndexState.not_analyzed))
                        .When(field.Name.StartsWith("__content", StringComparison.OrdinalIgnoreCase), p => p.Analyzer(SearchAnalyzer))
                        .When(Regex.Match(field.Name, "__content_en.*").Success, x => x.Analyzer("english"))
                        .When(Regex.Match(field.Name, "__content_de.*").Success, x => x.Analyzer("german"))
                        .When(Regex.Match(field.Name, "__content_ru.*").Success, x => x.Analyzer("russian"))
                        .When(field.ContainsAttribute(IndexType.NO), p => p.Index(IndexState.no));

                        properties.CustomProperty(field.Name, p => propertyMap);

                        submitMapping = true;
                    }

                    localDocument.Add(key, field.Value);
                }
            }

            // submit mapping
            if (submitMapping)
            {
                //Use ngrams analyzer for search in the middle of word
                //http://www.elasticsearch.org/guide/en/elasticsearch/guide/current/ngrams-compound-words.html
                var settings = new IndexSettingsBuilder()
                    .Analysis(als => als
                        .Analyzer(a => a.Custom(SearchAnalyzer, custom => custom
                            .Tokenizer(DefaultTokenizers.standard)
                            .Filter("trigrams_filter", DefaultTokenFilters.lowercase.ToString())))
                        .Filter(f => f.NGram("trigrams_filter", ng => ng
                                .MinGram(3)
                                .MaxGram(3)))).Build();

                if (!Client.IndexExists(new IndexExistsCommand(scope)))
                {
                    var response = Client.CreateIndex(new IndexCommand(scope), settings);
                    _settingsUpdated = true;
                    if (response.error != null)
                        throw new IndexBuildException(response.error);
                }
                else if (!_settingsUpdated)
                {
                    // We can't update settings on active index.
                    // So we need to close it, then update settings and then open index back.
                    Client.Close(new CloseCommand(scope));
                    Client.UpdateSettings(new UpdateSettingsCommand(scope), settings);
                    Client.Open(new OpenCommand(scope));
                    _settingsUpdated = true;
                }

                var mapBuilder = new MapBuilder<ESDocument>();
                var mappingNew = mapBuilder.RootObject(documentType, d => d.Properties(p => properties)).Build();

                var result = Client.PutMapping(new PutMappingCommand(scope, documentType), mappingNew);
                if (!result.acknowledged && result.error != null)
                    throw new IndexBuildException(result.error);
            }

            _pendingDocuments[core].Add(localDocument);

            // Auto commit changes when limit is reached
            if (AutoCommit && _pendingDocuments[core].Count > AutoCommitCount)
            {
                Commit(scope);
            }
        }

        public virtual int Remove(string scope, string documentType, string key, string value)
        {
            var result = Client.Delete(Commands.Delete(scope, documentType, value));

            if (result.error != null && !result.acknowledged)
                throw new IndexBuildException(result.error);

            return 1;
        }

        public virtual void RemoveAll(string scope, string documentType)
        {
            try
            {
                var result = Client.Delete(Commands.Delete(scope, documentType));

                if (result.error != null && !result.acknowledged)
                    throw new IndexBuildException(result.error);
            }
            catch (OperationException ex)
            {
                if (ex.HttpStatusCode == 404 && (ex.Message.Contains("TypeMissingException") || ex.Message.Contains("IndexMissingException")))
                {

                }
                else
                {
                    ThrowException("Failed to remove indexes", ex);
                }
            }
        }

        public virtual void Close(string scope, string documentType)
        {
        }

        public virtual void Commit(string scope)
        {
            var coreList = _pendingDocuments.Keys.ToList();

            foreach (var core in coreList)
            {
                var documents = _pendingDocuments[core];
                if (documents == null || documents.Count == 0)
                    continue;

                var coreArray = core.Split('.');
                var indexName = coreArray[0];
                var indexType = coreArray[1];

                var result = Client.IndexBulk(Commands.Bulk(indexName, indexType), documents);

                if (result == null)
                {
                    throw new IndexBuildException("no results");
                }

                foreach (var op in result.items)
                {
                    if (op.Result.error != null)
                    {
                        throw new IndexBuildException(op.Result.error);
                    }
                }

                // Remove documents
                _pendingDocuments[core].Clear();
            }
        }

        private string GetCoreName(string scope, string documentType)
        {
            return String.Format("{0}.{1}", scope.ToLower(), documentType);
        }

        private string GetDescription(ISearchFilterValue value, string locale)
        {
            if (value is AttributeFilterValue)
            {
                var v = value as AttributeFilterValue;
                return v.Value;
            }
            if (value is RangeFilterValue)
            {
                var v = value as RangeFilterValue;
                var returnVal = from d in v.Displays where d.Language.Equals(locale, StringComparison.OrdinalIgnoreCase) select d.Value;
                return returnVal.ToString();
            }
            if (value is CategoryFilterValue)
            {
                var v = value as CategoryFilterValue;
                return v.Name;
            }

            return String.Empty;
        }

        private void ThrowException(string message, Exception innerException)
        {
            throw new ElasticSearchException(String.Format("{0}. URL:{1}", message, ElasticServerUrl), innerException);
        }
    }
}
