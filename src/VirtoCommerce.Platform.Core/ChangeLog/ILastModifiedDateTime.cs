using System;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public interface ILastModifiedDateTime
    {
        DateTimeOffset LastModified { get; }
        void Reset();
    }
}
