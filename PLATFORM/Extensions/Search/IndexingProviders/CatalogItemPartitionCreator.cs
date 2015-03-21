using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks.Logging;

namespace VirtoCommerce.Search.Index
{
    /// <summary>
    /// Creates partitions (set of item ids) that can be used to processed during indexing by different threads.
    /// </summary>
    public class CatalogItemPartitionCreator : ISearchIndexPartitionCreator
    {
        private const int DatabasePartitionSizeCount = 5000; // the size of data loaded from database at one time
        private const int PartitionSizeCount = 100; // the maximum partition size, keep it smaller to prevent too big of the sql requests and too large messages in the queue

        readonly ICatalogRepository _catalogRepository;

        /// <summary>
        /// Gets the catalog repository.
        /// </summary>
        public ICatalogRepository CatalogRepository
        {
            get { return _catalogRepository; }
        }

        readonly IOperationLogRepository _logRepository;
        private readonly IReviewRepository _reviewRepository;

        /// <summary>
        /// Gets the log repository.
        /// </summary>
        public IOperationLogRepository LogRepository
        {
            get { return _logRepository; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItemPartitionCreator" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logRepository">The log repository.</param>
        /// <param name="reviewRepository">The review repository.</param>
        public CatalogItemPartitionCreator(ICatalogRepository repository, IOperationLogRepository logRepository, IReviewRepository reviewRepository)
        {
            _catalogRepository = repository;
            _logRepository = logRepository;
            _reviewRepository = reviewRepository;
        }

        /// <summary>
        /// Creates the partitions with specified job id.
        /// </summary>
        /// <param name="jobId">The job id that all partitions are created under. Allows to determine which job all individual partitions belong to.</param>
        /// <param name="lastBuild">The last build.</param>
        /// <returns></returns>
        public IEnumerable<Partition> CreatePartitions(string jobId, DateTime lastBuild)
        {
            return CreatePartitions(jobId, DatabasePartitionSizeCount, lastBuild);
        }

        /// <summary>
        /// Creates the partitions.
        /// </summary>
        /// <param name="jobId">The job id.</param>
        /// <param name="partitionSize">Size of the partition.</param>
        /// <param name="lastBuild">The last build.</param>
        /// <returns></returns>
        private IEnumerable<Partition> CreatePartitions(string jobId, int partitionSize, DateTime lastBuild)
        {
            Trace.TraceInformation("Creating partitions of size {0} from the time {1}", partitionSize, lastBuild);

            // If date pass is min date, do the rebuild
            var rebuild = lastBuild == DateTime.MinValue;

            // check for deletes
            if (!rebuild)
            {
                Trace.TraceInformation(String.Format("Doing a incremental rebuild"));

                IQueryable<string> delete = from d in LogRepository.OperationLogs
                    where
                        d.TableName.Equals("Items", StringComparison.OrdinalIgnoreCase) &&
                        d.OperationType.Equals("Deleted", StringComparison.OrdinalIgnoreCase) &&
                        d.LastModified > lastBuild
                    orderby d.ObjectId, d.ObjectType
                    select d.ObjectId;

                foreach (var result in ProcessQuery(jobId, OperationType.Remove, partitionSize, delete))
                {
                    yield return result;
                }
            }
            else
            {
                Trace.TraceInformation(String.Format("Doing a complete rebuild"));
            }

            // do complete rebuild
            IQueryable<string> query;

            if (!rebuild)
            {
                query =
                    _catalogRepository.Items.Where(i => i.LastModified > lastBuild)
                                            .OrderBy(i => i.ItemId)
                                            .Select(i => i.ItemId).AsNoTracking();

                //Reindex items where reviews were recently approved
                //var queryReviews = _reviewRepository.Reviews
                //    .Where(i => i.LastModified > lastBuild && i.Status == (int)ReviewStatus.Approved)                                         
                //    .OrderBy(i => i.ItemId)                                                   
                //    .Select(i => i.ItemId).AsNoTracking();

                //query = query.ToArray().Union(queryReviews.ToArray()).Distinct().AsQueryable();
            }
            else
            {
                query = _catalogRepository.Items.OrderBy(i => i.ItemId).Select(i => i.ItemId).AsNoTracking();
            }

            foreach (var result in ProcessQuery(jobId, OperationType.Index, partitionSize, query))
            {
                yield return result;
            }
        }

        /// <summary>
        /// Processes the query.
        /// </summary>
        /// <param name="jobId">The job id.</param>
        /// <param name="op">The op.</param>
        /// <param name="partitionSize">Size of the partition.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        private IEnumerable<Partition> ProcessQuery(string jobId, OperationType op, int partitionSize, IQueryable<string> query)
        {
            var skip = 0;
            var total = query.Count(); // only called once, since yield is used

            Trace.TraceInformation("Total items {0}", total);
            while (true)
            {
                // no need to process if total is 0 or past the skip amount
                if (total == 0 || total < skip)
                {
                    break;
                }

                Trace.TraceInformation("Processing items from {0} to {1} of {2}", skip, skip + partitionSize > total ? total : skip + partitionSize, total);
                var items = query.Skip(skip).Take(partitionSize).ToArray();
                if (!items.Any())
                {
                    break;
                }

                // split partitions into smaller ones, since we can load big chunks from db here but we can't save bigger chunks to the queue (there is a limit!)
                var skipPartitions = 0;
                while (true)
                {
                    var partitionItems = items.Skip(skipPartitions).Take(PartitionSizeCount).ToArray();

                    if (!partitionItems.Any())
                    {
                        break;
                    }

                    // add to the queue
                    yield return new Partition(partitionItems, op, jobId, skip + skipPartitions, total);

                    skipPartitions += PartitionSizeCount;
                }

                // increment
                skip += partitionSize;
            }
        }
    }
}