using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Search.Providers.Azure
{
    using System.Net;
    using System.Threading;

    using RedDog.Search;
    using RedDog.Search.Http;
    using RedDog.Search.Model;

    using VirtoCommerce.Foundation.Search;

    public class AzureSearchProvider : ISearchProvider
    {
        public AzureSearchProvider()
        {
        }

        public AzureSearchProvider(ISearchQueryBuilder queryBuilder, ISearchConnection connection)
        {
            this._queryBuilder = queryBuilder;
            _connection = connection;
        }

        private readonly ISearchQueryBuilder _queryBuilder;

        private readonly ISearchConnection _connection;

        private readonly Dictionary<string, List<AzureDocument>> _pendingDocuments = new Dictionary<string, List<AzureDocument>>();
        private readonly Dictionary<string, Index> _mappings = new Dictionary<string, Index>();


        private AzureSearchClient _Client = null;
        private AzureSearchClient Client
        {
            get
            {
                if (_Client == null)
                {
                    //var connection = ApiConnection.Create(_connection.DataSource, _connection.AccessKey);
                    // experimental
                    var connection = ApiConnection.Create("https://azsrchexp.cloudapp.net/", _connection.AccessKey);
                    _Client = new AzureSearchClient(connection);
                }

                return _Client;
            }
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

        public ISearchQueryBuilder QueryBuilder { get; private set; }

        public ISearchResults Search(string scope, ISearchCriteria criteria)
        {
            // Build query
            var builder = (SearchQuery)_queryBuilder.BuildQuery(criteria);

            SearchQueryResult resultDocs;

            // Add some error handling
            //try
            {
                var searchResponse = Client.Search(scope, builder).Result;
                if (!searchResponse.IsSuccess)
                {
                    throw new AzureSearchException(AzureSearchHelper.FormatSearchException(searchResponse));
                }

                resultDocs = searchResponse.Body;
            }
                /*
            catch (Exception ex)
            {
                throw ex;
            }
                 * */

            // Parse documents returned
            var documents = new ResultDocumentSet { TotalCount = resultDocs.Count };
            var docList = new List<ResultDocument>();
            foreach (var indexDoc in resultDocs.Records)
            {
                var document = new ResultDocument();
                foreach (var field in indexDoc.Properties.Keys)
                {
                    document.Add(new DocumentField(field, indexDoc.Properties[field]));
                }

                docList.Add(document);
            }

            documents.Documents = docList.ToArray();

            // Create search results object
            var results = new SearchResults(criteria, new[] { documents });

            return results;
        }

        public virtual void Index(string scope, string documentType, IDocument document)
        {
            var core = GetCoreName(scope, documentType);
            if (!_pendingDocuments.ContainsKey(core))
            {
                _pendingDocuments.Add(core, new List<AzureDocument>());
            }

            Index mapping = null;
            var indexAlreadyCreated = false;
            if (!_mappings.ContainsKey(core))
            {
                Thread.Sleep(3000);

                // Get mapping info
                mapping = Client.GetIndex(scope).Result;
                if (mapping != null)
                {
                    indexAlreadyCreated = true;
                    _mappings.Add(core, mapping);
                }
            }
            else
            {
                indexAlreadyCreated = true;
                mapping = _mappings[core];
            }

            var submitMapping = false;

            var localDocument = new AzureDocument();

            for (var index = 0; index < document.FieldCount; index++)
            {
                var field = document[index];

                var key = ConvertToAzureName(field.Name.ToLower());

                if (localDocument.ContainsKey(key))
                {
                    var objTemp = localDocument[key];
                    string[] objListTemp;
                    var temp = objTemp as string[];
                    if (temp != null)
                    {
                        var objList = new List<string>(temp) { ConvertToOffset(field.Value).ToString() };
                        objListTemp = objList.ToArray();
                    }
                    else
                    {
                        objListTemp = new string[] { objTemp.ToString(), ConvertToOffset(field.Value).ToString() };
                    }

                    localDocument[key] = objListTemp;
                }
                else
                {
                    if (mapping == null || !mapping.Fields.Any(x=>x.Name.Equals(key)))
                    {
                        if (mapping == null)
                        {
                            mapping = new Index(scope);
                        }
                       
                        var indexField = new IndexField(key, AzureTypeMapper.GetAzureSearchType(field));

                        indexField.IsFilterable();

                        if (key == ConvertToAzureName("__key"))
                        {
                            indexField.IsKey();
                        }

                        if (field.ContainsAttribute(IndexStore.YES))
                        {
                            indexField.IsRetrievable();
                        }

                        if (field.ContainsAttribute(IndexType.ANALYZED))
                        {
                            indexField.IsSearchable();
                        }


                        if (indexField.Type != FieldType.StringCollection)
                        {
                            indexField.IsSortable();
                        }

                        if (indexField.Type == FieldType.StringCollection || indexField.Type == FieldType.String)
                        {

                            if (field.ContainsAttribute(IndexType.ANALYZED))
                            {
                                indexField.IsSearchable();
                            }
                        }

                        mapping.Fields.Add(indexField);
                        submitMapping = true;
                    }

                    if (field.ContainsAttribute(IndexDataType.StringCollection))
                        localDocument.Add(key, ConvertToOffset(field.Values.OfType<string>().ToArray()));
                    else
                        localDocument.Add(key, ConvertToOffset(field.Value));
                }
            }

            // submit mapping
            if (submitMapping)
            {
                IApiResponse<Index> result = null;
                if (indexAlreadyCreated)
                {
                    result = Client.UpdateIndex(mapping).Result;    
                }
                else
                {
                    result = Client.CreateIndex(mapping).Result;    
                }
                
                if (!result.IsSuccess)
                {
                    throw new IndexBuildException(AzureSearchHelper.FormatSearchException(result));
                }
            }

            _pendingDocuments[core].Add(localDocument);

            // Auto commit changes when limit is reached
            if (AutoCommit && _pendingDocuments[core].Count > AutoCommitCount)
            {
                Commit(scope);
            }
        }

        public int Remove(string scope, string documentType, string key, string value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(string scope, string documentType)
        {
            var result = Client.DeleteIndex(scope).Result;
            if (!result.IsSuccess)
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    // ignore index not found exception as it might simply not exist and thus no deletion is necessary
                }
                else
                {
                    throw new IndexBuildException(AzureSearchHelper.FormatSearchException(result));    
                }
                
            }
        }

        public void Close(string scope, string documentType)
        {
        }

        public void Commit(string scope)
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

                var response = Client.IndexBulk(indexName, documents);
                var result = response.Result;

                if (!result.IsSuccess)
                {
                    throw new IndexBuildException(AzureSearchHelper.FormatSearchException(result));
                }

                foreach (var op in result.Body)
                {
                    if (!op.Status)
                    {
                        throw new IndexBuildException(op.ErrorMessage);
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

        private static string _SysFieldPrefix = "sys";
        private static string ConvertToAzureName(string original)
        {
            // Convert "-" to "_" since azure search doesn't allow those names
            if (original.Contains("-"))
            {
                original = original.Replace("-", "_");
            }

            if (original.StartsWith("__"))
            {
                original = _SysFieldPrefix + original;
            }

            return original;
        }

        private object ConvertToOffset(object value)
        {
            if (value is DateTime)
            {
                return AzureSearchHelper.ConvertToOffset((DateTime)value);
            }

            return value;
        }
    }
}
