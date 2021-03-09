using System;
using System.Linq;
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

            ObjectId = changedEntry.OldEntry.Id;
            ObjectType = GetTypeNameForLogging(changedEntry.OldEntry);
            OperationType = changedEntry.EntryState;

            return this;
        }

        #region ICloneable members

        public virtual object Clone()
        {
            return MemberwiseClone() as OperationLog;
        }

        #endregion

        /// <summary>
        /// Find the type name for operation log. It's a direct successor of AuditableEntity by default.
        /// </summary>
        /// <returns>Name of found type</returns>
        protected virtual string GetTypeNameForLogging<T>(T entry) where T : IEntity
        {
            if (entry is AuditableEntity)
            {
                return entry.GetType().GetTypeInheritanceChainTo(typeof(AuditableEntity)).Last().Name;
            }
            else if (entry is Entity)
            {
                return entry.GetType().GetTypeInheritanceChainTo(typeof(Entity)).Last().Name;
            }
            else
            {
                return entry.GetType().Name;
            }
        }
    }
}
