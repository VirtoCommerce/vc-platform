using System;

namespace VirtoCommerce.Platform.Core.Common
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Timeout for the commands that will be executed by the unit of work.
        /// </summary>
        TimeSpan? CommandTimeout { get; set; }

        int Commit();
        void CommitAndRefreshChanges();
        void RollbackChanges();
    }
}
