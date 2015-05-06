using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using u = Lucene.Net.Util;

namespace VirtoCommerce.SearchModule.Data.Providers.Lucene
{

    /// <summary>
    ///     File based search provider based on Lucene.
    /// </summary>
    public class LuceneSearchProvider : ISearchProvider
    {

        private static readonly Dictionary<string, IndexWriter> IndexFolders = new Dictionary<string, IndexWriter>();

        private static readonly object Providerlock = new object();



        private readonly ISearchConnection _connection;

        private readonly Dictionary<string, List<Document>> _pendingDocuments = new Dictionary<string, List<Document>>();

        private bool _autoCommit = true;

        private int _autoCommitCount = 100;

        private bool _isInitialized;

        private string _location = String.Empty;

        private ISearchQueryBuilder _queryBuilder = new LuceneSearchQueryBuilder();



        /// <summary>
        ///     Initializes a new instance of the <see cref="LuceneSearchProvider" /> class.
        /// </summary>
        /// <param name="queryBuilder">The query builder.</param>
        /// <param name="connection">The connection.</param>
        public LuceneSearchProvider(ISearchQueryBuilder queryBuilder, ISearchConnection connection)
        {
            _queryBuilder = queryBuilder;
            _connection = connection;
            Init();
        }



        /// <summary>
        ///     Gets or sets a value indicating whether [auto commit].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [auto commit]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoCommit
        {
            get
            {
                return _autoCommit;
            }
            set
            {
                _autoCommit = value;
            }
        }

