using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Threading;
using VirtoCommerce.Foundation.Frameworks.Logging;

namespace VirtoCommerce.Foundation.Data.Infrastructure.Interceptors
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Services.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Data.Entity;

    public class LogInterceptor : IInterceptor
    {
        OperationLogContext _logContext = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="LogInterceptor"/> class.
        /// </summary>
        /// <param name="logContext">The log context.</param>
        public LogInterceptor(OperationLogContext logContext)
        {
            _logContext = logContext;
        }

        /// <summary>
        /// Befores the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Before(InterceptionContext context)
        {
        }

        /// <summary>
        /// Executes after the changes to the context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void After(InterceptionContext context)
        {
            foreach (var entryWithState in context.EntriesByState.Where(x => x.Key != EntityState.Unchanged)) // added unchanged filter, so we don't log events for objects that haven't been changed
            {
                foreach (var entry in entryWithState)
                {
                    After(context.ObjectContext, entry, entryWithState.Key);
                }
                
            }

            _logContext.SaveChanges();
        }

        /// <summary>
        /// Executes after the changes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="item">The item.</param>
        /// <param name="state">The state.</param>
        private void After(ObjectContext context, DbEntityEntry item, EntityState state)
        {
            var now = DateTime.UtcNow;
            string entitySetName;
            var keys = state == EntityState.Deleted ? this.GetEntityKeyDeleted(item, out entitySetName) : this.GetEntityKeyUpdated(context, item, state, out entitySetName);
            if (keys != null)
            {
                var activityLog = this.StateEntry2OperationLog(entitySetName, item.GetType().Name, now, keys, state);
                this._logContext.Add(activityLog);
            }
        }

        /// <summary>
        /// Gets the entity key deleted.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="entitySetName">Name of the entity set.</param>
        /// <returns></returns>
        private string GetEntityKeyDeleted(DbEntityEntry entry, out string entitySetName)
        {
            var keyBuilder = new StringBuilder();
            var keys = entry.Entity.GetType().GetProperties().Where(w => w.GetCustomAttributes(typeof(KeyAttribute), true).Any()).Select(p => p.Name).ToArray();

            entitySetName = ResolveEntitySetName(entry.Entity.GetType());

            foreach (var key in keys)
            {
                var objectVal = entry.Property(key).CurrentValue;

                if (objectVal == null)
                {
                    continue;
                }

                var keyValue = objectVal.ToString();

                if (keyBuilder.Length > 0)
                {
                    keyBuilder.Append(",");
                }

                keyBuilder.Append(keyValue);
            }

            return keyBuilder.ToString();
        }

        /// <summary>
        /// Gets the entity key updated.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="state">The state.</param>
        /// <param name="entitySetName">Name of the entity set.</param>
        /// <returns>returns key</returns>
        private string GetEntityKeyUpdated(ObjectContext context, DbEntityEntry entry, EntityState state, out string entitySetName)
        {
            var keyBuilder = new StringBuilder();
            ObjectStateEntry newEntry = null;
            
            if (!context.ObjectStateManager.TryGetObjectStateEntry(entry.Entity, out newEntry))
            {
                entitySetName = String.Empty;
                Trace.TraceInformation("Can't find state entry for \"{0}\"", entry.ToString());
                return null;
            }

            entitySetName = newEntry.EntitySet.Name;
            var keys = state == EntityState.Added ? context.CreateEntityKey(newEntry.EntitySet.Name, entry.Entity) : newEntry.EntityKey;

            foreach (var key in keys.EntityKeyValues)
            {
                if (keyBuilder.Length > 0)
                {
                    keyBuilder.Append(",");
                }

                keyBuilder.Append(Convert.ToString(key.Value));
            }

            return keyBuilder.ToString();
        }

        /// <summary>
        /// States the entry2 operation log.
        /// </summary>
        /// <param name="entitySet">The entity set.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="now">The now.</param>
        /// <param name="keyValue">The key value.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// OperationLog object
        /// </returns>
        private OperationLog StateEntry2OperationLog(string entitySet, string objectType, DateTime now, string keyValue, EntityState state)
        {
            var userName = Thread.CurrentPrincipal.Identity.Name;

            if (String.IsNullOrEmpty(userName))
            {
                userName = "Anonymous";
            }

            var retVal = new OperationLog
            {
                LastModified = now,
                ObjectId = keyValue,
                ObjectType = objectType,
                TableName = entitySet,
                ModifiedBy = userName,
                OperationType = state.ToString()
            };

            return retVal;
        }

        /// <summary>
        /// Resolves the name of the entity set.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string ResolveEntitySetName(Type type)
        {
            var entitySetAttribute = (EntitySetAttribute)type.GetCustomAttributes(typeof(EntitySetAttribute), true).FirstOrDefault();

            if (entitySetAttribute != null)
            { 
                return entitySetAttribute.EntitySet;
                }

            return null;
        }
    }
}