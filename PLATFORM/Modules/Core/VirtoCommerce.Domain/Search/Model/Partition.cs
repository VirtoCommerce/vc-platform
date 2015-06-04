namespace VirtoCommerce.Domain.Search.Model
{
    /// <summary>
    /// Contains information about keys of items to be indexed. Partition is part of the bigger job that was split into mini jobs called partitions that
    /// could be processed by multiple job processers in parallel.
    /// </summary>
    public class Partition
    {
        /// <summary>
        /// Gets the type of the operation to perform on particular keys. Can be indexing or removing.
        /// </summary>
        /// <value>
        /// The type of the operation.
        /// </value>
        public OperationType OperationType { get; private set; }

        /// <summary>
        /// Gets the keys for objects to be processed.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public string[] Keys { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Partition" /> class.
        /// </summary>
        /// <param name="operationType">Type of the operation.</param>
        /// <param name="keys">The keys.</param>
        public Partition(OperationType operationType, string[] keys)
        {
            OperationType = operationType;
            Keys = keys;
        }
    }
}
