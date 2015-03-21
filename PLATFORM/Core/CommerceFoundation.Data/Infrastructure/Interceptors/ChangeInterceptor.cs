using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace VirtoCommerce.Foundation.Data.Infrastructure.Interceptors
{
    public class ChangeInterceptor<T> : TypeInterceptor
    {
        protected override void OnBefore(DbEntityEntry item)
        {
            T tItem = (T)item.Entity;
            switch (item.State)
            {
                case EntityState.Added:
                    this.OnBeforeInsert(item, tItem);
                    break;
                case EntityState.Deleted:
                    this.OnBeforeDelete(item, tItem);
                    break;
                case EntityState.Modified:
                    this.OnBeforeUpdate(item, tItem);
                    break;
            }
        }

        protected override void OnAfter(DbEntityEntry item, EntityState state)
        {
            T tItem = (T)item.Entity;
            switch (state)
            {
                case EntityState.Added:
                    this.OnAfterInsert(item, tItem);
                    break;
                case EntityState.Deleted:
                    this.OnAfterDelete(item, tItem);
                    break;
                case EntityState.Modified:
                    this.OnAfterUpdate(item, tItem);
                    break;
            }
        }

        public virtual void OnBeforeInsert(DbEntityEntry entry, T item)
        {
            return;
        }

        public virtual void OnAfterInsert(DbEntityEntry entry, T item)
        {
            return;
        }

        public virtual void OnBeforeUpdate(DbEntityEntry entry, T item)
        {
            return;
        }

        public virtual void OnAfterUpdate(DbEntityEntry entry, T item)
        {
            return;
        }

        public virtual void OnBeforeDelete(DbEntityEntry entry, T item)
        {
            return;
        }

        public virtual void OnAfterDelete(DbEntityEntry entry, T item)
        {
            return;
        }

        public ChangeInterceptor()
            : base(typeof(T))
        {

        }
    }

}
