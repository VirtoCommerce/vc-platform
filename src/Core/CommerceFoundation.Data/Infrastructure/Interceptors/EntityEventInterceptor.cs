using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks.Events;

namespace VirtoCommerce.Foundation.Data.Infrastructure.Interceptors
{
    public class EntityEventInterceptor : IInterceptor
    {
        IEntityEventContext _context = null;
        public EntityEventInterceptor(IEntityEventContext context)
        {
            _context = context;
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
            var args = CreateArgs(item, item.State);
            _context.RaiseBeforeEvent(item.Entity, args);
        }

        private void After(DbEntityEntry item, EntityState state)
        {
            var args = CreateArgs(item, state);
            _context.RaiseAfterEvent(item.Entity, args);
        }

        private EntityEventArgs CreateArgs(DbEntityEntry item, EntityState state)
        {
            PropertyValues originalValues = null;
            PropertyValues currentValues = null;

            if (state == EntityState.Modified && !(item.State == EntityState.Detached))
            {
                originalValues = new PropertyValues(item.OriginalValues);
                currentValues = new PropertyValues(item.CurrentValues);
            }

            var args = new EntityEventArgs(state, originalValues, currentValues);
            return args;
        }
    }
}
