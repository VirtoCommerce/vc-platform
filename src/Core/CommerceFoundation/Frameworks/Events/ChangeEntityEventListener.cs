using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Events
{
    public class ChangeEntityEventListener<T> : TypeEntityEventListener
    {
        protected override void OnEntityBeforeSaved(object sender, EntityEventArgs e)
        {
            T tItem = (T)sender;
            switch (e.State)
            {
                case EntityState.Added:
                    this.OnBeforeInsert(tItem, e);
                    break;
                case EntityState.Deleted:
                    this.OnBeforeDelete(tItem, e);
                    break;
                case EntityState.Modified:
                    this.OnBeforeUpdate(tItem, e);
                    break;
            }
        }

        protected override void OnEntityAfterSaved(object sender, EntityEventArgs e)
        {
            T tItem = (T)sender;
            switch (e.State)
            {
                case EntityState.Added:
                    this.OnAfterInsert(tItem, e);
                    break;
                case EntityState.Deleted:
                    this.OnAfterDelete(tItem, e);
                    break;
                case EntityState.Modified:
                    this.OnAfterUpdate(tItem, e);
                    break;
            }
        }

        public virtual void OnBeforeInsert(T item, EntityEventArgs e)
        {
            return;
        }

        public virtual void OnAfterInsert(T item, EntityEventArgs e)
        {
            return;
        }

        public virtual void OnBeforeUpdate(T item, EntityEventArgs e)
        {
            return;
        }

        public virtual void OnAfterUpdate(T item, EntityEventArgs e)
        {
            return;
        }

        public virtual void OnBeforeDelete(T item, EntityEventArgs e)
        {
            return;
        }

        public virtual void OnAfterDelete(T item, EntityEventArgs e)
        {
            return;
        }

        public ChangeEntityEventListener()
            : base(typeof(T))
        {

        }
    }
}
