using System;

namespace VirtoCommerce.Domain.Search.Model
{
    /// <summary>
    /// Contains information about keys of items to be indexed. Partition is part of the bigger job that was split into mini jobs called partitions that
    /// could be processed by multiple job processers in parallel.
    /// </summary>
    public class Partition
    {
        private string[] _keys;

        /// <summary>
        /// Gets the keys for objects to be processed.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public string[] Keys
        {
            get { return _keys; }
        }

        private OperationType _operationType = OperationType.Index;

        /// <summary>
        /// Gets the type of the operation to perform on particular keys. Can be indexing or removing
        /// </summary>
        /// <value>
        /// The type of the operation.
        /// </value>
        public OperationType OperationType
        {
            get { return _operationType; }
        }

        private long _total = 0;

        /// <summary>
        /// Gets the total number of keys in the job which this partition is part of.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public long Total
        {
            get { return _total; }
        }

        private long _start = 0;
        /// <summary>
        /// Gets the starting index for the partition in relation to the job.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public long Start
        {
            get { return _start; }
        }

        private string _jobId = String.Empty;
        /// <summary>
        /// Gets the job id.
        /// </summary>
        /// <value>
        /// The job id.
        /// </value>
        public string JobId
        {
            get { return _jobId; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Partition" /> class.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <param name="jobId">The job id.</param>
        /// <param name="start">The start.</param>
        /// <param name="total">The total.</param>
        public Partition(string[] keys, OperationType operationType, string jobId, long start = 0, long total = 0)
        {
            _keys = keys;
            _operationType = operationType;
            _total = total;
            _start = start;
            _jobId = jobId;
        }
    }
}
