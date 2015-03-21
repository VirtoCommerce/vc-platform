using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Logging
{
    public interface IOperationLogRepository : IRepository
    {
        IQueryable<OperationLog> OperationLogs { get; }
    }
}
