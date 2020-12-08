using System;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public interface ILastModifiedDateTime
    {
        DateTimeOffset GetLastModified(string entityName = null);
        void Reset(string entityName = null);
    }
}
