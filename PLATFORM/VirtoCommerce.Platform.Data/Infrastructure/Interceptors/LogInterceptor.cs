using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public class LogInterceptor : IInterceptor
    {
		private readonly Func<IPlatformRepository> _repositoryFactory;

		public LogInterceptor(Func<IPlatformRepository> repositoryFactory)
        {
			_repositoryFactory = repositoryFactory;
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
			using (var repository = _repositoryFactory())
			{
				foreach (var entryWithState in context.EntriesByState.Where(x => x.Key != EntityState.Unchanged)) // added unchanged filter, so we don't log events for objects that haven't been changed
				{
					foreach (var entry in entryWithState)
					{
						var now = DateTime.UtcNow;
						var keys = entryWithState.Key == EntityState.Deleted ? GetEntityKeyDeleted(entry) : GetEntityKeyUpdated(context.ObjectContext, entry, entryWithState.Key);
						if (keys != null)
						{
							var activityLog = StateEntry2OperationLog(entry.GetType().Name, now, keys, entryWithState.Key);
							repository.Add(activityLog);
						}
					}
				}
				repository.UnitOfWork.Commit();
			}
        }

      
        /// <summary>
        /// Gets the entity key deleted.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="entitySetName">Name of the entity set.</param>
        /// <returns></returns>
        private string GetEntityKeyDeleted(DbEntityEntry entry)
        {
            var keyBuilder = new StringBuilder();
            var keys = entry.Entity.GetType().GetProperties().Where(w => w.GetCustomAttributes(typeof(KeyAttribute), true).Any()).Select(p => p.Name).ToArray();

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
        private string GetEntityKeyUpdated(ObjectContext context, DbEntityEntry entry, EntityState state)
        {
            var keyBuilder = new StringBuilder();
            ObjectStateEntry newEntry = null;
            
            if (!context.ObjectStateManager.TryGetObjectStateEntry(entry.Entity, out newEntry))
            {
                 Trace.TraceInformation("Can't find state entry for \"{0}\"", entry.ToString());
                return null;
            }

          
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
        private OperationLog StateEntry2OperationLog(string objectType, DateTime now, string keyValue, EntityState state)
        {
            var userName = Thread.CurrentPrincipal.Identity.Name;

            if (String.IsNullOrEmpty(userName))
            {
                userName = "Anonymous";
            }

            var retVal = new OperationLog
            {
				Id = Guid.NewGuid().ToString("N"),
                CreatedDate = now,
                ObjectId = keyValue,
                ObjectType = objectType,
				CreatedBy = userName,
                OperationType = state.ToString()
            };

            return retVal;
        }

     
    }
}