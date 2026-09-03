using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    public interface ILastChangesNotifier
    {
        void OnEntitySaving(DbContext context, IEntity entity);
    }
}
