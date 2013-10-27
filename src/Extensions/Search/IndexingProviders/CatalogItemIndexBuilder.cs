using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks.Logging;

namespace VirtoCommerce.Search.Index
{
    public class CatalogItemIndexBuilder : ISearchIndexBuilder
    {
        /// <summary>
        /// Gets the catalog repository.
        /// </summary>
        public ICatalogRepository CatalogRepository { get; private set; }

        /// <summary>
        /// Gets the price list repository.
        /// </summary>
        public IPricelistRepository PriceListRepository { get; private set; }

        /// <summary>
        /// Gets the log repository.
        /// </summary>
        /// <value>
        /// The log repository.
        /// </value>
        public IOperationLogRepository LogRepository { get; private set; }

        /// <summary>
        /// Gets the search provider.
        /// </summary>
        /// <value>
        /// The search provider.
        /// </value>
        public ISearchProvider SearchProvider { get; private set; }

        /// <summary>
        /// Gets the cache repository.
        /// </summary>
        /// <value>
        /// The cache repository.
        /// </value>
        public ICacheRepository CacheRepository { get; private set; }

        /// <summary>
        /// Gets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        public string DocumentType
        {
            get
            {
                return "catalogitem";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItemIndexBuilder" /> class.
        /// </summary>
        /// <param name="searchProvider">The search provider.</param>
        /// <param name="catalogRepository">The catalog repository.</param>
        /// <param name="priceListRepository">The price list repository.</param>
        /// <param name="logRepository">The log repository.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public CatalogItemIndexBuilder(ISearchProvider searchProvider, ICatalogRepository catalogRepository, IPricelistRepository priceListRepository, IOperationLogRepository logRepository, ICacheRepository cacheRepository)
        {
            CatalogRepository = catalogRepository;
            PriceListRepository = priceListRepository;
            SearchProvider = searchProvider;
            LogRepository = logRepository;
            CacheRepository = cacheRepository;
        }

        /*
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItemIndexBuilder" /> class.
        /// </summary>
        /// <param name="catalogRepository">The catalog repository.</param>
        /// <param name="pricelistRepository">The pricelist repository.</param>
        /// <param name="logRepository">The log repository.</param>
        public CatalogItemIndexBuilder(ICatalogRepository catalogRepository, IPricelistRepository pricelistRepository, IOperationLogRepository logRepository, ICacheRepository cacheRepository)
        {
            SearchProvider = null;
            CatalogRepository = catalogRepository;
            PriceListRepository = pricelistRepository;
            LogRepository = logRepository;
            CacheRepository = cacheRepository;
        }
         * */

        /// <summary>
        /// Initializes the specified Index Builder. Typically creates partitions that will need be indexed.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="lastBuild">The last build.</param>
        /// <returns></returns>
        public IEnumerable<IMessage> CreatePartitions(string scope, DateTime lastBuild)
        {
            var creator = new CatalogItemPartitionCreator(CatalogRepository, LogRepository);
            var jobId = Guid.NewGuid();

            return creator.CreatePartitions(jobId.ToString(), lastBuild)
				.Select(partition => new SearchIndexMessage(jobId.ToString(), scope, DocumentType, partition));
        }

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public IEnumerable<IDocument> CreateDocuments<T>(T input)
        {
            var docCreator = new CatalogItemDocumentCreator(CatalogRepository, PriceListRepository, CacheRepository);

            // create index docs
            return docCreator.CreateDocument(input as Partition);
        }

        /// <summary>
        /// Publishes the documents.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="documents">The documents.</param>
        public void PublishDocuments(string scope, IDocument[] documents)
        {
            var publisher = new DocumentPublisher(SearchProvider);
            publisher.SubmitDocuments(scope, DocumentType, documents);
        }

        /// <summary>
        /// Removes the documents.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="documents">The documents.</param>
        public void RemoveDocuments(string scope, string[] documents)
        {
            var publisher = new DocumentPublisher(SearchProvider);
            publisher.RemoveDocuments(scope, DocumentType, documents);
        }
    }
}
