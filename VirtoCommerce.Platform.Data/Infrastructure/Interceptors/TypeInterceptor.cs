using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public abstract class TypeInterceptor : IInterceptor
    {
        private readonly System.Type _targetType;
        public Type TargetType { get { return _targetType; } }

        protected TypeInterceptor(System.Type targetType)
        {
            this._targetType = targetType;
        }

        public virtual bool IsTargetEntity(DbEntityEntry item)
        {
            return item.State != EntityState.Detached &&
                   this.TargetType.IsInstanceOfType(item.Entity);
        }

        public void Before(InterceptionContext context)
        {
            foreach (var entry in context.Entries)
                Before(entry);
        }

        public void After(InterceptionContext context)
        {
            foreach (var entryWithState in context.EntriesByState)
            {
                foreach (var entry in entryWithState)
                {
                    After(entry, entryWithState.Key);
                }
            }
        }

        private void Before(DbEntityEntry item)
        {
            if (this.IsTargetEntity(item))
                this.OnBefore(item);
        }

        protected abstract void OnBefore(DbEntityEntry item);

        private void After(DbEntityEntry item, EntityState state)
        {
            if (this.IsTargetEntity(item))
                this.OnAfter(item, state);
        }

        protected abstract void OnAfter(DbEntityEntry item, EntityState state);
    }

}
