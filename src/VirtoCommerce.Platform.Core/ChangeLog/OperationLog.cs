using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public class OperationLog : AuditableEntity, ICloneable
    {
        public string ObjectType { get; set; }

        public string ObjectId { get; set; }

        public EntryState OperationType { get; set; }

        public string Detail { get; set; }

        public virtual OperationLog FromChangedEntry<T>(GenericChangedEntry<T> changedEntry) where T : IEntity
        {
            if (changedEntry == null)
            {
                throw new ArgumentNullException(nameof(changedEntry));
            }

            return FromChangedEntry(changedEntry, changedEntry.OldEntry.GetType().Name);
        }

        public virtual OperationLog FromChangedEntry<T>(GenericChangedEntry<T> changedEntry, string objectType) where T : IEntity
        {
            if (changedEntry == null)
            {
                throw new ArgumentNullException(nameof(changedEntry));
            }

            ObjectId = changedEntry.OldEntry.Id;
            ObjectType = objectType;
            OperationType = changedEntry.EntryState;

            return this;
        }

        #region ICloneable members

        public virtual object Clone()
        {
            return MemberwiseClone() as OperationLog;
        }

        #endregion
    }
}
