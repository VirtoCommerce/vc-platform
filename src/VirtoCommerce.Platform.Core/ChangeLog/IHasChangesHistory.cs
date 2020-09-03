using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public interface IHasChangesHistory : IEntity
    {
        ICollection<OperationLog> OperationsLog { get; set; }
    }
}
