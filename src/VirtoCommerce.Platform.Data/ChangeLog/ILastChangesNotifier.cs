using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    /// <summary>
    /// Invalidates <see cref="ILastChangesService"/> for entities saved by the global entity triggers.
    /// </summary>
    public interface ILastChangesNotifier
    {
        void OnEntitySaving(DbContext context, IEntity entity);
    }
}