        /// <summary>
        ///     Gets or sets the auto commit count.
        /// </summary>
        /// <value>The auto commit count.</value>
        public int AutoCommitCount
        {
            get
            {
                return _autoCommitCount;
            }
            set
            {
                _autoCommitCount = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query builder.
        /// </summary>
        /// <value>
        ///     The query builder.
        /// </value>
        public ISearchQueryBuilder QueryBuilder
        {
            get
            {
                return _queryBuilder;
            }
            set
            {
                _queryBuilder = value;
            }
        }



        /// <summary>
        ///     Closes the specified provider.
        /// </summary>
        /// <param name="scope">Name of the application.</param>
        /// <param name="documentType">The documentType.</param>
        public virtual void Close(string scope, string documentType)
        {
            Close(scope, documentType, true);
        }

        /// <summary>
        ///     Writes all documents saved in memory to the index writer.
        /// </summary>
        /// <param name="scope">Name of the application.</param>
        public virtual void Commit(string scope)
        {
            var documentTypes = _pendingDocuments.Keys.ToList();

            lock (Providerlock)
            {
                foreach (var documentType in documentTypes)
                {
                    var documents = _pendingDocuments[documentType];
                    if (documents == null || documents.Count == 0)
                    {
                        continue;
                    }

                    var writer = GetIndexWriter(documentType, true, false);

                    foreach (var doc in documents)
                    {
                        writer.AddDocument(doc);
                    }

                    // Remove documents
                    _pendingDocuments[documentType].Clear();
                }
            }
        }

        /// <summary>
        ///     Adds the document to the index. Depending on the provider, the document will be commited only after commit is called.
        /// </summary>
        /// <param name="scope">The scope of the document, used to seperate indexes for different applications.</param>
        /// <param name="documentType">The type of the document, typically simply the name associated with an indexer like catalog, order or catalogitem.</param>
        /// <param name="document">The document.</param>
        public virtual void Index(string scope, string documentType, IDocument document)
        {
            var folderName = GetFolderName(scope, documentType);
            if (!_pendingDocuments.ContainsKey(folderName))
            {
                _pendingDocuments.Add(folderName, new List<Document>());
            }

            _pendingDocuments[folderName].Add(LuceneHelper.ConvertDocument(document));

            // Make sure to auto commit changes when limit is reached, still need to call close before changes are written
            if (AutoCommit && _pendingDocuments.Count > AutoCommitCount)
            {
                Commit(scope);
            }
        }

        /// <summary>
        ///     Removes the document type in the specified scope by document key value.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual int Remove(string scope, string documentType, string key, string value)
        {
            var term = new Term(key, value);

            // Close writer first
            Close(scope, documentType, false);

            var dir = FSDirectory.Open(new DirectoryInfo(GetFolderName(scope, documentType)));
            var indexReader = IndexReader.Open(dir, false);
            var num = indexReader.DeleteDocuments(term);
            indexReader.Dispose();
            return num;
        }

        /// <summary>
        ///     Removes all documents in the specified documentType and scope.
        /// </summary>
        /// <param name="scope">Name of the application.</param>
        /// <param name="documentType">The documentType.</param>
        public virtual void RemoveAll(string scope, string documentType)
        {
            // Make sure the existing writer is closed
            Close(scope, documentType, false);

            // retrieve foldername
            var folderName = GetFolderName(scope, documentType);

            // re-initialize the write, so all documents are deleted
            GetIndexWriter(folderName, true, true);

            // now close the write so changes are saved
            Close(scope, documentType, false);
        }

        /// <summary>
        ///     Searches the datasource using the specified criteria. Criteria is parsed by the query builder specified by
        ///     <typeparamref />
        ///     .
        /// </summary>
        /// <param name="scope">Name of the application.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        /// <exception cref="VirtoCommerce.Search.Providers.Lucene.LuceneSearchException"></exception>
        public virtual ISearchResults Search(string scope, ISearchCriteria criteria)
        {
            TopDocs docs;

            var folderName = GetFolderName(scope, criteria.DocumentType);

            var dir = FSDirectory.Open(new DirectoryInfo(GetDirectoryPath(folderName)));
            var searcher = new IndexSearcher(dir);

            var q = (QueryBuilder)QueryBuilder.BuildQuery(criteria);

            // filter out empty value
            var filter = q.Filter.ToString().Equals("BooleanFilter()") ? null : q.Filter;

            Debug.WriteLine("Search Lucene Query:{0}", (object)q.ToString());

            try
            {
                var numDocs = criteria.StartingRecord + criteria.RecordsToRetrieve;

                if (criteria.Sort != null)
                {
                    var fields = criteria.Sort.GetSort();

                    docs = searcher.Search(
                        q.Query,
                        filter,
                        numDocs,
                        new Sort(
                            fields.Select(field => new SortField(field.FieldName, field.DataType, field.IsDescending))
                                  .ToArray()));
                }
                else
                {
                    docs = searcher.Search(q.Query, filter, numDocs);
                }
            }
            catch (Exception ex)
            {
                throw new LuceneSearchException("Search exception", ex);
            }

            var results = new LuceneSearchResults(searcher, searcher.IndexReader, docs, criteria, q.Query);

            // Cleanup here
            searcher.IndexReader.Dispose();
            searcher.Dispose();
            return results.Results;
        }



        /// <summary>
        ///     Closes the specified documentType.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="documentType">The documentType.</param>
        /// <param name="optimize">
        ///     if set to <c>true</c> [optimize].
        /// </param>
        private void Close(string scope, string documentType, bool optimize)
        {
            lock (Providerlock)
            {
                var folderName = GetFolderName(scope, documentType);

                if (IndexFolders.ContainsKey(folderName) && IndexFolders[folderName] != null)
                {
                    var writer = IndexFolders[folderName];
                    if (optimize)
                    {
                        writer.Optimize();
                    }

                    writer.Dispose();
                    IndexFolders.Remove(folderName);
                }
            }
        }

        /// <summary>
        ///     Gets the directory path.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        private string GetDirectoryPath(string folderName)
        {
            return String.Format("{0}/{1}", _location, folderName);
        }

        /// <summary>
        ///     Gets the name of the folder.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <returns></returns>
        private string GetFolderName(string scope, string documentType)
        {
            return String.Format("{0}-{1}", scope, documentType);
        }

        /// <summary>
        ///     Gets the index writer.
        /// </summary>
        /// <param name="folderName">The folder name.</param>
        /// <param name="create">
        ///     if set to <c>true</c> [create].
        /// </param>
        /// <param name="isNew">
        ///     if set to <c>true</c> [is new].
        /// </param>
        /// <returns></returns>
        private IndexWriter GetIndexWriter(string folderName, bool create, bool isNew)
        {
            lock (Providerlock)
            {
                // Do this again to make sure _solr is still null
                if (!IndexFolders.ContainsKey(folderName) || IndexFolders[folderName] == null)
                {
                    if (!create)
                        return null;
                    var localDirectory = FSDirectory.Open(GetDirectoryPath(folderName));
                    if (!localDirectory.Directory.Exists)
                        isNew = true; // create new if directory doesn't exist
                    if (IndexFolders.ContainsKey(folderName))
                        IndexFolders.Remove(folderName);

                    var indexWriter = new IndexWriter(
                        localDirectory,
                        new StandardAnalyzer(u.Version.LUCENE_30),
                        isNew,
                        IndexWriter.MaxFieldLength.LIMITED);

                    IndexFolders.Add(folderName, indexWriter);
                }

                return IndexFolders[folderName];
            }
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        private void Init()
        {
            if (!_isInitialized)
            {
                // set location for indexes
                _location = _connection.DataSource;

                // resolve path, if we running in web environment
                if (_location.StartsWith("~"))
                {
                    if (HttpContext.Current == null)
                    {
                        _location = _location.Substring(1);
                        if (_location.StartsWith("/"))
                            _location = _location.Substring(1);

                        _location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _location);
                    }
                    else
                        _location = HttpContext.Current.Server.MapPath(_location);
                }

                _isInitialized = true;
            }
        }

    }
}