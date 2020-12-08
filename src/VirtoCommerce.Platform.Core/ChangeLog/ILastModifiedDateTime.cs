using System;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public interface ILastModifiedDateTime
    {
        public DateTimeOffset GetLastModified(string entityName = null);
        public void Reset(string entityName = null);
    }
}
