using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Search
{
    /// <summary>
    /// Implement this interface to create partition creator. This is a class that is called to create workload for search indexing.
    /// </summary>
    public interface ISearchIndexPartitionCreator
    {
        /// <summary>
        /// Creates the partitions with specified job id.
        /// </summary>
        /// <param name="jobId">The job id that all partitions are created under. Allows to determine which job all individual partitions belong to.</param>
        /// <param name="lastBuild">The last build.</param>
        /// <returns></returns>
        IEnumerable<Partition> CreatePartitions(string jobId, DateTime lastBuild);
    }
}
