using System;

namespace VirtoCommerce.Platform.Core.Common
{
    public interface IUnitOfWork
    {
        int Commit();
        void CommitAndRefreshChanges();
        void RollbackChanges();
    }
}
