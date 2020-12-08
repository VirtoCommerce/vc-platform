using System;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public interface ILastModifiedDateTime
    {
        /// <summary>
        /// An equivalent to call GetLastModified().
        /// Left for compatibility
        /// </summary>
        DateTimeOffset LastModified { get; }
        /// <summary>
        /// An equivalent to call Reset(null).
        /// Left for compatibility
        /// </summary>
        void Reset();
        void Reset(string entityName);
        DateTimeOffset GetLastModified(string entityName = null);
    }
}
