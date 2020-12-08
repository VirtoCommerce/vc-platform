using System;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public interface ILastChangesService
    {
        void Reset(string entityName);
        DateTimeOffset GetLastModified(string entityName);
    }
}
